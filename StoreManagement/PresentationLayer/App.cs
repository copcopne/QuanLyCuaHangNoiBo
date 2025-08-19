using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class App : Form
    {
        private Panel sidebar;
        private Button btnToggle;           // nút thu gọn
        private Button currentActive;       // đang chọn
        private FlowLayoutPanel navHost;  // chứa các nút

        // màu sắc
        private readonly Color C_BG = Color.FromArgb(0, 138, 230);       // nền xanh
        private readonly Color C_ITEM = Color.FromArgb(0, 148, 255);     // item thường
        private readonly Color C_ACTIVE = Color.FromArgb(0, 110, 190);   // item active
        private readonly Color C_HOVER = Color.FromArgb(0, 160, 255);    // hover
        private readonly Color C_TEXT = Color.White;
        private Entity.UserAccount CurrentUser => AuthenticateBUS.CurrentUser;

        public App()
        {
            InitializeComponent();
            BuildSidebar();
            this.MdiChildActivate += App_MdiChildActivate;
        }

        private void BuildSidebar()
        {
            if (sidebar != null)
            {
                Controls.Remove(sidebar);
                sidebar.Dispose();
                sidebar = null;
                navHost = null;
                currentActive = null;
            }

            // Không có user -> không build (caller xử lý login)
            if (CurrentUser == null) return;
            // panel trái
            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = C_BG
            };
            Controls.Add(sidebar);
            sidebar.BringToFront();

            // host các nút: TopDown, có padding ngoài
            navHost = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(12, 10, 12, 10), // padding quanh nhóm nút
                BackColor = C_BG
            };
            sidebar.Controls.Add(navHost);


            if (CurrentUser == null)
            {
                Logout();
            }
            String role = CurrentUser.Role.ToLower();

            if (role == "admin" || role == "stock_manager")
            {
                AddNavButton("Q.L đơn nhập kho", () => OpenChild<StockInManagementForm>());
                AddNavButton("Q.L sản phẩm", () => OpenChild<ProductManagementForm>());
            }

            if (role == "admin" || role == "sales_manager")
            {
                AddNavButton("Q.L hóa đơn", () => OpenChild<InvoiceManagementForm>());
                AddNavButton("Q.L vận chuyển", () => OpenChild<DeliveryManagementForm>());
            }

            if (role == "admin" || role == "cashier" || role == "sales_manager")
            {
                AddNavButton("Lập/xuất hóa đơn", () => OpenChild<InvoiceForm>());
            }



            if (role == "admin" || role == "employee_manager")
            {
                AddNavButton("Q.L nhân Viên", () => OpenChild<EmployeeManagementForm>());
                AddNavButton("Q.L tài khoản", () => OpenChild<UserAccountManagementForm>());
            }
            if (role == "admin")
            {
                AddNavButton("Q.L khách hàng", () => OpenChild<CustomerManagementForm>());
            }
            AddNavButton("Thiết Lập", () => OpenChild<SettingsForm>());
        }

        private void AddNavButton(string text, Action onClick)
        {
            var btn = new Button
            {
                Height = 48,
                Width = sidebar.Width - navHost.Padding.Left - navHost.Padding.Right,
                Text = "   " + text,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                FlatStyle = FlatStyle.Flat,
                ForeColor = C_TEXT,
                BackColor = C_ITEM,
                Tag = onClick,

                // khoảng trống GIỮA các tab
                Margin = new Padding(0, 6, 0, 6),

                // khoảng trống TRONG tab (đẩy chữ/ảnh vào trong)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = C_HOVER;
            btn.FlatAppearance.MouseDownBackColor = C_ACTIVE;

            btn.MouseEnter += (_, __) => { if (btn != currentActive) btn.BackColor = C_HOVER; };
            btn.MouseLeave += (_, __) => { if (btn != currentActive) btn.BackColor = C_ITEM; };

            btn.Click += (s, e) =>
            {
                if (currentActive != null && !currentActive.IsDisposed) currentActive.BackColor = C_ITEM;
                currentActive = btn;
                currentActive.BackColor = C_ACTIVE;
                (btn.Tag as Action)?.Invoke();
            };

            navHost.Controls.Add(btn); // THÊM VÀO navHost thay vì sidebar
        }
        // Mở MDI child: nếu đã có thì kích hoạt, không mở trùng
        private void OpenChild<T>() where T : Form, new()
        {
            var exists = this.MdiChildren.OfType<T>().FirstOrDefault();
            if (exists != null) { exists.Activate(); return; }

            var f = new T { MdiParent = this, WindowState = FormWindowState.Maximized };
            f.Show();
        }

        // Đồng bộ highlight khi đổi active child (vd: user Alt+Tab)
        private void App_MdiChildActivate(object sender, EventArgs e)
        {
            var child = this.ActiveMdiChild;
            if (child == null) return;
            var map = new Dictionary<Type, string> {
            { typeof(InvoiceManagementForm), "Q.L hóa đơn" },
            { typeof(CustomerManagementForm), "Q.L khách hàng" },
            { typeof(StockInManagementForm), "Q.L đơn nhập kho" },
            { typeof(EmployeeManagementForm), "Q.L nhân Viên" },
            { typeof(UserAccountManagementForm), "Q.L tài khoản" },
            { typeof(ProductManagementForm), "Q.L sản phẩm" },
            { typeof(DeliveryManagementForm), "Q.L vận chuyển" },
            { typeof(SettingsForm), "Thiết Lập" },
            { typeof(InvoiceForm), "Lập/xuất hóa đơn" }
        };

            if (map.TryGetValue(child.GetType(), out var title))
            {
                var btn = sidebar.Controls.OfType<Button>()
                            .FirstOrDefault(b => b.Text.Trim() == title || (sidebar.Width <= 60 && b.Image != null));
                if (btn != null && btn != currentActive)
                {
                    if (currentActive != null) currentActive.BackColor = C_ITEM;
                    currentActive = btn;
                    currentActive.BackColor = C_ACTIVE;
                }
            }
        }

        public void Logout()
        {
            // 1) clear session
            AuthenticateBUS.Logout();

            // 2) đóng tất cả form con
            foreach (var f in this.MdiChildren) f.Close();

            // 3) ẩn MDI, mở lại form đăng nhập dạng modal
            this.Hide();
            using (var login = new LoginForm())
            {
                login.StartPosition = FormStartPosition.CenterScreen;

                // Nếu đăng nhập lại thành công -> hiện MDI và init lại state nếu cần
                if (login.ShowDialog(this) == DialogResult.OK)
                {
                    BuildSidebar();
                    this.Show();
                }
                else
                {
                    // Không đăng nhập -> thoát app
                    this.Close();
                }
            }
        }


        private void App_Load(object sender, EventArgs e)
        {

        }
    }
}

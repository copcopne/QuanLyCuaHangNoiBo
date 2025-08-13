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

        public App()
        {
            InitializeComponent();
            BuildSidebar();
            this.MdiChildActivate += App_MdiChildActivate;
        }

        private void BuildSidebar()
        {
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


            // danh sách items
            AddNavButton("Lập/xuất hóa đơn", () => OpenChild<InvoiceManagementForm>());
            AddNavButton("Q.L khách hàng", () => OpenChild<CustomerManagementForm>());
            AddNavButton("Q.L đơn nhập kho", () => OpenChild<StockInManagementForm>());
            AddNavButton("Q.L nhân Viên", () => OpenChild<EmployeeManagementForm>());
            AddNavButton("Q.L tài khoản", () => OpenChild<UserAccountManagementForm>());
            AddNavButton("Thiết Lập", () => OpenChild<SettingsForm>());

            // canh giữa lần đầu và khi resize
            this.Shown += (_, __) => LayoutNav();
            sidebar.SizeChanged += (_, __) => LayoutNav();
        }


        private void AddNavButton(string text, Action onClick)
        {
            var btn = new Button
            {
                Height = 48,
                Width = sidebar.Width - navHost.Padding.Left - navHost.Padding.Right, // set tạm, sẽ cập nhật lại ở LayoutNav()
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
                Padding = new Padding(14, 0, 10, 0)
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

        private void LayoutNav()
        {
            if (navHost == null || sidebar == null) return;

            // set lại width cho tất cả nút theo độ rộng sidebar hiện tại
            int maxWidth = sidebar.Width - navHost.Padding.Left - navHost.Padding.Right;
            foreach (var b in navHost.Controls.OfType<Button>())
                b.Width = maxWidth;

            navHost.PerformLayout();
            int contentH = navHost.PreferredSize.Height;

            // tối thiểu đặt dưới logo, và canh giữa dọc phần còn lại
            int minTop = 8;
            int centerTop = (sidebar.ClientSize.Height - contentH) / 2;
            navHost.Top = Math.Max(minTop, centerTop);

            // canh trái
            navHost.Left = 0; // đã có Padding.Left nên để 0 là ổn
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
            { typeof(InvoiceManagementForm), "Lập/xuất hóa đơn" },
            { typeof(CustomerManagementForm), "Q.L khách hàng" },
            { typeof(StockInManagementForm), "Q.L đơn nhập kho" },
            { typeof(EmployeeManagementForm), "Q.L nhân Viên" },
            { typeof(UserAccountManagementForm), "Q.L tài khoản" },
            { typeof(SettingsForm), "Thiết Lập" }
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

        private void App_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using BusinessLayer;
using Entity;
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
    public partial class DeliveryDetailsForm : Form
    {
        private readonly salesysdbEntities context;
        private readonly DeliveryBUS deliveryBUS;
        private readonly EmployeeBUS employeeBUS;
        private Delivery delivery;
        public DeliveryDetailsForm(int deliveryID)
        {
            InitializeComponent();
            context = new salesysdbEntities();
            deliveryBUS = new DeliveryBUS(context);
            employeeBUS = new EmployeeBUS();
            delivery = deliveryBUS.GetDeliveryById(deliveryID);
        }

        private void DeliveryDetailsForm_Load(object sender, EventArgs e)
        {
            // Nạp các textbox và combobox với thông tin giao hàng
            if (delivery == null)
            {
                MessageBox.Show("Giao hàng không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            txtAddress.Text = delivery.DeliveryAddress;
            txtNote.Text = delivery.Notes;
            var employees = employeeBUS.GetEmployeesByPosition("delivery").ToList();
            employees.Insert(0, new Employee
            {
                EmployeeID = 0,
                FullName = "Không phân công"
            });

            cbEmployee.DataSource = employees;
            cbEmployee.DisplayMember = "FullName";
            cbEmployee.ValueMember = "EmployeeID";

            if (delivery.AssignedStaffID.HasValue)
                cbEmployee.SelectedValue = delivery.AssignedStaffID.Value;
            else
                cbEmployee.SelectedValue = 0;

            cbStatus.DataSource = new List<string> { "Chưa giao", "Đang giao", "Đã giao", "Đã hủy" };
            cbStatus.SelectedItem = delivery.Status;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ giao hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (delivery == null)
            {
                MessageBox.Show("Giao hàng không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            delivery.DeliveryAddress = txtAddress.Text.Trim();
            delivery.Notes = txtNote.Text.Trim();
            delivery.AssignedStaffID = (int)cbEmployee.SelectedValue == 0 ? null : (int?)cbEmployee.SelectedValue;
            delivery.Status = cbStatus.SelectedItem.ToString();
            try
            {
                deliveryBUS.UpdateDelivery(delivery);
                MessageBox.Show("Cập nhật giao hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật giao hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

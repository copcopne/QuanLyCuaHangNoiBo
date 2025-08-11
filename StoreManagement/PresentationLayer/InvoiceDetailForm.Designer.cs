namespace PresentationLayer
{
    partial class InvoiceDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbProductName = new System.Windows.Forms.ComboBox();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTotalPrice = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.SystemColors.GrayText;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.ForeColor = System.Drawing.SystemColors.Window;
            this.btnReturn.Location = new System.Drawing.Point(36, 335);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(290, 51);
            this.btnReturn.TabIndex = 18;
            this.btnReturn.Text = "Quay về";
            this.btnReturn.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSave.Location = new System.Drawing.Point(366, 335);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(288, 51);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUnitPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnitPrice.Location = new System.Drawing.Point(147, 256);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(179, 35);
            this.txtUnitPrice.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 24);
            this.label2.TabIndex = 13;
            this.label2.Text = "Đơn giá:";
            // 
            // txtAmount
            // 
            this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(147, 198);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(179, 35);
            this.txtAmount.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 24);
            this.label1.TabIndex = 11;
            this.label1.Text = "Số lượng:";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTitle.Location = new System.Drawing.Point(13, 15);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(260, 33);
            this.labelTitle.TabIndex = 19;
            this.labelTitle.Text = "Chi tiết hóa đơn #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 24);
            this.label3.TabIndex = 20;
            this.label3.Text = "Sản phẩm:";
            // 
            // cbProductName
            // 
            this.cbProductName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbProductName.FormattingEnabled = true;
            this.cbProductName.Location = new System.Drawing.Point(147, 138);
            this.cbProductName.Name = "cbProductName";
            this.cbProductName.Size = new System.Drawing.Size(507, 37);
            this.cbProductName.TabIndex = 21;
            // 
            // cbCategory
            // 
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(147, 78);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(507, 37);
            this.cbCategory.TabIndex = 23;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.cbCategory_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(32, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 24);
            this.label4.TabIndex = 22;
            this.label4.Text = "Danh mục:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(362, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 24);
            this.label5.TabIndex = 24;
            this.label5.Text = "Thành tiền:";
            // 
            // txtTotalPrice
            // 
            this.txtTotalPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalPrice.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPrice.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtTotalPrice.Location = new System.Drawing.Point(473, 198);
            this.txtTotalPrice.Name = "txtTotalPrice";
            this.txtTotalPrice.ReadOnly = true;
            this.txtTotalPrice.Size = new System.Drawing.Size(177, 35);
            this.txtTotalPrice.TabIndex = 25;
            // 
            // InvoiceDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 404);
            this.Controls.Add(this.txtTotalPrice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCategory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbProductName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtUnitPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.label1);
            this.Name = "InvoiceDetailForm";
            this.Text = "InvoiceDetailForm";
            this.Load += new System.EventHandler(this.InvoiceDetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbProductName;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTotalPrice;
    }
}
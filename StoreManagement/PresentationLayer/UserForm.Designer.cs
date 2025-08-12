namespace PresentationLayer
{
    partial class UserForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.grpBoxAccount = new System.Windows.Forms.GroupBox();
            this.checkBoxIsActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxChangePassword = new System.Windows.Forms.CheckBox();
            this.groupPassword = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.grpBoxAccount.SuspendLayout();
            this.groupPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(290, 455);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(145, 25);
            this.label13.TabIndex = 1;
            this.label13.Text = "Tên đăng nhập";
            // 
            // txtUsername
            // 
            this.txtUsername.Enabled = false;
            this.txtUsername.Location = new System.Drawing.Point(128, 31);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(243, 30);
            this.txtUsername.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(67, 86);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 25);
            this.label11.TabIndex = 5;
            this.label11.Text = "Vai trò";
            // 
            // txtRole
            // 
            this.txtRole.Location = new System.Drawing.Point(128, 83);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(243, 30);
            this.txtRole.TabIndex = 6;
            // 
            // grpBoxAccount
            // 
            this.grpBoxAccount.Controls.Add(this.checkBoxIsActive);
            this.grpBoxAccount.Controls.Add(this.label11);
            this.grpBoxAccount.Controls.Add(this.txtRole);
            this.grpBoxAccount.Controls.Add(this.label13);
            this.grpBoxAccount.Controls.Add(this.txtUsername);
            this.grpBoxAccount.Location = new System.Drawing.Point(35, 50);
            this.grpBoxAccount.Name = "grpBoxAccount";
            this.grpBoxAccount.Size = new System.Drawing.Size(409, 160);
            this.grpBoxAccount.TabIndex = 19;
            this.grpBoxAccount.TabStop = false;
            this.grpBoxAccount.Text = "Thông tin tài khoản";
            // 
            // checkBoxIsActive
            // 
            this.checkBoxIsActive.AutoSize = true;
            this.checkBoxIsActive.Location = new System.Drawing.Point(6, 130);
            this.checkBoxIsActive.Name = "checkBoxIsActive";
            this.checkBoxIsActive.Size = new System.Drawing.Size(173, 29);
            this.checkBoxIsActive.TabIndex = 7;
            this.checkBoxIsActive.Text = "Đang hoạt động";
            this.checkBoxIsActive.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(371, 455);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 35);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(123, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 36);
            this.label1.TabIndex = 16;
            this.label1.Text = "Thông tin tài khoản";
            // 
            // checkBoxChangePassword
            // 
            this.checkBoxChangePassword.AutoSize = true;
            this.checkBoxChangePassword.Location = new System.Drawing.Point(41, 226);
            this.checkBoxChangePassword.Name = "checkBoxChangePassword";
            this.checkBoxChangePassword.Size = new System.Drawing.Size(210, 29);
            this.checkBoxChangePassword.TabIndex = 23;
            this.checkBoxChangePassword.Text = "Cập nhật mật khẩu?";
            this.checkBoxChangePassword.UseVisualStyleBackColor = true;
            this.checkBoxChangePassword.CheckedChanged += new System.EventHandler(this.checkBoxChangePassword_CheckedChanged);
            // 
            // groupPassword
            // 
            this.groupPassword.Controls.Add(this.label4);
            this.groupPassword.Controls.Add(this.txtConfirm);
            this.groupPassword.Controls.Add(this.label2);
            this.groupPassword.Controls.Add(this.txtPassword);
            this.groupPassword.Controls.Add(this.label3);
            this.groupPassword.Controls.Add(this.txtOldPassword);
            this.groupPassword.Location = new System.Drawing.Point(35, 256);
            this.groupPassword.Name = "groupPassword";
            this.groupPassword.Size = new System.Drawing.Size(409, 177);
            this.groupPassword.TabIndex = 20;
            this.groupPassword.TabStop = false;
            this.groupPassword.Text = "Thông tin mật khẩu";
            this.groupPassword.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Xác nhận";
            // 
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(128, 123);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(243, 30);
            this.txtConfirm.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Mật khẩu mới";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(128, 77);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(243, 30);
            this.txtPassword.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Mật khẩu cũ";
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.Location = new System.Drawing.Point(128, 31);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.Size = new System.Drawing.Size(243, 30);
            this.txtOldPassword.TabIndex = 2;
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(469, 497);
            this.Controls.Add(this.groupPassword);
            this.Controls.Add(this.checkBoxChangePassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpBoxAccount);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UserForm";
            this.Text = "UserForm";
            this.Load += new System.EventHandler(this.UserForm_Load);
            this.grpBoxAccount.ResumeLayout(false);
            this.grpBoxAccount.PerformLayout();
            this.groupPassword.ResumeLayout(false);
            this.groupPassword.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.GroupBox grpBoxAccount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxChangePassword;
        private System.Windows.Forms.GroupBox groupPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOldPassword;
        private System.Windows.Forms.CheckBox checkBoxIsActive;
    }
}
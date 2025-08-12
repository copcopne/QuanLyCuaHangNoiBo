namespace PresentationLayer
{
    partial class StockInManagementForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddStockIn = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.gridViewStockIn = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewStockIn)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btnAddStockIn);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1175, 59);
            this.panel1.TabIndex = 4;
            // 
            // btnAddStockIn
            // 
            this.btnAddStockIn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddStockIn.AutoSize = true;
            this.btnAddStockIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddStockIn.Location = new System.Drawing.Point(931, 10);
            this.btnAddStockIn.Name = "btnAddStockIn";
            this.btnAddStockIn.Size = new System.Drawing.Size(241, 34);
            this.btnAddStockIn.TabIndex = 1;
            this.btnAddStockIn.Text = "Thêm đơn nhập hàng mới";
            this.btnAddStockIn.UseVisualStyleBackColor = true;
            this.btnAddStockIn.Click += new System.EventHandler(this.btnAddStockIn_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(12, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(913, 29);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // gridViewStockIn
            // 
            this.gridViewStockIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewStockIn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridViewStockIn.Location = new System.Drawing.Point(0, 65);
            this.gridViewStockIn.Name = "gridViewStockIn";
            this.gridViewStockIn.ReadOnly = true;
            this.gridViewStockIn.RowHeadersWidth = 51;
            this.gridViewStockIn.Size = new System.Drawing.Size(1175, 529);
            this.gridViewStockIn.TabIndex = 5;
            // 
            // StockInManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 594);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gridViewStockIn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "StockInManagementForm";
            this.Text = "StockInManagementForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewStockIn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddStockIn;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView gridViewStockIn;
    }
}
namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmChiTietSuatDien
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.cboSuKien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblBatDau = new System.Windows.Forms.Label();
            this.dtpBatDau = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblKetThuc = new System.Windows.Forms.Label();
            this.dtpKetThuc = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblDiaDiem = new System.Windows.Forms.Label();
            this.txtDiaDiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin Suất diễn";
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Location = new System.Drawing.Point(20, 70);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(50, 15);
            this.lblSuKien.TabIndex = 1;
            this.lblSuKien.Text = "Sự kiện:";
            // 
            // cboSuKien
            // 
            this.cboSuKien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuKien.BorderRadius = 6;
            this.cboSuKien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboSuKien.ItemHeight = 30;
            this.cboSuKien.Location = new System.Drawing.Point(20, 90);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(350, 36);
            this.cboSuKien.TabIndex = 2;
            // 
            // lblBatDau
            // 
            this.lblBatDau.AutoSize = true;
            this.lblBatDau.Location = new System.Drawing.Point(20, 140);
            this.lblBatDau.Name = "lblBatDau";
            this.lblBatDau.Size = new System.Drawing.Size(53, 15);
            this.lblBatDau.TabIndex = 3;
            this.lblBatDau.Text = "Bắt đầu:";
            // 
            // dtpBatDau
            // 
            this.dtpBatDau.BorderRadius = 6;
            this.dtpBatDau.Checked = true;
            this.dtpBatDau.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpBatDau.FillColor = System.Drawing.Color.White;
            this.dtpBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBatDau.Location = new System.Drawing.Point(20, 160);
            this.dtpBatDau.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpBatDau.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpBatDau.Name = "dtpBatDau";
            this.dtpBatDau.Size = new System.Drawing.Size(350, 36);
            this.dtpBatDau.TabIndex = 4;
            this.dtpBatDau.Value = new System.DateTime(2025, 1, 1, 0, 0, 0, 0);
            // 
            // lblKetThuc
            // 
            this.lblKetThuc.AutoSize = true;
            this.lblKetThuc.Location = new System.Drawing.Point(20, 210);
            this.lblKetThuc.Name = "lblKetThuc";
            this.lblKetThuc.Size = new System.Drawing.Size(56, 15);
            this.lblKetThuc.TabIndex = 5;
            this.lblKetThuc.Text = "Kết thúc:";
            // 
            // dtpKetThuc
            // 
            this.dtpKetThuc.BorderRadius = 6;
            this.dtpKetThuc.Checked = true;
            this.dtpKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpKetThuc.FillColor = System.Drawing.Color.White;
            this.dtpKetThuc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKetThuc.Location = new System.Drawing.Point(20, 230);
            this.dtpKetThuc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpKetThuc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpKetThuc.Name = "dtpKetThuc";
            this.dtpKetThuc.Size = new System.Drawing.Size(350, 36);
            this.dtpKetThuc.TabIndex = 6;
            this.dtpKetThuc.Value = new System.DateTime(2025, 1, 1, 0, 0, 0, 0);
            // 
            // lblDiaDiem
            // 
            this.lblDiaDiem.AutoSize = true;
            this.lblDiaDiem.Location = new System.Drawing.Point(20, 280);
            this.lblDiaDiem.Name = "lblDiaDiem";
            this.lblDiaDiem.Size = new System.Drawing.Size(57, 15);
            this.lblDiaDiem.TabIndex = 7;
            this.lblDiaDiem.Text = "Địa điểm:";
            // 
            // txtDiaDiem
            // 
            this.txtDiaDiem.BorderRadius = 6;
            this.txtDiaDiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDiaDiem.DefaultText = "";
            this.txtDiaDiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDiaDiem.Location = new System.Drawing.Point(20, 300);
            this.txtDiaDiem.Name = "txtDiaDiem";
            this.txtDiaDiem.PlaceholderText = "Nhập địa điểm...";
            this.txtDiaDiem.SelectedText = "";
            this.txtDiaDiem.Size = new System.Drawing.Size(350, 36);
            this.txtDiaDiem.TabIndex = 8;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Location = new System.Drawing.Point(20, 350);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(64, 15);
            this.lblTrangThai.TabIndex = 9;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.cboTrangThai.BorderRadius = 6;
            this.cboTrangThai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTrangThai.ItemHeight = 30;
            this.cboTrangThai.Items.AddRange(new object[] { "MoCuaBanVe", "DongBanVe", "DangDien", "DaKetThuc" });
            this.cboTrangThai.Location = new System.Drawing.Point(20, 370);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(350, 36);
            this.cboTrangThai.TabIndex = 10;
            // 
            // btnLuu
            // 
            this.btnLuu.BorderRadius = 6;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(20, 430);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(170, 40);
            this.btnLuu.TabIndex = 11;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderRadius = 6;
            this.btnHuy.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(200, 430);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(170, 40);
            this.btnHuy.TabIndex = 12;
            this.btnHuy.Text = "HỦY";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // FrmChiTietSuatDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 490);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.cboTrangThai);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.txtDiaDiem);
            this.Controls.Add(this.lblDiaDiem);
            this.Controls.Add(this.dtpKetThuc);
            this.Controls.Add(this.lblKetThuc);
            this.Controls.Add(this.dtpBatDau);
            this.Controls.Add(this.lblBatDau);
            this.Controls.Add(this.cboSuKien);
            this.Controls.Add(this.lblSuKien);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChiTietSuatDien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết Suất diễn";
            this.Load += new System.EventHandler(this.FrmChiTietSuatDien_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSuKien;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuKien;
        private System.Windows.Forms.Label lblBatDau;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpBatDau;
        private System.Windows.Forms.Label lblKetThuc;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpKetThuc;
        private System.Windows.Forms.Label lblDiaDiem;
        private Guna.UI2.WinForms.Guna2TextBox txtDiaDiem;
        private System.Windows.Forms.Label lblTrangThai;
        private Guna.UI2.WinForms.Guna2ComboBox cboTrangThai;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
    }
}
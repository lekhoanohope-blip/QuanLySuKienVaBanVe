using Guna.UI2.WinForms;
using System.Windows.Forms;

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
            this.lblGioBatDau = new System.Windows.Forms.Label();
            this.cboGioBatDau = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblKetThuc = new System.Windows.Forms.Label();
            this.dtpKetThuc = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblGioKetThuc = new System.Windows.Forms.Label();
            this.cboGioKetThuc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblDiaDiem = new System.Windows.Forms.Label();
            this.txtDiaDiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblLoaiSoDo = new System.Windows.Forms.Label();
            this.cboLoaiSoDo = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(24, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(268, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin Suất diễn";
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSuKien.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblSuKien.Location = new System.Drawing.Point(24, 72);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(54, 17);
            this.lblSuKien.TabIndex = 1;
            this.lblSuKien.Text = "Sự kiện:";
            // 
            // cboSuKien
            // 
            this.cboSuKien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuKien.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.cboSuKien.BorderRadius = 6;
            this.cboSuKien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.FocusedColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboSuKien.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboSuKien.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboSuKien.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cboSuKien.ItemHeight = 30;
            this.cboSuKien.Location = new System.Drawing.Point(24, 95);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(512, 36);
            this.cboSuKien.TabIndex = 2;
            // 
            // lblBatDau
            // 
            this.lblBatDau.AutoSize = true;
            this.lblBatDau.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBatDau.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblBatDau.Location = new System.Drawing.Point(24, 147);
            this.lblBatDau.Name = "lblBatDau";
            this.lblBatDau.Size = new System.Drawing.Size(88, 17);
            this.lblBatDau.TabIndex = 3;
            this.lblBatDau.Text = "Ngày bắt đầu:";
            // 
            // dtpBatDau
            // 
            this.dtpBatDau.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dtpBatDau.BorderRadius = 6;
            this.dtpBatDau.BorderThickness = 1;
            this.dtpBatDau.Checked = true;
            this.dtpBatDau.CheckedState.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dtpBatDau.CheckedState.FillColor = System.Drawing.Color.White;
            this.dtpBatDau.CheckedState.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dtpBatDau.CustomFormat = "dd/MM/yyyy";
            this.dtpBatDau.FillColor = System.Drawing.Color.White;
            this.dtpBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpBatDau.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dtpBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBatDau.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.dtpBatDau.Location = new System.Drawing.Point(24, 170);
            this.dtpBatDau.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpBatDau.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpBatDau.Name = "dtpBatDau";
            this.dtpBatDau.Size = new System.Drawing.Size(340, 36);
            this.dtpBatDau.TabIndex = 4;
            this.dtpBatDau.Value = new System.DateTime(2026, 1, 1, 0, 0, 0, 0);
            // 
            // lblGioBatDau
            // 
            this.lblGioBatDau.AutoSize = true;
            this.lblGioBatDau.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGioBatDau.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblGioBatDau.Location = new System.Drawing.Point(380, 147);
            this.lblGioBatDau.Name = "lblGioBatDau";
            this.lblGioBatDau.Size = new System.Drawing.Size(80, 17);
            this.lblGioBatDau.TabIndex = 5;
            this.lblGioBatDau.Text = "Giờ bắt đầu:";
            // 
            // cboGioBatDau
            // 
            this.cboGioBatDau.BackColor = System.Drawing.Color.Transparent;
            this.cboGioBatDau.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.cboGioBatDau.BorderRadius = 6;
            this.cboGioBatDau.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboGioBatDau.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGioBatDau.FocusedColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboGioBatDau.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboGioBatDau.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboGioBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboGioBatDau.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cboGioBatDau.ItemHeight = 28;
            this.cboGioBatDau.Location = new System.Drawing.Point(380, 170);
            this.cboGioBatDau.Name = "cboGioBatDau";
            this.cboGioBatDau.Size = new System.Drawing.Size(156, 36);
            this.cboGioBatDau.TabIndex = 6;
            // 
            // lblKetThuc
            // 
            this.lblKetThuc.AutoSize = true;
            this.lblKetThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblKetThuc.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblKetThuc.Location = new System.Drawing.Point(24, 222);
            this.lblKetThuc.Name = "lblKetThuc";
            this.lblKetThuc.Size = new System.Drawing.Size(90, 17);
            this.lblKetThuc.TabIndex = 7;
            this.lblKetThuc.Text = "Ngày kết thúc:";
            // 
            // dtpKetThuc
            // 
            this.dtpKetThuc.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dtpKetThuc.BorderRadius = 6;
            this.dtpKetThuc.BorderThickness = 1;
            this.dtpKetThuc.Checked = true;
            this.dtpKetThuc.CheckedState.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dtpKetThuc.CheckedState.FillColor = System.Drawing.Color.White;
            this.dtpKetThuc.CheckedState.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dtpKetThuc.CustomFormat = "dd/MM/yyyy";
            this.dtpKetThuc.FillColor = System.Drawing.Color.White;
            this.dtpKetThuc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpKetThuc.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dtpKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKetThuc.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.dtpKetThuc.Location = new System.Drawing.Point(24, 245);
            this.dtpKetThuc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpKetThuc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpKetThuc.Name = "dtpKetThuc";
            this.dtpKetThuc.Size = new System.Drawing.Size(340, 36);
            this.dtpKetThuc.TabIndex = 8;
            this.dtpKetThuc.Value = new System.DateTime(2026, 1, 1, 0, 0, 0, 0);
            // 
            // lblGioKetThuc
            // 
            this.lblGioKetThuc.AutoSize = true;
            this.lblGioKetThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGioKetThuc.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblGioKetThuc.Location = new System.Drawing.Point(380, 222);
            this.lblGioKetThuc.Name = "lblGioKetThuc";
            this.lblGioKetThuc.Size = new System.Drawing.Size(82, 17);
            this.lblGioKetThuc.TabIndex = 9;
            this.lblGioKetThuc.Text = "Giờ kết thúc:";
            // 
            // cboGioKetThuc
            // 
            this.cboGioKetThuc.BackColor = System.Drawing.Color.Transparent;
            this.cboGioKetThuc.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.cboGioKetThuc.BorderRadius = 6;
            this.cboGioKetThuc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboGioKetThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGioKetThuc.FocusedColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboGioKetThuc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboGioKetThuc.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboGioKetThuc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboGioKetThuc.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cboGioKetThuc.ItemHeight = 28;
            this.cboGioKetThuc.Location = new System.Drawing.Point(380, 245);
            this.cboGioKetThuc.Name = "cboGioKetThuc";
            this.cboGioKetThuc.Size = new System.Drawing.Size(156, 36);
            this.cboGioKetThuc.TabIndex = 10;
            // 
            // lblDiaDiem
            // 
            this.lblDiaDiem.AutoSize = true;
            this.lblDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDiaDiem.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblDiaDiem.Location = new System.Drawing.Point(24, 297);
            this.lblDiaDiem.Name = "lblDiaDiem";
            this.lblDiaDiem.Size = new System.Drawing.Size(63, 17);
            this.lblDiaDiem.TabIndex = 11;
            this.lblDiaDiem.Text = "Địa điểm:";
            // 
            // txtDiaDiem
            // 
            this.txtDiaDiem.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.txtDiaDiem.BorderRadius = 6;
            this.txtDiaDiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDiaDiem.DefaultText = "";
            this.txtDiaDiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtDiaDiem.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtDiaDiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDiaDiem.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtDiaDiem.Location = new System.Drawing.Point(24, 320);
            this.txtDiaDiem.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtDiaDiem.Name = "txtDiaDiem";
            this.txtDiaDiem.PlaceholderText = "Nhập địa điểm tổ chức...";
            this.txtDiaDiem.SelectedText = "";
            this.txtDiaDiem.Size = new System.Drawing.Size(512, 38);
            this.txtDiaDiem.TabIndex = 12;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTrangThai.Location = new System.Drawing.Point(24, 374);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(70, 17);
            this.lblTrangThai.TabIndex = 13;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.cboTrangThai.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.cboTrangThai.BorderRadius = 6;
            this.cboTrangThai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.FocusedColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboTrangThai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboTrangThai.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTrangThai.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cboTrangThai.ItemHeight = 30;
            this.cboTrangThai.Location = new System.Drawing.Point(24, 397);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(512, 36);
            this.cboTrangThai.TabIndex = 14;
            // 
            // lblLoaiSoDo
            // 
            this.lblLoaiSoDo.AutoSize = true;
            this.lblLoaiSoDo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblLoaiSoDo.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblLoaiSoDo.Location = new System.Drawing.Point(24, 449);
            this.lblLoaiSoDo.Name = "lblLoaiSoDo";
            this.lblLoaiSoDo.Size = new System.Drawing.Size(104, 17);
            this.lblLoaiSoDo.TabIndex = 15;
            this.lblLoaiSoDo.Text = "Chọn sơ đồ ghế:";
            // 
            // cboLoaiSoDo
            // 
            this.cboLoaiSoDo.BackColor = System.Drawing.Color.Transparent;
            this.cboLoaiSoDo.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.cboLoaiSoDo.BorderRadius = 6;
            this.cboLoaiSoDo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLoaiSoDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiSoDo.FocusedColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboLoaiSoDo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboLoaiSoDo.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cboLoaiSoDo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboLoaiSoDo.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cboLoaiSoDo.ItemHeight = 30;
            this.cboLoaiSoDo.Items.AddRange(new object[] {
            "Sơ đồ 1 (Chữ T)",
            "Sơ đồ 2 (Hình chữ nhật)",
            "Sơ đồ 3 (Lưới)"});
            this.cboLoaiSoDo.Location = new System.Drawing.Point(24, 472);
            this.cboLoaiSoDo.Name = "cboLoaiSoDo";
            this.cboLoaiSoDo.Size = new System.Drawing.Size(512, 36);
            this.cboLoaiSoDo.TabIndex = 16;
            // 
            // btnLuu
            // 
            this.btnLuu.BorderRadius = 6;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(24, 534);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(248, 43);
            this.btnLuu.TabIndex = 17;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderRadius = 6;
            this.btnHuy.FillColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(288, 534);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(248, 43);
            this.btnHuy.TabIndex = 18;
            this.btnHuy.Text = "HỦY";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // FrmChiTietSuatDien
            // 
            this.AcceptButton = this.btnLuu;
            this.CancelButton = this.btnHuy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(560, 602);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.cboLoaiSoDo);
            this.Controls.Add(this.lblLoaiSoDo);
            this.Controls.Add(this.cboTrangThai);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.txtDiaDiem);
            this.Controls.Add(this.lblDiaDiem);
            this.Controls.Add(this.cboGioKetThuc);
            this.Controls.Add(this.lblGioKetThuc);
            this.Controls.Add(this.dtpKetThuc);
            this.Controls.Add(this.lblKetThuc);
            this.Controls.Add(this.cboGioBatDau);
            this.Controls.Add(this.lblGioBatDau);
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
        private System.Windows.Forms.Label lblGioBatDau;
        private Guna.UI2.WinForms.Guna2ComboBox cboGioBatDau;
        private System.Windows.Forms.Label lblKetThuc;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpKetThuc;
        private System.Windows.Forms.Label lblGioKetThuc;
        private Guna.UI2.WinForms.Guna2ComboBox cboGioKetThuc;
        private System.Windows.Forms.Label lblDiaDiem;
        private Guna.UI2.WinForms.Guna2TextBox txtDiaDiem;
        private System.Windows.Forms.Label lblTrangThai;
        private Guna.UI2.WinForms.Guna2ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblLoaiSoDo;
        private Guna.UI2.WinForms.Guna2ComboBox cboLoaiSoDo;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
    }
}

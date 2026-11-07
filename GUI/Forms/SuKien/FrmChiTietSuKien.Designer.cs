namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmChiTietSuKien
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
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblLoai = new System.Windows.Forms.Label();
            this.txtLoai = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new Guna.UI2.WinForms.Guna2TextBox();
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
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(24, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin Sự kiện";
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTen.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTen.Location = new System.Drawing.Point(24, 70);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(78, 17);
            this.lblTen.TabIndex = 1;
            this.lblTen.Text = "Tên sự kiện:";
            // 
            // txtTen
            // 
            this.txtTen.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.txtTen.BorderRadius = 6;
            this.txtTen.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTen.DefaultText = "";
            this.txtTen.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtTen.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTen.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtTen.Location = new System.Drawing.Point(24, 92);
            this.txtTen.Name = "txtTen";
            this.txtTen.PlaceholderText = "Nhập tên sự kiện...";
            this.txtTen.SelectedText = "";
            this.txtTen.Size = new System.Drawing.Size(472, 36);
            this.txtTen.TabIndex = 2;
            // 
            // lblLoai
            // 
            this.lblLoai.AutoSize = true;
            this.lblLoai.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblLoai.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblLoai.Location = new System.Drawing.Point(24, 142);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(55, 17);
            this.lblLoai.TabIndex = 3;
            this.lblLoai.Text = "Thể loại:";
            // 
            // txtLoai
            // 
            this.txtLoai.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.txtLoai.BorderRadius = 6;
            this.txtLoai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLoai.DefaultText = "";
            this.txtLoai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtLoai.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtLoai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLoai.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtLoai.Location = new System.Drawing.Point(24, 164);
            this.txtLoai.Name = "txtLoai";
            this.txtLoai.PlaceholderText = "Nhập thể loại...";
            this.txtLoai.SelectedText = "";
            this.txtLoai.Size = new System.Drawing.Size(472, 36);
            this.txtLoai.TabIndex = 4;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblMoTa.Location = new System.Drawing.Point(24, 214);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(44, 17);
            this.lblMoTa.TabIndex = 5;
            this.lblMoTa.Text = "Mô tả:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.txtMoTa.BorderRadius = 6;
            this.txtMoTa.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMoTa.DefaultText = "";
            this.txtMoTa.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtMoTa.HoverState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMoTa.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtMoTa.Location = new System.Drawing.Point(24, 236);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.PlaceholderText = "Nhập mô tả sự kiện...";
            this.txtMoTa.SelectedText = "";
            this.txtMoTa.Size = new System.Drawing.Size(472, 64);
            this.txtMoTa.TabIndex = 6;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTrangThai.Location = new System.Drawing.Point(24, 314);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(70, 17);
            this.lblTrangThai.TabIndex = 7;
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
            this.cboTrangThai.Location = new System.Drawing.Point(24, 336);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(472, 36);
            this.cboTrangThai.TabIndex = 8;
            // 
            // btnLuu
            // 
            this.btnLuu.BorderRadius = 6;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(24, 396);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(228, 42);
            this.btnLuu.TabIndex = 9;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderRadius = 6;
            this.btnHuy.FillColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(268, 396);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(228, 42);
            this.btnHuy.TabIndex = 10;
            this.btnHuy.Text = "HỦY";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // FrmChiTietSuKien
            // 
            this.AcceptButton = this.btnLuu;
            this.CancelButton = this.btnHuy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 462);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.cboTrangThai);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.txtMoTa);
            this.Controls.Add(this.lblMoTa);
            this.Controls.Add(this.txtLoai);
            this.Controls.Add(this.lblLoai);
            this.Controls.Add(this.txtTen);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChiTietSuKien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết Sự kiện";
            this.Load += new System.EventHandler(this.FrmChiTietSuKien_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTen;
        private Guna.UI2.WinForms.Guna2TextBox txtTen;
        private System.Windows.Forms.Label lblLoai;
        private Guna.UI2.WinForms.Guna2TextBox txtLoai;
        private System.Windows.Forms.Label lblMoTa;
        private Guna.UI2.WinForms.Guna2TextBox txtMoTa;
        private System.Windows.Forms.Label lblTrangThai;
        private Guna.UI2.WinForms.Guna2ComboBox cboTrangThai;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
    }
}

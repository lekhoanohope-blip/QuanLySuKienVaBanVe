namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmChiTietKhachHang
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblMaKH = new System.Windows.Forms.Label();
            this.lblTenKH = new System.Windows.Forms.Label();
            this.lblSDT = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhoneHint = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.txtMaKH = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTenKH = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtSDT = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();

            this.pnlFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFields.AutoScroll = true;
            this.pnlFields.AutoScrollMinSize = new System.Drawing.Size(0, 492);
            this.pnlFields.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblTitle, this.lblSubtitle,
                this.lblMaKH, this.txtMaKH, this.lblTenKH, this.txtTenKH, this.lblSDT, this.txtSDT,
                this.lblPhoneHint, this.lblEmail, this.txtEmail, this.lblError });
            this.lblTitle.Location = new System.Drawing.Point(24, 22);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Text = "Thêm khách hàng";
            this.lblSubtitle.Location = new System.Drawing.Point(24, 62);
            this.lblSubtitle.Size = new System.Drawing.Size(424, 24);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSubtitle.Text = "Thông tin có dấu * là bắt buộc · Ctrl+S để lưu.";
            this.lblMaKH.Location = new System.Drawing.Point(24, 102);
            this.lblMaKH.AutoSize = true;
            this.lblMaKH.Text = "Mã khách hàng";
            this.lblTenKH.Location = new System.Drawing.Point(24, 184);
            this.lblTenKH.AutoSize = true;
            this.lblTenKH.Text = "Họ và tên *";
            this.lblSDT.Location = new System.Drawing.Point(24, 266);
            this.lblSDT.AutoSize = true;
            this.lblSDT.Text = "Số điện thoại *";
            this.lblEmail.Location = new System.Drawing.Point(24, 370);
            this.lblEmail.AutoSize = true;
            this.lblEmail.Text = "Email (không bắt buộc)";
            this.txtMaKH.Name = "txtMaKH";
            this.txtMaKH.Location = new System.Drawing.Point(24, 128);
            this.txtMaKH.Size = new System.Drawing.Size(408, 40);
            this.txtMaKH.BorderRadius = 6;
            this.txtMaKH.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.txtMaKH.FillColor = System.Drawing.Color.FromArgb(243, 246, 249);
            this.txtMaKH.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtMaKH.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMaKH.ReadOnly = true;
            this.txtMaKH.TabStop = false;
            this.txtMaKH.AccessibleName = "Mã khách hàng tự động";
            this.txtTenKH.Name = "txtTenKH";
            this.txtTenKH.Location = new System.Drawing.Point(24, 210);
            this.txtTenKH.Size = new System.Drawing.Size(408, 40);
            this.txtTenKH.BorderRadius = 6;
            this.txtTenKH.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.txtTenKH.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtTenKH.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenKH.PlaceholderText = "Nhập họ và tên khách hàng";
            this.txtTenKH.PlaceholderForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtTenKH.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtTenKH.MaxLength = 100;
            this.txtTenKH.TabIndex = 0;
            this.txtTenKH.AccessibleName = "Họ và tên, bắt buộc";
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Location = new System.Drawing.Point(24, 292);
            this.txtSDT.Size = new System.Drawing.Size(408, 40);
            this.txtSDT.BorderRadius = 6;
            this.txtSDT.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.txtSDT.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSDT.PlaceholderText = "Nhập số điện thoại";
            this.txtSDT.PlaceholderForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtSDT.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtSDT.MaxLength = 10;
            this.txtSDT.TabIndex = 1;
            this.txtSDT.AccessibleName = "Số điện thoại, bắt buộc";
            this.lblPhoneHint.Location = new System.Drawing.Point(24, 338);
            this.lblPhoneHint.Size = new System.Drawing.Size(408, 22);
            this.lblPhoneHint.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblPhoneHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPhoneHint.Text = "10 chữ số, bắt đầu bằng 0. Mỗi số chỉ dùng cho một khách hàng.";
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Location = new System.Drawing.Point(24, 396);
            this.txtEmail.Size = new System.Drawing.Size(408, 40);
            this.txtEmail.BorderRadius = 6;
            this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.txtEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.PlaceholderText = "ten@domain.com";
            this.txtEmail.PlaceholderForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.TabIndex = 2;
            this.txtEmail.AccessibleName = "Email, không bắt buộc";
            this.lblError.Location = new System.Drawing.Point(24, 446);
            this.lblError.Size = new System.Drawing.Size(408, 54);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(185, 28, 28);
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Height = 76;
            this.pnlActions.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlActions.WrapContents = false;
            this.pnlActions.BackColor = System.Drawing.Color.FromArgb(243, 246, 249);
            this.pnlActions.Controls.Add(this.btnLuu);
            this.pnlActions.Controls.Add(this.btnHuy);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Text = "Thêm khách hàng";
            this.btnLuu.Size = new System.Drawing.Size(176, 40);
            this.btnLuu.BorderRadius = 6;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnLuu.HoverState.FillColor = System.Drawing.Color.FromArgb(5, 150, 105);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.AccessibleDescription = "Lưu khách hàng, phím tắt Ctrl+S";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Size = new System.Drawing.Size(100, 40);
            this.btnHuy.BorderRadius = 6;
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            this.errorProvider.ContainerControl = this;
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;

            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(464, 588);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.BackColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Controls.Add(this.pnlFields);
            this.Controls.Add(this.pnlActions);
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmChiTietKhachHang_KeyDown);
            this.pnlFields.TabIndex = 0;
            this.pnlActions.TabIndex = 1;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlFields;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblMaKH;
        private System.Windows.Forms.Label lblTenKH;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhoneHint;
        private System.Windows.Forms.Label lblError;
        private Guna.UI2.WinForms.Guna2TextBox txtMaKH;
        private Guna.UI2.WinForms.Guna2TextBox txtTenKH;
        private Guna.UI2.WinForms.Guna2TextBox txtSDT;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}

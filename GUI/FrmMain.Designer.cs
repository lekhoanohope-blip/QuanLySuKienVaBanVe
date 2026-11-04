namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pnlSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnDonVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnTaiKhoan = new Guna.UI2.WinForms.Guna2Button();
            this.btnBaoCao = new Guna.UI2.WinForms.Guna2Button();
            this.btnNhatKy = new Guna.UI2.WinForms.Guna2Button();
            this.btnDangXuat = new Guna.UI2.WinForms.Guna2Button();
            this.btnCaiDat = new Guna.UI2.WinForms.Guna2Button();
            this.btnKhuVuc = new Guna.UI2.WinForms.Guna2Button();
            this.btnBanVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnKhachHang = new Guna.UI2.WinForms.Guna2Button();
            this.btnSuatDien = new Guna.UI2.WinForms.Guna2Button();
            this.btnSuKien = new Guna.UI2.WinForms.Guna2Button();
            this.pnlLogo = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblLogo = new System.Windows.Forms.Label();
            this.picCrowd = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.controlBoxClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.controlBoxMaximize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.controlBoxMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.ptbAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.btnThongBao = new Guna.UI2.WinForms.Guna2Button();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.resizeForm = new Guna.UI2.WinForms.Guna2ResizeForm(this.components);
            this.resizeBox = new Guna.UI2.WinForms.Guna2ResizeBox();
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCrowd)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.pnlSidebar.Controls.Add(this.btnDonVe);
            this.pnlSidebar.Controls.Add(this.btnTaiKhoan);
            this.pnlSidebar.Controls.Add(this.btnBaoCao);
            this.pnlSidebar.Controls.Add(this.btnNhatKy);
            this.pnlSidebar.Controls.Add(this.btnDangXuat);
            this.pnlSidebar.Controls.Add(this.btnCaiDat);
            this.pnlSidebar.Controls.Add(this.btnKhuVuc);
            this.pnlSidebar.Controls.Add(this.btnBanVe);
            this.pnlSidebar.Controls.Add(this.btnKhachHang);
            this.pnlSidebar.Controls.Add(this.btnSuatDien);
            this.pnlSidebar.Controls.Add(this.btnSuKien);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Controls.Add(this.picCrowd);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(312, 1000);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnDonVe
            // 
            this.btnDonVe.FillColor = System.Drawing.Color.Transparent;
            this.btnDonVe.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDonVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnDonVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnDonVe.Location = new System.Drawing.Point(0, 625);
            this.btnDonVe.Margin = new System.Windows.Forms.Padding(4);
            this.btnDonVe.Name = "btnDonVe";
            this.btnDonVe.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnDonVe.Size = new System.Drawing.Size(312, 62);
            this.btnDonVe.TabIndex = 10;
            this.btnDonVe.Text = "Đơn vé ";
            this.btnDonVe.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnDonVe.Click += new System.EventHandler(this.btnDonVe_Click);
            // 
            // btnTaiKhoan
            // 
            this.btnTaiKhoan.FillColor = System.Drawing.Color.Transparent;
            this.btnTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnTaiKhoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnTaiKhoan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnTaiKhoan.Location = new System.Drawing.Point(0, 475);
            this.btnTaiKhoan.Margin = new System.Windows.Forms.Padding(4);
            this.btnTaiKhoan.Name = "btnTaiKhoan";
            this.btnTaiKhoan.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnTaiKhoan.Size = new System.Drawing.Size(312, 62);
            this.btnTaiKhoan.TabIndex = 9;
            this.btnTaiKhoan.Text = "Quản lý Tài khoản";
            this.btnTaiKhoan.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTaiKhoan.Click += new System.EventHandler(this.btnTaiKhoan_Click);
            // 
            // btnBaoCao
            // 
            this.btnBaoCao.FillColor = System.Drawing.Color.Transparent;
            this.btnBaoCao.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnBaoCao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBaoCao.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnBaoCao.Location = new System.Drawing.Point(0, 550);
            this.btnBaoCao.Margin = new System.Windows.Forms.Padding(4);
            this.btnBaoCao.Name = "btnBaoCao";
            this.btnBaoCao.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnBaoCao.Size = new System.Drawing.Size(312, 62);
            this.btnBaoCao.TabIndex = 8;
            this.btnBaoCao.Text = "Báo cáo && Thống kê";
            this.btnBaoCao.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnBaoCao.Click += new System.EventHandler(this.btnBaoCao_Click);
            // 
            // btnNhatKy
            // 
            this.btnNhatKy.FillColor = System.Drawing.Color.Transparent;
            this.btnNhatKy.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnNhatKy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnNhatKy.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnNhatKy.Location = new System.Drawing.Point(0, 700);
            this.btnNhatKy.Margin = new System.Windows.Forms.Padding(4);
            this.btnNhatKy.Name = "btnNhatKy";
            this.btnNhatKy.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNhatKy.Size = new System.Drawing.Size(312, 62);
            this.btnNhatKy.TabIndex = 11;
            this.btnNhatKy.Text = "Nhật ký";
            this.btnNhatKy.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNhatKy.Click += new System.EventHandler(this.btnNhatKy_Click);
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDangXuat.FillColor = System.Drawing.Color.Transparent;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDangXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnDangXuat.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnDangXuat.Location = new System.Drawing.Point(0, 674);
            this.btnDangXuat.Margin = new System.Windows.Forms.Padding(4);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnDangXuat.Size = new System.Drawing.Size(312, 88);
            this.btnDangXuat.TabIndex = 7;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnCaiDat
            // 
            this.btnCaiDat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCaiDat.FillColor = System.Drawing.Color.Transparent;
            this.btnCaiDat.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCaiDat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnCaiDat.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnCaiDat.Location = new System.Drawing.Point(0, 762);
            this.btnCaiDat.Margin = new System.Windows.Forms.Padding(4);
            this.btnCaiDat.Name = "btnCaiDat";
            this.btnCaiDat.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnCaiDat.Size = new System.Drawing.Size(312, 88);
            this.btnCaiDat.TabIndex = 6;
            this.btnCaiDat.Text = "Cài đặt";
            this.btnCaiDat.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnKhuVuc
            // 
            this.btnKhuVuc.FillColor = System.Drawing.Color.Transparent;
            this.btnKhuVuc.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnKhuVuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnKhuVuc.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnKhuVuc.Location = new System.Drawing.Point(0, 475);
            this.btnKhuVuc.Margin = new System.Windows.Forms.Padding(4);
            this.btnKhuVuc.Name = "btnKhuVuc";
            this.btnKhuVuc.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnKhuVuc.Size = new System.Drawing.Size(312, 62);
            this.btnKhuVuc.TabIndex = 8;
            this.btnKhuVuc.Text = "Khu vực && Giá vé";
            this.btnKhuVuc.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnKhuVuc.Click += new System.EventHandler(this.btnKhuVuc_Click);
            // 
            // btnBanVe
            // 
            this.btnBanVe.FillColor = System.Drawing.Color.Transparent;
            this.btnBanVe.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnBanVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBanVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnBanVe.Location = new System.Drawing.Point(0, 550);
            this.btnBanVe.Margin = new System.Windows.Forms.Padding(4);
            this.btnBanVe.Name = "btnBanVe";
            this.btnBanVe.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnBanVe.Size = new System.Drawing.Size(312, 62);
            this.btnBanVe.TabIndex = 5;
            this.btnBanVe.Text = "Bán vé";
            this.btnBanVe.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnBanVe.Click += new System.EventHandler(this.btnBanVe_Click);
            // 
            // btnKhachHang
            // 
            this.btnKhachHang.FillColor = System.Drawing.Color.Transparent;
            this.btnKhachHang.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnKhachHang.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnKhachHang.Location = new System.Drawing.Point(0, 400);
            this.btnKhachHang.Margin = new System.Windows.Forms.Padding(4);
            this.btnKhachHang.Name = "btnKhachHang";
            this.btnKhachHang.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnKhachHang.Size = new System.Drawing.Size(312, 62);
            this.btnKhachHang.TabIndex = 4;
            this.btnKhachHang.Text = "Khách hàng";
            this.btnKhachHang.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnKhachHang.Click += new System.EventHandler(this.btnKhachHang_Click);
            // 
            // btnSuatDien
            // 
            this.btnSuatDien.FillColor = System.Drawing.Color.Transparent;
            this.btnSuatDien.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnSuatDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnSuatDien.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnSuatDien.Location = new System.Drawing.Point(0, 325);
            this.btnSuatDien.Margin = new System.Windows.Forms.Padding(4);
            this.btnSuatDien.Name = "btnSuatDien";
            this.btnSuatDien.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnSuatDien.Size = new System.Drawing.Size(312, 62);
            this.btnSuatDien.TabIndex = 3;
            this.btnSuatDien.Text = "Suất diễn && Khu vực";
            this.btnSuatDien.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSuatDien.Click += new System.EventHandler(this.btnSuatDien_Click);
            // 
            // btnSuKien
            // 
            this.btnSuKien.FillColor = System.Drawing.Color.Transparent;
            this.btnSuKien.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnSuKien.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.btnSuKien.Location = new System.Drawing.Point(0, 250);
            this.btnSuKien.Margin = new System.Windows.Forms.Padding(4);
            this.btnSuKien.Name = "btnSuKien";
            this.btnSuKien.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnSuKien.Size = new System.Drawing.Size(312, 62);
            this.btnSuKien.TabIndex = 2;
            this.btnSuKien.Text = "Quản lý Sự kiện";
            this.btnSuKien.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSuKien.Click += new System.EventHandler(this.btnSuKien_Click);
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.pnlLogo.Controls.Add(this.lblSubtitle);
            this.pnlLogo.Controls.Add(this.picLogo);
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(312, 150);
            this.pnlLogo.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(197)))), ((int)(((byte)(193)))));
            this.lblSubtitle.Location = new System.Drawing.Point(72, 68);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(176, 20);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "Quản lý sự kiện và bán vé";
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(20, 38);
            this.picLogo.Margin = new System.Windows.Forms.Padding(4);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(44, 44);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(72, 40);
            this.lblLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(182, 41);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "EventTicket";
            // 
            // picCrowd
            // 
            this.picCrowd.Dock = System.Windows.Forms.DockStyle.None;
            this.picCrowd.Image = ((System.Drawing.Image)(resources.GetObject("picCrowd.Image")));
            this.picCrowd.Location = new System.Drawing.Point(0, 850);
            this.picCrowd.Margin = new System.Windows.Forms.Padding(4);
            this.picCrowd.Name = "picCrowd";
            this.picCrowd.Size = new System.Drawing.Size(312, 150);
            this.picCrowd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCrowd.TabIndex = 11;
            this.picCrowd.TabStop = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(239)))));
            this.pnlHeader.Controls.Add(this.controlBoxClose);
            this.pnlHeader.Controls.Add(this.controlBoxMaximize);
            this.pnlHeader.Controls.Add(this.controlBoxMinimize);
            this.pnlHeader.Controls.Add(this.lblUser);
            this.pnlHeader.Controls.Add(this.ptbAvatar);
            this.pnlHeader.Controls.Add(this.btnThongBao);
            this.pnlHeader.Controls.Add(this.txtTimKiem);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(312, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1438, 88);
            this.pnlHeader.TabIndex = 1;
            // 
            // controlBoxClose
            // 
            this.controlBoxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBoxClose.BorderRadius = 4;
            this.controlBoxClose.FillColor = System.Drawing.Color.Transparent;
            this.controlBoxClose.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.controlBoxClose.HoverState.IconColor = System.Drawing.Color.White;
            this.controlBoxClose.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.controlBoxClose.Location = new System.Drawing.Point(1369, 19);
            this.controlBoxClose.Margin = new System.Windows.Forms.Padding(4);
            this.controlBoxClose.Name = "controlBoxClose";
            this.controlBoxClose.Size = new System.Drawing.Size(52, 44);
            this.controlBoxClose.TabIndex = 6;
            // 
            // controlBoxMaximize
            // 
            this.controlBoxMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBoxMaximize.BorderRadius = 4;
            this.controlBoxMaximize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.controlBoxMaximize.FillColor = System.Drawing.Color.Transparent;
            this.controlBoxMaximize.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.controlBoxMaximize.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.controlBoxMaximize.Location = new System.Drawing.Point(1306, 19);
            this.controlBoxMaximize.Margin = new System.Windows.Forms.Padding(4);
            this.controlBoxMaximize.Name = "controlBoxMaximize";
            this.controlBoxMaximize.Size = new System.Drawing.Size(52, 44);
            this.controlBoxMaximize.TabIndex = 5;
            // 
            // controlBoxMinimize
            // 
            this.controlBoxMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBoxMinimize.BorderRadius = 4;
            this.controlBoxMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.controlBoxMinimize.FillColor = System.Drawing.Color.Transparent;
            this.controlBoxMinimize.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.controlBoxMinimize.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.controlBoxMinimize.Location = new System.Drawing.Point(1244, 19);
            this.controlBoxMinimize.Margin = new System.Windows.Forms.Padding(4);
            this.controlBoxMinimize.Name = "controlBoxMinimize";
            this.controlBoxMinimize.Size = new System.Drawing.Size(52, 44);
            this.controlBoxMinimize.TabIndex = 4;
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUser.Location = new System.Drawing.Point(1081, 31);
            this.lblUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(123, 23);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "Nguyễn Văn A";
            // 
            // ptbAvatar
            // 
            this.ptbAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ptbAvatar.ImageRotate = 0F;
            this.ptbAvatar.Location = new System.Drawing.Point(1012, 15);
            this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
            this.ptbAvatar.Name = "ptbAvatar";
            this.ptbAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ptbAvatar.Size = new System.Drawing.Size(56, 56);
            this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbAvatar.TabIndex = 2;
            this.ptbAvatar.TabStop = false;
            // 
            // btnThongBao
            // 
            this.btnThongBao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThongBao.BorderRadius = 20;
            this.btnThongBao.FillColor = System.Drawing.Color.White;
            this.btnThongBao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnThongBao.ForeColor = System.Drawing.Color.DimGray;
            this.btnThongBao.Location = new System.Drawing.Point(938, 19);
            this.btnThongBao.Margin = new System.Windows.Forms.Padding(4);
            this.btnThongBao.Name = "btnThongBao";
            this.btnThongBao.Size = new System.Drawing.Size(50, 50);
            this.btnThongBao.TabIndex = 1;
            this.btnThongBao.Text = "🔔";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.txtTimKiem.BorderRadius = 20;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.Location = new System.Drawing.Point(25, 19);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "🔍 Tìm kiếm...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(562, 50);
            this.txtTimKiem.TabIndex = 0;
            // 
            // resizeForm
            // 
            this.resizeForm.TargetForm = this;
            // 
            // resizeBox
            // 
            this.resizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resizeBox.BackColor = System.Drawing.Color.Transparent;
            this.resizeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.resizeBox.Location = new System.Drawing.Point(1720, 970);
            this.resizeBox.Margin = new System.Windows.Forms.Padding(4);
            this.resizeBox.Name = "resizeBox";
            this.resizeBox.Size = new System.Drawing.Size(25, 25);
            this.resizeBox.TabIndex = 99;
            this.resizeBox.TargetControl = this;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(312, 88);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1438, 912);
            this.pnlContent.TabIndex = 2;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1750, 1000);
            this.Controls.Add(this.resizeBox);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1375, 875);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventTicket";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCrowd)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private Guna.UI2.WinForms.Guna2Panel pnlLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.PictureBox picCrowd;
        private Guna.UI2.WinForms.Guna2Button btnSuKien;
        private Guna.UI2.WinForms.Guna2Button btnSuatDien;
        private Guna.UI2.WinForms.Guna2Button btnKhachHang;
        private Guna.UI2.WinForms.Guna2Button btnKhuVuc;
        private Guna.UI2.WinForms.Guna2Button btnBanVe;
        private Guna.UI2.WinForms.Guna2Button btnCaiDat;
        private Guna.UI2.WinForms.Guna2Button btnDangXuat;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnThongBao;
        private Guna.UI2.WinForms.Guna2CirclePictureBox ptbAvatar;
        private System.Windows.Forms.Label lblUser;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private Guna.UI2.WinForms.Guna2ControlBox controlBoxClose;
        private Guna.UI2.WinForms.Guna2ControlBox controlBoxMaximize;
        private Guna.UI2.WinForms.Guna2ControlBox controlBoxMinimize;
        private Guna.UI2.WinForms.Guna2ResizeForm resizeForm;
        private Guna.UI2.WinForms.Guna2ResizeBox resizeBox;
        private Guna.UI2.WinForms.Guna2Button btnDonVe;
        private Guna.UI2.WinForms.Guna2Button btnTaiKhoan;
        private Guna.UI2.WinForms.Guna2Button btnBaoCao;
        private Guna.UI2.WinForms.Guna2Button btnNhatKy;
    }
}
namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmDoiVe
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.grpThongTinCu = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lblDonGoc = new System.Windows.Forms.Label();
            this.lblKhachHang = new System.Windows.Forms.Label();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.lblSuatCu = new System.Windows.Forms.Label();
            this.lblVeCu = new System.Windows.Forms.Label();
            this.lblGiaCu = new System.Windows.Forms.Label();
            this.grpChonMoi = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lblChonSuat = new System.Windows.Forms.Label();
            this.cboSuatMoi = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblChonKhuVuc = new System.Windows.Forms.Label();
            this.cboKhuVucMoi = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pnlSeatSelectorHost = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvGheTrong = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaGhe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaGheHienThi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHangVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPlaceholderNote = new System.Windows.Forms.Label();
            this.pnlChenhLech = new Guna.UI2.WinForms.Guna2Panel();
            this.lblPhuongThuc = new System.Windows.Forms.Label();
            this.cboPhuongThuc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblGiaCuHienThi = new System.Windows.Forms.Label();
            this.lblGiaMoiHienThi = new System.Windows.Forms.Label();
            this.lblKetQuaChenhLech = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnXacNhan = new Guna.UI2.WinForms.Guna2Button();
            this.btnDong = new Guna.UI2.WinForms.Guna2Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlTop.SuspendLayout();
            this.grpThongTinCu.SuspendLayout();
            this.grpChonMoi.SuspendLayout();
            this.pnlSeatSelectorHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGheTrong)).BeginInit();
            this.pnlChenhLech.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.lblSubtitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(20, 16);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(684, 60);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(252, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Đổi Vé Theo Quy Định";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSubtitle.Location = new System.Drawing.Point(2, 34);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(515, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Áp dụng trước giờ diễn ít nhất 12 giờ · Chỉ đổi sang suất của cùng sự kiện · Lưu vết mã đơn gốc";
            // 
            // grpThongTinCu
            // 
            this.grpThongTinCu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.grpThongTinCu.BorderRadius = 8;
            this.grpThongTinCu.Controls.Add(this.lblDonGoc);
            this.grpThongTinCu.Controls.Add(this.lblKhachHang);
            this.grpThongTinCu.Controls.Add(this.lblSuKien);
            this.grpThongTinCu.Controls.Add(this.lblSuatCu);
            this.grpThongTinCu.Controls.Add(this.lblVeCu);
            this.grpThongTinCu.Controls.Add(this.lblGiaCu);
            this.grpThongTinCu.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.grpThongTinCu.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTinCu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpThongTinCu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpThongTinCu.Location = new System.Drawing.Point(20, 76);
            this.grpThongTinCu.Name = "grpThongTinCu";
            this.grpThongTinCu.Size = new System.Drawing.Size(684, 116);
            this.grpThongTinCu.TabIndex = 1;
            this.grpThongTinCu.Text = "1. Thông tin vé gốc cần đổi";
            // 
            // lblDonGoc
            // 
            this.lblDonGoc.AutoSize = true;
            this.lblDonGoc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDonGoc.Location = new System.Drawing.Point(16, 48);
            this.lblDonGoc.Name = "lblDonGoc";
            this.lblDonGoc.Size = new System.Drawing.Size(89, 15);
            this.lblDonGoc.TabIndex = 0;
            this.lblDonGoc.Text = "Đơn gốc: #0";
            // 
            // lblKhachHang
            // 
            this.lblKhachHang.AutoSize = true;
            this.lblKhachHang.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKhachHang.Location = new System.Drawing.Point(180, 48);
            this.lblKhachHang.Name = "lblKhachHang";
            this.lblKhachHang.Size = new System.Drawing.Size(107, 15);
            this.lblKhachHang.TabIndex = 1;
            this.lblKhachHang.Text = "Khách hàng: —";
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSuKien.Location = new System.Drawing.Point(400, 48);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(68, 15);
            this.lblSuKien.TabIndex = 2;
            this.lblSuKien.Text = "Sự kiện: —";
            // 
            // lblSuatCu
            // 
            this.lblSuatCu.AutoSize = true;
            this.lblSuatCu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSuatCu.Location = new System.Drawing.Point(16, 80);
            this.lblSuatCu.Name = "lblSuatCu";
            this.lblSuatCu.Size = new System.Drawing.Size(87, 15);
            this.lblSuatCu.TabIndex = 3;
            this.lblSuatCu.Text = "Suất diễn: —";
            // 
            // lblVeCu
            // 
            this.lblVeCu.AutoSize = true;
            this.lblVeCu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblVeCu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.lblVeCu.Location = new System.Drawing.Point(260, 80);
            this.lblVeCu.Name = "lblVeCu";
            this.lblVeCu.Size = new System.Drawing.Size(63, 15);
            this.lblVeCu.TabIndex = 4;
            this.lblVeCu.Text = "Ghế cũ: —";
            // 
            // lblGiaCu
            // 
            this.lblGiaCu.AutoSize = true;
            this.lblGiaCu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGiaCu.Location = new System.Drawing.Point(480, 80);
            this.lblGiaCu.Name = "lblGiaCu";
            this.lblGiaCu.Size = new System.Drawing.Size(107, 15);
            this.lblGiaCu.TabIndex = 5;
            this.lblGiaCu.Text = "Giá vé cũ: 0 VNĐ";
            // 
            // grpChonMoi
            // 
            this.grpChonMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.grpChonMoi.BorderRadius = 8;
            this.grpChonMoi.Controls.Add(this.lblChonSuat);
            this.grpChonMoi.Controls.Add(this.cboSuatMoi);
            this.grpChonMoi.Controls.Add(this.lblChonKhuVuc);
            this.grpChonMoi.Controls.Add(this.cboKhuVucMoi);
            this.grpChonMoi.Controls.Add(this.pnlSeatSelectorHost);
            this.grpChonMoi.Controls.Add(this.lblPlaceholderNote);
            this.grpChonMoi.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.grpChonMoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChonMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpChonMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpChonMoi.Location = new System.Drawing.Point(20, 192);
            this.grpChonMoi.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.grpChonMoi.Name = "grpChonMoi";
            this.grpChonMoi.Size = new System.Drawing.Size(684, 308);
            this.grpChonMoi.TabIndex = 2;
            this.grpChonMoi.Text = "2. Chọn suất diễn & Vị trí ghế mới mong muốn";
            // 
            // lblChonSuat
            // 
            this.lblChonSuat.AutoSize = true;
            this.lblChonSuat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChonSuat.Location = new System.Drawing.Point(16, 48);
            this.lblChonSuat.Name = "lblChonSuat";
            this.lblChonSuat.Size = new System.Drawing.Size(110, 15);
            this.lblChonSuat.TabIndex = 0;
            this.lblChonSuat.Text = "Suất diễn đích:";
            // 
            // cboSuatMoi
            // 
            this.cboSuatMoi.BackColor = System.Drawing.Color.Transparent;
            this.cboSuatMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.cboSuatMoi.BorderRadius = 6;
            this.cboSuatMoi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuatMoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuatMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboSuatMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboSuatMoi.ItemHeight = 30;
            this.cboSuatMoi.Location = new System.Drawing.Point(135, 42);
            this.cboSuatMoi.Name = "cboSuatMoi";
            this.cboSuatMoi.Size = new System.Drawing.Size(320, 36);
            this.cboSuatMoi.TabIndex = 1;
            this.cboSuatMoi.SelectedIndexChanged += new System.EventHandler(this.cboSuatMoi_SelectedIndexChanged);
            // 
            // lblChonKhuVuc
            // 
            this.lblChonKhuVuc.AutoSize = true;
            this.lblChonKhuVuc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChonKhuVuc.Location = new System.Drawing.Point(470, 48);
            this.lblChonKhuVuc.Name = "lblChonKhuVuc";
            this.lblChonKhuVuc.Size = new System.Drawing.Size(53, 15);
            this.lblChonKhuVuc.TabIndex = 2;
            this.lblChonKhuVuc.Text = "Khu vực:";
            // 
            // cboKhuVucMoi
            // 
            this.cboKhuVucMoi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboKhuVucMoi.BackColor = System.Drawing.Color.Transparent;
            this.cboKhuVucMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.cboKhuVucMoi.BorderRadius = 6;
            this.cboKhuVucMoi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboKhuVucMoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhuVucMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboKhuVucMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboKhuVucMoi.ItemHeight = 30;
            this.cboKhuVucMoi.Location = new System.Drawing.Point(530, 42);
            this.cboKhuVucMoi.Name = "cboKhuVucMoi";
            this.cboKhuVucMoi.Size = new System.Drawing.Size(138, 36);
            this.cboKhuVucMoi.TabIndex = 3;
            this.cboKhuVucMoi.SelectedIndexChanged += new System.EventHandler(this.cboKhuVucMoi_SelectedIndexChanged);
            // 
            // pnlSeatSelectorHost
            // 
            this.pnlSeatSelectorHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSeatSelectorHost.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSeatSelectorHost.BorderRadius = 6;
            this.pnlSeatSelectorHost.BorderThickness = 1;
            this.pnlSeatSelectorHost.Controls.Add(this.dgvGheTrong);
            this.pnlSeatSelectorHost.Location = new System.Drawing.Point(16, 88);
            this.pnlSeatSelectorHost.Name = "pnlSeatSelectorHost";
            this.pnlSeatSelectorHost.Size = new System.Drawing.Size(652, 192);
            this.pnlSeatSelectorHost.TabIndex = 4;
            // 
            // dgvGheTrong
            // 
            this.dgvGheTrong.AllowUserToAddRows = false;
            this.dgvGheTrong.AllowUserToDeleteRows = false;
            this.dgvGheTrong.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.dgvGheTrong.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGheTrong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGheTrong.BackgroundColor = System.Drawing.Color.White;
            this.dgvGheTrong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGheTrong.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvGheTrong.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGheTrong.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGheTrong.ColumnHeadersHeight = 36;
            this.dgvGheTrong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGheTrong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaGhe,
            this.colMaGheHienThi,
            this.colTenKV,
            this.colHangVe,
            this.colGiaVe});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGheTrong.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGheTrong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGheTrong.AutoGenerateColumns = false;
            this.dgvGheTrong.EnableHeadersVisualStyles = false;
            this.dgvGheTrong.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvGheTrong.Location = new System.Drawing.Point(0, 0);
            this.dgvGheTrong.MultiSelect = false;
            this.dgvGheTrong.Name = "dgvGheTrong";
            this.dgvGheTrong.ReadOnly = true;
            this.dgvGheTrong.RowHeadersVisible = false;
            this.dgvGheTrong.RowTemplate.Height = 40;
            this.dgvGheTrong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGheTrong.Size = new System.Drawing.Size(652, 192);
            this.dgvGheTrong.TabIndex = 0;
            this.dgvGheTrong.SelectionChanged += new System.EventHandler(this.dgvGheTrong_SelectionChanged);
            this.dgvGheTrong.CurrentCellChanged += new System.EventHandler(this.dgvGheTrong_CurrentCellChanged);
            this.dgvGheTrong.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvGheTrong_MouseDown);
            // 
            // colMaGhe
            // 
            this.colMaGhe.DataPropertyName = "MaGhe";
            this.colMaGhe.HeaderText = "Mã";
            this.colMaGhe.Name = "colMaGhe";
            this.colMaGhe.ReadOnly = true;
            this.colMaGhe.Visible = false;
            // 
            // colMaGheHienThi
            // 
            this.colMaGheHienThi.DataPropertyName = "MaGheHienThi";
            this.colMaGheHienThi.FillWeight = 45F;
            this.colMaGheHienThi.HeaderText = "Vị trí ghế";
            this.colMaGheHienThi.MinimumWidth = 80;
            this.colMaGheHienThi.Name = "colMaGheHienThi";
            this.colMaGheHienThi.ReadOnly = true;
            // 
            // colTenKV
            // 
            this.colTenKV.DataPropertyName = "TenKV";
            this.colTenKV.FillWeight = 80F;
            this.colTenKV.HeaderText = "Khu vực";
            this.colTenKV.MinimumWidth = 140;
            this.colTenKV.Name = "colTenKV";
            this.colTenKV.ReadOnly = true;
            // 
            // colHangVe
            // 
            this.colHangVe.DataPropertyName = "HangVe";
            this.colHangVe.FillWeight = 50F;
            this.colHangVe.HeaderText = "Hạng vé";
            this.colHangVe.MinimumWidth = 90;
            this.colHangVe.Name = "colHangVe";
            this.colHangVe.ReadOnly = true;
            // 
            // colGiaVe
            // 
            this.colGiaVe.DataPropertyName = "GiaVe";
            this.colGiaVe.FillWeight = 60F;
            this.colGiaVe.HeaderText = "Giá vé";
            this.colGiaVe.MinimumWidth = 100;
            this.colGiaVe.Name = "colGiaVe";
            this.colGiaVe.ReadOnly = true;
            // 
            // lblPlaceholderNote
            // 
            this.lblPlaceholderNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlaceholderNote.AutoSize = true;
            this.lblPlaceholderNote.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblPlaceholderNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblPlaceholderNote.Location = new System.Drawing.Point(16, 286);
            this.lblPlaceholderNote.Name = "lblPlaceholderNote";
            this.lblPlaceholderNote.Size = new System.Drawing.Size(0, 13);
            this.lblPlaceholderNote.TabIndex = 5;
            this.lblPlaceholderNote.Text = "";
            this.lblPlaceholderNote.Visible = false;
            // 
            // pnlChenhLech
            // 
            this.pnlChenhLech.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlChenhLech.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlChenhLech.BorderRadius = 8;
            this.pnlChenhLech.BorderThickness = 1;
            this.pnlChenhLech.Controls.Add(this.lblPhuongThuc);
            this.pnlChenhLech.Controls.Add(this.cboPhuongThuc);
            this.pnlChenhLech.Controls.Add(this.lblGiaCuHienThi);
            this.pnlChenhLech.Controls.Add(this.lblGiaMoiHienThi);
            this.pnlChenhLech.Controls.Add(this.lblKetQuaChenhLech);
            this.pnlChenhLech.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlChenhLech.Location = new System.Drawing.Point(20, 494);
            this.pnlChenhLech.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlChenhLech.Name = "pnlChenhLech";
            this.pnlChenhLech.Size = new System.Drawing.Size(684, 96);
            this.pnlChenhLech.TabIndex = 3;
            // 
            // lblPhuongThuc
            // 
            this.lblPhuongThuc.AutoSize = true;
            this.lblPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPhuongThuc.Location = new System.Drawing.Point(16, 18);
            this.lblPhuongThuc.Name = "lblPhuongThuc";
            this.lblPhuongThuc.Size = new System.Drawing.Size(82, 15);
            this.lblPhuongThuc.TabIndex = 0;
            this.lblPhuongThuc.Text = "Phương thức:";
            // 
            // cboPhuongThuc
            // 
            this.cboPhuongThuc.BackColor = System.Drawing.Color.Transparent;
            this.cboPhuongThuc.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.cboPhuongThuc.BorderRadius = 6;
            this.cboPhuongThuc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboPhuongThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboPhuongThuc.ItemHeight = 26;
            this.cboPhuongThuc.Location = new System.Drawing.Point(16, 42);
            this.cboPhuongThuc.Name = "cboPhuongThuc";
            this.cboPhuongThuc.Size = new System.Drawing.Size(180, 32);
            this.cboPhuongThuc.TabIndex = 1;
            // 
            // lblGiaCuHienThi
            // 
            this.lblGiaCuHienThi.AutoSize = true;
            this.lblGiaCuHienThi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGiaCuHienThi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblGiaCuHienThi.Location = new System.Drawing.Point(220, 14);
            this.lblGiaCuHienThi.Name = "lblGiaCuHienThi";
            this.lblGiaCuHienThi.Size = new System.Drawing.Size(110, 17);
            this.lblGiaCuHienThi.TabIndex = 2;
            this.lblGiaCuHienThi.Text = "Giá vé cũ: 0 VNĐ";
            // 
            // lblGiaMoiHienThi
            // 
            this.lblGiaMoiHienThi.AutoSize = true;
            this.lblGiaMoiHienThi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGiaMoiHienThi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblGiaMoiHienThi.Location = new System.Drawing.Point(220, 38);
            this.lblGiaMoiHienThi.Name = "lblGiaMoiHienThi";
            this.lblGiaMoiHienThi.Size = new System.Drawing.Size(160, 17);
            this.lblGiaMoiHienThi.TabIndex = 3;
            this.lblGiaMoiHienThi.Text = "Giá vé mới: Chưa chọn ghế";
            // 
            // lblKetQuaChenhLech
            // 
            this.lblKetQuaChenhLech.AutoSize = true;
            this.lblKetQuaChenhLech.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblKetQuaChenhLech.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKetQuaChenhLech.Location = new System.Drawing.Point(220, 64);
            this.lblKetQuaChenhLech.Name = "lblKetQuaChenhLech";
            this.lblKetQuaChenhLech.Size = new System.Drawing.Size(262, 20);
            this.lblKetQuaChenhLech.TabIndex = 4;
            this.lblKetQuaChenhLech.Text = "Chênh lệch: Vui lòng chọn ghế mới";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnXacNhan);
            this.pnlActions.Controls.Add(this.btnDong);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlActions.Location = new System.Drawing.Point(20, 590);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlActions.Size = new System.Drawing.Size(684, 56);
            this.pnlActions.TabIndex = 4;
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BorderRadius = 6;
            this.btnXacNhan.Enabled = false;
            this.btnXacNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(128)))), ((int)(((byte)(61)))));
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(101)))), ((int)(((byte)(52)))));
            this.btnXacNhan.Location = new System.Drawing.Point(524, 13);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(160, 38);
            this.btnXacNhan.TabIndex = 0;
            this.btnXacNhan.Text = "Xác nhận đổi vé";
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // btnDong
            // 
            this.btnDong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnDong.BorderRadius = 6;
            this.btnDong.BorderThickness = 1;
            this.btnDong.FillColor = System.Drawing.Color.White;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnDong.Location = new System.Drawing.Point(418, 13);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(100, 38);
            this.btnDong.TabIndex = 1;
            this.btnDong.Text = "Hủy bỏ";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // FrmDoiVe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(724, 674);
            this.Controls.Add(this.grpChonMoi);
            this.Controls.Add(this.pnlChenhLech);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.grpThongTinCu);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDoiVe";
            this.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đổi vé theo quy định";
            this.Load += new System.EventHandler(this.FrmDoiVe_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpThongTinCu.ResumeLayout(false);
            this.grpThongTinCu.PerformLayout();
            this.grpChonMoi.ResumeLayout(false);
            this.grpChonMoi.PerformLayout();
            this.pnlSeatSelectorHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGheTrong)).EndInit();
            this.pnlChenhLech.ResumeLayout(false);
            this.pnlChenhLech.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private Guna.UI2.WinForms.Guna2GroupBox grpThongTinCu;
        private System.Windows.Forms.Label lblDonGoc;
        private System.Windows.Forms.Label lblKhachHang;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.Label lblSuatCu;
        private System.Windows.Forms.Label lblVeCu;
        private System.Windows.Forms.Label lblGiaCu;
        private Guna.UI2.WinForms.Guna2GroupBox grpChonMoi;
        private System.Windows.Forms.Label lblChonSuat;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuatMoi;
        private System.Windows.Forms.Label lblChonKhuVuc;
        private Guna.UI2.WinForms.Guna2ComboBox cboKhuVucMoi;
        private Guna.UI2.WinForms.Guna2Panel pnlSeatSelectorHost;
        private Guna.UI2.WinForms.Guna2DataGridView dgvGheTrong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaGhe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaGheHienThi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHangVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
        private System.Windows.Forms.Label lblPlaceholderNote;
        private Guna.UI2.WinForms.Guna2Panel pnlChenhLech;
        private System.Windows.Forms.Label lblPhuongThuc;
        private Guna.UI2.WinForms.Guna2ComboBox cboPhuongThuc;
        private System.Windows.Forms.Label lblGiaCuHienThi;
        private System.Windows.Forms.Label lblGiaMoiHienThi;
        private System.Windows.Forms.Label lblKetQuaChenhLech;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private Guna.UI2.WinForms.Guna2Button btnXacNhan;
        private Guna.UI2.WinForms.Guna2Button btnDong;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}

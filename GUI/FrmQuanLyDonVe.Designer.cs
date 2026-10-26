namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmQuanLyDonVe
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.cboLocTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnDoiVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuyVe = new Guna.UI2.WinForms.Guna2Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pnlMasterCard = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvDonVe = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaDonGoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenKH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThoiGianBatDau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayDat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlMasterHeader = new System.Windows.Forms.Panel();
            this.lblMasterTitle = new System.Windows.Forms.Label();
            this.pnlDetailCard = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvChiTietVe = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaCT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaVeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHangVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaGheHienThi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThaiVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetailHeader = new System.Windows.Forms.Panel();
            this.lblRuleBadge = new System.Windows.Forms.Label();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.TableLayoutPanel();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.lblSearchHint = new System.Windows.Forms.Label();
            this.searchTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTitle.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlMasterCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonVe)).BeginInit();
            this.pnlMasterHeader.SuspendLayout();
            this.pnlDetailCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietVe)).BeginInit();
            this.pnlDetailHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(24, 16);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(952, 44);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(325, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý Đơn vé & Hủy / Đổi";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.txtTimKiem);
            this.pnlHeader.Controls.Add(this.cboLocTrangThai);
            this.pnlHeader.Controls.Add(this.btnDoiVe);
            this.pnlHeader.Controls.Add(this.btnHuyVe);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(24, 60);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(952, 64);
            this.pnlHeader.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.txtTimKiem.BorderRadius = 8;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 14);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtTimKiem.PlaceholderText = "Tìm theo mã đơn (#10) hoặc số điện thoại…";
            this.txtTimKiem.Size = new System.Drawing.Size(320, 36);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.cboLocTrangThai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.cboLocTrangThai.BorderRadius = 6;
            this.cboLocTrangThai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboLocTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboLocTrangThai.ItemHeight = 30;
            this.cboLocTrangThai.Location = new System.Drawing.Point(330, 14);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(180, 36);
            this.cboLocTrangThai.TabIndex = 1;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.cboLocTrangThai_SelectedIndexChanged);
            // 
            // btnDoiVe
            // 
            this.btnDoiVe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDoiVe.BorderRadius = 6;
            this.btnDoiVe.Enabled = false;
            this.btnDoiVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnDoiVe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDoiVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnDoiVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(254)))));
            this.btnDoiVe.Location = new System.Drawing.Point(672, 14);
            this.btnDoiVe.Name = "btnDoiVe";
            this.btnDoiVe.Size = new System.Drawing.Size(130, 36);
            this.btnDoiVe.TabIndex = 2;
            this.btnDoiVe.Text = "Đổi vé (≥12h)";
            this.btnDoiVe.Click += new System.EventHandler(this.btnDoiVe_Click);
            // 
            // btnHuyVe
            // 
            this.btnHuyVe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuyVe.BorderRadius = 6;
            this.btnHuyVe.Enabled = false;
            this.btnHuyVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnHuyVe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuyVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnHuyVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.btnHuyVe.Location = new System.Drawing.Point(812, 14);
            this.btnHuyVe.Name = "btnHuyVe";
            this.btnHuyVe.Size = new System.Drawing.Size(140, 36);
            this.btnHuyVe.TabIndex = 3;
            this.btnHuyVe.Text = "Hủy đơn (≥24h)";
            this.btnHuyVe.Click += new System.EventHandler(this.btnHuyVe_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(24, 124);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnlMasterCard);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.pnlDetailCard);
            this.splitContainer.Size = new System.Drawing.Size(952, 470);
            this.splitContainer.SplitterDistance = 270;
            this.splitContainer.SplitterWidth = 6;
            this.splitContainer.TabIndex = 2;
            // 
            // pnlMasterCard
            // 
            this.pnlMasterCard.BackColor = System.Drawing.Color.White;
            this.pnlMasterCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlMasterCard.BorderRadius = 8;
            this.pnlMasterCard.BorderThickness = 1;
            this.pnlMasterCard.Controls.Add(this.dgvDonVe);
            this.pnlMasterCard.Controls.Add(this.pnlMasterHeader);
            this.pnlMasterCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMasterCard.Location = new System.Drawing.Point(0, 0);
            this.pnlMasterCard.Name = "pnlMasterCard";
            this.pnlMasterCard.Size = new System.Drawing.Size(952, 270);
            this.pnlMasterCard.TabIndex = 0;
            // 
            // dgvDonVe
            // 
            this.dgvDonVe.AllowUserToAddRows = false;
            this.dgvDonVe.AllowUserToDeleteRows = false;
            this.dgvDonVe.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.dgvDonVe.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDonVe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDonVe.BackgroundColor = System.Drawing.Color.White;
            this.dgvDonVe.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDonVe.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDonVe.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDonVe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDonVe.ColumnHeadersHeight = 40;
            this.dgvDonVe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDonVe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaDon,
            this.colMaDonGoc,
            this.colTenKH,
            this.colSDT,
            this.colTenSK,
            this.colThoiGianBatDau,
            this.colNgayDat,
            this.colTongTien,
            this.colTrangThai});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDonVe.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDonVe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDonVe.AutoGenerateColumns = false;
            this.dgvDonVe.EnableHeadersVisualStyles = false;
            this.dgvDonVe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvDonVe.Location = new System.Drawing.Point(0, 40);
            this.dgvDonVe.MultiSelect = false;
            this.dgvDonVe.Name = "dgvDonVe";
            this.dgvDonVe.ReadOnly = true;
            this.dgvDonVe.RowHeadersVisible = false;
            this.dgvDonVe.RowTemplate.Height = 44;
            this.dgvDonVe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDonVe.Size = new System.Drawing.Size(952, 230);
            this.dgvDonVe.TabIndex = 1;
            this.dgvDonVe.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDonVe_CellPainting);
            this.dgvDonVe.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDonVe_CellFormatting);
            this.dgvDonVe.SelectionChanged += new System.EventHandler(this.dgvDonVe_SelectionChanged);
            // 
            // colMaDon
            // 
            this.colMaDon.DataPropertyName = "MaDon";
            this.colMaDon.FillWeight = 45F;
            this.colMaDon.HeaderText = "Mã đơn";
            this.colMaDon.MinimumWidth = 75;
            this.colMaDon.Name = "colMaDon";
            this.colMaDon.ReadOnly = true;
            // 
            // colMaDonGoc
            // 
            this.colMaDonGoc.DataPropertyName = "MaDonGoc";
            this.colMaDonGoc.FillWeight = 45F;
            this.colMaDonGoc.HeaderText = "Đơn gốc";
            this.colMaDonGoc.MinimumWidth = 75;
            this.colMaDonGoc.Name = "colMaDonGoc";
            this.colMaDonGoc.ReadOnly = true;
            // 
            // colTenKH
            // 
            this.colTenKH.DataPropertyName = "TenKH";
            this.colTenKH.FillWeight = 85F;
            this.colTenKH.HeaderText = "Khách hàng";
            this.colTenKH.MinimumWidth = 140;
            this.colTenKH.Name = "colTenKH";
            this.colTenKH.ReadOnly = true;
            // 
            // colSDT
            // 
            this.colSDT.DataPropertyName = "SDT";
            this.colSDT.FillWeight = 60F;
            this.colSDT.HeaderText = "Số ĐT";
            this.colSDT.MinimumWidth = 100;
            this.colSDT.Name = "colSDT";
            this.colSDT.ReadOnly = true;
            // 
            // colTenSK
            // 
            this.colTenSK.DataPropertyName = "TenSK";
            this.colTenSK.FillWeight = 95F;
            this.colTenSK.HeaderText = "Sự kiện";
            this.colTenSK.MinimumWidth = 160;
            this.colTenSK.Name = "colTenSK";
            this.colTenSK.ReadOnly = true;
            // 
            // colThoiGianBatDau
            // 
            this.colThoiGianBatDau.DataPropertyName = "ThoiGianBatDau";
            this.colThoiGianBatDau.FillWeight = 80F;
            this.colThoiGianBatDau.HeaderText = "Giờ diễn";
            this.colThoiGianBatDau.MinimumWidth = 135;
            this.colThoiGianBatDau.Name = "colThoiGianBatDau";
            this.colThoiGianBatDau.ReadOnly = true;
            // 
            // colNgayDat
            // 
            this.colNgayDat.DataPropertyName = "NgayDat";
            this.colNgayDat.FillWeight = 80F;
            this.colNgayDat.HeaderText = "Ngày đặt";
            this.colNgayDat.MinimumWidth = 135;
            this.colNgayDat.Name = "colNgayDat";
            this.colNgayDat.ReadOnly = true;
            // 
            // colTongTien
            // 
            this.colTongTien.DataPropertyName = "TongTien";
            this.colTongTien.FillWeight = 60F;
            this.colTongTien.HeaderText = "Tổng tiền";
            this.colTongTien.MinimumWidth = 100;
            this.colTongTien.Name = "colTongTien";
            this.colTongTien.ReadOnly = true;
            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.FillWeight = 65F;
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.MinimumWidth = 110;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            // 
            // pnlMasterHeader
            // 
            this.pnlMasterHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlMasterHeader.Controls.Add(this.lblMasterTitle);
            this.pnlMasterHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMasterHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlMasterHeader.Name = "pnlMasterHeader";
            this.pnlMasterHeader.Size = new System.Drawing.Size(952, 40);
            this.pnlMasterHeader.TabIndex = 0;
            // 
            // lblMasterTitle
            // 
            this.lblMasterTitle.AutoSize = true;
            this.lblMasterTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMasterTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblMasterTitle.Location = new System.Drawing.Point(14, 11);
            this.lblMasterTitle.Name = "lblMasterTitle";
            this.lblMasterTitle.Size = new System.Drawing.Size(155, 19);
            this.lblMasterTitle.TabIndex = 0;
            this.lblMasterTitle.Text = "DANH SÁCH ĐƠN VÉ";
            // 
            // pnlDetailCard
            // 
            this.pnlDetailCard.BackColor = System.Drawing.Color.White;
            this.pnlDetailCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlDetailCard.BorderRadius = 8;
            this.pnlDetailCard.BorderThickness = 1;
            this.pnlDetailCard.Controls.Add(this.dgvChiTietVe);
            this.pnlDetailCard.Controls.Add(this.pnlDetailHeader);
            this.pnlDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailCard.Location = new System.Drawing.Point(0, 0);
            this.pnlDetailCard.Name = "pnlDetailCard";
            this.pnlDetailCard.Size = new System.Drawing.Size(952, 194);
            this.pnlDetailCard.TabIndex = 0;
            // 
            // dgvChiTietVe
            // 
            this.dgvChiTietVe.AllowUserToAddRows = false;
            this.dgvChiTietVe.AllowUserToDeleteRows = false;
            this.dgvChiTietVe.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.dgvChiTietVe.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvChiTietVe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTietVe.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTietVe.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvChiTietVe.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvChiTietVe.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiTietVe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvChiTietVe.ColumnHeadersHeight = 36;
            this.dgvChiTietVe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvChiTietVe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaCT,
            this.colMaVeCode,
            this.colTenKV,
            this.colHangVe,
            this.colMaGheHienThi,
            this.colGiaVe,
            this.colTrangThaiVe});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiTietVe.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvChiTietVe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTietVe.AutoGenerateColumns = false;
            this.dgvChiTietVe.EnableHeadersVisualStyles = false;
            this.dgvChiTietVe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvChiTietVe.Location = new System.Drawing.Point(0, 42);
            this.dgvChiTietVe.MultiSelect = false;
            this.dgvChiTietVe.Name = "dgvChiTietVe";
            this.dgvChiTietVe.ReadOnly = true;
            this.dgvChiTietVe.RowHeadersVisible = false;
            this.dgvChiTietVe.RowTemplate.Height = 40;
            this.dgvChiTietVe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTietVe.Size = new System.Drawing.Size(952, 152);
            this.dgvChiTietVe.TabIndex = 1;
            this.dgvChiTietVe.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvChiTietVe_CellPainting);
            this.dgvChiTietVe.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvChiTietVe_CellFormatting);
            this.dgvChiTietVe.SelectionChanged += new System.EventHandler(this.dgvChiTietVe_SelectionChanged);
            // 
            // colMaCT
            // 
            this.colMaCT.DataPropertyName = "MaCT";
            this.colMaCT.FillWeight = 40F;
            this.colMaCT.HeaderText = "Mã vé";
            this.colMaCT.MinimumWidth = 70;
            this.colMaCT.Name = "colMaCT";
            this.colMaCT.ReadOnly = true;
            // 
            // colMaVeCode
            // 
            this.colMaVeCode.DataPropertyName = "MaVeCode";
            this.colMaVeCode.FillWeight = 90F;
            this.colMaVeCode.HeaderText = "Mã code vé";
            this.colMaVeCode.MinimumWidth = 140;
            this.colMaVeCode.Name = "colMaVeCode";
            this.colMaVeCode.ReadOnly = true;
            // 
            // colTenKV
            // 
            this.colTenKV.DataPropertyName = "TenKV";
            this.colTenKV.FillWeight = 75F;
            this.colTenKV.HeaderText = "Khu vực";
            this.colTenKV.MinimumWidth = 130;
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
            // colMaGheHienThi
            // 
            this.colMaGheHienThi.DataPropertyName = "MaGheHienThi";
            this.colMaGheHienThi.FillWeight = 45F;
            this.colMaGheHienThi.HeaderText = "Ghế ngồi";
            this.colMaGheHienThi.MinimumWidth = 80;
            this.colMaGheHienThi.Name = "colMaGheHienThi";
            this.colMaGheHienThi.ReadOnly = true;
            // 
            // colGiaVe
            // 
            this.colGiaVe.DataPropertyName = "GiaVe";
            this.colGiaVe.FillWeight = 55F;
            this.colGiaVe.HeaderText = "Giá vé";
            this.colGiaVe.MinimumWidth = 100;
            this.colGiaVe.Name = "colGiaVe";
            this.colGiaVe.ReadOnly = true;
            // 
            // colTrangThaiVe
            // 
            this.colTrangThaiVe.DataPropertyName = "TrangThai";
            this.colTrangThaiVe.FillWeight = 55F;
            this.colTrangThaiVe.HeaderText = "Trạng thái vé";
            this.colTrangThaiVe.MinimumWidth = 100;
            this.colTrangThaiVe.Name = "colTrangThaiVe";
            this.colTrangThaiVe.ReadOnly = true;
            this.colTrangThaiVe.ReadOnly = true;
            // 
            // pnlDetailHeader
            // 
            this.pnlDetailHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlDetailHeader.Controls.Add(this.lblRuleBadge);
            this.pnlDetailHeader.Controls.Add(this.lblDetailTitle);
            this.pnlDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetailHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlDetailHeader.Name = "pnlDetailHeader";
            this.pnlDetailHeader.Size = new System.Drawing.Size(952, 42);
            this.pnlDetailHeader.TabIndex = 0;
            // 
            // lblRuleBadge
            // 
            this.lblRuleBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRuleBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRuleBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblRuleBadge.Location = new System.Drawing.Point(420, 11);
            this.lblRuleBadge.Name = "lblRuleBadge";
            this.lblRuleBadge.Size = new System.Drawing.Size(518, 20);
            this.lblRuleBadge.TabIndex = 1;
            this.lblRuleBadge.Text = "Chọn 1 đơn vé để xem quy định hủy (≥24h) và đổi (≥12h)";
            this.lblRuleBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblDetailTitle.Location = new System.Drawing.Point(14, 11);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(206, 19);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "CHI TIẾT VÉ TRONG ĐƠN: —";
            // 
            // pnlFooter
            // 
            this.pnlFooter.ColumnCount = 2;
            this.pnlFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.pnlFooter.Controls.Add(this.lblCount, 0, 0);
            this.pnlFooter.Controls.Add(this.btnLamMoi, 1, 0);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(24, 624);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.RowCount = 1;
            this.pnlFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFooter.Size = new System.Drawing.Size(952, 52);
            this.pnlFooter.TabIndex = 3;
            // 
            // lblCount
            // 
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCount.Location = new System.Drawing.Point(3, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(816, 52);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "Đang tải dữ liệu…";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLamMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnLamMoi.BorderRadius = 6;
            this.btnLamMoi.BorderThickness = 1;
            this.btnLamMoi.FillColor = System.Drawing.Color.White;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnLamMoi.Location = new System.Drawing.Point(826, 8);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(123, 36);
            this.btnLamMoi.TabIndex = 1;
            this.btnLamMoi.Text = "Làm mới (F5)";
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // lblSearchHint
            // 
            this.lblSearchHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSearchHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSearchHint.Location = new System.Drawing.Point(24, 594);
            this.lblSearchHint.Name = "lblSearchHint";
            this.lblSearchHint.Size = new System.Drawing.Size(952, 30);
            this.lblSearchHint.TabIndex = 4;
            this.lblSearchHint.Text = "Tra cứu nhanh bằng Mã đơn (#10) hoặc Số điện thoại · Phím tắt: F5 làm mới · Hủy v" +
    "é cần trước ≥ 24h, Đổi vé cần trước ≥ 12h.";
            this.lblSearchHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchTimer
            // 
            this.searchTimer.Interval = 300;
            this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);
            // 
            // FrmQuanLyDonVe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 692);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.lblSearchHint);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FrmQuanLyDonVe";
            this.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý đơn vé & Hủy / Đổi";
            this.Load += new System.EventHandler(this.FrmQuanLyDonVe_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmQuanLyDonVe_KeyDown);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlMasterCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonVe)).EndInit();
            this.pnlMasterHeader.ResumeLayout(false);
            this.pnlMasterHeader.PerformLayout();
            this.pnlDetailCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietVe)).EndInit();
            this.pnlDetailHeader.ResumeLayout(false);
            this.pnlDetailHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2ComboBox cboLocTrangThai;
        private Guna.UI2.WinForms.Guna2Button btnDoiVe;
        private Guna.UI2.WinForms.Guna2Button btnHuyVe;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Guna.UI2.WinForms.Guna2Panel pnlMasterCard;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDonVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaDonGoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGianBatDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayDat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.Panel pnlMasterHeader;
        private System.Windows.Forms.Label lblMasterTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlDetailCard;
        private Guna.UI2.WinForms.Guna2DataGridView dgvChiTietVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaCT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaVeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHangVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaGheHienThi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThaiVe;
        private System.Windows.Forms.Panel pnlDetailHeader;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblRuleBadge;
        private System.Windows.Forms.TableLayoutPanel pnlFooter;
        private System.Windows.Forms.Label lblCount;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private System.Windows.Forms.Label lblSearchHint;
        private System.Windows.Forms.Timer searchTimer;
    }
}

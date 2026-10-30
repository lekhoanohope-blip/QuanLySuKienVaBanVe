using System;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmQuanLyKhuVuc
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTopAction = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.cboSuKien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSuatDien = new System.Windows.Forms.Label();
            this.cboSuatDien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtTimKiemKV = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnThemKhuVuc = new Guna.UI2.WinForms.Guna2Button();
            this.pnlCards = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlCard4 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblCard4Value = new System.Windows.Forms.Label();
            this.lblCard4Title = new System.Windows.Forms.Label();
            this.pnlCard3 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblCard3Value = new System.Windows.Forms.Label();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.pnlCard2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblCard2Value = new System.Windows.Forms.Label();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.pnlCard1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblCard1Value = new System.Windows.Forms.Label();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.pnlGrid = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.dgvKhuVuc = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaVung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoaiKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSucChua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDangGiu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConLai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThaoTac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTitleGrid = new System.Windows.Forms.Label();
            this.pnlBottom = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlLeftMap = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.pnlSeatMap = new System.Windows.Forms.Panel();
            this.pnlMapHeader = new System.Windows.Forms.Panel();
            this.btnToggleInfo = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitleMap = new System.Windows.Forms.Label();
            this.pnlRightInfo = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblTitleInfo = new System.Windows.Forms.Label();
            this.lblTrangThaiForm = new System.Windows.Forms.Label();
            this.lblMaVung = new System.Windows.Forms.Label();
            this.cboMaVung = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblTenKV = new System.Windows.Forms.Label();
            this.txtTenKV = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblLoaiKV = new System.Windows.Forms.Label();
            this.cboLoaiKV = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSucChua = new System.Windows.Forms.Label();
            this.txtSucChua = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSucChuaHint = new System.Windows.Forms.Label();
            this.lblGiaVe = new System.Windows.Forms.Label();
            this.txtGiaVe = new Guna.UI2.WinForms.Guna2TextBox();
            this.toggleBanVe = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.lblChoPhepBan = new System.Windows.Forms.Label();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.pnlLegend = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLegendTitle = new System.Windows.Forms.Label();
            this.pnlBottomSpacer = new System.Windows.Forms.Panel();
            this.pnlTopAction.SuspendLayout();
            this.pnlCards.SuspendLayout();
            this.pnlCard4.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhuVuc)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlLeftMap.SuspendLayout();
            this.pnlMapHeader.SuspendLayout();
            this.pnlRightInfo.SuspendLayout();
            this.pnlLegend.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopAction
            // 
            this.pnlTopAction.Controls.Add(this.lblTitle);
            this.pnlTopAction.Controls.Add(this.lblSubTitle);
            this.pnlTopAction.Controls.Add(this.lblSuKien);
            this.pnlTopAction.Controls.Add(this.cboSuKien);
            this.pnlTopAction.Controls.Add(this.lblSuatDien);
            this.pnlTopAction.Controls.Add(this.cboSuatDien);
            this.pnlTopAction.Controls.Add(this.txtTimKiemKV);
            this.pnlTopAction.Controls.Add(this.btnThemKhuVuc);
            this.pnlTopAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopAction.Location = new System.Drawing.Point(0, 0);
            this.pnlTopAction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlTopAction.Name = "pnlTopAction";
            this.pnlTopAction.Size = new System.Drawing.Size(1179, 118);
            this.pnlTopAction.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(335, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý khu vực && giá vé";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubTitle.Location = new System.Drawing.Point(21, 46);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(344, 20);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Thiết lập khu vực chỗ ngồi và giá vé theo suất diễn";
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSuKien.Location = new System.Drawing.Point(20, 74);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(60, 20);
            this.lblSuKien.TabIndex = 2;
            this.lblSuKien.Text = "Sự kiện:";
            // 
            // cboSuKien
            // 
            this.cboSuKien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuKien.BorderRadius = 6;
            this.cboSuKien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.FocusedColor = System.Drawing.Color.Empty;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboSuKien.ItemHeight = 28;
            this.cboSuKien.Location = new System.Drawing.Point(80, 68);
            this.cboSuKien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(240, 34);
            this.cboSuKien.TabIndex = 3;
            // 
            // lblSuatDien
            // 
            this.lblSuatDien.AutoSize = true;
            this.lblSuatDien.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSuatDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSuatDien.Location = new System.Drawing.Point(335, 74);
            this.lblSuatDien.Name = "lblSuatDien";
            this.lblSuatDien.Size = new System.Drawing.Size(74, 20);
            this.lblSuatDien.TabIndex = 4;
            this.lblSuatDien.Text = "Suất diễn:";
            // 
            // cboSuatDien
            // 
            this.cboSuatDien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuatDien.BorderRadius = 6;
            this.cboSuatDien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuatDien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuatDien.FocusedColor = System.Drawing.Color.Empty;
            this.cboSuatDien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSuatDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboSuatDien.ItemHeight = 28;
            this.cboSuatDien.Location = new System.Drawing.Point(411, 68);
            this.cboSuatDien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboSuatDien.Name = "cboSuatDien";
            this.cboSuatDien.Size = new System.Drawing.Size(271, 34);
            this.cboSuatDien.TabIndex = 5;
            // 
            // txtTimKiemKV
            // 
            this.txtTimKiemKV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimKiemKV.BorderRadius = 6;
            this.txtTimKiemKV.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiemKV.DefaultText = "";
            this.txtTimKiemKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTimKiemKV.Location = new System.Drawing.Point(928, 68);
            this.txtTimKiemKV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKiemKV.Name = "txtTimKiemKV";
            this.txtTimKiemKV.PlaceholderText = "Tìm kiếm khu vực...";
            this.txtTimKiemKV.SelectedText = "";
            this.txtTimKiemKV.Size = new System.Drawing.Size(220, 34);
            this.txtTimKiemKV.TabIndex = 6;
            // 
            // btnThemKhuVuc
            // 
            this.btnThemKhuVuc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemKhuVuc.BorderRadius = 6;
            this.btnThemKhuVuc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnThemKhuVuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThemKhuVuc.ForeColor = System.Drawing.Color.White;
            this.btnThemKhuVuc.Location = new System.Drawing.Point(1008, 14);
            this.btnThemKhuVuc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThemKhuVuc.Name = "btnThemKhuVuc";
            this.btnThemKhuVuc.Size = new System.Drawing.Size(140, 36);
            this.btnThemKhuVuc.TabIndex = 7;
            this.btnThemKhuVuc.Text = "Thêm khu vực";
            this.btnThemKhuVuc.Click += new System.EventHandler(this.btnThemKhuVuc_Click);
            // 
            // pnlCards
            // 
            this.pnlCards.Controls.Add(this.pnlCard4);
            this.pnlCards.Controls.Add(this.pnlCard3);
            this.pnlCards.Controls.Add(this.pnlCard2);
            this.pnlCards.Controls.Add(this.pnlCard1);
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Location = new System.Drawing.Point(0, 118);
            this.pnlCards.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(20, 4, 20, 7);
            this.pnlCards.Size = new System.Drawing.Size(1179, 114);
            this.pnlCards.TabIndex = 1;
            // 
            // pnlCard4
            // 
            this.pnlCard4.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard4.Controls.Add(this.lblCard4Value);
            this.pnlCard4.Controls.Add(this.lblCard4Title);
            this.pnlCard4.FillColor = System.Drawing.Color.White;
            this.pnlCard4.Location = new System.Drawing.Point(860, 5);
            this.pnlCard4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.Radius = 8;
            this.pnlCard4.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlCard4.ShadowDepth = 20;
            this.pnlCard4.Size = new System.Drawing.Size(265, 89);
            this.pnlCard4.TabIndex = 3;
            // 
            // lblCard4Value
            // 
            this.lblCard4Value.AutoSize = true;
            this.lblCard4Value.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCard4Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.lblCard4Value.Location = new System.Drawing.Point(13, 30);
            this.lblCard4Value.Name = "lblCard4Value";
            this.lblCard4Value.Size = new System.Drawing.Size(52, 35);
            this.lblCard4Value.TabIndex = 1;
            this.lblCard4Value.Text = "0 đ";
            // 
            // lblCard4Title
            // 
            this.lblCard4Title.AutoSize = true;
            this.lblCard4Title.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard4Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCard4Title.Location = new System.Drawing.Point(13, 10);
            this.lblCard4Title.Name = "lblCard4Title";
            this.lblCard4Title.Size = new System.Drawing.Size(117, 20);
            this.lblCard4Title.TabIndex = 0;
            this.lblCard4Title.Text = "Giá vé thấp nhất";
            // 
            // pnlCard3
            // 
            this.pnlCard3.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard3.Controls.Add(this.lblCard3Value);
            this.pnlCard3.Controls.Add(this.lblCard3Title);
            this.pnlCard3.FillColor = System.Drawing.Color.White;
            this.pnlCard3.Location = new System.Drawing.Point(580, 5);
            this.pnlCard3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.Radius = 8;
            this.pnlCard3.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlCard3.ShadowDepth = 20;
            this.pnlCard3.Size = new System.Drawing.Size(265, 89);
            this.pnlCard3.TabIndex = 2;
            // 
            // lblCard3Value
            // 
            this.lblCard3Value.AutoSize = true;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblCard3Value.Location = new System.Drawing.Point(13, 30);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(29, 35);
            this.lblCard3Value.TabIndex = 1;
            this.lblCard3Value.Text = "0";
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCard3Title.Location = new System.Drawing.Point(13, 10);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(100, 20);
            this.lblCard3Title.TabIndex = 0;
            this.lblCard3Title.Text = "Đang mở bán";
            // 
            // pnlCard2
            // 
            this.pnlCard2.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard2.Controls.Add(this.lblCard2Value);
            this.pnlCard2.Controls.Add(this.lblCard2Title);
            this.pnlCard2.FillColor = System.Drawing.Color.White;
            this.pnlCard2.Location = new System.Drawing.Point(300, 5);
            this.pnlCard2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.Radius = 8;
            this.pnlCard2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlCard2.ShadowDepth = 20;
            this.pnlCard2.Size = new System.Drawing.Size(265, 89);
            this.pnlCard2.TabIndex = 1;
            // 
            // lblCard2Value
            // 
            this.lblCard2Value.AutoSize = true;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCard2Value.Location = new System.Drawing.Point(13, 30);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(29, 35);
            this.lblCard2Value.TabIndex = 1;
            this.lblCard2Value.Text = "0";
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCard2Title.Location = new System.Drawing.Point(13, 10);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(105, 20);
            this.lblCard2Title.TabIndex = 0;
            this.lblCard2Title.Text = "Tổng sức chứa";
            // 
            // pnlCard1
            // 
            this.pnlCard1.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard1.Controls.Add(this.lblCard1Value);
            this.pnlCard1.Controls.Add(this.lblCard1Title);
            this.pnlCard1.FillColor = System.Drawing.Color.White;
            this.pnlCard1.Location = new System.Drawing.Point(20, 5);
            this.pnlCard1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.Radius = 8;
            this.pnlCard1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlCard1.ShadowDepth = 20;
            this.pnlCard1.Size = new System.Drawing.Size(265, 89);
            this.pnlCard1.TabIndex = 0;
            // 
            // lblCard1Value
            // 
            this.lblCard1Value.AutoSize = true;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCard1Value.Location = new System.Drawing.Point(13, 30);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(29, 35);
            this.lblCard1Value.TabIndex = 1;
            this.lblCard1Value.Text = "0";
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCard1Title.Location = new System.Drawing.Point(13, 10);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(97, 20);
            this.lblCard1Title.TabIndex = 0;
            this.lblCard1Title.Text = "Tổng khu vực";
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.Transparent;
            this.pnlGrid.Controls.Add(this.dgvKhuVuc);
            this.pnlGrid.Controls.Add(this.lblTitleGrid);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrid.FillColor = System.Drawing.Color.White;
            this.pnlGrid.Location = new System.Drawing.Point(0, 232);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(16, 10, 16, 12);
            this.pnlGrid.Radius = 8;
            this.pnlGrid.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlGrid.ShadowDepth = 20;
            this.pnlGrid.Size = new System.Drawing.Size(1179, 354);
            this.pnlGrid.TabIndex = 2;
            // 
            // dgvKhuVuc
            // 
            this.dgvKhuVuc.AllowUserToAddRows = false;
            this.dgvKhuVuc.AllowUserToDeleteRows = false;
            this.dgvKhuVuc.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.dgvKhuVuc.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKhuVuc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKhuVuc.ColumnHeadersHeight = 36;
            this.dgvKhuVuc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaKV,
            this.colMaVung,
            this.colTenKV,
            this.colLoaiKV,
            this.colSucChua,
            this.colGiaVe,
            this.colDaBan,
            this.colDangGiu,
            this.colConLai,
            this.colTrangThai,
            this.colThaoTac});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKhuVuc.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvKhuVuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKhuVuc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvKhuVuc.Location = new System.Drawing.Point(16, 40);
            this.dgvKhuVuc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvKhuVuc.MultiSelect = false;
            this.dgvKhuVuc.Name = "dgvKhuVuc";
            this.dgvKhuVuc.ReadOnly = true;
            this.dgvKhuVuc.RowHeadersVisible = false;
            this.dgvKhuVuc.RowHeadersWidth = 30;
            this.dgvKhuVuc.RowTemplate.Height = 40;
            this.dgvKhuVuc.Size = new System.Drawing.Size(1147, 302);
            this.dgvKhuVuc.TabIndex = 1;
            this.dgvKhuVuc.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.dgvKhuVuc.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.Height = 36;
            this.dgvKhuVuc.ThemeStyle.ReadOnly = true;
            this.dgvKhuVuc.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvKhuVuc.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.dgvKhuVuc.ThemeStyle.RowsStyle.Height = 40;
            this.dgvKhuVuc.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
            this.dgvKhuVuc.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvKhuVuc.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvKhuVuc_CellMouseClick);
            this.dgvKhuVuc.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvKhuVuc_CellPainting);
            this.dgvKhuVuc.SelectionChanged += new System.EventHandler(this.dgvKhuVuc_SelectionChanged);
            // 
            // colMaKV
            // 
            this.colMaKV.HeaderText = "Mã KV";
            this.colMaKV.MinimumWidth = 6;
            this.colMaKV.Name = "colMaKV";
            this.colMaKV.ReadOnly = true;
            // 
            // colMaVung
            // 
            this.colMaVung.HeaderText = "Mã vùng";
            this.colMaVung.MinimumWidth = 6;
            this.colMaVung.Name = "colMaVung";
            this.colMaVung.ReadOnly = true;
            // 
            // colTenKV
            // 
            this.colTenKV.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTenKV.HeaderText = "Tên khu vực";
            this.colTenKV.MinimumWidth = 140;
            this.colTenKV.Name = "colTenKV";
            this.colTenKV.ReadOnly = true;
            // 
            // colLoaiKV
            // 
            this.colLoaiKV.HeaderText = "Hạng vé";
            this.colLoaiKV.MinimumWidth = 6;
            this.colLoaiKV.Name = "colLoaiKV";
            this.colLoaiKV.ReadOnly = true;
            // 
            // colSucChua
            // 
            this.colSucChua.HeaderText = "Sức chứa";
            this.colSucChua.MinimumWidth = 6;
            this.colSucChua.Name = "colSucChua";
            this.colSucChua.ReadOnly = true;
            // 
            // colGiaVe
            // 
            this.colGiaVe.HeaderText = "Giá vé";
            this.colGiaVe.MinimumWidth = 6;
            this.colGiaVe.Name = "colGiaVe";
            this.colGiaVe.ReadOnly = true;
            // 
            // colDaBan
            // 
            this.colDaBan.HeaderText = "Đã bán";
            this.colDaBan.MinimumWidth = 6;
            this.colDaBan.Name = "colDaBan";
            this.colDaBan.ReadOnly = true;
            // 
            // colDangGiu
            // 
            this.colDangGiu.HeaderText = "Đang giữ";
            this.colDangGiu.MinimumWidth = 6;
            this.colDangGiu.Name = "colDangGiu";
            this.colDangGiu.ReadOnly = true;
            // 
            // colConLai
            // 
            this.colConLai.HeaderText = "Còn trống";
            this.colConLai.MinimumWidth = 6;
            this.colConLai.Name = "colConLai";
            this.colConLai.ReadOnly = true;
            // 
            // colTrangThai
            // 
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.MinimumWidth = 6;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            // 
            // colThaoTac
            // 
            this.colThaoTac.HeaderText = "Thao tác";
            this.colThaoTac.MinimumWidth = 6;
            this.colThaoTac.Name = "colThaoTac";
            this.colThaoTac.ReadOnly = true;
            // 
            // lblTitleGrid
            // 
            this.lblTitleGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleGrid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleGrid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitleGrid.Location = new System.Drawing.Point(16, 10);
            this.lblTitleGrid.Name = "lblTitleGrid";
            this.lblTitleGrid.Size = new System.Drawing.Size(1147, 30);
            this.lblTitleGrid.TabIndex = 0;
            this.lblTitleGrid.Text = "Danh sách khu vực";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlLeftMap);
            this.pnlBottom.Controls.Add(this.pnlRightInfo);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottom.Location = new System.Drawing.Point(0, 586);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlBottom.Size = new System.Drawing.Size(1179, 519);
            this.pnlBottom.TabIndex = 3;
            // 
            // pnlLeftMap
            // 
            this.pnlLeftMap.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeftMap.Controls.Add(this.pnlSeatMap);
            this.pnlLeftMap.Controls.Add(this.pnlMapHeader);
            this.pnlLeftMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeftMap.FillColor = System.Drawing.Color.White;
            this.pnlLeftMap.Location = new System.Drawing.Point(0, 10);
            this.pnlLeftMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlLeftMap.Name = "pnlLeftMap";
            this.pnlLeftMap.Radius = 8;
            this.pnlLeftMap.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlLeftMap.ShadowDepth = 20;
            this.pnlLeftMap.Size = new System.Drawing.Size(799, 499);
            this.pnlLeftMap.TabIndex = 0;
            // 
            // pnlSeatMap
            // 
            this.pnlSeatMap.AutoScroll = true;
            this.pnlSeatMap.BackColor = System.Drawing.Color.White;
            this.pnlSeatMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSeatMap.Location = new System.Drawing.Point(0, 42);
            this.pnlSeatMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlSeatMap.Name = "pnlSeatMap";
            this.pnlSeatMap.Size = new System.Drawing.Size(799, 457);
            this.pnlSeatMap.TabIndex = 1;
            this.pnlSeatMap.Resize += new System.EventHandler(this.pnlSeatMap_Resize);
            // 
            // pnlMapHeader
            // 
            this.pnlMapHeader.Controls.Add(this.btnToggleInfo);
            this.pnlMapHeader.Controls.Add(this.lblTitleMap);
            this.pnlMapHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMapHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlMapHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMapHeader.Name = "pnlMapHeader";
            this.pnlMapHeader.Size = new System.Drawing.Size(799, 42);
            this.pnlMapHeader.TabIndex = 0;
            // 
            // btnToggleInfo
            // 
            this.btnToggleInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleInfo.BorderRadius = 6;
            this.btnToggleInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnToggleInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToggleInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnToggleInfo.Location = new System.Drawing.Point(753, 7);
            this.btnToggleInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnToggleInfo.Name = "btnToggleInfo";
            this.btnToggleInfo.Size = new System.Drawing.Size(35, 28);
            this.btnToggleInfo.TabIndex = 1;
            this.btnToggleInfo.Text = ">";
            this.btnToggleInfo.Click += new System.EventHandler(this.btnToggleInfo_Click);
            // 
            // lblTitleMap
            // 
            this.lblTitleMap.AutoSize = true;
            this.lblTitleMap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleMap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitleMap.Location = new System.Drawing.Point(16, 12);
            this.lblTitleMap.Name = "lblTitleMap";
            this.lblTitleMap.Size = new System.Drawing.Size(196, 23);
            this.lblTitleMap.TabIndex = 0;
            this.lblTitleMap.Text = "Sơ đồ phân bố khu vực";
            // 
            // pnlRightInfo
            // 
            this.pnlRightInfo.AutoScroll = true;
            this.pnlRightInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlRightInfo.Controls.Add(this.lblTitleInfo);
            this.pnlRightInfo.Controls.Add(this.lblTrangThaiForm);
            this.pnlRightInfo.Controls.Add(this.lblMaVung);
            this.pnlRightInfo.Controls.Add(this.cboMaVung);
            this.pnlRightInfo.Controls.Add(this.lblTenKV);
            this.pnlRightInfo.Controls.Add(this.txtTenKV);
            this.pnlRightInfo.Controls.Add(this.lblLoaiKV);
            this.pnlRightInfo.Controls.Add(this.cboLoaiKV);
            this.pnlRightInfo.Controls.Add(this.lblSucChua);
            this.pnlRightInfo.Controls.Add(this.txtSucChua);
            this.pnlRightInfo.Controls.Add(this.lblSucChuaHint);
            this.pnlRightInfo.Controls.Add(this.lblGiaVe);
            this.pnlRightInfo.Controls.Add(this.txtGiaVe);
            this.pnlRightInfo.Controls.Add(this.toggleBanVe);
            this.pnlRightInfo.Controls.Add(this.lblChoPhepBan);
            this.pnlRightInfo.Controls.Add(this.btnLuu);
            this.pnlRightInfo.Controls.Add(this.btnHuy);
            this.pnlRightInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightInfo.FillColor = System.Drawing.Color.White;
            this.pnlRightInfo.Location = new System.Drawing.Point(799, 10);
            this.pnlRightInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlRightInfo.Name = "pnlRightInfo";
            this.pnlRightInfo.Radius = 8;
            this.pnlRightInfo.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.pnlRightInfo.ShadowDepth = 20;
            this.pnlRightInfo.Size = new System.Drawing.Size(380, 499);
            this.pnlRightInfo.TabIndex = 1;
            // 
            // lblTitleInfo
            // 
            this.lblTitleInfo.AutoSize = true;
            this.lblTitleInfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitleInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitleInfo.Location = new System.Drawing.Point(20, 12);
            this.lblTitleInfo.Name = "lblTitleInfo";
            this.lblTitleInfo.Size = new System.Drawing.Size(175, 25);
            this.lblTitleInfo.TabIndex = 0;
            this.lblTitleInfo.Text = "Thông tin khu vực";
            // 
            // lblTrangThaiForm
            // 
            this.lblTrangThaiForm.AutoSize = true;
            this.lblTrangThaiForm.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblTrangThaiForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblTrangThaiForm.Location = new System.Drawing.Point(21, 37);
            this.lblTrangThaiForm.Name = "lblTrangThaiForm";
            this.lblTrangThaiForm.Size = new System.Drawing.Size(148, 20);
            this.lblTrangThaiForm.TabIndex = 1;
            this.lblTrangThaiForm.Text = "Thiết lập khu vực mới";
            // 
            // lblMaVung
            // 
            this.lblMaVung.AutoSize = true;
            this.lblMaVung.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMaVung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblMaVung.Location = new System.Drawing.Point(20, 62);
            this.lblMaVung.Name = "lblMaVung";
            this.lblMaVung.Size = new System.Drawing.Size(117, 20);
            this.lblMaVung.TabIndex = 2;
            this.lblMaVung.Text = "Vùng trên sơ đồ:";
            // 
            // cboMaVung
            // 
            this.cboMaVung.BackColor = System.Drawing.Color.Transparent;
            this.cboMaVung.BorderRadius = 6;
            this.cboMaVung.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboMaVung.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaVung.FocusedColor = System.Drawing.Color.Empty;
            this.cboMaVung.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboMaVung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboMaVung.ItemHeight = 26;
            this.cboMaVung.Location = new System.Drawing.Point(20, 84);
            this.cboMaVung.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboMaVung.Name = "cboMaVung";
            this.cboMaVung.Size = new System.Drawing.Size(340, 32);
            this.cboMaVung.TabIndex = 3;
            this.cboMaVung.SelectedIndexChanged += new System.EventHandler(this.cboMaVung_SelectedIndexChanged);
            // 
            // lblTenKV
            // 
            this.lblTenKV.AutoSize = true;
            this.lblTenKV.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTenKV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblTenKV.Location = new System.Drawing.Point(20, 122);
            this.lblTenKV.Name = "lblTenKV";
            this.lblTenKV.Size = new System.Drawing.Size(89, 20);
            this.lblTenKV.TabIndex = 4;
            this.lblTenKV.Text = "Tên khu vực:";
            // 
            // txtTenKV
            // 
            this.txtTenKV.BorderRadius = 6;
            this.txtTenKV.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenKV.DefaultText = "";
            this.txtTenKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenKV.Location = new System.Drawing.Point(20, 144);
            this.txtTenKV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTenKV.Name = "txtTenKV";
            this.txtTenKV.PlaceholderText = "Nhập tên khu vực";
            this.txtTenKV.SelectedText = "";
            this.txtTenKV.Size = new System.Drawing.Size(340, 32);
            this.txtTenKV.TabIndex = 5;
            // 
            // lblLoaiKV
            // 
            this.lblLoaiKV.AutoSize = true;
            this.lblLoaiKV.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLoaiKV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblLoaiKV.Location = new System.Drawing.Point(20, 182);
            this.lblLoaiKV.Name = "lblLoaiKV";
            this.lblLoaiKV.Size = new System.Drawing.Size(67, 20);
            this.lblLoaiKV.TabIndex = 6;
            this.lblLoaiKV.Text = "Hạng vé:";
            // 
            // cboLoaiKV
            // 
            this.cboLoaiKV.BackColor = System.Drawing.Color.Transparent;
            this.cboLoaiKV.BorderRadius = 6;
            this.cboLoaiKV.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLoaiKV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiKV.FocusedColor = System.Drawing.Color.Empty;
            this.cboLoaiKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboLoaiKV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboLoaiKV.ItemHeight = 26;
            this.cboLoaiKV.Items.AddRange(new object[] {
            "VIP",
            "Phổ thông",
            "Thường"});
            this.cboLoaiKV.Location = new System.Drawing.Point(20, 204);
            this.cboLoaiKV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboLoaiKV.Name = "cboLoaiKV";
            this.cboLoaiKV.Size = new System.Drawing.Size(340, 32);
            this.cboLoaiKV.TabIndex = 7;
            // 
            // lblSucChua
            // 
            this.lblSucChua.AutoSize = true;
            this.lblSucChua.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSucChua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSucChua.Location = new System.Drawing.Point(20, 242);
            this.lblSucChua.Name = "lblSucChua";
            this.lblSucChua.Size = new System.Drawing.Size(72, 20);
            this.lblSucChua.TabIndex = 8;
            this.lblSucChua.Text = "Sức chứa:";
            // 
            // txtSucChua
            // 
            this.txtSucChua.BorderRadius = 6;
            this.txtSucChua.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSucChua.DefaultText = "";
            this.txtSucChua.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSucChua.Location = new System.Drawing.Point(20, 263);
            this.txtSucChua.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSucChua.Name = "txtSucChua";
            this.txtSucChua.PlaceholderText = "20";
            this.txtSucChua.SelectedText = "";
            this.txtSucChua.Size = new System.Drawing.Size(340, 32);
            this.txtSucChua.TabIndex = 9;
            // 
            // lblSucChuaHint
            // 
            this.lblSucChuaHint.AutoSize = true;
            this.lblSucChuaHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSucChuaHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSucChuaHint.Location = new System.Drawing.Point(20, 298);
            this.lblSucChuaHint.Name = "lblSucChuaHint";
            this.lblSucChuaHint.Size = new System.Drawing.Size(285, 19);
            this.lblSucChuaHint.TabIndex = 10;
            this.lblSucChuaHint.Text = "Tự động sinh ma trận ghế (10 ghế mỗi hàng)";
            // 
            // lblGiaVe
            // 
            this.lblGiaVe.AutoSize = true;
            this.lblGiaVe.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblGiaVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblGiaVe.Location = new System.Drawing.Point(20, 322);
            this.lblGiaVe.Name = "lblGiaVe";
            this.lblGiaVe.Size = new System.Drawing.Size(98, 20);
            this.lblGiaVe.TabIndex = 11;
            this.lblGiaVe.Text = "Giá vé (VNĐ):";
            // 
            // txtGiaVe
            // 
            this.txtGiaVe.BorderRadius = 6;
            this.txtGiaVe.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGiaVe.DefaultText = "";
            this.txtGiaVe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGiaVe.Location = new System.Drawing.Point(20, 345);
            this.txtGiaVe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGiaVe.Name = "txtGiaVe";
            this.txtGiaVe.PlaceholderText = "250000";
            this.txtGiaVe.SelectedText = "";
            this.txtGiaVe.Size = new System.Drawing.Size(340, 32);
            this.txtGiaVe.TabIndex = 12;
            // 
            // toggleBanVe
            // 
            this.toggleBanVe.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.toggleBanVe.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.toggleBanVe.CheckedState.InnerColor = System.Drawing.Color.White;
            this.toggleBanVe.Location = new System.Drawing.Point(20, 390);
            this.toggleBanVe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toggleBanVe.Name = "toggleBanVe";
            this.toggleBanVe.Size = new System.Drawing.Size(45, 22);
            this.toggleBanVe.TabIndex = 13;
            this.toggleBanVe.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.toggleBanVe.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.toggleBanVe.UncheckedState.InnerColor = System.Drawing.Color.White;
            // 
            // lblChoPhepBan
            // 
            this.lblChoPhepBan.AutoSize = true;
            this.lblChoPhepBan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChoPhepBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblChoPhepBan.Location = new System.Drawing.Point(75, 391);
            this.lblChoPhepBan.Name = "lblChoPhepBan";
            this.lblChoPhepBan.Size = new System.Drawing.Size(121, 20);
            this.lblChoPhepBan.TabIndex = 14;
            this.lblChoPhepBan.Text = "Cho phép bán vé";
            // 
            // btnLuu
            // 
            this.btnLuu.BorderRadius = 6;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(20, 432);
            this.btnLuu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(215, 38);
            this.btnLuu.TabIndex = 15;
            this.btnLuu.Text = "Tạo khu vực và sinh ghế";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnHuy.BorderRadius = 6;
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnHuy.Location = new System.Drawing.Point(245, 432);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(115, 38);
            this.btnHuy.TabIndex = 16;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // pnlLegend
            // 
            this.pnlLegend.BackColor = System.Drawing.Color.White;
            this.pnlLegend.Controls.Add(this.lblLegendTitle);
            this.pnlLegend.Location = new System.Drawing.Point(0, 0);
            this.pnlLegend.Name = "pnlLegend";
            this.pnlLegend.Size = new System.Drawing.Size(220, 500);
            this.pnlLegend.TabIndex = 2;
            this.pnlLegend.Visible = false;
            // 
            // lblLegendTitle
            // 
            this.lblLegendTitle.AutoSize = true;
            this.lblLegendTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLegendTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblLegendTitle.Location = new System.Drawing.Point(3, 0);
            this.lblLegendTitle.Name = "lblLegendTitle";
            this.lblLegendTitle.Size = new System.Drawing.Size(75, 20);
            this.lblLegendTitle.TabIndex = 0;
            this.lblLegendTitle.Text = "Chú thích";
            // 
            // pnlBottomSpacer
            // 
            this.pnlBottomSpacer.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottomSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottomSpacer.Location = new System.Drawing.Point(0, 1105);
            this.pnlBottomSpacer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBottomSpacer.Name = "pnlBottomSpacer";
            this.pnlBottomSpacer.Size = new System.Drawing.Size(1179, 34);
            this.pnlBottomSpacer.TabIndex = 4;
            // 
            // FrmQuanLyKhuVuc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(950, 960);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1200, 884);
            this.Controls.Add(this.pnlBottomSpacer);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlTopAction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmQuanLyKhuVuc";
            this.Text = "Quản lý khu vực && giá vé";
            this.Load += new System.EventHandler(this.FrmQuanLyKhuVuc_Load);
            this.SizeChanged += new System.EventHandler(this.FrmQuanLyKhuVuc_SizeChanged);
            this.pnlTopAction.ResumeLayout(false);
            this.pnlTopAction.PerformLayout();
            this.pnlCards.ResumeLayout(false);
            this.pnlCard4.ResumeLayout(false);
            this.pnlCard4.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhuVuc)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlLeftMap.ResumeLayout(false);
            this.pnlMapHeader.ResumeLayout(false);
            this.pnlMapHeader.PerformLayout();
            this.pnlRightInfo.ResumeLayout(false);
            this.pnlRightInfo.PerformLayout();
            this.pnlLegend.ResumeLayout(false);
            this.pnlLegend.PerformLayout();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel pnlTopAction;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblSuKien;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuKien;
        private System.Windows.Forms.Label lblSuatDien;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuatDien;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiemKV;
        private Guna.UI2.WinForms.Guna2Button btnThemKhuVuc;
        private Guna.UI2.WinForms.Guna2Panel pnlCards;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlCard1;
        private System.Windows.Forms.Label lblCard1Title;
        private System.Windows.Forms.Label lblCard1Value;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlCard2;
        private System.Windows.Forms.Label lblCard2Title;
        private System.Windows.Forms.Label lblCard2Value;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlCard3;
        private System.Windows.Forms.Label lblCard3Title;
        private System.Windows.Forms.Label lblCard3Value;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlCard4;
        private System.Windows.Forms.Label lblCard4Title;
        private System.Windows.Forms.Label lblCard4Value;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlGrid;
        private System.Windows.Forms.Label lblTitleGrid;
        private Guna.UI2.WinForms.Guna2DataGridView dgvKhuVuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaVung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoaiKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSucChua;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDangGiu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConLai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThaoTac;
        private Guna.UI2.WinForms.Guna2Panel pnlBottom;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlLeftMap;
        private System.Windows.Forms.Panel pnlMapHeader;
        private System.Windows.Forms.Label lblTitleMap;
        private Guna.UI2.WinForms.Guna2Button btnToggleInfo;
        private System.Windows.Forms.Panel pnlSeatMap;
        private System.Windows.Forms.FlowLayoutPanel pnlLegend;
        private System.Windows.Forms.Label lblLegendTitle;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlRightInfo;
        private System.Windows.Forms.Label lblTitleInfo;
        private System.Windows.Forms.Label lblTrangThaiForm;
        private System.Windows.Forms.Label lblMaVung;
        private Guna.UI2.WinForms.Guna2ComboBox cboMaVung;
        private System.Windows.Forms.Label lblTenKV;
        private Guna.UI2.WinForms.Guna2TextBox txtTenKV;
        private System.Windows.Forms.Label lblLoaiKV;
        private Guna.UI2.WinForms.Guna2ComboBox cboLoaiKV;
        private System.Windows.Forms.Label lblSucChua;
        private Guna.UI2.WinForms.Guna2TextBox txtSucChua;
        private System.Windows.Forms.Label lblSucChuaHint;
        private System.Windows.Forms.Label lblGiaVe;
        private Guna.UI2.WinForms.Guna2TextBox txtGiaVe;
        private System.Windows.Forms.Label lblChoPhepBan;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggleBanVe;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private System.Windows.Forms.Panel pnlBottomSpacer;
    }
}

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
            this.btnThemKhuVuc = new Guna.UI2.WinForms.Guna2Button();
            this.txtTimKiemKV = new Guna.UI2.WinForms.Guna2TextBox();
            this.cboSuatDien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cboSuKien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
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
            this.colTenKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoaiKV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSucChua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConLai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSua = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colXoa = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblTitleGrid = new System.Windows.Forms.Label();
            this.pnlBottom = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlLeftMap = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.pnlSeatMap = new System.Windows.Forms.Panel();
            this.pnlLegend = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLegendTitle = new System.Windows.Forms.Label();
            this.lblTitleMap = new System.Windows.Forms.Label();
            this.pnlRightInfo = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.toggleBanVe = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.lblChoPhepBan = new System.Windows.Forms.Label();
            this.txtGiaVe = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGiaVe = new System.Windows.Forms.Label();
            this.txtSucChua = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSucChua = new System.Windows.Forms.Label();
            this.cboLoaiKV = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblLoaiKV = new System.Windows.Forms.Label();
            this.txtTenKV = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTenKV = new System.Windows.Forms.Label();
            this.lblTitleInfo = new System.Windows.Forms.Label();
            this.guna2Button10 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button7 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button6 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button5 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.btnStage = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.btnToggleInfo = new Guna.UI2.WinForms.Guna2Button();

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
            this.pnlLegend.SuspendLayout();
            this.pnlRightInfo.SuspendLayout();
            this.SuspendLayout();

            // 1. Thu nhỏ Form thông tin bên phải lại một chút
            this.pnlRightInfo.Size = new System.Drawing.Size(360, 587);

            // 2. Cho Sơ đồ chiếm toàn bộ phần còn lại
            this.pnlSeatMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSeatMap.AutoScroll = true;

            // 
            // pnlTopAction
            // 
            this.pnlTopAction.Controls.Add(this.btnThemKhuVuc);
            this.pnlTopAction.Controls.Add(this.txtTimKiemKV);
            this.pnlTopAction.Controls.Add(this.cboSuatDien);
            this.pnlTopAction.Controls.Add(this.cboSuKien);
            this.pnlTopAction.Controls.Add(this.lblSubTitle);
            this.pnlTopAction.Controls.Add(this.lblTitle);
            this.pnlTopAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopAction.Location = new System.Drawing.Point(0, 0);
            this.pnlTopAction.Name = "pnlTopAction";
            this.pnlTopAction.Size = new System.Drawing.Size(1314, 107);
            this.pnlTopAction.TabIndex = 0;
            // 
            // btnThemKhuVuc
            // 
            this.btnThemKhuVuc.BorderRadius = 8;
            this.btnThemKhuVuc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnThemKhuVuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThemKhuVuc.ForeColor = System.Drawing.Color.White;
            this.btnThemKhuVuc.Location = new System.Drawing.Point(1154, 59);
            this.btnThemKhuVuc.Name = "btnThemKhuVuc";
            this.btnThemKhuVuc.Size = new System.Drawing.Size(137, 38);
            this.btnThemKhuVuc.TabIndex = 5;
            this.btnThemKhuVuc.Text = "+ Thêm khu vực";
            this.btnThemKhuVuc.Click += new System.EventHandler(this.btnThemKhuVuc_Click);
            // 
            // txtTimKiemKV
            // 
            this.txtTimKiemKV.BorderRadius = 8;
            this.txtTimKiemKV.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiemKV.DefaultText = "";
            this.txtTimKiemKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTimKiemKV.Location = new System.Drawing.Point(823, 59);
            this.txtTimKiemKV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKiemKV.Name = "txtTimKiemKV";
            this.txtTimKiemKV.PlaceholderText = "🔍 Tìm khu vực...";
            this.txtTimKiemKV.SelectedText = "";
            this.txtTimKiemKV.Size = new System.Drawing.Size(297, 38);
            this.txtTimKiemKV.TabIndex = 4;
            // 
            // cboSuatDien
            // 
            this.cboSuatDien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuatDien.BorderRadius = 8;
            this.cboSuatDien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuatDien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuatDien.FocusedColor = System.Drawing.Color.Empty;
            this.cboSuatDien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSuatDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboSuatDien.ItemHeight = 30;
            this.cboSuatDien.Location = new System.Drawing.Point(457, 59);
            this.cboSuatDien.Name = "cboSuatDien";
            this.cboSuatDien.Size = new System.Drawing.Size(319, 36);
            this.cboSuatDien.TabIndex = 3;
            this.cboSuatDien.SelectedIndexChanged += new System.EventHandler(this.cboSuatDien_SelectedIndexChanged);
            // 
            // cboSuKien
            // 
            this.cboSuKien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuKien.BorderRadius = 8;
            this.cboSuKien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.FocusedColor = System.Drawing.Color.Empty;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboSuKien.ItemHeight = 30;
            this.cboSuKien.Location = new System.Drawing.Point(23, 59);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(388, 36);
            this.cboSuKien.TabIndex = 2;
            this.cboSuKien.SelectedIndexChanged += new System.EventHandler(this.cboSuKien_SelectedIndexChanged);
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSubTitle.Location = new System.Drawing.Point(25, 34);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(344, 20);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Thiết lập khu vực chỗ ngồi và giá vé theo suất diễn";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(23, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(312, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý khu vực & giá vé";
            // 
            // pnlCards
            // 
            this.pnlCards.Controls.Add(this.pnlCard4);
            this.pnlCards.Controls.Add(this.pnlCard3);
            this.pnlCards.Controls.Add(this.pnlCard2);
            this.pnlCards.Controls.Add(this.pnlCard1);
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Location = new System.Drawing.Point(0, 107);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Size = new System.Drawing.Size(1314, 96);
            this.pnlCards.TabIndex = 1;
            // 
            // pnlCard4
            // 
            this.pnlCard4.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard4.Controls.Add(this.lblCard4Value);
            this.pnlCard4.Controls.Add(this.lblCard4Title);
            this.pnlCard4.FillColor = System.Drawing.Color.White;
            this.pnlCard4.Location = new System.Drawing.Point(1000, 11);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.Radius = 10;
            this.pnlCard4.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard4.ShadowDepth = 20;
            this.pnlCard4.Size = new System.Drawing.Size(297, 75);
            this.pnlCard4.TabIndex = 3;
            // 
            // lblCard4Value
            // 
            this.lblCard4Value.AutoSize = true;
            this.lblCard4Value.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCard4Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.lblCard4Value.Location = new System.Drawing.Point(17, 32);
            this.lblCard4Value.Name = "lblCard4Value";
            this.lblCard4Value.Size = new System.Drawing.Size(144, 37);
            this.lblCard4Value.TabIndex = 1;
            this.lblCard4Value.Text = "350.000 đ";
            // 
            // lblCard4Title
            // 
            this.lblCard4Title.AutoSize = true;
            this.lblCard4Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard4Title.ForeColor = System.Drawing.Color.Gray;
            this.lblCard4Title.Location = new System.Drawing.Point(17, 11);
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
            this.pnlCard3.Location = new System.Drawing.Point(674, 11);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.Radius = 10;
            this.pnlCard3.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard3.ShadowDepth = 20;
            this.pnlCard3.Size = new System.Drawing.Size(297, 75);
            this.pnlCard3.TabIndex = 2;
            // 
            // lblCard3Value
            // 
            this.lblCard3Value.AutoSize = true;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblCard3Value.Location = new System.Drawing.Point(17, 32);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(33, 37);
            this.lblCard3Value.TabIndex = 1;
            this.lblCard3Value.Text = "3";
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard3Title.ForeColor = System.Drawing.Color.Gray;
            this.lblCard3Title.Location = new System.Drawing.Point(17, 11);
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
            this.pnlCard2.Location = new System.Drawing.Point(349, 11);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.Radius = 10;
            this.pnlCard2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard2.ShadowDepth = 20;
            this.pnlCard2.Size = new System.Drawing.Size(297, 75);
            this.pnlCard2.TabIndex = 1;
            // 
            // lblCard2Value
            // 
            this.lblCard2Value.AutoSize = true;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.Location = new System.Drawing.Point(17, 32);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(88, 37);
            this.lblCard2Value.TabIndex = 1;
            this.lblCard2Value.Text = "1.250";
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard2Title.ForeColor = System.Drawing.Color.Gray;
            this.lblCard2Title.Location = new System.Drawing.Point(17, 11);
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
            this.pnlCard1.Location = new System.Drawing.Point(23, 11);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.Radius = 10;
            this.pnlCard1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard1.ShadowDepth = 20;
            this.pnlCard1.Size = new System.Drawing.Size(297, 75);
            this.pnlCard1.TabIndex = 0;
            // 
            // lblCard1Value
            // 
            this.lblCard1Value.AutoSize = true;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.Location = new System.Drawing.Point(17, 32);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(33, 37);
            this.lblCard1Value.TabIndex = 1;
            this.lblCard1Value.Text = "4";
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard1Title.ForeColor = System.Drawing.Color.Gray;
            this.lblCard1Title.Location = new System.Drawing.Point(17, 11);
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
            this.pnlGrid.Location = new System.Drawing.Point(0, 203);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Radius = 10;
            this.pnlGrid.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlGrid.ShadowDepth = 20;
            this.pnlGrid.Size = new System.Drawing.Size(1314, 277);
            this.pnlGrid.TabIndex = 2;
            // 
            // dgvKhuVuc
            // 
            this.dgvKhuVuc.AllowUserToAddRows = false;
            this.dgvKhuVuc.AllowUserToDeleteRows = false;
            this.dgvKhuVuc.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvKhuVuc.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKhuVuc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKhuVuc.ColumnHeadersHeight = 35;
            this.dgvKhuVuc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvKhuVuc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaKV,
            this.colTenKV,
            this.colLoaiKV,
            this.colSucChua,
            this.colGiaVe,
            this.colDaBan,
            this.colConLai,
            this.colTrangThai,
            this.colSua,
            this.colXoa});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKhuVuc.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvKhuVuc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvKhuVuc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvKhuVuc.Location = new System.Drawing.Point(0, 42);
            this.dgvKhuVuc.Name = "dgvKhuVuc";
            this.dgvKhuVuc.ReadOnly = true;
            this.dgvKhuVuc.RowHeadersVisible = false;
            this.dgvKhuVuc.RowHeadersWidth = 51;
            this.dgvKhuVuc.RowTemplate.Height = 45;
            this.dgvKhuVuc.Size = new System.Drawing.Size(1314, 235);
            this.dgvKhuVuc.TabIndex = 1;
            this.dgvKhuVuc.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvKhuVuc.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.dgvKhuVuc.ThemeStyle.HeaderStyle.Height = 35;
            this.dgvKhuVuc.ThemeStyle.ReadOnly = true;
            this.dgvKhuVuc.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvKhuVuc.ThemeStyle.RowsStyle.Height = 45;
            this.dgvKhuVuc.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKhuVuc_CellContentClick);
            // 
            // colMaKV
            // 
            this.colMaKV.HeaderText = "Mã khu vực";
            this.colMaKV.MinimumWidth = 6;
            this.colMaKV.Name = "colMaKV";
            this.colMaKV.ReadOnly = true;
            // 
            // colTenKV
            // 
            this.colTenKV.HeaderText = "Tên khu vực";
            this.colTenKV.MinimumWidth = 6;
            this.colTenKV.Name = "colTenKV";
            this.colTenKV.ReadOnly = true;
            // 
            // colLoaiKV
            // 
            this.colLoaiKV.HeaderText = "Loại khu vực";
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
            // colConLai
            // 
            this.colConLai.HeaderText = "Còn lại";
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
            // colSua
            // 
            this.colSua.HeaderText = "Sửa";
            this.colSua.MinimumWidth = 6;
            this.colSua.Name = "colSua";
            this.colSua.ReadOnly = true;
            this.colSua.Text = "Sửa";
            this.colSua.UseColumnTextForButtonValue = true;
            this.colSua.Width = 70;
            // 
            // colXoa
            // 
            this.colXoa.HeaderText = "Xóa";
            this.colXoa.MinimumWidth = 6;
            this.colXoa.Name = "colXoa";
            this.colXoa.ReadOnly = true;
            this.colXoa.Text = "Xóa";
            this.colXoa.UseColumnTextForButtonValue = true;
            this.colXoa.Width = 70;
            // 
            // lblTitleGrid
            // 
            this.lblTitleGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleGrid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleGrid.Location = new System.Drawing.Point(0, 0);
            this.lblTitleGrid.Name = "lblTitleGrid";
            this.lblTitleGrid.Padding = new System.Windows.Forms.Padding(17, 11, 0, 0);
            this.lblTitleGrid.Size = new System.Drawing.Size(1314, 43);
            this.lblTitleGrid.TabIndex = 0;
            this.lblTitleGrid.Text = "Danh sách khu vực";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlLeftMap);
            this.pnlBottom.Controls.Add(this.pnlRightInfo);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottom.Location = new System.Drawing.Point(0, 480);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1314, 587);
            this.pnlBottom.TabIndex = 3;
            // 
            // pnlLeftMap
            // 
            this.pnlLeftMap.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeftMap.Controls.Add(this.pnlSeatMap);
            this.pnlLeftMap.Controls.Add(this.lblTitleMap);
            this.pnlLeftMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeftMap.FillColor = System.Drawing.Color.White;
            this.pnlLeftMap.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftMap.Name = "pnlLeftMap";
            this.pnlLeftMap.Radius = 10;
            this.pnlLeftMap.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlLeftMap.ShadowDepth = 20;
            this.pnlLeftMap.Size = new System.Drawing.Size(857, 587);
            this.pnlLeftMap.TabIndex = 0;
            // 
            // pnlSeatMap 
            // 
            this.pnlSeatMap.BackColor = System.Drawing.Color.White;
            this.pnlSeatMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSeatMap.Location = new System.Drawing.Point(0, 0);
            this.pnlSeatMap.Name = "pnlSeatMap";
            this.pnlSeatMap.Size = new System.Drawing.Size(430, 501);
            this.pnlSeatMap.TabIndex = 3;
            this.pnlSeatMap.Resize += new System.EventHandler(this.pnlSeatMap_Resize);

            // 
            // pnlLegend 
            // 
            this.pnlLegend.AutoScroll = false;
            this.pnlLegend.BackColor = System.Drawing.Color.White;
            this.pnlLegend.Controls.Add(this.lblLegendTitle);
            this.pnlLegend.Location = new System.Drawing.Point(0, 0);
            this.pnlLegend.Name = "pnlLegend";
            this.pnlLegend.Size = new System.Drawing.Size(220, 500);
            this.pnlLegend.TabIndex = 1;
            this.pnlLegend.Visible = false;
            // 
            // btnToggleInfo 
            // 
            this.btnToggleInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnToggleInfo.BorderRadius = 6;
            this.btnToggleInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnToggleInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnToggleInfo.ForeColor = System.Drawing.Color.Black;
            this.btnToggleInfo.Location = new System.Drawing.Point(400, 10);
            this.btnToggleInfo.Name = "btnToggleInfo";
            this.btnToggleInfo.Size = new System.Drawing.Size(30, 30);
            this.btnToggleInfo.TabIndex = 4;
            this.btnToggleInfo.Text = "▶";
            this.btnToggleInfo.Click += new System.EventHandler(this.btnToggleInfo_Click);
            this.pnlLeftMap.Controls.Add(this.btnToggleInfo);
            this.btnToggleInfo.BringToFront();
            // 
            // lblLegendTitle
            // 
            this.lblLegendTitle.AutoSize = true;
            this.lblLegendTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLegendTitle.Location = new System.Drawing.Point(3, 0);
            this.lblLegendTitle.Name = "lblLegendTitle";
            this.lblLegendTitle.Size = new System.Drawing.Size(75, 20);
            this.lblLegendTitle.TabIndex = 0;
            this.lblLegendTitle.Text = "Chú thích";
            // 
            // lblTitleMap
            // 
            this.lblTitleMap.AutoSize = true;
            this.lblTitleMap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleMap.Location = new System.Drawing.Point(17, 16);
            this.lblTitleMap.Name = "lblTitleMap";
            this.lblTitleMap.Size = new System.Drawing.Size(225, 23);
            this.lblTitleMap.TabIndex = 0;
            this.lblTitleMap.Text = "🗺️ Sơ đồ phân bố khu vực";
            // 
            // pnlRightInfo
            // 
            this.pnlRightInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlRightInfo.Controls.Add(this.btnHuy);
            this.pnlRightInfo.Controls.Add(this.btnLuu);
            this.pnlRightInfo.Controls.Add(this.toggleBanVe);
            this.pnlRightInfo.Controls.Add(this.lblChoPhepBan);
            this.pnlRightInfo.Controls.Add(this.txtGiaVe);
            this.pnlRightInfo.Controls.Add(this.lblGiaVe);
            this.pnlRightInfo.Controls.Add(this.txtSucChua);
            this.pnlRightInfo.Controls.Add(this.lblSucChua);
            this.pnlRightInfo.Controls.Add(this.cboLoaiKV);
            this.pnlRightInfo.Controls.Add(this.lblLoaiKV);
            this.pnlRightInfo.Controls.Add(this.txtTenKV);
            this.pnlRightInfo.Controls.Add(this.lblTenKV);
            this.pnlRightInfo.Controls.Add(this.lblTitleInfo);
            this.pnlRightInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightInfo.FillColor = System.Drawing.Color.White;
            this.pnlRightInfo.Location = new System.Drawing.Point(857, 0);
            this.pnlRightInfo.Name = "pnlRightInfo";
            this.pnlRightInfo.Radius = 10;
            this.pnlRightInfo.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlRightInfo.ShadowDepth = 20;
            this.pnlRightInfo.Size = new System.Drawing.Size(457, 587);
            this.pnlRightInfo.TabIndex = 1;
            // 
            // btnHuy
            // 
            this.btnHuy.BorderColor = System.Drawing.Color.Silver;
            this.btnHuy.BorderRadius = 6;
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.Gray;
            this.btnHuy.Location = new System.Drawing.Point(240, 416);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(183, 38);
            this.btnHuy.TabIndex = 12;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BorderRadius = 6;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(23, 416);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(183, 38);
            this.btnLuu.TabIndex = 11;
            this.btnLuu.Text = "Lưu thay đổi";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // toggleBanVe
            // 
            this.toggleBanVe.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.toggleBanVe.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.toggleBanVe.CheckedState.InnerColor = System.Drawing.Color.White;
            this.toggleBanVe.Location = new System.Drawing.Point(23, 363);
            this.toggleBanVe.Name = "toggleBanVe";
            this.toggleBanVe.Size = new System.Drawing.Size(57, 21);
            this.toggleBanVe.TabIndex = 10;
            this.toggleBanVe.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.toggleBanVe.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.toggleBanVe.UncheckedState.InnerColor = System.Drawing.Color.White;
            // 
            // lblChoPhepBan
            // 
            this.lblChoPhepBan.AutoSize = true;
            this.lblChoPhepBan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChoPhepBan.Location = new System.Drawing.Point(91, 365);
            this.lblChoPhepBan.Name = "lblChoPhepBan";
            this.lblChoPhepBan.Size = new System.Drawing.Size(121, 20);
            this.lblChoPhepBan.TabIndex = 9;
            this.lblChoPhepBan.Text = "Cho phép bán vé";
            // 
            // txtGiaVe
            // 
            this.txtGiaVe.BorderRadius = 6;
            this.txtGiaVe.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGiaVe.DefaultText = "";
            this.txtGiaVe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGiaVe.Location = new System.Drawing.Point(240, 299);
            this.txtGiaVe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGiaVe.Name = "txtGiaVe";
            this.txtGiaVe.PlaceholderText = "750000";
            this.txtGiaVe.SelectedText = "";
            this.txtGiaVe.Size = new System.Drawing.Size(183, 32);
            this.txtGiaVe.TabIndex = 8;
            // 
            // lblGiaVe
            // 
            this.lblGiaVe.AutoSize = true;
            this.lblGiaVe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGiaVe.Location = new System.Drawing.Point(237, 277);
            this.lblGiaVe.Name = "lblGiaVe";
            this.lblGiaVe.Size = new System.Drawing.Size(95, 20);
            this.lblGiaVe.TabIndex = 7;
            this.lblGiaVe.Text = "Giá vé (VNĐ)";
            // 
            // txtSucChua
            // 
            this.txtSucChua.BorderRadius = 6;
            this.txtSucChua.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSucChua.DefaultText = "";
            this.txtSucChua.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSucChua.Location = new System.Drawing.Point(23, 299);
            this.txtSucChua.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSucChua.Name = "txtSucChua";
            this.txtSucChua.PlaceholderText = "150";
            this.txtSucChua.SelectedText = "";
            this.txtSucChua.Size = new System.Drawing.Size(183, 32);
            this.txtSucChua.TabIndex = 6;
            // 
            // lblSucChua
            // 
            this.lblSucChua.AutoSize = true;
            this.lblSucChua.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSucChua.Location = new System.Drawing.Point(19, 277);
            this.lblSucChua.Name = "lblSucChua";
            this.lblSucChua.Size = new System.Drawing.Size(69, 20);
            this.lblSucChua.TabIndex = 5;
            this.lblSucChua.Text = "Sức chứa";
            // 
            // cboLoaiKV
            // 
            this.cboLoaiKV.BackColor = System.Drawing.Color.Transparent;
            this.cboLoaiKV.BorderRadius = 6;
            this.cboLoaiKV.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLoaiKV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiKV.FocusedColor = System.Drawing.Color.Empty;
            this.cboLoaiKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboLoaiKV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboLoaiKV.ItemHeight = 24;
            this.cboLoaiKV.Items.AddRange(new object[] {
            "VIP",
            "Pho thong",
            "Thuong"});
            this.cboLoaiKV.Location = new System.Drawing.Point(240, 235);
            this.cboLoaiKV.Name = "cboLoaiKV";
            this.cboLoaiKV.Size = new System.Drawing.Size(182, 30);
            this.cboLoaiKV.TabIndex = 4;
            // 
            // lblLoaiKV
            // 
            this.lblLoaiKV.AutoSize = true;
            this.lblLoaiKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLoaiKV.Location = new System.Drawing.Point(237, 213);
            this.lblLoaiKV.Name = "lblLoaiKV";
            this.lblLoaiKV.Size = new System.Drawing.Size(91, 20);
            this.lblLoaiKV.TabIndex = 3;
            this.lblLoaiKV.Text = "Loại khu vực";
            // 
            // txtTenKV
            // 
            this.txtTenKV.BorderRadius = 6;
            this.txtTenKV.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenKV.DefaultText = "";
            this.txtTenKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenKV.Location = new System.Drawing.Point(23, 235);
            this.txtTenKV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTenKV.Name = "txtTenKV";
            this.txtTenKV.PlaceholderText = "VIP";
            this.txtTenKV.SelectedText = "";
            this.txtTenKV.Size = new System.Drawing.Size(183, 32);
            this.txtTenKV.TabIndex = 2;
            // 
            // lblTenKV
            // 
            this.lblTenKV.AutoSize = true;
            this.lblTenKV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTenKV.Location = new System.Drawing.Point(19, 213);
            this.lblTenKV.Name = "lblTenKV";
            this.lblTenKV.Size = new System.Drawing.Size(86, 20);
            this.lblTenKV.TabIndex = 1;
            this.lblTenKV.Text = "Tên khu vực";
            // 
            // lblTitleInfo
            // 
            this.lblTitleInfo.AutoSize = true;
            this.lblTitleInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleInfo.Location = new System.Drawing.Point(17, 21);
            this.lblTitleInfo.Name = "lblTitleInfo";
            this.lblTitleInfo.Size = new System.Drawing.Size(185, 23);
            this.lblTitleInfo.TabIndex = 0;
            this.lblTitleInfo.Text = "ℹ️ Thông tin khu vực";
            // 
            // guna2Button10
            // 
            this.guna2Button10.BorderRadius = 8;
            this.guna2Button10.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.guna2Button10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button10.ForeColor = System.Drawing.Color.White;
            this.guna2Button10.Location = new System.Drawing.Point(69, 420);
            this.guna2Button10.Name = "guna2Button10";
            this.guna2Button10.Size = new System.Drawing.Size(464, 66);
            this.guna2Button10.TabIndex = 10;
            this.guna2Button10.Tag = "CAT";
            this.guna2Button10.Text = "CAT";
            this.guna2Button10.Click += new System.EventHandler(this.Zone_Click);
            // 
            // guna2Button7
            // 
            this.guna2Button7.BorderRadius = 8;
            this.guna2Button7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.guna2Button7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button7.ForeColor = System.Drawing.Color.White;
            this.guna2Button7.Location = new System.Drawing.Point(17, 96);
            this.guna2Button7.Name = "guna2Button7";
            this.guna2Button7.Size = new System.Drawing.Size(70, 278);
            this.guna2Button7.TabIndex = 7;
            this.guna2Button7.Tag = "GA";
            this.guna2Button7.Text = "GA A";
            this.guna2Button7.Click += new System.EventHandler(this.Zone_Click);
            // 
            // guna2Button6
            // 
            this.guna2Button6.BorderRadius = 8;
            this.guna2Button6.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.guna2Button6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button6.ForeColor = System.Drawing.Color.White;
            this.guna2Button6.Location = new System.Drawing.Point(514, 96);
            this.guna2Button6.Name = "guna2Button6";
            this.guna2Button6.Size = new System.Drawing.Size(70, 278);
            this.guna2Button6.TabIndex = 6;
            this.guna2Button6.Tag = "GA";
            this.guna2Button6.Text = "GA B";
            this.guna2Button6.Click += new System.EventHandler(this.Zone_Click);
            // 
            // guna2Button5
            // 
            this.guna2Button5.BorderRadius = 8;
            this.guna2Button5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(51)))), ((int)(((byte)(234)))));
            this.guna2Button5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button5.ForeColor = System.Drawing.Color.White;
            this.guna2Button5.Location = new System.Drawing.Point(93, 160);
            this.guna2Button5.Name = "guna2Button5";
            this.guna2Button5.Size = new System.Drawing.Size(142, 129);
            this.guna2Button5.TabIndex = 5;
            this.guna2Button5.Tag = "VIP";
            this.guna2Button5.Text = "VIP A";
            this.guna2Button5.Click += new System.EventHandler(this.Zone_Click);
            // 
            // guna2Button4
            // 
            this.guna2Button4.BorderRadius = 8;
            this.guna2Button4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(51)))), ((int)(((byte)(234)))));
            this.guna2Button4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button4.ForeColor = System.Drawing.Color.White;
            this.guna2Button4.Location = new System.Drawing.Point(362, 160);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.Size = new System.Drawing.Size(132, 129);
            this.guna2Button4.TabIndex = 4;
            this.guna2Button4.Tag = "VIP";
            this.guna2Button4.Text = "VIP B";
            this.guna2Button4.Click += new System.EventHandler(this.Zone_Click);
            // 
            // guna2Button2
            // 
            this.guna2Button2.BorderRadius = 8;
            this.guna2Button2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.guna2Button2.Location = new System.Drawing.Point(128, 331);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(329, 71);
            this.guna2Button2.TabIndex = 2;
            this.guna2Button2.Tag = "STAGE";
            this.guna2Button2.Text = "STAGE";
            // 
            // btnStage
            // 
            this.btnStage.BorderRadius = 8;
            this.btnStage.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.btnStage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnStage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.btnStage.Location = new System.Drawing.Point(148, 73);
            this.btnStage.Name = "btnStage";
            this.btnStage.Size = new System.Drawing.Size(300, 60);
            this.btnStage.TabIndex = 0;
            this.btnStage.Tag = "STAGE";
            this.btnStage.Text = "STAGE";
            // 
            // guna2Button1
            // 
            this.guna2Button1.BorderRadius = 8;
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.guna2Button1.Location = new System.Drawing.Point(256, 112);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(80, 230);
            this.guna2Button1.TabIndex = 1;
            this.guna2Button1.Tag = "STAGE";
            this.guna2Button1.Text = "LỐI ĐI";
            // 
            // guna2Button3
            // 
            this.guna2Button3.BorderRadius = 8;
            this.guna2Button3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.guna2Button3.Location = new System.Drawing.Point(93, 39);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(413, 45);
            this.guna2Button3.TabIndex = 3;
            this.guna2Button3.Tag = "STAGE";
            this.guna2Button3.Text = "STAGE";
            // 
            // FrmQuanLyKhuVuc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(1335, 853);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlTopAction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmQuanLyKhuVuc";
            this.Text = "Quản lý Khu vực & Giá vé";
            this.Load += new System.EventHandler(this.FrmQuanLyKhuVuc_Load);
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
            this.pnlLeftMap.PerformLayout();
            this.pnlLegend.ResumeLayout(false);
            this.pnlLegend.PerformLayout();
            this.pnlRightInfo.ResumeLayout(false);
            this.pnlRightInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel pnlTopAction;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuKien;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoaiKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSucChua;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConLai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewButtonColumn colSua;
        private System.Windows.Forms.DataGridViewButtonColumn colXoa;
        private Guna.UI2.WinForms.Guna2Panel pnlBottom;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlLeftMap;
        private System.Windows.Forms.Label lblTitleMap;
        private System.Windows.Forms.FlowLayoutPanel pnlLegend;
        private System.Windows.Forms.Label lblLegendTitle;
        private System.Windows.Forms.Panel pnlSeatMap;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlRightInfo;
        private System.Windows.Forms.Label lblTitleInfo;
        private Guna.UI2.WinForms.Guna2TextBox txtTenKV;
        private System.Windows.Forms.Label lblTenKV;
        private Guna.UI2.WinForms.Guna2ComboBox cboLoaiKV;
        private System.Windows.Forms.Label lblLoaiKV;
        private Guna.UI2.WinForms.Guna2TextBox txtGiaVe;
        private System.Windows.Forms.Label lblGiaVe;
        private Guna.UI2.WinForms.Guna2TextBox txtSucChua;
        private System.Windows.Forms.Label lblSucChua;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggleBanVe;
        private System.Windows.Forms.Label lblChoPhepBan;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Button btnStage;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private Guna.UI2.WinForms.Guna2Button guna2Button5;
        private Guna.UI2.WinForms.Guna2Button guna2Button7;
        private Guna.UI2.WinForms.Guna2Button guna2Button6;
        private Guna.UI2.WinForms.Guna2Button guna2Button10;
        private Guna.UI2.WinForms.Guna2Button btnToggleInfo;
    }
}
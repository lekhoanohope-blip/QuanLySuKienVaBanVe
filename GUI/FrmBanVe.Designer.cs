namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmBanVe
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

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.cboSuatDien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSuatDien = new System.Windows.Forms.Label();
            this.cboSuKien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMainBody = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlSeatMapContainer = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlSeatMapCanvas = new System.Windows.Forms.Panel();
            this.lblSeatMapTitle = new System.Windows.Forms.Label();
            this.pnlRightAction = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlPaymentCard = new Guna.UI2.WinForms.Guna2Panel();
            this.btnDatVe = new Guna.UI2.WinForms.Guna2Button();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.pnlQuantityCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblGhiChuSoLuong = new System.Windows.Forms.Label();
            this.btnTang = new Guna.UI2.WinForms.Guna2Button();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.btnGiam = new Guna.UI2.WinForms.Guna2Button();
            this.lblQuantityTitle = new System.Windows.Forms.Label();
            this.pnlTicketInfoCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSelectedGiaVe = new System.Windows.Forms.Label();
            this.lblSelectedSucChua = new System.Windows.Forms.Label();
            this.lblSelectedHangVe = new System.Windows.Forms.Label();
            this.lblSelectedTenKV = new System.Windows.Forms.Label();
            this.lblCardTitle = new System.Windows.Forms.Label();
            this.pnlLegendCard = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlKhuVucSummaryList = new System.Windows.Forms.Panel();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.pnlLegendDots = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLegendTitle = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlMainBody.SuspendLayout();
            this.pnlSeatMapContainer.SuspendLayout();
            this.pnlRightAction.SuspendLayout();
            this.pnlPaymentCard.SuspendLayout();
            this.pnlQuantityCard.SuspendLayout();
            this.pnlTicketInfoCard.SuspendLayout();
            this.pnlLegendCard.SuspendLayout();
            this.pnlKhuVucSummaryList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTop.BorderThickness = 1;
            this.pnlTop.Controls.Add(this.txtTimKiem);
            this.pnlTop.Controls.Add(this.cboSuatDien);
            this.pnlTop.Controls.Add(this.lblSuatDien);
            this.pnlTop.Controls.Add(this.cboSuKien);
            this.pnlTop.Controls.Add(this.lblSuKien);
            this.pnlTop.Controls.Add(this.lblSubTitle);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 110);
            this.pnlTop.TabIndex = 0;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtTimKiem.BorderRadius = 6;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtTimKiem.Location = new System.Drawing.Point(830, 65);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtTimKiem.PlaceholderText = "Tìm nhanh khu vực / vé...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(220, 34);
            this.txtTimKiem.TabIndex = 6;
            // 
            // cboSuatDien
            // 
            this.cboSuatDien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuatDien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.cboSuatDien.BorderRadius = 6;
            this.cboSuatDien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuatDien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuatDien.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.cboSuatDien.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.cboSuatDien.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.cboSuatDien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboSuatDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cboSuatDien.ItemHeight = 28;
            this.cboSuatDien.Location = new System.Drawing.Point(455, 65);
            this.cboSuatDien.Name = "cboSuatDien";
            this.cboSuatDien.Size = new System.Drawing.Size(360, 34);
            this.cboSuatDien.TabIndex = 5;
            // 
            // lblSuatDien
            // 
            this.lblSuatDien.AutoSize = true;
            this.lblSuatDien.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSuatDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblSuatDien.Location = new System.Drawing.Point(380, 73);
            this.lblSuatDien.Name = "lblSuatDien";
            this.lblSuatDien.Size = new System.Drawing.Size(82, 21);
            this.lblSuatDien.TabIndex = 4;
            this.lblSuatDien.Text = "Suất diễn:";
            // 
            // cboSuKien
            // 
            this.cboSuKien.BackColor = System.Drawing.Color.Transparent;
            this.cboSuKien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.cboSuKien.BorderRadius = 6;
            this.cboSuKien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.cboSuKien.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.cboSuKien.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cboSuKien.ItemHeight = 28;
            this.cboSuKien.Location = new System.Drawing.Point(85, 65);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(280, 34);
            this.cboSuKien.TabIndex = 3;
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblSuKien.Location = new System.Drawing.Point(22, 73);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(67, 21);
            this.lblSuKien.TabIndex = 2;
            this.lblSuKien.Text = "Sự kiện:";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubTitle.Location = new System.Drawing.Point(22, 45);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(0, 21);
            this.lblSubTitle.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Bán vé sự kiện";
            // 
            // pnlMainBody
            // 
            this.pnlMainBody.Controls.Add(this.pnlSeatMapContainer);
            this.pnlMainBody.Controls.Add(this.pnlRightAction);
            this.pnlMainBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainBody.Location = new System.Drawing.Point(0, 110);
            this.pnlMainBody.Name = "pnlMainBody";
            this.pnlMainBody.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMainBody.Size = new System.Drawing.Size(1200, 690);
            this.pnlMainBody.TabIndex = 1;
            // 
            // pnlSeatMapContainer
            // 
            this.pnlSeatMapContainer.BackColor = System.Drawing.Color.White;
            this.pnlSeatMapContainer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSeatMapContainer.BorderRadius = 10;
            this.pnlSeatMapContainer.BorderThickness = 1;
            this.pnlSeatMapContainer.Controls.Add(this.pnlSeatMapCanvas);
            this.pnlSeatMapContainer.Controls.Add(this.lblSeatMapTitle);
            this.pnlSeatMapContainer.Location = new System.Drawing.Point(16, 16);
            this.pnlSeatMapContainer.Name = "pnlSeatMapContainer";
            this.pnlSeatMapContainer.Padding = new System.Windows.Forms.Padding(12);
            this.pnlSeatMapContainer.Size = new System.Drawing.Size(700, 658);
            this.pnlSeatMapContainer.TabIndex = 0;
            // 
            // pnlSeatMapCanvas
            // 
            this.pnlSeatMapCanvas.AutoScroll = true;
            this.pnlSeatMapCanvas.BackColor = System.Drawing.Color.White;
            this.pnlSeatMapCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSeatMapCanvas.Location = new System.Drawing.Point(12, 40);
            this.pnlSeatMapCanvas.Name = "pnlSeatMapCanvas";
            this.pnlSeatMapCanvas.Size = new System.Drawing.Size(676, 606);
            this.pnlSeatMapCanvas.TabIndex = 1;
            // 
            // lblSeatMapTitle
            // 
            this.lblSeatMapTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSeatMapTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblSeatMapTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblSeatMapTitle.Location = new System.Drawing.Point(12, 12);
            this.lblSeatMapTitle.Name = "lblSeatMapTitle";
            this.lblSeatMapTitle.Size = new System.Drawing.Size(676, 28);
            this.lblSeatMapTitle.TabIndex = 0;
            this.lblSeatMapTitle.Text = "SƠ ĐỒ SEATMAP KHU VỰC - Vui lòng click chọn khu vực";
            // 
            // pnlRightAction
            // 
            this.pnlRightAction.AutoScroll = true;
            this.pnlRightAction.Controls.Add(this.pnlPaymentCard);
            this.pnlRightAction.Controls.Add(this.pnlQuantityCard);
            this.pnlRightAction.Controls.Add(this.pnlTicketInfoCard);
            this.pnlRightAction.Controls.Add(this.pnlLegendCard);
            this.pnlRightAction.Location = new System.Drawing.Point(725, 16);
            this.pnlRightAction.Name = "pnlRightAction";
            this.pnlRightAction.Size = new System.Drawing.Size(459, 658);
            this.pnlRightAction.TabIndex = 1;
            // 
            // pnlPaymentCard
            // 
            this.pnlPaymentCard.BackColor = System.Drawing.Color.White;
            this.pnlPaymentCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlPaymentCard.BorderRadius = 8;
            this.pnlPaymentCard.BorderThickness = 1;
            this.pnlPaymentCard.Controls.Add(this.btnDatVe);
            this.pnlPaymentCard.Controls.Add(this.lblTongTien);
            this.pnlPaymentCard.Controls.Add(this.lblTotalTitle);
            this.pnlPaymentCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPaymentCard.Location = new System.Drawing.Point(0, 449);
            this.pnlPaymentCard.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlPaymentCard.Name = "pnlPaymentCard";
            this.pnlPaymentCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlPaymentCard.Size = new System.Drawing.Size(459, 176);
            this.pnlPaymentCard.TabIndex = 3;
            // 
            // btnDatVe
            // 
            this.btnDatVe.BorderRadius = 8;
            this.btnDatVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnDatVe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDatVe.ForeColor = System.Drawing.Color.White;
            this.btnDatVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.btnDatVe.Location = new System.Drawing.Point(21, 106);
            this.btnDatVe.Name = "btnDatVe";
            this.btnDatVe.Size = new System.Drawing.Size(410, 48);
            this.btnDatVe.TabIndex = 2;
            this.btnDatVe.Text = "ĐẶT VÉ";
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblTongTien.Location = new System.Drawing.Point(15, 45);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(57, 37);
            this.lblTongTien.TabIndex = 1;
            this.lblTongTien.Text = "0 đ";
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblTotalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTotalTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Size = new System.Drawing.Size(435, 20);
            this.lblTotalTitle.TabIndex = 0;
            this.lblTotalTitle.Text = "4. Tổng giá vé cần thanh toán";
            // 
            // pnlQuantityCard
            // 
            this.pnlQuantityCard.BackColor = System.Drawing.Color.White;
            this.pnlQuantityCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlQuantityCard.BorderRadius = 8;
            this.pnlQuantityCard.BorderThickness = 1;
            this.pnlQuantityCard.Controls.Add(this.lblGhiChuSoLuong);
            this.pnlQuantityCard.Controls.Add(this.btnTang);
            this.pnlQuantityCard.Controls.Add(this.lblSoLuong);
            this.pnlQuantityCard.Controls.Add(this.btnGiam);
            this.pnlQuantityCard.Controls.Add(this.lblQuantityTitle);
            this.pnlQuantityCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuantityCard.Location = new System.Drawing.Point(0, 332);
            this.pnlQuantityCard.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlQuantityCard.Name = "pnlQuantityCard";
            this.pnlQuantityCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlQuantityCard.Size = new System.Drawing.Size(459, 117);
            this.pnlQuantityCard.TabIndex = 2;
            // 
            // lblGhiChuSoLuong
            // 
            this.lblGhiChuSoLuong.AutoSize = true;
            this.lblGhiChuSoLuong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblGhiChuSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblGhiChuSoLuong.Location = new System.Drawing.Point(215, 58);
            this.lblGhiChuSoLuong.Name = "lblGhiChuSoLuong";
            this.lblGhiChuSoLuong.Size = new System.Drawing.Size(214, 20);
            this.lblGhiChuSoLuong.TabIndex = 4;
            this.lblGhiChuSoLuong.Text = "(Tối thiểu: 1 vé, tối đa: theo chỗ)";
            // 
            // btnTang
            // 
            this.btnTang.BorderRadius = 6;
            this.btnTang.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnTang.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnTang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTang.Location = new System.Drawing.Point(155, 45);
            this.btnTang.Name = "btnTang";
            this.btnTang.Size = new System.Drawing.Size(45, 42);
            this.btnTang.TabIndex = 3;
            this.btnTang.Text = "+";
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.lblSoLuong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSoLuong.Location = new System.Drawing.Point(75, 45);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(70, 42);
            this.lblSoLuong.TabIndex = 2;
            this.lblSoLuong.Text = "1";
            this.lblSoLuong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGiam
            // 
            this.btnGiam.BorderRadius = 6;
            this.btnGiam.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnGiam.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnGiam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnGiam.Location = new System.Drawing.Point(18, 45);
            this.btnGiam.Name = "btnGiam";
            this.btnGiam.Size = new System.Drawing.Size(45, 42);
            this.btnGiam.TabIndex = 1;
            this.btnGiam.Text = "-";
            // 
            // lblQuantityTitle
            // 
            this.lblQuantityTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQuantityTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblQuantityTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblQuantityTitle.Location = new System.Drawing.Point(12, 12);
            this.lblQuantityTitle.Name = "lblQuantityTitle";
            this.lblQuantityTitle.Size = new System.Drawing.Size(435, 20);
            this.lblQuantityTitle.TabIndex = 0;
            this.lblQuantityTitle.Text = "3. Số lượng vé";
            // 
            // pnlTicketInfoCard
            // 
            this.pnlTicketInfoCard.BackColor = System.Drawing.Color.White;
            this.pnlTicketInfoCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTicketInfoCard.BorderRadius = 8;
            this.pnlTicketInfoCard.BorderThickness = 1;
            this.pnlTicketInfoCard.Controls.Add(this.lblSelectedGiaVe);
            this.pnlTicketInfoCard.Controls.Add(this.lblSelectedSucChua);
            this.pnlTicketInfoCard.Controls.Add(this.lblSelectedHangVe);
            this.pnlTicketInfoCard.Controls.Add(this.lblSelectedTenKV);
            this.pnlTicketInfoCard.Controls.Add(this.lblCardTitle);
            this.pnlTicketInfoCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTicketInfoCard.Location = new System.Drawing.Point(0, 185);
            this.pnlTicketInfoCard.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlTicketInfoCard.Name = "pnlTicketInfoCard";
            this.pnlTicketInfoCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlTicketInfoCard.Size = new System.Drawing.Size(459, 147);
            this.pnlTicketInfoCard.TabIndex = 1;
            // 
            // lblSelectedGiaVe
            // 
            this.lblSelectedGiaVe.AutoSize = true;
            this.lblSelectedGiaVe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedGiaVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
            this.lblSelectedGiaVe.Location = new System.Drawing.Point(14, 107);
            this.lblSelectedGiaVe.Name = "lblSelectedGiaVe";
            this.lblSelectedGiaVe.Size = new System.Drawing.Size(129, 28);
            this.lblSelectedGiaVe.TabIndex = 4;
            this.lblSelectedGiaVe.Text = "Đơn giá: 0 đ";
            // 
            // lblSelectedSucChua
            // 
            this.lblSelectedSucChua.AutoSize = true;
            this.lblSelectedSucChua.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSelectedSucChua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSelectedSucChua.Location = new System.Drawing.Point(215, 77);
            this.lblSelectedSucChua.Name = "lblSelectedSucChua";
            this.lblSelectedSucChua.Size = new System.Drawing.Size(104, 21);
            this.lblSelectedSucChua.TabIndex = 3;
            this.lblSelectedSucChua.Text = "Còn trống: --";
            // 
            // lblSelectedHangVe
            // 
            this.lblSelectedHangVe.AutoSize = true;
            this.lblSelectedHangVe.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSelectedHangVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSelectedHangVe.Location = new System.Drawing.Point(12, 77);
            this.lblSelectedHangVe.Name = "lblSelectedHangVe";
            this.lblSelectedHangVe.Size = new System.Drawing.Size(90, 21);
            this.lblSelectedHangVe.TabIndex = 2;
            this.lblSelectedHangVe.Text = "Hạng vé: --";
            // 
            // lblSelectedTenKV
            // 
            this.lblSelectedTenKV.AutoSize = true;
            this.lblSelectedTenKV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedTenKV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblSelectedTenKV.Location = new System.Drawing.Point(14, 38);
            this.lblSelectedTenKV.Name = "lblSelectedTenKV";
            this.lblSelectedTenKV.Size = new System.Drawing.Size(214, 28);
            this.lblSelectedTenKV.TabIndex = 1;
            this.lblSelectedTenKV.Text = "Khu vực: (Chưa chọn)";
            // 
            // lblCardTitle
            // 
            this.lblCardTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCardTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblCardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCardTitle.Location = new System.Drawing.Point(12, 12);
            this.lblCardTitle.Name = "lblCardTitle";
            this.lblCardTitle.Size = new System.Drawing.Size(435, 20);
            this.lblCardTitle.TabIndex = 0;
            this.lblCardTitle.Text = "2. Thông tin vé chi tiết";
            // 
            // pnlLegendCard
            // 
            this.pnlLegendCard.BackColor = System.Drawing.Color.White;
            this.pnlLegendCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlLegendCard.BorderRadius = 8;
            this.pnlLegendCard.BorderThickness = 1;
            this.pnlLegendCard.Controls.Add(this.pnlKhuVucSummaryList);
            this.pnlLegendCard.Controls.Add(this.pnlLegendDots);
            this.pnlLegendCard.Controls.Add(this.lblLegendTitle);
            this.pnlLegendCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLegendCard.Location = new System.Drawing.Point(0, 0);
            this.pnlLegendCard.Name = "pnlLegendCard";
            this.pnlLegendCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlLegendCard.Size = new System.Drawing.Size(459, 185);
            this.pnlLegendCard.TabIndex = 0;
            // 
            // pnlKhuVucSummaryList
            // 
            this.pnlKhuVucSummaryList.Controls.Add(this.dgvSummary);
            this.pnlKhuVucSummaryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKhuVucSummaryList.Location = new System.Drawing.Point(12, 64);
            this.pnlKhuVucSummaryList.Name = "pnlKhuVucSummaryList";
            this.pnlKhuVucSummaryList.Size = new System.Drawing.Size(435, 109);
            this.pnlKhuVucSummaryList.TabIndex = 2;
            // 
            // dgvSummary
            // 
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSummary.Location = new System.Drawing.Point(0, 0);
            this.dgvSummary.MultiSelect = false;
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.ReadOnly = true;
            this.dgvSummary.RowHeadersVisible = false;
            this.dgvSummary.RowHeadersWidth = 51;
            this.dgvSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSummary.Size = new System.Drawing.Size(435, 109);
            this.dgvSummary.TabIndex = 0;
            // 
            // pnlLegendDots
            // 
            this.pnlLegendDots.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLegendDots.Location = new System.Drawing.Point(12, 34);
            this.pnlLegendDots.Name = "pnlLegendDots";
            this.pnlLegendDots.Size = new System.Drawing.Size(435, 30);
            this.pnlLegendDots.TabIndex = 1;
            // 
            // lblLegendTitle
            // 
            this.lblLegendTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLegendTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblLegendTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblLegendTitle.Location = new System.Drawing.Point(12, 12);
            this.lblLegendTitle.Name = "lblLegendTitle";
            this.lblLegendTitle.Size = new System.Drawing.Size(435, 22);
            this.lblLegendTitle.TabIndex = 0;
            this.lblLegendTitle.Text = "1. Chú thích & Bảng giá khu vực";
            this.lblLegendTitle.Click += new System.EventHandler(this.lblLegendTitle_Click);
            // 
            // FrmBanVe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlMainBody);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmBanVe";
            this.Text = "Bán vé sự kiện";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMainBody.ResumeLayout(false);
            this.pnlSeatMapContainer.ResumeLayout(false);
            this.pnlRightAction.ResumeLayout(false);
            this.pnlPaymentCard.ResumeLayout(false);
            this.pnlPaymentCard.PerformLayout();
            this.pnlQuantityCard.ResumeLayout(false);
            this.pnlQuantityCard.PerformLayout();
            this.pnlTicketInfoCard.ResumeLayout(false);
            this.pnlTicketInfoCard.PerformLayout();
            this.pnlLegendCard.ResumeLayout(false);
            this.pnlKhuVucSummaryList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblSuKien;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuKien;
        private System.Windows.Forms.Label lblSuatDien;
        private Guna.UI2.WinForms.Guna2ComboBox cboSuatDien;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Panel pnlMainBody;
        private Guna.UI2.WinForms.Guna2Panel pnlSeatMapContainer;
        private System.Windows.Forms.Label lblSeatMapTitle;
        private System.Windows.Forms.Panel pnlSeatMapCanvas;
        private Guna.UI2.WinForms.Guna2Panel pnlRightAction;
        private Guna.UI2.WinForms.Guna2Panel pnlLegendCard;
        private System.Windows.Forms.Label lblLegendTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlLegendDots;
        private System.Windows.Forms.Panel pnlKhuVucSummaryList;
        private System.Windows.Forms.DataGridView dgvSummary;
        private Guna.UI2.WinForms.Guna2Panel pnlTicketInfoCard;
        private System.Windows.Forms.Label lblCardTitle;
        private System.Windows.Forms.Label lblSelectedTenKV;
        private System.Windows.Forms.Label lblSelectedHangVe;
        private System.Windows.Forms.Label lblSelectedSucChua;
        private System.Windows.Forms.Label lblSelectedGiaVe;
        private Guna.UI2.WinForms.Guna2Panel pnlQuantityCard;
        private System.Windows.Forms.Label lblQuantityTitle;
        private Guna.UI2.WinForms.Guna2Button btnGiam;
        private System.Windows.Forms.Label lblSoLuong;
        private Guna.UI2.WinForms.Guna2Button btnTang;
        private System.Windows.Forms.Label lblGhiChuSoLuong;
        private Guna.UI2.WinForms.Guna2Panel pnlPaymentCard;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTongTien;
        private Guna.UI2.WinForms.Guna2Button btnDatVe;
    }
}

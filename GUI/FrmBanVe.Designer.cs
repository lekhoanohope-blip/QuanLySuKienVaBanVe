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
            this.pnlMapHeader = new System.Windows.Forms.Panel();
            this.lblMapSubHeader = new System.Windows.Forms.Label();
            this.lblMapHeader = new System.Windows.Forms.Label();
            this.pnlRightAction = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlRightCard = new Guna.UI2.WinForms.Guna2Panel();
            this.flpPriceList = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlBottomAction = new System.Windows.Forms.Panel();
            this.btnInVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnHintSelectZone = new Guna.UI2.WinForms.Guna2Button();
            this.lblPriceListTitle = new System.Windows.Forms.Label();
            this.pnlSidebarDivider = new System.Windows.Forms.Panel();
            this.pnlEventLocationRow = new System.Windows.Forms.Panel();
            this.lblEventLocation = new System.Windows.Forms.Label();
            this.pbEventLocIcon = new System.Windows.Forms.PictureBox();
            this.pnlEventTimeRow = new System.Windows.Forms.Panel();
            this.lblEventTime = new System.Windows.Forms.Label();
            this.pbEventTimeIcon = new System.Windows.Forms.PictureBox();
            this.lblEventTitle = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlMainBody.SuspendLayout();
            this.pnlSeatMapContainer.SuspendLayout();
            this.pnlMapHeader.SuspendLayout();
            this.pnlRightAction.SuspendLayout();
            this.pnlRightCard.SuspendLayout();
            this.pnlBottomAction.SuspendLayout();
            this.pnlEventLocationRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEventLocIcon)).BeginInit();
            this.pnlEventTimeRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEventTimeIcon)).BeginInit();
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
            this.pnlSeatMapContainer.Controls.Add(this.pnlMapHeader);
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
            this.pnlSeatMapCanvas.Location = new System.Drawing.Point(12, 64);
            this.pnlSeatMapCanvas.Name = "pnlSeatMapCanvas";
            this.pnlSeatMapCanvas.Size = new System.Drawing.Size(676, 582);
            this.pnlSeatMapCanvas.TabIndex = 1;
            // 
            // pnlMapHeader
            // 
            this.pnlMapHeader.Controls.Add(this.lblMapSubHeader);
            this.pnlMapHeader.Controls.Add(this.lblMapHeader);
            this.pnlMapHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMapHeader.Location = new System.Drawing.Point(12, 12);
            this.pnlMapHeader.Name = "pnlMapHeader";
            this.pnlMapHeader.Size = new System.Drawing.Size(676, 52);
            this.pnlMapHeader.TabIndex = 0;
            // 
            // lblMapSubHeader
            // 
            this.lblMapSubHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMapSubHeader.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMapSubHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblMapSubHeader.Location = new System.Drawing.Point(0, 26);
            this.lblMapSubHeader.Name = "lblMapSubHeader";
            this.lblMapSubHeader.Size = new System.Drawing.Size(676, 22);
            this.lblMapSubHeader.TabIndex = 1;
            this.lblMapSubHeader.Text = "Bấm vào khu vực để chọn vé";
            this.lblMapSubHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMapHeader
            // 
            this.lblMapHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMapHeader.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblMapHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblMapHeader.Location = new System.Drawing.Point(0, 0);
            this.lblMapHeader.Name = "lblMapHeader";
            this.lblMapHeader.Size = new System.Drawing.Size(676, 26);
            this.lblMapHeader.TabIndex = 0;
            this.lblMapHeader.Text = "Chọn khu vực";
            this.lblMapHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlRightAction
            // 
            this.pnlRightAction.AutoScroll = true;
            this.pnlRightAction.Controls.Add(this.pnlRightCard);
            this.pnlRightAction.Location = new System.Drawing.Point(725, 16);
            this.pnlRightAction.Name = "pnlRightAction";
            this.pnlRightAction.Size = new System.Drawing.Size(459, 658);
            this.pnlRightAction.TabIndex = 1;
            // 
            // pnlRightCard
            // 
            this.pnlRightCard.BackColor = System.Drawing.Color.White;
            this.pnlRightCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlRightCard.BorderRadius = 10;
            this.pnlRightCard.BorderThickness = 1;
            this.pnlRightCard.Controls.Add(this.flpPriceList);
            this.pnlRightCard.Controls.Add(this.pnlBottomAction);
            this.pnlRightCard.Controls.Add(this.lblPriceListTitle);
            this.pnlRightCard.Controls.Add(this.pnlSidebarDivider);
            this.pnlRightCard.Controls.Add(this.pnlEventLocationRow);
            this.pnlRightCard.Controls.Add(this.pnlEventTimeRow);
            this.pnlRightCard.Controls.Add(this.lblEventTitle);
            this.pnlRightCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightCard.Location = new System.Drawing.Point(0, 0);
            this.pnlRightCard.Name = "pnlRightCard";
            this.pnlRightCard.Padding = new System.Windows.Forms.Padding(16);
            this.pnlRightCard.Size = new System.Drawing.Size(459, 658);
            this.pnlRightCard.TabIndex = 0;
            // 
            // flpPriceList
            // 
            this.flpPriceList.AutoScroll = true;
            this.flpPriceList.BackColor = System.Drawing.Color.White;
            this.flpPriceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPriceList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpPriceList.Location = new System.Drawing.Point(16, 199);
            this.flpPriceList.Name = "flpPriceList";
            this.flpPriceList.Size = new System.Drawing.Size(427, 396);
            this.flpPriceList.TabIndex = 6;
            this.flpPriceList.WrapContents = false;
            // 
            // pnlBottomAction
            // 
            this.pnlBottomAction.Controls.Add(this.btnInVe);
            this.pnlBottomAction.Controls.Add(this.btnHintSelectZone);
            this.pnlBottomAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomAction.Location = new System.Drawing.Point(16, 595);
            this.pnlBottomAction.Name = "pnlBottomAction";
            this.pnlBottomAction.Size = new System.Drawing.Size(427, 47);
            this.pnlBottomAction.TabIndex = 5;
            this.pnlBottomAction.Visible = false;
            // 
            // btnInVe
            // 
            this.btnInVe.BorderRadius = 6;
            this.btnInVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInVe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnInVe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnInVe.ForeColor = System.Drawing.Color.White;
            this.btnInVe.Location = new System.Drawing.Point(0, 44);
            this.btnInVe.Name = "btnInVe";
            this.btnInVe.Size = new System.Drawing.Size(427, 3);
            this.btnInVe.TabIndex = 1;
            this.btnInVe.Text = "In vé vừa đặt (Đang phát triển)";
            this.btnInVe.Visible = false;
            this.btnInVe.Click += new System.EventHandler(this.btnInVe_Click);
            // 
            // btnHintSelectZone
            // 
            this.btnHintSelectZone.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnHintSelectZone.BorderRadius = 6;
            this.btnHintSelectZone.BorderThickness = 1;
            this.btnHintSelectZone.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHintSelectZone.Enabled = false;
            this.btnHintSelectZone.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnHintSelectZone.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnHintSelectZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnHintSelectZone.Location = new System.Drawing.Point(0, 0);
            this.btnHintSelectZone.Name = "btnHintSelectZone";
            this.btnHintSelectZone.Size = new System.Drawing.Size(427, 44);
            this.btnHintSelectZone.TabIndex = 0;
            this.btnHintSelectZone.Text = "Vui lòng chọn khu vực trên sơ đồ ≫";
            this.btnHintSelectZone.Visible = false;
            // 
            // lblPriceListTitle
            // 
            this.lblPriceListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPriceListTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPriceListTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblPriceListTitle.Location = new System.Drawing.Point(16, 167);
            this.lblPriceListTitle.Name = "lblPriceListTitle";
            this.lblPriceListTitle.Size = new System.Drawing.Size(427, 32);
            this.lblPriceListTitle.TabIndex = 4;
            this.lblPriceListTitle.Text = "Giá vé";
            // 
            // pnlSidebarDivider
            // 
            this.pnlSidebarDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSidebarDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarDivider.Location = new System.Drawing.Point(16, 156);
            this.pnlSidebarDivider.Name = "pnlSidebarDivider";
            this.pnlSidebarDivider.Size = new System.Drawing.Size(427, 11);
            this.pnlSidebarDivider.TabIndex = 3;
            // 
            // pnlEventLocationRow
            // 
            this.pnlEventLocationRow.Controls.Add(this.lblEventLocation);
            this.pnlEventLocationRow.Controls.Add(this.pbEventLocIcon);
            this.pnlEventLocationRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventLocationRow.Location = new System.Drawing.Point(16, 110);
            this.pnlEventLocationRow.Name = "pnlEventLocationRow";
            this.pnlEventLocationRow.Size = new System.Drawing.Size(427, 46);
            this.pnlEventLocationRow.TabIndex = 2;
            // 
            // lblEventLocation
            // 
            this.lblEventLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventLocation.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblEventLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblEventLocation.Location = new System.Drawing.Point(26, 1);
            this.lblEventLocation.Name = "lblEventLocation";
            this.lblEventLocation.Size = new System.Drawing.Size(401, 44);
            this.lblEventLocation.TabIndex = 1;
            this.lblEventLocation.Text = "Hội trường A - HCMUE (280 An Dương Vương, Q.5)";
            // 
            // pbEventLocIcon
            // 
            this.pbEventLocIcon.Location = new System.Drawing.Point(0, 3);
            this.pbEventLocIcon.Name = "pbEventLocIcon";
            this.pbEventLocIcon.Size = new System.Drawing.Size(18, 18);
            this.pbEventLocIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEventLocIcon.TabIndex = 0;
            this.pbEventLocIcon.TabStop = false;
            // 
            // pnlEventTimeRow
            // 
            this.pnlEventTimeRow.Controls.Add(this.lblEventTime);
            this.pnlEventTimeRow.Controls.Add(this.pbEventTimeIcon);
            this.pnlEventTimeRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventTimeRow.Location = new System.Drawing.Point(16, 82);
            this.pnlEventTimeRow.Name = "pnlEventTimeRow";
            this.pnlEventTimeRow.Size = new System.Drawing.Size(427, 28);
            this.pnlEventTimeRow.TabIndex = 1;
            // 
            // lblEventTime
            // 
            this.lblEventTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventTime.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblEventTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblEventTime.Location = new System.Drawing.Point(26, 0);
            this.lblEventTime.Name = "lblEventTime";
            this.lblEventTime.Size = new System.Drawing.Size(401, 24);
            this.lblEventTime.TabIndex = 1;
            this.lblEventTime.Text = "19:30 - 22:00, 20 Tháng 11, 2026";
            this.lblEventTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbEventTimeIcon
            // 
            this.pbEventTimeIcon.Location = new System.Drawing.Point(0, 6);
            this.pbEventTimeIcon.Name = "pbEventTimeIcon";
            this.pbEventTimeIcon.Size = new System.Drawing.Size(18, 18);
            this.pbEventTimeIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEventTimeIcon.TabIndex = 0;
            this.pbEventTimeIcon.TabStop = false;
            // 
            // lblEventTitle
            // 
            this.lblEventTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEventTitle.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblEventTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblEventTitle.Location = new System.Drawing.Point(16, 16);
            this.lblEventTitle.Name = "lblEventTitle";
            this.lblEventTitle.Size = new System.Drawing.Size(427, 66);
            this.lblEventTitle.TabIndex = 0;
            this.lblEventTitle.Text = "Tên sự kiện";
            // 
            // FrmBanVe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlMainBody);
            this.Controls.Add(this.pnlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmBanVe";
            this.Text = "Bán vé sự kiện";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMainBody.ResumeLayout(false);
            this.pnlSeatMapContainer.ResumeLayout(false);
            this.pnlMapHeader.ResumeLayout(false);
            this.pnlRightAction.ResumeLayout(false);
            this.pnlRightCard.ResumeLayout(false);
            this.pnlBottomAction.ResumeLayout(false);
            this.pnlEventLocationRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbEventLocIcon)).EndInit();
            this.pnlEventTimeRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbEventTimeIcon)).EndInit();
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
        private System.Windows.Forms.Panel pnlMapHeader;
        private System.Windows.Forms.Label lblMapHeader;
        private System.Windows.Forms.Label lblMapSubHeader;
        private System.Windows.Forms.Panel pnlSeatMapCanvas;
        private Guna.UI2.WinForms.Guna2Panel pnlRightAction;
        private Guna.UI2.WinForms.Guna2Panel pnlRightCard;
        private System.Windows.Forms.Label lblEventTitle;
        private System.Windows.Forms.Panel pnlEventTimeRow;
        private System.Windows.Forms.PictureBox pbEventTimeIcon;
        private System.Windows.Forms.Label lblEventTime;
        private System.Windows.Forms.Panel pnlEventLocationRow;
        private System.Windows.Forms.PictureBox pbEventLocIcon;
        private System.Windows.Forms.Label lblEventLocation;
        private System.Windows.Forms.Panel pnlSidebarDivider;
        private System.Windows.Forms.Label lblPriceListTitle;
        private System.Windows.Forms.FlowLayoutPanel flpPriceList;
        private System.Windows.Forms.Panel pnlBottomAction;
        private Guna.UI2.WinForms.Guna2Button btnHintSelectZone;
        private Guna.UI2.WinForms.Guna2Button btnInVe;
    }
}

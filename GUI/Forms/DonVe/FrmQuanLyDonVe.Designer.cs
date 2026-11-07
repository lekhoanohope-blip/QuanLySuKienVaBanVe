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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlPageContent = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pnlMasterCard = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvDonVe = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlMasterHeader = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblMasterTitle = new System.Windows.Forms.Label();
            this.pnlDetailCard = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvChiTietVe = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlDetailActions = new System.Windows.Forms.Panel();
            this.btnHuyVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnDoiVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnInVe = new Guna.UI2.WinForms.Guna2Button();
            this.pnlDetailInfo = new System.Windows.Forms.Panel();

            this.btnCopyMaVe = new Guna.UI2.WinForms.Guna2Button();
            this.lblTicketsTitle = new System.Windows.Forms.Label();
            this.pnlDetailDivider = new System.Windows.Forms.Panel();
            this.lblRuleBadge = new System.Windows.Forms.Label();
            this.lblDetailEventTime = new System.Windows.Forms.Label();
            this.lblDetailEventName = new System.Windows.Forms.Label();
            this.lblCustomerEmail = new System.Windows.Forms.Label();
            this.lblCustomerPhone = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblDetailBadge = new System.Windows.Forms.Label();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.cboLocTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.searchTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTitle.SuspendLayout();
            this.pnlPageContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlMasterCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonVe)).BeginInit();
            this.pnlMasterHeader.SuspendLayout();
            this.pnlDetailCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietVe)).BeginInit();
            this.pnlDetailActions.SuspendLayout();
            this.pnlDetailInfo.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(1190, 50);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 2);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(117, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Đơn vé";
            // 
            // pnlPageContent
            // 
            this.pnlPageContent.Controls.Add(this.splitContainer);
            this.pnlPageContent.Controls.Add(this.pnlHeader);
            this.pnlPageContent.Controls.Add(this.pnlTitle);
            this.pnlPageContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPageContent.Location = new System.Drawing.Point(30, 20);
            this.pnlPageContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlPageContent.Name = "pnlPageContent";
            this.pnlPageContent.Size = new System.Drawing.Size(1190, 825);
            this.pnlPageContent.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 115);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnlMasterCard);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.pnlDetailCard);
            this.splitContainer.Size = new System.Drawing.Size(1190, 710);
            this.splitContainer.SplitterDistance = 625;
            this.splitContainer.SplitterWidth = 12;
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
            this.pnlMasterCard.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMasterCard.Name = "pnlMasterCard";
            this.pnlMasterCard.Size = new System.Drawing.Size(625, 710);
            this.pnlMasterCard.TabIndex = 0;
            // 
            // dgvDonVe
            // 
            this.dgvDonVe.AllowUserToAddRows = false;
            this.dgvDonVe.AllowUserToDeleteRows = false;
            this.dgvDonVe.AllowUserToResizeRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            this.dgvDonVe.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDonVe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvDonVe.ColumnHeadersHeight = 30;
            this.dgvDonVe.ColumnHeadersVisible = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDonVe.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvDonVe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDonVe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvDonVe.Location = new System.Drawing.Point(0, 50);
            this.dgvDonVe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDonVe.MultiSelect = false;
            this.dgvDonVe.Name = "dgvDonVe";
            this.dgvDonVe.ReadOnly = true;
            this.dgvDonVe.RowHeadersVisible = false;
            this.dgvDonVe.RowHeadersWidth = 51;
            this.dgvDonVe.RowTemplate.Height = 74;
            this.dgvDonVe.Size = new System.Drawing.Size(625, 660);
            this.dgvDonVe.TabIndex = 1;
            this.dgvDonVe.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDonVe.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvDonVe.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvDonVe.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvDonVe.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.dgvDonVe.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDonVe.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvDonVe.ThemeStyle.ReadOnly = true;
            this.dgvDonVe.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvDonVe.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.dgvDonVe.ThemeStyle.RowsStyle.Height = 74;
            this.dgvDonVe.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.dgvDonVe.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvDonVe.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDonVe_CellPainting);
            this.dgvDonVe.SelectionChanged += new System.EventHandler(this.dgvDonVe_SelectionChanged);
            this.dgvDonVe.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvDonVe_Paint);
            // 
            // pnlMasterHeader
            // 
            this.pnlMasterHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlMasterHeader.Controls.Add(this.lblCount);
            this.pnlMasterHeader.Controls.Add(this.lblMasterTitle);
            this.pnlMasterHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMasterHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlMasterHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMasterHeader.Name = "pnlMasterHeader";
            this.pnlMasterHeader.Size = new System.Drawing.Size(625, 50);
            this.pnlMasterHeader.TabIndex = 0;
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCount.Location = new System.Drawing.Point(400, 12);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(208, 25);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "Đang tải...";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMasterTitle
            // 
            this.lblMasterTitle.AutoSize = true;
            this.lblMasterTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblMasterTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblMasterTitle.Location = new System.Drawing.Point(18, 12);
            this.lblMasterTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMasterTitle.Name = "lblMasterTitle";
            this.lblMasterTitle.Size = new System.Drawing.Size(138, 25);
            this.lblMasterTitle.TabIndex = 0;
            this.lblMasterTitle.Text = "Danh sách đơn";
            // 
            // pnlDetailCard
            // 
            this.pnlDetailCard.BackColor = System.Drawing.Color.White;
            this.pnlDetailCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlDetailCard.BorderRadius = 8;
            this.pnlDetailCard.BorderThickness = 1;
            this.pnlDetailCard.Controls.Add(this.dgvChiTietVe);
            this.pnlDetailCard.Controls.Add(this.pnlDetailActions);
            this.pnlDetailCard.Controls.Add(this.pnlDetailInfo);
            this.pnlDetailCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailCard.Location = new System.Drawing.Point(0, 0);
            this.pnlDetailCard.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDetailCard.Name = "pnlDetailCard";
            this.pnlDetailCard.Size = new System.Drawing.Size(553, 710);
            this.pnlDetailCard.TabIndex = 0;
            // 
            // dgvChiTietVe
            // 
            this.dgvChiTietVe.AllowUserToAddRows = false;
            this.dgvChiTietVe.AllowUserToDeleteRows = false;
            this.dgvChiTietVe.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            this.dgvChiTietVe.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiTietVe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvChiTietVe.ColumnHeadersHeight = 30;
            this.dgvChiTietVe.ColumnHeadersVisible = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiTietVe.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvChiTietVe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTietVe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvChiTietVe.Location = new System.Drawing.Point(0, 232);
            this.dgvChiTietVe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvChiTietVe.MultiSelect = false;
            this.dgvChiTietVe.Name = "dgvChiTietVe";
            this.dgvChiTietVe.ReadOnly = true;
            this.dgvChiTietVe.RowHeadersVisible = false;
            this.dgvChiTietVe.RowHeadersWidth = 51;
            this.dgvChiTietVe.RowTemplate.Height = 44;
            this.dgvChiTietVe.Size = new System.Drawing.Size(553, 410);
            this.dgvChiTietVe.TabIndex = 1;
            this.dgvChiTietVe.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiTietVe.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvChiTietVe.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvChiTietVe.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvChiTietVe.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.dgvChiTietVe.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvChiTietVe.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvChiTietVe.ThemeStyle.ReadOnly = true;
            this.dgvChiTietVe.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvChiTietVe.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.dgvChiTietVe.ThemeStyle.RowsStyle.Height = 44;
            this.dgvChiTietVe.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.dgvChiTietVe.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvChiTietVe.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvChiTietVe_CellPainting);
            this.dgvChiTietVe.SelectionChanged += new System.EventHandler(this.dgvChiTietVe_SelectionChanged);
            this.dgvChiTietVe.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvChiTietVe_Paint);
            // 
            // pnlDetailActions
            // 
            this.pnlDetailActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlDetailActions.Controls.Add(this.btnHuyVe);
            this.pnlDetailActions.Controls.Add(this.btnDoiVe);
            this.pnlDetailActions.Controls.Add(this.btnInVe);
            this.pnlDetailActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailActions.Location = new System.Drawing.Point(0, 642);
            this.pnlDetailActions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDetailActions.Name = "pnlDetailActions";
            this.pnlDetailActions.Size = new System.Drawing.Size(553, 68);
            this.pnlDetailActions.TabIndex = 2;
            // 
            // btnHuyVe
            // 
            this.btnHuyVe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuyVe.BorderRadius = 6;
            this.btnHuyVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuyVe.Enabled = false;
            this.btnHuyVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnHuyVe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuyVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnHuyVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.btnHuyVe.Location = new System.Drawing.Point(413, 12);
            this.btnHuyVe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHuyVe.Name = "btnHuyVe";
            this.btnHuyVe.Size = new System.Drawing.Size(125, 42);
            this.btnHuyVe.TabIndex = 2;
            this.btnHuyVe.Text = "Hủy đơn";
            this.btnHuyVe.Click += new System.EventHandler(this.btnHuyVe_Click);
            // 
            // btnDoiVe
            // 
            this.btnDoiVe.BorderRadius = 6;
            this.btnDoiVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDoiVe.Enabled = false;
            this.btnDoiVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnDoiVe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDoiVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.btnDoiVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(254)))));
            this.btnDoiVe.Location = new System.Drawing.Point(142, 12);
            this.btnDoiVe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDoiVe.Name = "btnDoiVe";
            this.btnDoiVe.Size = new System.Drawing.Size(150, 42);
            this.btnDoiVe.TabIndex = 1;
            this.btnDoiVe.Text = "Đổi vé chọn";
            this.btnDoiVe.Click += new System.EventHandler(this.btnDoiVe_Click);
            // 
            // btnInVe
            // 
            this.btnInVe.BorderRadius = 6;
            this.btnInVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInVe.Enabled = false;
            this.btnInVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnInVe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnInVe.ForeColor = System.Drawing.Color.White;
            this.btnInVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(216)))));
            this.btnInVe.Location = new System.Drawing.Point(15, 12);
            this.btnInVe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInVe.Name = "btnInVe";
            this.btnInVe.Size = new System.Drawing.Size(119, 42);
            this.btnInVe.TabIndex = 0;
            this.btnInVe.Text = "In vé";
            this.btnInVe.Click += new System.EventHandler(this.btnInVe_Click);
            // 
            // pnlDetailInfo
            // 
            this.pnlDetailInfo.BackColor = System.Drawing.Color.White;
            this.pnlDetailInfo.Controls.Add(this.btnCopyMaVe);

            this.pnlDetailInfo.Controls.Add(this.lblTicketsTitle);
            this.pnlDetailInfo.Controls.Add(this.pnlDetailDivider);
            this.pnlDetailInfo.Controls.Add(this.lblRuleBadge);
            this.pnlDetailInfo.Controls.Add(this.lblDetailEventTime);
            this.pnlDetailInfo.Controls.Add(this.lblDetailEventName);
            this.pnlDetailInfo.Controls.Add(this.lblCustomerEmail);
            this.pnlDetailInfo.Controls.Add(this.lblCustomerPhone);
            this.pnlDetailInfo.Controls.Add(this.lblCustomerName);
            this.pnlDetailInfo.Controls.Add(this.lblDetailBadge);
            this.pnlDetailInfo.Controls.Add(this.lblDetailTitle);
            this.pnlDetailInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetailInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlDetailInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDetailInfo.Name = "pnlDetailInfo";
            this.pnlDetailInfo.Padding = new System.Windows.Forms.Padding(20, 15, 20, 5);
            this.pnlDetailInfo.Size = new System.Drawing.Size(553, 232);
            this.pnlDetailInfo.TabIndex = 0;
            // 
            // btnCopyMaVe
            // 
            this.btnCopyMaVe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyMaVe.BorderRadius = 4;
            this.btnCopyMaVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyMaVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnCopyMaVe.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnCopyMaVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnCopyMaVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnCopyMaVe.Location = new System.Drawing.Point(400, 194);
            this.btnCopyMaVe.Name = "btnCopyMaVe";
            this.btnCopyMaVe.Size = new System.Drawing.Size(130, 28);
            this.btnCopyMaVe.TabIndex = 10;
            this.btnCopyMaVe.Text = "Sao chép mã";
            this.btnCopyMaVe.Click += new System.EventHandler(this.btnCopyMaVe_Click);
            // 
            // lblTicketsTitle
            // 
            this.lblTicketsTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTicketsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTicketsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTicketsTitle.Location = new System.Drawing.Point(20, 191);
            this.lblTicketsTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTicketsTitle.Name = "lblTicketsTitle";
            this.lblTicketsTitle.Size = new System.Drawing.Size(513, 35);
            this.lblTicketsTitle.TabIndex = 9;
            this.lblTicketsTitle.Text = "Vé trong đơn";
            this.lblTicketsTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnlDetailDivider
            // 
            this.pnlDetailDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlDetailDivider.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailDivider.Location = new System.Drawing.Point(20, 226);
            this.pnlDetailDivider.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDetailDivider.Name = "pnlDetailDivider";
            this.pnlDetailDivider.Size = new System.Drawing.Size(513, 1);
            this.pnlDetailDivider.TabIndex = 8;
            // 
            // lblRuleBadge
            // 
            this.lblRuleBadge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRuleBadge.AutoEllipsis = true;
            this.lblRuleBadge.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRuleBadge.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblRuleBadge.Location = new System.Drawing.Point(20, 158);
            this.lblRuleBadge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRuleBadge.Name = "lblRuleBadge";
            this.lblRuleBadge.Size = new System.Drawing.Size(513, 25);
            this.lblRuleBadge.TabIndex = 7;
            this.lblRuleBadge.Text = "Chọn đơn vé để xem chi tiết";
            this.lblRuleBadge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailEventTime
            // 
            this.lblDetailEventTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetailEventTime.AutoEllipsis = true;
            this.lblDetailEventTime.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDetailEventTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblDetailEventTime.Location = new System.Drawing.Point(20, 130);
            this.lblDetailEventTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDetailEventTime.Name = "lblDetailEventTime";
            this.lblDetailEventTime.Size = new System.Drawing.Size(513, 25);
            this.lblDetailEventTime.TabIndex = 6;
            this.lblDetailEventTime.Text = "—";
            // 
            // lblDetailEventName
            // 
            this.lblDetailEventName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetailEventName.AutoEllipsis = true;
            this.lblDetailEventName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetailEventName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblDetailEventName.Location = new System.Drawing.Point(20, 102);
            this.lblDetailEventName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDetailEventName.Name = "lblDetailEventName";
            this.lblDetailEventName.Size = new System.Drawing.Size(513, 25);
            this.lblDetailEventName.TabIndex = 5;
            this.lblDetailEventName.Text = "—";
            // 
            // lblCustomerEmail
            // 
            this.lblCustomerEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerEmail.AutoEllipsis = true;
            this.lblCustomerEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCustomerEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCustomerEmail.Location = new System.Drawing.Point(20, 102);
            this.lblCustomerEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomerEmail.Name = "lblCustomerEmail";
            this.lblCustomerEmail.Size = new System.Drawing.Size(513, 22);
            this.lblCustomerEmail.TabIndex = 4;
            this.lblCustomerEmail.Text = "email@example.com";
            this.lblCustomerEmail.Visible = false;
            // 
            // lblCustomerPhone
            // 
            this.lblCustomerPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerPhone.AutoEllipsis = true;
            this.lblCustomerPhone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCustomerPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblCustomerPhone.Location = new System.Drawing.Point(20, 75);
            this.lblCustomerPhone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomerPhone.Name = "lblCustomerPhone";
            this.lblCustomerPhone.Size = new System.Drawing.Size(513, 22);
            this.lblCustomerPhone.TabIndex = 3;
            this.lblCustomerPhone.Text = "—";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerName.AutoEllipsis = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblCustomerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCustomerName.Location = new System.Drawing.Point(20, 48);
            this.lblCustomerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(513, 25);
            this.lblCustomerName.TabIndex = 2;
            this.lblCustomerName.Text = "—";
            // 
            // lblDetailBadge
            // 
            this.lblDetailBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetailBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetailBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(95)))), ((int)(((byte)(70)))));
            this.lblDetailBadge.Location = new System.Drawing.Point(413, 15);
            this.lblDetailBadge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDetailBadge.Name = "lblDetailBadge";
            this.lblDetailBadge.Size = new System.Drawing.Size(121, 30);
            this.lblDetailBadge.TabIndex = 1;
            this.lblDetailBadge.Text = "Đã thanh toán";
            this.lblDetailBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblDetailTitle.Location = new System.Drawing.Point(18, 14);
            this.lblDetailTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(120, 30);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Đơn vé: —";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnLamMoi);
            this.pnlHeader.Controls.Add(this.cboLocTrangThai);
            this.pnlHeader.Controls.Add(this.txtTimKiem);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 50);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1190, 65);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLamMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnLamMoi.BorderRadius = 6;
            this.btnLamMoi.BorderThickness = 1;
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FillColor = System.Drawing.Color.White;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnLamMoi.Location = new System.Drawing.Point(1065, 10);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(125, 45);
            this.btnLamMoi.TabIndex = 2;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.cboLocTrangThai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.cboLocTrangThai.BorderRadius = 6;
            this.cboLocTrangThai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.FocusedColor = System.Drawing.Color.Empty;
            this.cboLocTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboLocTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cboLocTrangThai.ItemHeight = 30;
            this.cboLocTrangThai.Location = new System.Drawing.Point(425, 10);
            this.cboLocTrangThai.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(224, 36);
            this.cboLocTrangThai.TabIndex = 1;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.cboLocTrangThai_SelectedIndexChanged);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.txtTimKiem.BorderRadius = 6;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 10);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtTimKiem.PlaceholderText = "Tìm mã đơn, khách hàng, SĐT, mã vé...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(406, 45);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // searchTimer
            // 
            this.searchTimer.Interval = 300;
            this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);
            // 
            // FrmQuanLyDonVe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1250, 865);
            this.Controls.Add(this.pnlPageContent);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(996, 613);
            this.Name = "FrmQuanLyDonVe";
            this.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý đơn vé";
            this.Load += new System.EventHandler(this.FrmQuanLyDonVe_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmQuanLyDonVe_KeyDown);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlPageContent.ResumeLayout(false);
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
            this.pnlDetailActions.ResumeLayout(false);
            this.pnlDetailInfo.ResumeLayout(false);
            this.pnlDetailInfo.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlPageContent;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2ComboBox cboLocTrangThai;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Guna.UI2.WinForms.Guna2Panel pnlMasterCard;
        private System.Windows.Forms.Panel pnlMasterHeader;
        private System.Windows.Forms.Label lblMasterTitle;
        private System.Windows.Forms.Label lblCount;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDonVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaDonGoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGianBatDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayDat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoVeHopLe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongSoVe;
        private Guna.UI2.WinForms.Guna2Panel pnlDetailCard;
        private System.Windows.Forms.Panel pnlDetailInfo;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblDetailBadge;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblCustomerPhone;
        private System.Windows.Forms.Label lblCustomerEmail;
        private System.Windows.Forms.Label lblDetailEventName;
        private System.Windows.Forms.Label lblDetailEventTime;
        private System.Windows.Forms.Label lblRuleBadge;
        private System.Windows.Forms.Panel pnlDetailDivider;
        private System.Windows.Forms.Label lblTicketsTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvChiTietVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaGheHienThi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaCT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaVeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHangVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThaiVe;
        private System.Windows.Forms.Panel pnlDetailActions;
        private Guna.UI2.WinForms.Guna2Button btnInVe;
        private Guna.UI2.WinForms.Guna2Button btnDoiVe;
        private Guna.UI2.WinForms.Guna2Button btnHuyVe;

        private Guna.UI2.WinForms.Guna2Button btnCopyMaVe;
        private System.Windows.Forms.Timer searchTimer;
    }
}
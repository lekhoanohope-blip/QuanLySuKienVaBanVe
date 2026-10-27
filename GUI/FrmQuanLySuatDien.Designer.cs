namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmQuanLySuatDien
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
            this.pnlSubSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMenuKhuVuc = new Guna.UI2.WinForms.Guna2Button();
            this.btnMenuSuatDien = new Guna.UI2.WinForms.Guna2Button();
            this.pnlMainContent = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.btnSua = new Guna.UI2.WinForms.Guna2Button();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlBottom = new Guna.UI2.WinForms.Guna2Panel();
            this.lblPagination = new System.Windows.Forms.Label();
            this.dgvSuatDien = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaSuat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThoiGian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiaDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThaoTac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSubSidebar.SuspendLayout();
            this.pnlMainContent.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuatDien)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSubSidebar
            // 
            this.pnlSubSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSubSidebar.Controls.Add(this.btnMenuKhuVuc);
            this.pnlSubSidebar.Controls.Add(this.btnMenuSuatDien);
            this.pnlSubSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSubSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSubSidebar.Name = "pnlSubSidebar";
            this.pnlSubSidebar.Size = new System.Drawing.Size(200, 700);
            this.pnlSubSidebar.TabIndex = 0;
            // 
            // btnMenuKhuVuc
            // 
            this.btnMenuKhuVuc.BorderRadius = 8;
            this.btnMenuKhuVuc.FillColor = System.Drawing.Color.Transparent;
            this.btnMenuKhuVuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuKhuVuc.ForeColor = System.Drawing.Color.Gray;
            this.btnMenuKhuVuc.Location = new System.Drawing.Point(10, 70);
            this.btnMenuKhuVuc.Name = "btnMenuKhuVuc";
            this.btnMenuKhuVuc.Size = new System.Drawing.Size(180, 45);
            this.btnMenuKhuVuc.TabIndex = 1;
            this.btnMenuKhuVuc.Text = "Khu vực & Giá vé";
            this.btnMenuKhuVuc.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMenuKhuVuc.Click += new System.EventHandler(this.btnMenuKhuVuc_Click);
            // 
            // btnMenuSuatDien
            // 
            this.btnMenuSuatDien.BorderRadius = 8;
            this.btnMenuSuatDien.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnMenuSuatDien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuSuatDien.ForeColor = System.Drawing.Color.White;
            this.btnMenuSuatDien.Location = new System.Drawing.Point(10, 20);
            this.btnMenuSuatDien.Name = "btnMenuSuatDien";
            this.btnMenuSuatDien.Size = new System.Drawing.Size(180, 45);
            this.btnMenuSuatDien.TabIndex = 0;
            this.btnMenuSuatDien.Text = "Danh sách Suất diễn";
            this.btnMenuSuatDien.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMenuSuatDien.Click += new System.EventHandler(this.btnMenuSuatDien_Click);
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.Controls.Add(this.pnlHeader);
            this.pnlMainContent.Controls.Add(this.pnlBottom);
            this.pnlMainContent.Controls.Add(this.dgvSuatDien);
            this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContent.Location = new System.Drawing.Point(200, 0);
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Size = new System.Drawing.Size(800, 700);
            this.pnlMainContent.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnXoa);
            this.pnlHeader.Controls.Add(this.btnSua);
            this.pnlHeader.Controls.Add(this.btnThem);
            this.pnlHeader.Controls.Add(this.txtTimKiem);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(800, 72);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnXoa
            // 
            this.btnXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoa.BorderRadius = 6;
            this.btnXoa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXoa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnXoa.Location = new System.Drawing.Point(680, 20);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 36);
            this.btnXoa.TabIndex = 4;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSua.BorderRadius = 6;
            this.btnSua.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnSua.Location = new System.Drawing.Point(570, 20);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(100, 36);
            this.btnSua.TabIndex = 3;
            this.btnSua.Text = "Sửa";
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnThem
            // 
            this.btnThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThem.BorderRadius = 6;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(450, 20);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(110, 36);
            this.btnThem.TabIndex = 2;
            this.btnThem.Text = "+ Thêm mới";
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.txtTimKiem.BorderRadius = 8;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.Location = new System.Drawing.Point(20, 20);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "🔍 Tìm kiếm suất diễn...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(350, 36);
            this.txtTimKiem.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(390, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý suất diễn";
            this.lblTitle.Visible = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblPagination);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 650);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(800, 50);
            this.pnlBottom.TabIndex = 2;
            // 
            // lblPagination
            // 
            this.lblPagination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPagination.AutoSize = true;
            this.lblPagination.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPagination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblPagination.Location = new System.Drawing.Point(700, 15);
            this.lblPagination.Name = "lblPagination";
            this.lblPagination.Size = new System.Drawing.Size(80, 19);
            this.lblPagination.TabIndex = 0;
            this.lblPagination.Text = "<   1   >";
            // 
            // dgvSuatDien
            // 
            this.dgvSuatDien.AllowUserToAddRows = false;
            this.dgvSuatDien.AllowUserToDeleteRows = false;
            this.dgvSuatDien.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.dgvSuatDien.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSuatDien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuatDien.BackgroundColor = System.Drawing.Color.White;
            this.dgvSuatDien.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuatDien.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSuatDien.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSuatDien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSuatDien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSuatDien.ColumnHeadersHeight = 48;
            this.dgvSuatDien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSuat,
            this.colTenSK,
            this.colThoiGian,
            this.colDiaDiem,
            this.colTrangThai,
            this.colThaoTac});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(229, 247, 239);
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSuatDien.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSuatDien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuatDien.EnableHeadersVisualStyles = false;
            this.dgvSuatDien.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvSuatDien.Location = new System.Drawing.Point(0, 72);
            this.dgvSuatDien.Name = "dgvSuatDien";
            this.dgvSuatDien.ReadOnly = true;
            this.dgvSuatDien.RowHeadersVisible = false;
            this.dgvSuatDien.MultiSelect = false;
            this.dgvSuatDien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuatDien.ShowCellToolTips = true;
            this.dgvSuatDien.RowTemplate.Height = 58;
            this.dgvSuatDien.RowTemplate.MinimumHeight = 58;
            this.dgvSuatDien.Size = new System.Drawing.Size(800, 578);
            this.dgvSuatDien.TabIndex = 3;
            this.dgvSuatDien.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(250, 252, 253);
            this.dgvSuatDien.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.dgvSuatDien.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvSuatDien.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.dgvSuatDien.ThemeStyle.HeaderStyle.Height = 48;
            this.dgvSuatDien.ThemeStyle.RowsStyle.Height = 58;
            this.dgvSuatDien.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvSuatDien.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(229, 247, 239);
            this.dgvSuatDien.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvSuatDien.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSuatDien_CellPainting);
            this.dgvSuatDien.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSuatDien_CellMouseClick);
            // 
            // colMaSuat
            // 
            this.colMaSuat.HeaderText = "Mã Suất";
            this.colMaSuat.Name = "colMaSuat";
            this.colMaSuat.Visible = false;
            // 
            // colTenSK
            // 
            this.colTenSK.FillWeight = 60F;
            this.colTenSK.MinimumWidth = 220;
            this.colTenSK.HeaderText = "Tên sự kiện";
            this.colTenSK.Name = "colTenSK";
            // 
            // colThoiGian
            // 
            this.colThoiGian.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThoiGian.Width = 170;
            this.colThoiGian.MinimumWidth = 170;
            this.colThoiGian.HeaderText = "Thời gian";
            this.colThoiGian.Name = "colThoiGian";
            // 
            // colDiaDiem
            // 
            this.colDiaDiem.FillWeight = 40F;
            this.colDiaDiem.MinimumWidth = 190;
            this.colDiaDiem.HeaderText = "Địa điểm";
            this.colDiaDiem.Name = "colDiaDiem";
            // 
            // colTrangThai
            // 
            this.colTrangThai.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTrangThai.Width = 154;
            this.colTrangThai.MinimumWidth = 154;
            this.colTrangThai.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTrangThai.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTrangThai.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colTrangThai.DefaultCellStyle.Padding = System.Windows.Forms.Padding.Empty;
            this.colTrangThai.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.Name = "colTrangThai";
            // 
            // colThaoTac
            // 
            this.colThaoTac.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThaoTac.Width = 112;
            this.colThaoTac.MinimumWidth = 112;
            this.colThaoTac.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colThaoTac.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colThaoTac.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colThaoTac.DefaultCellStyle.Padding = System.Windows.Forms.Padding.Empty;
            this.colThaoTac.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colThaoTac.HeaderText = "Thao tác";
            this.colThaoTac.Name = "colThaoTac";
            // 
            // FrmQuanLySuatDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.pnlMainContent);
            this.Controls.Add(this.pnlSubSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmQuanLySuatDien";
            this.Padding = new System.Windows.Forms.Padding(24, 16, 24, 24);
            this.Text = "Quản Lý Suất Diễn";
            this.Load += new System.EventHandler(this.FrmQuanLySuatDien_Load);
            this.pnlSubSidebar.ResumeLayout(false);
            this.pnlMainContent.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuatDien)).EndInit();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlSubSidebar;
        private Guna.UI2.WinForms.Guna2Button btnMenuSuatDien;
        private Guna.UI2.WinForms.Guna2Button btnMenuKhuVuc;
        private Guna.UI2.WinForms.Guna2Panel pnlMainContent;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2Panel pnlBottom;
        private System.Windows.Forms.Label lblPagination;
        private Guna.UI2.WinForms.Guna2DataGridView dgvSuatDien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSuat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThaoTac;
    }
}

namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmQuanLySuKien
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
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.btnSua = new Guna.UI2.WinForms.Guna2Button();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvSuKien = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoaiSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoTa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThaoTac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuKien)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnXoa);
            this.pnlHeader.Controls.Add(this.btnSua);
            this.pnlHeader.Controls.Add(this.btnThem);
            this.pnlHeader.Controls.Add(this.txtTimKiem);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(24, 16);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(952, 72);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnXoa
            // 
            this.btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnXoa.BorderRadius = 6;
            this.btnXoa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXoa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnXoa.Location = new System.Drawing.Point(852, 20);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 36);
            this.btnXoa.TabIndex = 4;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnSua.BorderRadius = 6;
            this.btnSua.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnSua.Location = new System.Drawing.Point(742, 20);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(100, 36);
            this.btnSua.TabIndex = 3;
            this.btnSua.Text = "Sửa";
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnThem
            // 
            this.btnThem.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnThem.BorderRadius = 6;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(622, 20);
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
            this.txtTimKiem.Location = new System.Drawing.Point(0, 20);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "🔍 Tìm kiếm sự kiện...";
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
            this.lblTitle.Size = new System.Drawing.Size(160, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý sự kiện";
            this.lblTitle.Visible = false;
            // 
            // dgvSuKien
            // 
            this.dgvSuKien.AllowUserToAddRows = false;
            this.dgvSuKien.AllowUserToDeleteRows = false;
            this.dgvSuKien.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(250, 252, 253);
            this.dgvSuKien.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSuKien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuKien.BackgroundColor = System.Drawing.Color.White;
            this.dgvSuKien.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuKien.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSuKien.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSuKien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSuKien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSuKien.ColumnHeadersHeight = 48;
            this.dgvSuKien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSK,
            this.colTenSK,
            this.colLoaiSK,
            this.colMoTa,
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
            this.dgvSuKien.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSuKien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuKien.EnableHeadersVisualStyles = false;
            this.dgvSuKien.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvSuKien.Location = new System.Drawing.Point(24, 88);
            this.dgvSuKien.Name = "dgvSuKien";
            this.dgvSuKien.ReadOnly = true;
            this.dgvSuKien.RowHeadersVisible = false;
            this.dgvSuKien.MultiSelect = false;
            this.dgvSuKien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuKien.ShowCellToolTips = true;
            this.dgvSuKien.RowTemplate.Height = 58;
            this.dgvSuKien.RowTemplate.MinimumHeight = 58;
            this.dgvSuKien.Size = new System.Drawing.Size(952, 588);
            this.dgvSuKien.TabIndex = 1;
            this.dgvSuKien.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(250, 252, 253);
            this.dgvSuKien.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvSuKien.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvSuKien.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.dgvSuKien.ThemeStyle.HeaderStyle.Height = 48;
            this.dgvSuKien.ThemeStyle.RowsStyle.Height = 58;
            this.dgvSuKien.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvSuKien.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(229, 247, 239);
            this.dgvSuKien.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvSuKien.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSuKien_CellPainting);
            this.dgvSuKien.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSuKien_CellMouseClick);
            // 
            // colMaSK
            // 
            this.colMaSK.HeaderText = "Mã SK";
            this.colMaSK.Name = "colMaSK";
            this.colMaSK.Visible = false;
            // 
            // colTenSK
            // 
            this.colTenSK.FillWeight = 46F;
            this.colTenSK.MinimumWidth = 240;
            this.colTenSK.HeaderText = "Tên sự kiện";
            this.colTenSK.Name = "colTenSK";
            // 
            // colLoaiSK
            // 
            this.colLoaiSK.FillWeight = 16F;
            this.colLoaiSK.MinimumWidth = 112;
            this.colLoaiSK.HeaderText = "Thể loại";
            this.colLoaiSK.Name = "colLoaiSK";
            // 
            // colMoTa
            // 
            this.colMoTa.FillWeight = 38F;
            this.colMoTa.MinimumWidth = 180;
            this.colMoTa.HeaderText = "Mô tả";
            this.colMoTa.Name = "colMoTa";
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
            // FrmQuanLySuKien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(243, 246, 249);
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.dgvSuKien);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmQuanLySuKien";
            this.Padding = new System.Windows.Forms.Padding(24, 16, 24, 24);
            this.Text = "Quản lý Sự kiện";
            this.Load += new System.EventHandler(this.FrmQuanLySuKien_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuKien)).EndInit();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2DataGridView dgvSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoaiSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoTa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThaoTac;
    }
}

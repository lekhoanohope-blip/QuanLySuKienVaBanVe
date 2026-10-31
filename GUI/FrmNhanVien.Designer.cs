namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.btnSua = new Guna.UI2.WinForms.Guna2Button();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.pnlTable = new System.Windows.Forms.Panel();
            this.dgvNhanVien = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaTK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenTK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChucVu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatKhau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThaoTac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlMessage = new System.Windows.Forms.TableLayoutPanel();
            this.lblMessageTitle = new System.Windows.Forms.Label();
            this.lblMessageDescription = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.TableLayoutPanel();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.lblSearchHint = new System.Windows.Forms.Label();
            this.searchTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();

            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Height = 48;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.lblTitle.Text = "Quản lý nhân viên";
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(0, 4);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 72;
            this.pnlHeader.Controls.AddRange(new System.Windows.Forms.Control[] { this.txtTimKiem, this.btnThem, this.btnSua, this.btnXoa });
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.BorderRadius = 8;
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtTimKiem.PlaceholderText = "Tìm theo tên tài khoản…";
            this.txtTimKiem.PlaceholderForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtTimKiem.Size = new System.Drawing.Size(350, 36);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.AccessibleName = "Tìm tài khoản theo tên";
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            this.btnThem.Name = "btnThem";
            this.btnThem.Text = "+ Thêm mới";
            this.btnThem.Size = new System.Drawing.Size(110, 36);
            this.btnThem.BorderRadius = 6;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnThem.HoverState.FillColor = System.Drawing.Color.FromArgb(5, 150, 105);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThem.TabIndex = 1;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            this.btnSua.Name = "btnSua";
            this.btnSua.Text = "Sửa";
            this.btnSua.Size = new System.Drawing.Size(100, 36);
            this.btnSua.BorderRadius = 6;
            this.btnSua.FillColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.btnSua.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSua.Enabled = false;
            this.btnSua.TabIndex = 2;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Text = "Xóa";
            this.btnXoa.Size = new System.Drawing.Size(100, 36);
            this.btnXoa.BorderRadius = 6;
            this.btnXoa.FillColor = System.Drawing.Color.FromArgb(254, 226, 226);
            this.btnXoa.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXoa.Enabled = false;
            this.btnXoa.TabIndex = 3;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            this.pnlTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTable.BackColor = System.Drawing.Color.White;
            this.pnlTable.Controls.Add(this.pnlMessage);
            this.pnlTable.Controls.Add(this.dgvNhanVien);
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhanVien.AutoGenerateColumns = false;
            this.dgvNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNhanVien.AllowUserToAddRows = false;
            this.dgvNhanVien.AllowUserToDeleteRows = false;
            this.dgvNhanVien.AllowUserToResizeRows = false;
            this.dgvNhanVien.ReadOnly = true;
            this.dgvNhanVien.MultiSelect = false;
            this.dgvNhanVien.RowHeadersVisible = false;
            this.dgvNhanVien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhanVien.BackgroundColor = System.Drawing.Color.White;
            this.dgvNhanVien.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNhanVien.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvNhanVien.GridColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.dgvNhanVien.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvNhanVien.EnableHeadersVisualStyles = false;
            this.dgvNhanVien.ColumnHeadersHeight = 48;
            this.dgvNhanVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle {
                BackColor = System.Drawing.Color.FromArgb(243, 244, 246), ForeColor = System.Drawing.Color.FromArgb(107, 114, 128),
                SelectionBackColor = System.Drawing.Color.FromArgb(243, 244, 246), SelectionForeColor = System.Drawing.Color.FromArgb(107, 114, 128),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), Padding = new System.Windows.Forms.Padding(16, 0, 16, 0),
                WrapMode = System.Windows.Forms.DataGridViewTriState.False, Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft };
            this.dgvNhanVien.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle {
                BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.FromArgb(71, 85, 105),
                SelectionBackColor = System.Drawing.Color.FromArgb(229, 247, 239), SelectionForeColor = System.Drawing.Color.FromArgb(30, 41, 59),
                Font = new System.Drawing.Font("Segoe UI", 9.5F), Padding = new System.Windows.Forms.Padding(16, 10, 16, 10),
                WrapMode = System.Windows.Forms.DataGridViewTriState.False, Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft };
            this.dgvNhanVien.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 252, 253);
            this.dgvNhanVien.RowTemplate.Height = 58;
            this.dgvNhanVien.RowTemplate.MinimumHeight = 58;
            this.dgvNhanVien.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.dgvNhanVien.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.dgvNhanVien.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvNhanVien.ThemeStyle.HeaderStyle.Height = 48;
            this.dgvNhanVien.ThemeStyle.RowsStyle.Height = 58;
            this.dgvNhanVien.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(229, 247, 239);
            this.dgvNhanVien.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvNhanVien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colMaTK, this.colTenTK, this.colChucVu, this.colMatKhau, this.colThaoTac });
            this.dgvNhanVien.TabIndex = 3;
            this.dgvNhanVien.AccessibleName = "Danh sách tài khoản";
            this.dgvNhanVien.SelectionChanged += new System.EventHandler(this.dgvNhanVien_SelectionChanged);
            this.dgvNhanVien.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvNhanVien_CellPainting);
            this.dgvNhanVien.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvNhanVien_CellMouseClick);
            this.dgvNhanVien.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhanVien_CellDoubleClick);
            this.dgvNhanVien.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dgvNhanVien_CellToolTipTextNeeded);
            this.dgvNhanVien.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvNhanVien_CellMouseMove);
            this.dgvNhanVien.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhanVien_CellMouseLeave);
            this.colMaTK.Name = "colMaTK";
            this.colMaTK.DataPropertyName = "MaTK";
            this.colMaTK.HeaderText = "Mã TK";
            this.colMaTK.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colMaTK.Width = 100;
            this.colTenTK.Name = "colTenTK";
            this.colTenTK.DataPropertyName = "TenDangNhap";
            this.colTenTK.HeaderText = "Tên TK";
            this.colTenTK.FillWeight = 45F;
            this.colTenTK.MinimumWidth = 200;
            this.colChucVu.Name = "colChucVu";
            this.colChucVu.DataPropertyName = "ChucVu";
            this.colChucVu.HeaderText = "Chức vụ";
            this.colChucVu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colChucVu.Width = 150;
            this.colMatKhau.Name = "colMatKhau";
            this.colMatKhau.DataPropertyName = "MatKhau";
            this.colMatKhau.HeaderText = "Mật khẩu";
            this.colMatKhau.MinimumWidth = 240;
            this.colMatKhau.FillWeight = 55F;
                        this.colThaoTac.Name = "colThaoTac";
            this.colThaoTac.HeaderText = "Thao tác";
            this.colThaoTac.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThaoTac.Width = 112;
            this.colThaoTac.MinimumWidth = 112;
            this.colThaoTac.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colThaoTac.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colThaoTac.DefaultCellStyle.Padding = System.Windows.Forms.Padding.Empty;
            this.colThaoTac.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessage.BackColor = System.Drawing.Color.White;
            this.pnlMessage.ColumnCount = 1;
            this.pnlMessage.RowCount = 4;
            this.pnlMessage.Padding = new System.Windows.Forms.Padding(24);
            this.pnlMessage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMessage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlMessage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMessage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMessage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlMessage.Controls.Add(this.lblMessageTitle, 0, 1);
            this.pnlMessage.Controls.Add(this.lblMessageDescription, 0, 2);
            this.lblMessageTitle.Text = "Đang tải tài khoản…";
            this.lblMessageTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblMessageTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblMessageTitle.AutoSize = true;
            this.lblMessageTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessageTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblMessageDescription.Text = "Vui lòng chờ trong giây lát.";
            this.lblMessageDescription.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblMessageDescription.AutoSize = true;
            this.lblMessageDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessageDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 56;
            this.pnlFooter.ColumnCount = 2;
            this.pnlFooter.RowCount = 1;
            this.pnlFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.pnlFooter.Controls.Add(this.lblCount, 0, 0);
            this.pnlFooter.Controls.Add(this.btnLamMoi, 1, 0);
            this.lblCount.Text = "Đang tải dữ liệu…";
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnLamMoi.Text = "Làm mới (F5)";
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLamMoi.Size = new System.Drawing.Size(126, 36);
            this.btnLamMoi.BorderRadius = 6;
            this.btnLamMoi.FillColor = System.Drawing.Color.White;
            this.btnLamMoi.BorderThickness = 1;
            this.btnLamMoi.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            this.lblSearchHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSearchHint.Height = 30;
            this.lblSearchHint.Text = "Tra cứu theo tên tài khoản · Ctrl+F: tìm kiếm · Ctrl+N: thêm · Ctrl+E: sửa";
            this.lblSearchHint.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSearchHint.AutoEllipsis = true;
            this.searchTimer.Interval = 300;
            this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Padding = new System.Windows.Forms.Padding(24, 16, 24, 24);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.BackColor = System.Drawing.Color.FromArgb(243, 246, 249);
            this.Controls.Add(this.pnlTable);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.lblSearchHint);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlTitle);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý nhân viên";
            this.Name = "FrmNhanVien";
            this.KeyPreview = true;
            this.Load += new System.EventHandler(this.FrmNhanVien_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmNhanVien_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private System.Windows.Forms.Panel pnlTable;
        private Guna.UI2.WinForms.Guna2DataGridView dgvNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaTK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenTK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChucVu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatKhau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThaoTac;
        private System.Windows.Forms.TableLayoutPanel pnlMessage;
        private System.Windows.Forms.Label lblMessageTitle;
        private System.Windows.Forms.Label lblMessageDescription;
        private System.Windows.Forms.TableLayoutPanel pnlFooter;
        private System.Windows.Forms.Label lblCount;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private System.Windows.Forms.Label lblSearchHint;
        private System.Windows.Forms.Timer searchTimer;
    }
}

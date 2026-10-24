using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmMain : Form
    {
        public bool DangXuat { get; private set; }
        private Form currentFormChild;
        private System.Windows.Forms.Timer timerClock; // Khai báo Timer
        private Panel dashboardCanvas;
        private bool adjustingWindow;
        private bool layingOutDashboard;
        private bool compactLogo;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void DragWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        public FrmMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            resizeBox.BringToFront();
            if (btnDangXuat != null) btnDangXuat.Click += btnDangXuat_Click;
            ApplyPhanQuyen();

            Shown += FrmMain_Shown;
            SizeChanged += (s, e) => { LayoutMainHeader(); LayoutSidebar(); LayoutDashboard(); };
            LocationChanged += (s, e) =>
            {
                if (!adjustingWindow && WindowState == FormWindowState.Normal)
                {
                    Rectangle workingArea = Screen.FromControl(this).WorkingArea;
                    if (MaximizedBounds != workingArea)
                    {
                        MaximizedBounds = workingArea;
                        MaximumSize = workingArea.Size;
                    }
                }
            };
            DpiChanged += (s, e) =>
            {
                FitToWorkingArea();
                LayoutMainHeader();
                LayoutSidebar();
                LayoutDashboard();
            };
            pnlDashboard.SizeChanged += (s, e) => LayoutDashboard();

            // Hỗ trợ kéo di chuyển cửa sổ
            pnlHeader.MouseDown += DragWindow_MouseDown;
            pnlLogo.MouseDown += DragWindow_MouseDown;
            lblLogo.MouseDown += DragWindow_MouseDown;

            // Nháy đúp vào thanh tiêu đề để phóng to / thu nhỏ
            pnlHeader.DoubleClick += (s, e) =>
            {
                this.WindowState = (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
            };

            // Nút đóng thoát hẳn app
            controlBoxClose.Click += (s, e) => Application.Exit();

            // Khởi tạo Timer chạy mỗi 1 giây
            timerClock = new System.Windows.Forms.Timer();
            timerClock.Interval = 1000;
            timerClock.Tick += TimerClock_Tick;
        }

        private int Px(int logicalPixels)
        {
            return (int)Math.Round(logicalPixels * DeviceDpi / 96f);
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            InitializeDashboardCanvas();
            FitToWorkingArea();
            LayoutMainHeader();
            LayoutSidebar();
            LayoutDashboard();
            BeginInvoke(new MethodInvoker(ClearDashboardSelection));
        }

        private void FitToWorkingArea()
        {
            if (adjustingWindow) return;
            adjustingWindow = true;
            try
            {
                Rectangle workingArea = Screen.FromControl(this).WorkingArea;
                MaximizedBounds = workingArea;
                MinimumSize = new Size(Math.Min(Px(900), workingArea.Width),
                    Math.Min(Px(620), workingArea.Height));
                MaximumSize = workingArea.Size;

                if (WindowState == FormWindowState.Normal)
                {
                    int width = Math.Min(Width, workingArea.Width);
                    int height = Math.Min(Height, workingArea.Height);
                    SetBounds(workingArea.Left + (workingArea.Width - width) / 2,
                        workingArea.Top + (workingArea.Height - height) / 2, width, height);
                }
            }
            finally { adjustingWindow = false; }
        }

        private void LayoutMainHeader()
        {
            if (pnlHeader == null || txtTimKiem == null) return;
            int gap = Px(16);
            bool showUser = pnlHeader.ClientSize.Width >= Px(540);
            lblUser.Visible = showUser;
            ptbAvatar.Visible = showUser;
            btnThongBao.Visible = showUser;

            int rightEdge = showUser ? btnThongBao.Left : controlBoxMinimize.Left;
            int searchWidth = Math.Min(Px(450), rightEdge - txtTimKiem.Left - gap);
            txtTimKiem.Visible = searchWidth >= Px(160);
            if (txtTimKiem.Visible) txtTimKiem.Width = searchWidth;
        }

        private void LayoutSidebar()
        {
            if (pnlSidebar == null) return;
            int sidebarWidth = Width < Px(1050) ? Px(200) : Px(250);
            if (pnlSidebar.Width != sidebarWidth) pnlSidebar.Width = sidebarWidth;

            // Short screens scroll the last two links instead of letting them cover navigation.
            pnlSidebar.AutoScroll = true;
            btnDangXuat.Dock = DockStyle.None;
            btnCaiDat.Dock = DockStyle.None;
            int linkTop = Math.Max(Px(520), pnlSidebar.ClientSize.Height - Px(140));
            pnlSidebar.AutoScrollMinSize = new Size(0, linkTop + Px(140));
            int viewportWidth = sidebarWidth -
                (linkTop + Px(140) > pnlSidebar.ClientSize.Height ? SystemInformation.VerticalScrollBarWidth : 0);
            int currentY = Px(140);
            Control[] navButtons = new Control[] { btnTrangChu, btnSuKien, btnSuatDien, btnKhachHang, btnKhuVuc, btnBanVe };
            foreach (Control btn in navButtons)
            {
                if (btn != null)
                {
                    btn.Width = viewportWidth;
                    if (btn.Visible)
                    {
                        btn.SetBounds(0, currentY, viewportWidth, Px(50));
                        currentY += Px(60);
                    }
                }
            }
            btnDangXuat.Width = viewportWidth;
            btnCaiDat.Width = viewportWidth;
            if (btnCaiDat.Visible)
            {
                btnDangXuat.SetBounds(0, linkTop, viewportWidth, Px(70));
                btnCaiDat.SetBounds(0, linkTop + Px(70), viewportWidth, Px(70));
            }
            else
            {
                btnDangXuat.SetBounds(0, linkTop + Px(70), viewportWidth, Px(70));
            }

            bool compact = sidebarWidth < Px(220);
            if (compact != compactLogo)
            {
                compactLogo = compact;
                lblLogo.Font = new Font("Segoe UI", compact ? 14f : 18f, FontStyle.Bold);
                lblLogo.Location = new Point(Px(compact ? 18 : 45), Px(45));
            }
        }

        private void InitializeDashboardCanvas()
        {
            if (dashboardCanvas != null) return;
            dashboardCanvas = new Panel { BackColor = pnlContent.BackColor, Location = Point.Empty };
            Control[] items = pnlDashboard.Controls.Cast<Control>().ToArray();
            pnlDashboard.SuspendLayout();
            foreach (Control item in items) dashboardCanvas.Controls.Add(item);
            pnlDashboard.Controls.Add(dashboardCanvas);
            pnlDashboard.ResumeLayout();
        }

        private void LayoutDashboard()
        {
            if (dashboardCanvas == null || dashboardCanvas.IsDisposed ||
                currentFormChild != null || layingOutDashboard) return;

            layingOutDashboard = true;
            int margin = Px(20);
            int gap = Px(20);
            int canvasWidth = Math.Max(Px(340),
                pnlDashboard.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 1);
            int contentWidth = canvasWidth - 2 * margin;
            pnlDashboard.SuspendLayout();
            dashboardCanvas.SuspendLayout();
            try
            {
                dashboardCanvas.Width = canvasWidth;
                pnlGreeting.SetBounds(margin, margin,
                    Math.Min(contentWidth, Px(650)), Px(60));

                lblTime.AutoSize = false;
                lblTime.Dock = DockStyle.Fill;
                lblTime.TextAlign = ContentAlignment.MiddleCenter;
                int timeWidth = Math.Min(contentWidth,
                    Math.Max(Px(280), TextRenderer.MeasureText(lblTime.Text, lblTime.Font).Width + Px(20)));
                bool splitIntro = contentWidth >= pnlGreeting.Width + timeWidth + gap;
                pnlTime.SetBounds(splitIntro ? canvasWidth - margin - timeWidth : margin,
                    splitIntro ? margin : margin + Px(70), timeWidth, Px(45));

                int y = splitIntro ? margin + Px(80) : margin + Px(135);
                Control[] cards = { pnlCard1, pnlCard2, pnlCard3, pnlCard4 };
                int cardColumns = contentWidth >= Px(1030) ? 4 : contentWidth >= Px(490) ? 2 : 1;
                int cardWidth = (contentWidth - gap * (cardColumns - 1)) / cardColumns;
                for (int i = 0; i < cards.Length; i++)
                    cards[i].SetBounds(margin + (i % cardColumns) * (cardWidth + gap),
                        y + (i / cardColumns) * (Px(140) + gap), cardWidth, Px(140));
                y += ((cards.Length + cardColumns - 1) / cardColumns) * (Px(140) + gap) + Px(10);

                bool splitPanels = contentWidth >= Px(760);
                int leftWidth = splitPanels ? (int)Math.Round((contentWidth - gap) * 0.6) : contentWidth;
                int rightWidth = contentWidth - leftWidth - gap;
                pnlChart1.SetBounds(margin, y, leftWidth, Px(250));
                pnlChart2.SetBounds(splitPanels ? margin + leftWidth + gap : margin,
                    splitPanels ? y : y + Px(250) + gap,
                    splitPanels ? rightWidth : contentWidth, Px(250));
                y += splitPanels ? Px(250) + gap : 2 * (Px(250) + gap);
                int tableHeight = Px(360);
                bool splitTables = contentWidth >= Px(1030);
                pnlTable1.SetBounds(margin, y, splitTables ? leftWidth : contentWidth, tableHeight);
                pnlTable2.SetBounds(splitTables ? margin + leftWidth + gap : margin,
                    splitTables ? y : y + tableHeight + gap,
                    splitTables ? rightWidth : contentWidth, tableHeight);
                y += splitTables ? tableHeight + gap : 2 * (tableHeight + gap);
                dashboardCanvas.Height = y + margin;
                // AutoScroll keeps the previous virtual bounds after controls shrink.
                // Recreate its scroll range when the window changes size.
                pnlDashboard.AutoScroll = false;
                pnlDashboard.AutoScroll = true;
            }
            finally
            {
                dashboardCanvas.ResumeLayout();
                pnlDashboard.ResumeLayout();
                layingOutDashboard = false;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            timerClock.Start(); // Bật đồng hồ khi form load
            TimerClock_Tick(null, null); // Cập nhật giờ ngay lập tức
            LoadDashboardData();
        }

        // Hàm cập nhật thời gian thực
        private void TimerClock_Tick(object sender, EventArgs e)
        {
            // Định dạng: Thứ Hai, 28/10/2025 | 16:42:05
            lblTime.Text = "📅 " + DateTime.Now.ToString("dddd, dd/MM/yyyy") + " | 🕒 " + DateTime.Now.ToString("HH:mm:ss");
        }

        // =================== LOAD DASHBOARD DATA TỪ BLL ===================
        private void LoadDashboardData()
        {
            try
            {
                // 1. LOAD THẺ THỐNG KÊ
                var cards = ThongKeBLL.LayThongKeCards();
                lblCard1Value.Text = cards.DoanhThu;
                lblCard2Value.Text = cards.SoVe;
                lblCard3Value.Text = cards.SoSuKien;
                lblCard4Value.Text = cards.SoDonHuy;

                // 2. LOAD BIỂU ĐỒ DOANH THU
                chartDoanhThu.Series[0].Points.Clear();
                DataTable dtDoanhThu = ThongKeBLL.LayDoanhThu7Ngay();
                foreach (DataRow row in dtDoanhThu.Rows)
                {
                    DateTime ngay = Convert.ToDateTime(row["Ngay"]);
                    double doanhThu = Convert.ToDouble(row["DoanhThu"]);
                    chartDoanhThu.Series[0].Points.AddXY(ngay.ToString("dd/MM"), doanhThu);
                }

                // 3. LOAD BIỂU ĐỒ TỶ LỆ
                chartTyLe.Series[0].Points.Clear();
                DataTable dtTyLe = ThongKeBLL.LayTyLeGhe();
                foreach (DataRow row in dtTyLe.Rows)
                {
                    string trangThai = row["TrangThai"]?.ToString() ?? "";
                    int soLuong = Convert.ToInt32(row["SoLuong"]);
                    int index = chartTyLe.Series[0].Points.AddXY(trangThai, soLuong);
                    chartTyLe.Series[0].Points[index].LegendText = trangThai;
                }

                // 4. LOAD BẢNG SỰ KIỆN SẮP DIỄN RA
                dgvSuKienSapDien.DataSource = ThongKeBLL.LaySuKienSapDien(5);
                if (dgvSuKienSapDien.Columns["Thời gian"] != null)
                    dgvSuKienSapDien.Columns["Thời gian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // 5. LOAD BẢNG TOP SỰ KIỆN
                dgvTopSuKien.DataSource = ThongKeBLL.LayTopSuKienDoanhThu(5);
                if (dgvTopSuKien.Columns["Doanh thu"] != null)
                    dgvTopSuKien.Columns["Doanh thu"].DefaultCellStyle.Format = "N0";
                ConfigureDashboardColumns();
            }
            catch { /* Bỏ qua lỗi nếu chưa có data */ }
        }

        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (grid.Columns[e.ColumnIndex].Name == "Tình trạng")
            {
                string status = e.Value?.ToString() ?? "";
                Color badgeColor = Color.FromArgb(241, 245, 249);
                Color textColor = Color.FromArgb(71, 85, 105);

                if (status == "Còn vé") { badgeColor = Color.FromArgb(209, 250, 229); textColor = Color.FromArgb(6, 95, 70); }
                else if (status == "Sắp diễn ra" || status == "Đang diễn ra") { badgeColor = Color.FromArgb(219, 234, 254); textColor = Color.FromArgb(30, 64, 175); }
                else if (status == "Hết vé") { badgeColor = Color.FromArgb(254, 226, 226); textColor = Color.FromArgb(153, 27, 27); }

                ManagementGridRenderer.PaintStatus(grid, e, status, badgeColor, textColor);
            }
        }

        private void ConfigureDashboardColumns()
        {
            SetDashboardColumn(dgvSuKienSapDien, "Tên sự kiện", 150, 55);
            SetDashboardColumn(dgvSuKienSapDien, "Thời gian", 150, 0);
            SetDashboardColumn(dgvSuKienSapDien, "Địa điểm", 140, 45);
            SetDashboardColumn(dgvSuKienSapDien, "Tình trạng", 130, 0);
            DataGridViewColumn status = dgvSuKienSapDien.Columns["Tình trạng"];
            if (status != null)
            {
                status.DefaultCellStyle.Padding = Padding.Empty;
                status.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                status.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            SetDashboardColumn(dgvTopSuKien, "Tên sự kiện", 160, 100);
            SetDashboardColumn(dgvTopSuKien, "Doanh thu", 140, 0);
            DataGridViewColumn revenue = dgvTopSuKien.Columns["Doanh thu"];
            if (revenue != null)
            {
                revenue.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                revenue.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            ClearDashboardSelection();
        }

        private void ClearDashboardSelection()
        {
            // Run once after the form receives focus so binding does not select the first row again.
            foreach (DataGridView grid in new DataGridView[] { dgvSuKienSapDien, dgvTopSuKien })
            {
                grid.ClearSelection();
                grid.CurrentCell = null;
            }
        }

        private void SetDashboardColumn(DataGridView grid, string name, int minimumWidth, int fillWeight)
        {
            DataGridViewColumn column = grid.Columns[name];
            if (column == null) return;
            column.MinimumWidth = Px(minimumWidth);
            column.AutoSizeMode = fillWeight > 0 ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None;
            if (fillWeight > 0) column.FillWeight = fillWeight;
            else column.Width = Px(minimumWidth);
        }

        public void OpenChildForm(Form childForm, Guna.UI2.WinForms.Guna2Button btnActive)
        {
            if (currentFormChild != null) currentFormChild.Close();
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
            resizeBox.BringToFront();

            ResetButtonColors();
            if (btnActive != null) { btnActive.FillColor = Color.FromArgb(16, 185, 129); btnActive.ForeColor = Color.White; }
        }

        private void ResetButtonColors()
        {
            Color defaultColor = Color.Transparent;
            Color defaultTextColor = Color.FromArgb(203, 213, 225);
            if (btnTrangChu != null) { btnTrangChu.FillColor = defaultColor; btnTrangChu.ForeColor = defaultTextColor; }
            if (btnSuKien != null) { btnSuKien.FillColor = defaultColor; btnSuKien.ForeColor = defaultTextColor; }
            if (btnSuatDien != null) { btnSuatDien.FillColor = defaultColor; btnSuatDien.ForeColor = defaultTextColor; }
            if (btnKhuVuc != null) { btnKhuVuc.FillColor = defaultColor; btnKhuVuc.ForeColor = defaultTextColor; }
            if (btnKhachHang != null) { btnKhachHang.FillColor = defaultColor; btnKhachHang.ForeColor = defaultTextColor; }
            if (btnBanVe != null) { btnBanVe.FillColor = defaultColor; btnBanVe.ForeColor = defaultTextColor; }
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            if (currentFormChild != null) { currentFormChild.Close(); currentFormChild = null; }
            if (pnlDashboard != null) { pnlDashboard.Visible = true; pnlDashboard.BringToFront(); }
            LayoutDashboard();
            ResetButtonColors();
            if (btnTrangChu != null) { btnTrangChu.FillColor = Color.FromArgb(16, 185, 129); btnTrangChu.ForeColor = Color.White; }
            LoadDashboardData();
        }

        public void btnSuKien_Click(object sender, EventArgs e)
        {
            if (!PhienDangNhap.LaQuanLy)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Quản lý Sự kiện!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (pnlDashboard != null) pnlDashboard.Visible = false;
            OpenChildForm(new FrmQuanLySuKien(), btnSuKien);
        }

        public void btnSuatDien_Click(object sender, EventArgs e)
        {
            if (!PhienDangNhap.LaQuanLy)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Quản lý Suất diễn!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (pnlDashboard != null) pnlDashboard.Visible = false;
            OpenChildForm(new FrmQuanLySuatDien(), btnSuatDien);
        }

        private void btnKhuVuc_Click(object sender, EventArgs e)
        {
            if (!PhienDangNhap.LaQuanLy)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Quản lý Khu vực & Giá vé!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Tính năng Quản lý Khu vực & Giá vé đang được phát triển!", "Thông báo");
        }

        private void chartDoanhThu_Click(object sender, EventArgs e)
        {

        }

        private void ApplyPhanQuyen()
        {
            if (PhienDangNhap.DaDangNhap)
            {
                var tk = PhienDangNhap.TaiKhoan;
                if (lblUser != null)
                {
                    lblUser.Text = $"{tk.TenDangNhap} ({tk.VaiTro})";
                }
                bool laQuanLy = PhienDangNhap.LaQuanLy;

                // Nhân viên chỉ được xem Bán vé, Khách hàng, Trang chủ
                // Ẩn toàn bộ phân hệ Quản trị khỏi thanh điều hướng
                if (btnSuKien != null) btnSuKien.Visible = laQuanLy;
                if (btnSuatDien != null) btnSuatDien.Visible = laQuanLy;
                if (btnKhuVuc != null) btnKhuVuc.Visible = laQuanLy;
                if (btnCaiDat != null) btnCaiDat.Visible = laQuanLy;

                LayoutSidebar();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DangXuat = true;
                PhienDangNhap.KetThuc();
                Close();
            }
        }
    }
}

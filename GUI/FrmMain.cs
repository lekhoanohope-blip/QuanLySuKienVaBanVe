using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmMain : Form
    {
        // SỬA LẠI CHUỖI KẾT NỐI CHO ĐÚNG VỚI MÁY CỦA BẠN
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True";
        private Form currentFormChild;
        private System.Windows.Forms.Timer timerClock; // Khai báo Timer

        public FrmMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Khởi tạo Timer chạy mỗi 1 giây
            timerClock = new System.Windows.Forms.Timer();
            timerClock.Interval = 1000;
            timerClock.Tick += TimerClock_Tick;
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

        // =================== LOAD DASHBOARD DATA TỪ SQL ===================
        private void LoadDashboardData()
        {
            // 1. LOAD THẺ THỐNG KÊ
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sqlDoanhThu = "SELECT SUM(TongTien) FROM DonVe WHERE TrangThai = N'DaThanhToan'";
                    lblCard1Value.Text = ExecuteScalar(conn, sqlDoanhThu) + " đ";

                    string sqlSoVe = "SELECT COUNT(*) FROM ChiTietVe WHERE TrangThai = N'HopLe'";
                    lblCard2Value.Text = ExecuteScalar(conn, sqlSoVe) + " vé";

                    string sqlSuKien = "SELECT COUNT(*) FROM SuKien WHERE TrangThai = N'DangDien'";
                    lblCard3Value.Text = ExecuteScalar(conn, sqlSuKien) + " sự kiện";

                    string sqlDonHuy = "SELECT COUNT(*) FROM DonVe WHERE TrangThai = N'DaHuy'";
                    lblCard4Value.Text = ExecuteScalar(conn, sqlDonHuy) + " đơn";
                }
            }
            catch { /* Bỏ qua lỗi nếu chưa có data */ }

            // 2. LOAD BIỂU ĐỒ DOANH THU
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    chartDoanhThu.Series[0].Points.Clear();
                    string query = @"
                        SELECT CAST(NgayDat AS DATE) as Ngay, SUM(TongTien) as DoanhThu 
                        FROM DonVe 
                        WHERE TrangThai = N'DaThanhToan' AND NgayDat >= DATEADD(day, -7, GETDATE())
                        GROUP BY CAST(NgayDat AS DATE) ORDER BY Ngay ASC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime ngay = Convert.ToDateTime(reader["Ngay"]);
                            double doanhThu = Convert.ToDouble(reader["DoanhThu"]);
                            chartDoanhThu.Series[0].Points.AddXY(ngay.ToString("dd/MM"), doanhThu);
                        }
                    }
                }
            }
            catch { /* Bỏ qua lỗi nếu chưa có data */ }

            // 3. LOAD BIỂU ĐỒ TỶ LỆ
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    chartTyLe.Series[0].Points.Clear();
                    string query = "SELECT TrangThai, COUNT(*) as SoLuong FROM Ghe GROUP BY TrangThai";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string trangThai = reader["TrangThai"].ToString();
                            int soLuong = Convert.ToInt32(reader["SoLuong"]);
                            int index = chartTyLe.Series[0].Points.AddXY(trangThai, soLuong);
                            chartTyLe.Series[0].Points[index].LegendText = trangThai;
                        }
                    }
                }
            }
            catch { /* Bỏ qua lỗi nếu chưa có data */ }

            // 4. LOAD BẢNG SỰ KIỆN SẮP DIỄN RA
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 5 sk.TenSK AS [Tên sự kiện], sd.ThoiGianBatDau AS [Thời gian], sd.DiaDiem AS [Địa điểm], 
                               CASE 
                                   WHEN sd.TrangThai = N'DaKetThuc' THEN N'Hết vé'
                                   WHEN sd.ThoiGianBatDau <= GETDATE() THEN N'Đang diễn ra'
                                   ELSE N'Còn vé'
                               END AS [Tình trạng]
                        FROM SuatDien sd
                        JOIN SuKien sk ON sd.MaSK = sk.MaSK
                        WHERE sd.ThoiGianBatDau > DATEADD(day, -1, GETDATE())
                        ORDER BY sd.ThoiGianBatDau ASC";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvSuKienSapDien.DataSource = dt;
                        if (dgvSuKienSapDien.Columns["Thời gian"] != null)
                            dgvSuKienSapDien.Columns["Thời gian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                }
            }
            catch { /* Bỏ qua lỗi nếu chưa có data */ }

            // 5. LOAD BẢNG TOP SỰ KIỆN
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 5 sk.TenSK AS [Tên sự kiện], SUM(dv.TongTien) AS [Doanh thu]
                        FROM DonVe dv
                        JOIN SuatDien sd ON dv.MaSuat = sd.MaSuat
                        JOIN SuKien sk ON sd.MaSK = sk.MaSK
                        WHERE dv.TrangThai = N'DaThanhToan'
                        GROUP BY sk.TenSK
                        ORDER BY [Doanh thu] DESC";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvTopSuKien.DataSource = dt;
                        if (dgvTopSuKien.Columns["Doanh thu"] != null)
                            dgvTopSuKien.Columns["Doanh thu"].DefaultCellStyle.Format = "N0";
                    }
                }
            }
            catch { /* Bỏ qua lỗi nếu chưa có data */ }
        }

        private string ExecuteScalar(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    if (result is int || result is long) return result.ToString();
                    return Convert.ToDouble(result).ToString("N0");
                }
                return "0";
            }
        }

        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSuKienSapDien.Columns[e.ColumnIndex].HeaderText == "Tình trạng")
            {
                e.PaintBackground(e.CellBounds, true);
                string status = e.Value?.ToString() ?? "";
                Color badgeColor = Color.Gray;
                Color textColor = Color.White;

                if (status == "Còn vé") { badgeColor = Color.FromArgb(209, 250, 229); textColor = Color.FromArgb(6, 95, 70); }
                else if (status == "Sắp diễn ra" || status == "Đang diễn ra") { badgeColor = Color.FromArgb(219, 234, 254); textColor = Color.FromArgb(30, 64, 175); }
                else if (status == "Hết vé") { badgeColor = Color.FromArgb(254, 226, 226); textColor = Color.FromArgb(153, 27, 27); }

                Rectangle rect = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + 10, e.CellBounds.Width - 20, e.CellBounds.Height - 20);
                using (GraphicsPath path = GetRoundedRect(rect, 15))
                using (SolidBrush brush = new SolidBrush(badgeColor))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
                TextRenderer.DrawText(e.Graphics, status, e.CellStyle.Font, rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
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
            ResetButtonColors();
            if (btnTrangChu != null) { btnTrangChu.FillColor = Color.FromArgb(16, 185, 129); btnTrangChu.ForeColor = Color.White; }
            LoadDashboardData();
        }

        public void btnSuKien_Click(object sender, EventArgs e)
        {
            if (pnlDashboard != null) pnlDashboard.Visible = false;
            OpenChildForm(new FrmQuanLySuKien(), btnSuKien);
        }

        public void btnSuatDien_Click(object sender, EventArgs e)
        {
            if (pnlDashboard != null) pnlDashboard.Visible = false;
            OpenChildForm(new FrmQuanLySuatDien(), btnSuatDien);
        }

        private void btnKhuVuc_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng Quản lý Khu vực & Giá vé đang được phát triển!", "Thông báo");
        }
    }
}
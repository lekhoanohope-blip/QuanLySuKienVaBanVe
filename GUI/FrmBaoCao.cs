using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmBaoCao : Form
    {
        private static readonly Color XanhLa = Color.FromArgb(13, 148, 136);
        private static readonly Color ChuXam = Color.FromArgb(100, 116, 139);
        private static readonly Color ChuDam = Color.FromArgb(30, 41, 59);
        private static readonly Color Tang = Color.FromArgb(22, 163, 74);
        private static readonly Color Giam = Color.FromArgb(220, 38, 38);
        private static readonly Color VienNhat = Color.FromArgb(226, 232, 240);

        private DataTable bangHienTai;
        private bool dangNap;

        // Animation biểu đồ: mỗi lần nạp lại, giá trị chạy nội suy từ số đang
        // hiển thị sang số mới trong ~0,36 giây (24 bước, ease-out), thay vì
        // nhảy phắt một cái. Timer nằm trong components nên form đóng là tự hủy.
        private Timer timerHoatHinh;
        private int buocHoatHinh;
        private const int TongBuocHoatHinh = 24;
        private Series chuoiDuong;
        private Series chuoiTron;
        private double[] duongTu, duongDen, tronTu, tronDen;

        // Bảng màu dùng chung cho lát bánh và ô màu trong chú thích (đúng thứ tự).
        // Bảng màu bánh tròn: tông pastel hiện đại (teal/vàng/hồng/xanh lam/xanh nõn).
        private static readonly Color[] MauTron = new[]
        {
            Color.FromArgb(45, 212, 191),
            Color.FromArgb(251, 191, 36),
            Color.FromArgb(244, 114, 182),
            Color.FromArgb(96, 165, 250),
            Color.FromArgb(163, 230, 53)
        };


        public FrmBaoCao()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ApDungKieuGuna();
            DungKieuBieuDo(chartDuong, false);
            DungKieuBieuDo(chartTron, false);
        }

        /// <summary>
        /// Trang trí chạy lúc khởi tạo, cố tình để ngoài Designer để lần sau mở
        /// Designer sửa rồi lưu thì Visual Studio không ghi đè mất.
        /// </summary>
        private void ApDungKieuGuna()
        {
            // Thẻ KPI + khung biểu đồ/lưới + thanh lọc: bo tròn lớn + bóng đổ mềm
            // màu slate nhạt, nhìn nổi khỏi nền mà không gắt.
            LamMemThe(theDoanhThu, 20);
            LamMemThe(theVe, 20);
            LamMemThe(theLapDay, 20);
            LamMemThe(pnlDuong, 18);
            LamMemThe(pnlTron, 18);
            LamMemThe(pnlLuoi, 18);
            LamMemThe(pnlLoc, 16);

            // Chip icon: chuyển màu nhẹ theo chiều dọc + bo tròn ảnh icon cho khớp chip.
            DatChipIcon(iconDoanhThu, picIconDoanhThu, Color.FromArgb(223, 245, 236));
            DatChipIcon(iconVe, picIconVe, Color.FromArgb(253, 238, 221));
            DatChipIcon(iconLapDay, picIconLapDay, Color.FromArgb(239, 233, 253));

            // Bo tròn khung biểu đồ và lưới cho khớp góc panel chứa.
            foreach (var khungBieuDo in new Control[] { chartDuong, chartTron, dgv })
            {
                var elipKhung = new Guna.UI2.WinForms.Guna2Elipse();
                elipKhung.TargetControl = khungBieuDo;
                elipKhung.BorderRadius = 18;
            }

            // Tiêu đề mục: to và đậm hơn một nấc cho rõ tầng bậc.
            foreach (var t in new[] { lblTieuDeDuong, lblTieuDeTron, lblChiTiet })
            {
                t.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
                t.ForeColor = ChuDam;
            }
            lblDem.Font = new Font("Segoe UI", 9.5f);
            lblDem.ForeColor = ChuXam;

            // Nút chính: gradient teal + bóng đổ cùng tông, hover đậm hơn.
            btnXuat.BorderRadius = 14;
            btnXuat.FillColor = Color.FromArgb(13, 148, 136);
            btnXuat.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            btnXuat.ForeColor = Color.White;
            btnXuat.Animated = true;
            btnXuat.Cursor = Cursors.Hand;
            btnXuat.HoverState.FillColor = Color.FromArgb(15, 118, 110);
            btnXuat.ShadowDecoration.Enabled = true;
            btnXuat.ShadowDecoration.Color = Color.FromArgb(70, 20, 184, 166);
            btnXuat.ShadowDecoration.Depth = 10;
            btnXuat.ShadowDecoration.BorderRadius = 14;

            // Ô lọc: viền nhạt, bo tròn, rê chuột viền đậm hơn, focus viền teal.
            foreach (var cbo in new[] { cboSuKien, cboSuatDien, cboBang })
            {
                cbo.BorderRadius = 12;
                cbo.BorderColor = VienNhat;
                cbo.HoverState.BorderColor = Color.FromArgb(203, 213, 225);
                cbo.FocusedState.BorderColor = XanhLa;
            }
            foreach (var dtp in new[] { dtpTu, dtpDen })
            {
                dtp.BorderRadius = 12;
                dtp.FillColor = Color.White;
                dtp.ForeColor = Color.FromArgb(51, 65, 85);
                dtp.Font = new Font("Segoe UI", 10f);
            }
            foreach (var lb in new[] { lblTuNgay, lblMuiTen, lblSuKien, lblSuatDien })
                lb.ForeColor = ChuXam;

            // Kiểu lưới của Guna2 (phần này Designer của Guna2 không tự giữ).
            dgv.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(71, 85, 105);
            dgv.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgv.ThemeStyle.HeaderStyle.Height = 42;
            dgv.ThemeStyle.RowsStyle.Height = 38;
            dgv.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(204, 251, 241);
            dgv.ThemeStyle.RowsStyle.SelectionForeColor = ChuDam;
            dgv.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.GridColor = Color.FromArgb(232, 237, 242);
        }

        private static void DungKieuBieuDo(Chart chart, bool coChuThich)
        {
            chart.BackColor = Color.White;
            chart.BorderlineWidth = 0;
            ChartArea area = chart.ChartAreas[0];
            area.BackColor = Color.Transparent;
            area.BorderWidth = 0;
            area.AxisX.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(148, 163, 184);
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisY.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(148, 163, 184);
            area.AxisY.LabelStyle.Format = "#,##0";
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(238, 242, 247);
            area.AxisY.MajorTickMark.Enabled = false;
            if (coChuThich && chart.Legends.Count == 0)
            {
                chart.Legends.Add(new Legend("Main")
                {
                    Docking = Docking.Right,
                    Alignment = StringAlignment.Center,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = ChuDam
                });
            }
        }

        // ------------------------------ Sự kiện giao diện ------------------------------

        private void FrmBaoCao_Load(object sender, EventArgs e)
        {
            if (!TopLevel) MinimumSize = Size.Empty;
            try
            {
                dangNap = true;
                dtpTu.Value = DateTime.Today.AddDays(-30);
                dtpDen.Value = DateTime.Today;
                cboBang.SelectedIndex = 0;
                NapSuKien();
                NapSuatDien();
            }
            finally
            {
                dangNap = false;
            }
            TaiDuLieu();
            SapXepHangLoc();
        }

        private void dtpTu_ValueChanged(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void dtpDen_ValueChanged(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangNap) return;
            NapSuatDien();
            TaiDuLieu();
        }

        private void cboSuatDien_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void cboBang_SelectedIndexChanged(object sender, EventArgs e)
        {
            NapBangChiTiet();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            XuatCsv();
        }

        private void pnlDauLuoi_Resize(object sender, EventArgs e)
        {
            cboBang.Location = new Point(pnlDauLuoi.Width - 192, (pnlDauLuoi.Height - cboBang.Height) / 2);
        }

        // ------------------------------ Nạp dữ liệu ------------------------------

        private void NapSuKien()
        {
            DataTable dt = SuKienBLL.LayDanhSach();
            DataRow tatCa = dt.NewRow();
            tatCa["MaSK"] = 0;
            tatCa["TenSK"] = "Tất cả sự kiện";
            dt.Rows.InsertAt(tatCa, 0);
            cboSuKien.DisplayMember = "TenSK";
            cboSuKien.ValueMember = "MaSK";
            cboSuKien.DataSource = dt;
        }

        private void NapSuatDien()
        {
            int maSK = LayMaChon(cboSuKien);
            DataTable tatCaSuat = SuatDienBLL.LayDanhSach();
            var bang = new DataTable();
            bang.Columns.Add("MaSuat", typeof(int));
            bang.Columns.Add("HienThi", typeof(string));
            bang.Rows.Add(0, "Tất cả");
            foreach (DataRow row in tatCaSuat.Rows)
            {
                if (maSK != 0 && Convert.ToInt32(row["MaSK"]) != maSK) continue;
                DateTime batDau = Convert.ToDateTime(row["ThoiGianBatDau"]);
                bang.Rows.Add(
                    Convert.ToInt32(row["MaSuat"]),
                    string.Format("{0} — {1:dd/MM/yyyy HH:mm}", row["TenSK"], batDau));
            }
            // Giữ nguyên cờ dangNap của nơi gọi, chỉ chặn sự kiện của chính combo này.
            bool coCu = dangNap;
            dangNap = true;
            try
            {
                cboSuatDien.DisplayMember = "HienThi";
                cboSuatDien.ValueMember = "MaSuat";
                cboSuatDien.DataSource = bang;
            }
            finally
            {
                dangNap = coCu;
            }
        }

        private static int LayMaChon(ComboBox cbo)
        {
            if (cbo == null || cbo.SelectedValue == null) return 0;
            int v;
            return int.TryParse(cbo.SelectedValue.ToString(), out v) ? v : 0;
        }

        private void TaiDuLieu()
        {
            if (dangNap) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                int maSK = LayMaChon(cboSuKien);
                int maSuat = LayMaChon(cboSuatDien);
                DateTime tu = dtpTu.Value.Date;
                DateTime den = dtpDen.Value.Date;

                KpiBaoCao kpi = BaoCaoBLL.LayKpi(maSK, maSuat, tu, den);
                lblDoanhThu.Text = string.Format("{0:N0} đ", kpi.TongDoanhThu);
                lblVe.Text = kpi.TongVeDaBan.ToString("N0");
                lblLapDay.Text = kpi.TyLeLapDay.HasValue
                    ? kpi.TyLeLapDay.Value.ToString("N1") + "%"
                    : "—";
                DatXuHuong(lblTrendDoanhThu, kpi.TangTruongDoanhThu);
                DatXuHuong(lblTrendVe, kpi.TangTruongVe);
                DatXuHuong(lblTrendLapDay, kpi.ChenhLechLapDay);

                // Biểu đồ đường: khoảng ngắn thì theo ngày, khoảng dài thì gom theo tháng.
                int soNgay = (den - tu).Days + 1;
                string cheDo = soNgay <= 45 ? "Ngay" : "Thang";
                lblTieuDeDuong.Text = cheDo == "Ngay" ? "Doanh thu theo ngày" : "Doanh thu theo tháng";
                VeDuongDoanhThu(BaoCaoBLL.DoanhThuTheoThoiGian(maSK, maSuat, tu, den, cheDo), cheDo);

                VeBanhTron(BaoCaoBLL.DoanhThuTheoSuKien(maSK, maSuat, tu, den));

                NapBangChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Lỗi tải báo cáo: " + ex.Message,
                    "Báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static void DatXuHuong(Label lbl, decimal? phanTram)
        {
            if (!phanTram.HasValue)
            {
                lbl.Text = "Chưa có số liệu kỳ trước";
                lbl.ForeColor = ChuXam;
                return;
            }
            decimal v = Math.Round(phanTram.Value, 1);
            if (v > 0)
            {
                lbl.Text = "▲ +" + v.ToString("N1") + "% so với kỳ trước";
                lbl.ForeColor = Tang;
            }
            else if (v < 0)
            {
                lbl.Text = "▼ " + v.ToString("N1") + "% so với kỳ trước";
                lbl.ForeColor = Giam;
            }
            else
            {
                lbl.Text = "■ 0,0% so với kỳ trước";
                lbl.ForeColor = ChuXam;
            }
        }

        private void DinhDangCotLuoi()
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                string h = col.HeaderText;
                if (h.Contains("Mã")) { col.FillWeight = 55; col.MinimumWidth = 55; }
                else if (h.Contains("Ngày")) { col.FillWeight = 105; col.MinimumWidth = 100; }
                else if (h.Contains("Khách")) { col.FillWeight = 165; col.MinimumWidth = 120; }
                else if (h.Contains("SĐT")) { col.FillWeight = 85; col.MinimumWidth = 80; }
                else if (h.Contains("Sự kiện")) { col.FillWeight = 165; col.MinimumWidth = 120; }
                else if (h.Contains("Suất")) { col.FillWeight = 105; col.MinimumWidth = 100; }
                else if (h.Contains("Số vé")) { col.FillWeight = 55; col.MinimumWidth = 55; }
                else if (h.Contains("Tổng tiền")) { col.FillWeight = 95; col.MinimumWidth = 90; }
                else if (h.Contains("Trạng thái")) { col.FillWeight = 95; col.MinimumWidth = 90; }
                else if (h.Contains("Khu vực")) { col.FillWeight = 130; col.MinimumWidth = 110; }
                else if (h.Contains("Hạng")) { col.FillWeight = 90; col.MinimumWidth = 80; }
                else if (h.Contains("ghế") || h.Contains("Ghế")) { col.FillWeight = 80; col.MinimumWidth = 70; }
                else if (h.Contains("bán") || h.Contains("giữ") || h.Contains("trống")) { col.FillWeight = 70; col.MinimumWidth = 65; }
                else if (h.Contains("Tỷ lệ")) { col.FillWeight = 110; col.MinimumWidth = 100; }
            }
        }

        private void pnlLoc_SizeChanged(object sender, EventArgs e)
        {
            SapXepHangLoc();
        }

        private void SapXepHangLoc()
        {
            if (pnlLoc == null || pnlNut == null || cboSuKien == null || cboSuatDien == null
                || lblSuKien == null || lblSuatDien == null) return;
            int nutLeft = pnlLoc.ClientSize.Width - pnlNut.Width - 12;
            int x = 386;
            lblSuKien.Location = new Point(x, 22);
            x += 54 + 8;
            int conLai = nutLeft - x - 64 - 12;
            if (conLai < 200) conLai = 200;
            int rong1 = conLai / 2;
            int rong2 = conLai - rong1;
            cboSuKien.SetBounds(x, 13, rong1, 36);
            x += rong1 + 12;
            lblSuatDien.Location = new Point(x, 22);
            x += 64 + 8;
            cboSuatDien.SetBounds(x, 13, rong2, 36);
        }

        private void NapBangChiTiet()
        {
            if (dangNap) return;
            int maSK = LayMaChon(cboSuKien);
            int maSuat = LayMaChon(cboSuatDien);
            if (cboBang.SelectedIndex == 1)
                bangHienTai = BaoCaoBLL.TyLeLapDayTheoKhuVuc(maSK, maSuat);
            else
                bangHienTai = BaoCaoBLL.DanhSachDonVe(maSK, maSuat, dtpTu.Value.Date, dtpDen.Value.Date);

            dgv.DataSource = bangHienTai;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.HeaderText.Contains("Tỷ lệ"))
                    col.DefaultCellStyle.Format = "0.0\"%\"";
                else if (col.ValueType == typeof(DateTime))
                    col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                else if (col.ValueType == typeof(decimal))
                    col.DefaultCellStyle.Format = "#,##0";
            }
            DinhDangCotLuoi();
            lblDem.Text = string.Format("Tổng: {0:N0} dòng", bangHienTai.Rows.Count);
        }

        // ------------------------------ Biểu đồ ------------------------------


        // Đọc giá trị Y đang hiển thị của một biểu đồ, tra theo nhãn, để làm
        // điểm bắt đầu cho animation lần vẽ sau.
        private static Dictionary<string, double> DocGiaTriHienTai(Chart chart)
        {
            var kq = new Dictionary<string, double>();
            if (chart.Series.Count > 0)
                foreach (DataPoint p in chart.Series[0].Points)
                    if (!kq.ContainsKey(p.AxisLabel)) kq[p.AxisLabel] = p.YValues[0];
            return kq;
        }

        private void BatDauHoatHinh(Series chuoi, double[] tu, double[] dich, bool laDuong)
        {
            if (laDuong) { chuoiDuong = chuoi; duongTu = tu; duongDen = dich; }
            else { chuoiTron = chuoi; tronTu = tu; tronDen = dich; }
            buocHoatHinh = 0;
            if (timerHoatHinh == null)
            {
                timerHoatHinh = components != null ? new Timer(components) : new Timer();
                timerHoatHinh.Interval = 15;
                timerHoatHinh.Tick += TimerHoatHinh_Tick;
            }
            timerHoatHinh.Stop();
            timerHoatHinh.Start();
        }

        private void TimerHoatHinh_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || Disposing) { timerHoatHinh.Stop(); return; }
            buocHoatHinh++;
            float t = Math.Min(1f, buocHoatHinh / (float)TongBuocHoatHinh);
            float nhe = 1f - (1f - t) * (1f - t) * (1f - t); // ease-out: nhanh đầu, chậm cuối
            bool xong = t >= 1f;
            if (chuoiDuong != null && chuoiDuong.Points.Count == duongDen.Length)
            {
                for (int i = 0; i < duongDen.Length; i++)
                    chuoiDuong.Points[i].YValues[0] = xong ? duongDen[i] : duongTu[i] + (duongDen[i] - duongTu[i]) * nhe;
                chartDuong.Invalidate();
            }
            if (chuoiTron != null && chuoiTron.Points.Count == tronDen.Length)
            {
                for (int i = 0; i < tronDen.Length; i++)
                    chuoiTron.Points[i].YValues[0] = xong ? tronDen[i] : tronTu[i] + (tronDen[i] - tronTu[i]) * nhe;
                chartTron.Invalidate();
            }
            if (xong)
            {
                timerHoatHinh.Stop();
                ChartArea area = chartDuong.ChartAreas[0];
                area.AxisY.Maximum = double.NaN;
                area.RecalculateAxesScale();
                chuoiDuong = null;
                chuoiTron = null;
            }
        }

        private void VeDuongDoanhThu(DataTable dt, string cheDo)
        {
            // Đọc giá trị đang hiển thị để animation bắt đầu từ đó: lọc lại
            // giữa chừng vẫn mượt, không rớt về 0 rồi mới chạy lên.
            Dictionary<string, double> giaTriCu = DocGiaTriHienTai(chartDuong);
            chartDuong.Series.Clear();
            chartDuong.Titles.Clear();
            chuoiDuong = null;
            ChartArea area = chartDuong.ChartAreas[0];
            if (dt.Rows.Count == 0)
            {
                chartDuong.Titles.Add("Chưa có dữ liệu trong khoảng đã chọn");
                area.AxisY.Maximum = double.NaN;
                return;
            }

            var s = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.SplineArea,
                Color = Color.FromArgb(130, 20, 184, 166),
                BackGradientStyle = GradientStyle.TopBottom,
                BackSecondaryColor = Color.FromArgb(10, 20, 184, 166),
                BorderColor = XanhLa,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                MarkerColor = Color.White,
                MarkerBorderColor = XanhLa,
                MarkerBorderWidth = 2
            };
            chartDuong.Series.Add(s);

            string[] nhan = new string[dt.Rows.Count];
            double[] dich = new double[dt.Rows.Count];
            double lonNhat = 0.0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nhan[i] = DinhDangNhan(dt.Rows[i]["Nhan"], cheDo);
                dich[i] = Convert.ToDouble(dt.Rows[i]["DoanhThu"]);
                if (dich[i] > lonNhat) lonNhat = dich[i];
            }
            double[] tu = new double[dich.Length];
            for (int i = 0; i < dich.Length; i++)
            {
                double cu;
                tu[i] = giaTriCu.TryGetValue(nhan[i], out cu) ? cu : 0.0;
                s.Points.AddXY(nhan[i], tu[i]);
            }

            area.AxisX.Interval = Math.Max(1, (int)Math.Ceiling(dt.Rows.Count / 12.0));
            area.AxisX.LabelStyle.Angle = dt.Rows.Count > 12 ? -30 : 0;
            // Giữ trần trục Y cố định trong lúc chạy animation để lưới không giật.
            area.AxisY.Maximum = lonNhat > 0 ? lonNhat * 1.1 : double.NaN;
            area.RecalculateAxesScale();
            BatDauHoatHinh(s, tu, dich, true);
        }

        private static string DinhDangNhan(object nhan, string cheDo)
        {
            string s = Convert.ToString(nhan);
            DateTime d;
            if (cheDo == "Ngay" && DateTime.TryParse(s, out d)) return d.ToString("dd/MM");
            if (cheDo == "Thang" && DateTime.TryParseExact(s, "yyyy-MM",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                return d.ToString("MM/yyyy");
            return s;
        }

        private void VeBanhTron(DataTable dt)
        {
            Dictionary<string, double> giaTriCu = DocGiaTriHienTai(chartTron);
            chartTron.Series.Clear();
            chartTron.Titles.Clear();
            chuoiTron = null;
            if (pnlChuThichTron != null) pnlChuThichTron.Controls.Clear();
            if (dt.Rows.Count == 0)
            {
                chartTron.Titles.Add("Chưa có dữ liệu trong khoảng đã chọn");
                return;
            }

            var s = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Doughnut,
                BorderColor = Color.White,
                BorderWidth = 2
            };
            s["DoughnutRadius"] = "55";
            s["PieLabelStyle"] = "Disabled";
            chartTron.Series.Add(s);
            chartTron.Palette = ChartColorPalette.None;
            chartTron.PaletteCustomColors = MauTron;
            DataRow[] hang = dt.Select(string.Empty, "DoanhThu DESC");
            const int gioiHan = 8;
            int soLat = Math.Min(hang.Length, gioiHan) + (hang.Length > gioiHan ? 1 : 0);
            string[] nhan = new string[soLat];
            double[] dich = new double[soLat];
            for (int i = 0; i < hang.Length && i < gioiHan; i++)
            {
                nhan[i] = Convert.ToString(hang[i]["TenSK"]);
                dich[i] = Convert.ToDouble(hang[i]["DoanhThu"]);
            }
            if (hang.Length > gioiHan)
            {
                decimal conLai = 0m;
                for (int i = gioiHan; i < hang.Length; i++)
                    conLai += Convert.ToDecimal(hang[i]["DoanhThu"]);
                nhan[gioiHan] = "Khác (" + (hang.Length - gioiHan) + " sự kiện)";
                dich[gioiHan] = Convert.ToDouble(conLai);
            }
            double[] tu = new double[dich.Length];
            for (int i = 0; i < dich.Length; i++)
            {
                double cu;
                tu[i] = giaTriCu.TryGetValue(nhan[i], out cu) ? cu : 0.0;
                s.Points.AddXY(nhan[i], tu[i]);
            }
            // Chú thích lấy số ĐÍCH ngay từ đầu để % luôn đúng trong lúc lát bánh chạy.
            CapNhatChuThichTron(nhan, dich);
            BatDauHoatHinh(s, tu, dich, false);
        }

        // ------------------------------ Xuất CSV ------------------------------

        private void CapNhatChuThichTron(string[] nhan, double[] giaTri)
        {
            if (pnlChuThichTron == null) return;
            pnlChuThichTron.Controls.Clear();
            decimal tong = 0m;
            foreach (double g in giaTri) tong += Convert.ToDecimal(g);
            int y = 10;
            int rong = pnlChuThichTron.ClientSize.Width - 12 - SystemInformation.VerticalScrollBarWidth;
            if (rong < 120) rong = 120;
            for (int i = 0; i < nhan.Length; i++)
            {
                int phanTram = tong > 0 ? (int)Math.Round(100m * Convert.ToDecimal(giaTri[i]) / tong) : 0;
                string chu = nhan[i] + "  " + phanTram + "%";
                var lbl = new Label
                {
                    Text = chu,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = ChuDam,
                    AutoSize = true,
                    MaximumSize = new Size(rong - 26, 0),
                    Location = new Point(26, 3)
                };
                int caoHang = Math.Max(22, lbl.PreferredSize.Height + 6);
                var mau = new Panel
                {
                    BackColor = MauTron[i % MauTron.Length],
                    Location = new Point(6, (caoHang - 14) / 2),
                    Size = new Size(14, 14)
                };
                var hang = new Panel
                {
                    Location = new Point(6, y),
                    Size = new Size(rong, caoHang)
                };
                hang.Controls.Add(mau);
                hang.Controls.Add(lbl);
                pnlChuThichTron.Controls.Add(hang);
                y += caoHang + 8;
            }
            pnlChuThichTron.AutoScrollMinSize = new Size(0, y);
        }

        // Thẻ trắng bo tròn + bóng đổ mềm (bóng cùng tông slate, không đen gắt).
        private static void LamMemThe(Guna.UI2.WinForms.Guna2Panel the, int boTron)
        {
            if (the == null) return;
            the.BorderRadius = boTron;
            the.FillColor = Color.White;
            the.ShadowDecoration.Enabled = true;
            the.ShadowDecoration.Color = Color.FromArgb(38, 15, 23, 42);
            the.ShadowDecoration.Depth = 14;
            the.ShadowDecoration.BorderRadius = boTron;
        }

        // Chip icon: nền màu nhạt + bo tròn ảnh icon cho khớp góc chip.
        private static void DatChipIcon(Guna.UI2.WinForms.Guna2Panel chip, PictureBox anh, Color mau)
        {
            if (chip == null) return;
            chip.BorderRadius = 18;
            chip.FillColor = mau;
            if (anh == null) return;
            var elip = new Guna.UI2.WinForms.Guna2Elipse();
            elip.TargetControl = anh;
            elip.BorderRadius = 18;
        }

        private void XuatCsv()
        {
            if (bangHienTai == null || bangHienTai.Rows.Count == 0)
            {
                MessageBox.Show(this, "Không có dữ liệu để xuất.",
                    "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "bao-cao.csv"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < bangHienTai.Columns.Count; i++)
                    {
                        if (i > 0) sb.Append(';');
                        sb.Append(GhiCsv(bangHienTai.Columns[i].ColumnName));
                    }
                    sb.AppendLine();
                    foreach (DataRow row in bangHienTai.Rows)
                    {
                        for (int i = 0; i < bangHienTai.Columns.Count; i++)
                        {
                            if (i > 0) sb.Append(';');
                            sb.Append(GhiCsv(DinhDangGiaTriCsv(row[i])));
                        }
                        sb.AppendLine();
                    }
                    // BOM UTF-8 để Excel mở đúng tiếng Việt; dấu ';' hợp với Excel bản Việt.
                    File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
                    MessageBox.Show(this,
                        string.Format("Đã xuất {0:N0} dòng ra file CSV.", bangHienTai.Rows.Count),
                        "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Lỗi xuất file: " + ex.Message,
                        "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string DinhDangGiaTriCsv(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            if (v is DateTime d) return d.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            return Convert.ToString(v, CultureInfo.InvariantCulture);
        }

        private static string GhiCsv(string s)
        {
            if (s == null) return "";
            if (s.IndexOfAny(new[] { ';', '"', '\r', '\n' }) < 0) return s;
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }
    }
}


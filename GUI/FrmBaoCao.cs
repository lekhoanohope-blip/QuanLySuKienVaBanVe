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
        private static readonly Color XanhLa = Color.FromArgb(15, 118, 110);
        private static readonly Color ChuXam = Color.FromArgb(100, 116, 139);
        private static readonly Color ChuDam = Color.FromArgb(30, 41, 59);
        private static readonly Color Tang = Color.FromArgb(21, 128, 61);
        private static readonly Color Giam = Color.FromArgb(220, 38, 38);
        private static readonly Color VienNhat = Color.FromArgb(226, 232, 240);

        private DataTable bangHienTai;
        private bool dangNap;

        // Nút in danh sách khách: tạo bằng code (không sửa Designer) để khỏi
        // đụng file Designer, vị trí do SapXepHangLoc sắp xếp cùng hàng lọc.
        private Guna.UI2.WinForms.Guna2Button btnInDsKhach;
        // Chống lặp bố cục: SapXepHangLoc đổi chiều cao pnlLoc sẽ kích hoạt
        // lại SizeChanged của chính nó.
        private bool dangSapXep;

        private Timer timerHoatHinh;
        private int buocHoatHinh;
        private const int TongBuocHoatHinh = 20;
        private Series chuoiDuong;
        private Series chuoiTron;
        private double[] duongTu, duongDen, tronTu, tronDen;

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
            TaoNutInDsKhach();
            ApDungKieuGuna();
            DungKieuBieuDo(chartDuong, false);
            DungKieuBieuDo(chartTron, false);
            // Chú thích bánh tròn: hẹp thì xuống đáy, rộng thì sang phải.
            // Chỉ chuyển khi panel đã có kích thước thật (sự kiện SizeChanged),
            // gọi ngay trong constructor sẽ ép Chart về chiều cao <= 0 và ném lỗi.
            pnlTron.SizeChanged += (s, e) => DieuChinhChuThichTron();
            // Các khối (KPI, biểu đồ, lưới) tự giãn theo cửa sổ, không chung
            // bảng chứa: chỉ chạy khi form đã có kích thước thật.
            SizeChanged += (s, e) => SapXepCacKhoi();
        }

        private void ApDungKieuGuna()
        {
            LamMemThe(theDoanhThu, 16);
            LamMemThe(theVe, 16);
            LamMemThe(theLapDay, 16);
            LamMemThe(pnlDuong, 16);
            LamMemThe(pnlTron, 16);
            LamMemThe(pnlLuoi, 16);
            LamMemThe(pnlLoc, 16);

            DatChipIcon(iconDoanhThu, picIconDoanhThu, Color.FromArgb(223, 245, 236));
            DatChipIcon(iconVe, picIconVe, Color.FromArgb(253, 238, 221));
            DatChipIcon(iconLapDay, picIconLapDay, Color.FromArgb(239, 233, 253));

            foreach (var khungBieuDo in new Control[] { chartDuong, chartTron, dgv })
            {
                var elipKhung = new Guna.UI2.WinForms.Guna2Elipse
                {
                    TargetControl = khungBieuDo,
                    BorderRadius = 14
                };
            }

            foreach (var t in new[] { lblTieuDeDuong, lblTieuDeTron, lblChiTiet })
            {
                t.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
                t.ForeColor = ChuDam;
            }
            lblDem.Font = new Font("Segoe UI", 9f);
            lblDem.ForeColor = ChuXam;

            btnXuat.BorderRadius = 12;
            btnXuat.FillColor = XanhLa;
            btnXuat.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnXuat.ForeColor = Color.White;
            btnXuat.Animated = true;
            btnXuat.Cursor = Cursors.Hand;
            btnXuat.HoverState.FillColor = Color.FromArgb(17, 94, 89);
            btnXuat.ShadowDecoration.Enabled = true;
            btnXuat.ShadowDecoration.Color = Color.FromArgb(50, 13, 148, 136);
            btnXuat.ShadowDecoration.Depth = 8;
            btnXuat.ShadowDecoration.BorderRadius = 12;

            // Nút phụ "In DS khách": nền trắng, viền + chữ teal đậm, cùng bo tròn.
            if (btnInDsKhach != null)
            {
                btnInDsKhach.BorderRadius = 12;
                btnInDsKhach.BorderThickness = 1;
                btnInDsKhach.BorderColor = XanhLa;
                btnInDsKhach.FillColor = Color.White;
                btnInDsKhach.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                btnInDsKhach.ForeColor = XanhLa;
                btnInDsKhach.Animated = true;
                btnInDsKhach.Cursor = Cursors.Hand;
                btnInDsKhach.HoverState.FillColor = Color.FromArgb(204, 251, 241);
                btnInDsKhach.HoverState.BorderColor = XanhLa;
                btnInDsKhach.HoverState.ForeColor = XanhLa;
            }

            foreach (var cbo in new[] { cboSuKien, cboSuatDien, cboBang })
            {
                cbo.BorderRadius = 10;
                cbo.BorderColor = VienNhat;
                cbo.HoverState.BorderColor = Color.FromArgb(203, 213, 225);
                cbo.FocusedState.BorderColor = XanhLa;
            }
            // Thanh cuộn/viền của danh sách xổ xuống ăn theo màu viền lúc
            // focus: để 2 combo lọc là xám slate cho khỏi còn vệt xanh, còn
            // combo chọn bảng giữ viền teal như cũ. Viền lúc focus của 2
            // combo này cũng chuyển sang xám theo, vì chung một màu.
            foreach (var cbo in new[] { cboSuKien, cboSuatDien })
                cbo.FocusedState.BorderColor = ChuXam;
            foreach (var dtp in new[] { dtpTu, dtpDen })
            {
                dtp.BorderRadius = 10;
                dtp.FillColor = Color.FromArgb(241, 245, 249);
                dtp.ForeColor = Color.FromArgb(51, 65, 85);
                dtp.Font = new Font("Segoe UI", 9.5f);
            }
            foreach (var lb in new[] { lblTuNgay, lblMuiTen, lblSuKien, lblSuatDien })
                lb.ForeColor = ChuXam;

            // Dòng xu hướng trên thẻ KPI: chữ 9pt + cho xuống dòng để khỏi bị
            // cắt đuôi khi thẻ hẹp; nới chiều cao dòng xu hướng.
            foreach (var pnlChu in new[] { pnlChuDoanhThu, pnlChuVe, pnlChuLapDay })
                if (pnlChu != null) pnlChu.Padding = new Padding(0, 14, 10, 6);
            foreach (var lb in new[] { lblTrendDoanhThu, lblTrendVe, lblTrendLapDay })
            {
                if (lb == null) continue;
                lb.Font = new Font("Segoe UI", 9f);
                lb.AutoSize = false;
                lb.Height = 30;
            }

            // Thứ tự Tab theo luồng thao tác: lọc trước, xuất sau; biểu đồ
            // không tương tác bàn phím nên rút khỏi thứ tự Tab.
            dtpTu.TabIndex = 0;
            dtpDen.TabIndex = 1;
            cboSuKien.TabIndex = 2;
            cboSuatDien.TabIndex = 3;
            pnlNut.TabIndex = 4;
            chartDuong.TabStop = false;
            chartTron.TabStop = false;

            dgv.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(71, 85, 105);
            dgv.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ThemeStyle.HeaderStyle.Height = 40;
            dgv.ThemeStyle.RowsStyle.Height = 36;
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
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5f);
            area.AxisX.LabelStyle.ForeColor = ChuXam;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisY.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5f);
            area.AxisY.LabelStyle.ForeColor = ChuXam;
            area.AxisY.LabelStyle.Format = "#,##0";
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(241, 245, 249);
            area.AxisY.MajorTickMark.Enabled = false;
            if (coChuThich && chart.Legends.Count == 0)
            {
                chart.Legends.Add(new Legend("Main")
                {
                    Docking = Docking.Right,
                    Alignment = StringAlignment.Center,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = ChuDam
                });
            }
        }

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
            SapXepCacKhoi();
        }

        private void dtpTu_ValueChanged(object sender, EventArgs e) => TaiDuLieu();
        private void dtpDen_ValueChanged(object sender, EventArgs e) => TaiDuLieu();
        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangNap) return;
            NapSuatDien();
            TaiDuLieu();
        }
        private void cboSuatDien_SelectedIndexChanged(object sender, EventArgs e) => TaiDuLieu();
        private void cboBang_SelectedIndexChanged(object sender, EventArgs e) => NapBangChiTiet();
        private void btnXuat_Click(object sender, EventArgs e) => XuatExcel();

        private void TaoNutInDsKhach()
        {
            btnInDsKhach = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "In DS khách",
                Size = new Size(150, 40)
            };
            btnInDsKhach.Click += BtnInDsKhach_Click;
            pnlLoc.Controls.Add(btnInDsKhach);
        }

        private void BtnInDsKhach_Click(object sender, EventArgs e)
        {
            int maSuat = LayMaChon(cboSuatDien);
            if (maSuat == 0)
            {
                MessageBox.Show(this, "Hãy chọn một suất diễn cụ thể để in danh sách khách.",
                    "In danh sách khách", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            InDanhSachKhach.In(maSuat, this);
        }

        /// <summary>
        /// Chú thích bánh tròn: panel hẹp thì chuyển xuống đáy (khỏi đè tiêu đề
        /// và bóp hình tròn), rộng thì trở lại neo phải. Chỉ đổi khi cần để
        /// khỏi lặp bố cục.
        /// </summary>
        private void DieuChinhChuThichTron()
        {
            if (pnlChuThichTron == null || pnlTron == null) return;
            int rong = pnlTron.ClientSize.Width;
            int cao = pnlTron.ClientSize.Height;
            // Chưa có kích thước thật thì chưa đụng vào: ép Dock lúc này
            // sẽ tính bố cục với chiều cao 0 và Chart ném lỗi Height <= 0.
            // SizeChanged sẽ gọi lại hàm này khi panel đã lên hình.
            if (rong <= 0 || cao <= 100) return;
            // Chỉ xuống đáy khi còn đủ chỗ cho hình tròn (tiêu đề 44 + đáy 90
            // + tối thiểu ~34 cho Chart); thấp hơn thì giữ neo phải cho an toàn.
            bool xuongDay = rong < 420 && cao > 170;
            if (xuongDay && pnlChuThichTron.Dock == DockStyle.Bottom) return;
            if (!xuongDay && pnlChuThichTron.Dock == DockStyle.Right) return;
            // Đặt kích thước TRƯỚC khi đổi Dock, và treo bố cục trong lúc đổi.
            // Đổi Dock trước rồi mới đặt chiều cao sẽ có một lượt bố cục trung
            // gian mà chú thích vẫn cao 280px, bóp Chart về chiều cao âm và
            // ném lỗi — đúng lỗi vừa gặp khi form đang được nhúng vào FrmMain.
            pnlTron.SuspendLayout();
            try
            {
                if (xuongDay)
                {
                    pnlChuThichTron.Height = 90;
                    pnlChuThichTron.Dock = DockStyle.Bottom;
                }
                else
                {
                    pnlChuThichTron.Width = 250;
                    pnlChuThichTron.Dock = DockStyle.Right;
                }
            }
            finally
            {
                pnlTron.ResumeLayout(true);
            }
        }

        /// <summary>
        /// Các khối tự giãn theo cửa sổ (KPI chia đều, biểu đồ 58/42, lưới
        /// chiếm phần còn lại), theo đúng kiểu FrmQuanLyKhuVuc và trang chủ
        /// trong repo vẫn làm: vị trí tính lúc chạy, Designer chỉ giữ hình
        /// lúc thiết kế. Chỉ tính khi form đã có kích thước thật.
        /// </summary>
        private void SapXepCacKhoi()
        {
            if (theDoanhThu == null || theVe == null || theLapDay == null
                || pnlDuong == null || pnlTron == null || pnlLuoi == null
                || pnlLoc == null) return;
            // Lúc nhúng form vào FrmMain (đổi FormBorderStyle, Dock Fill),
            // kích thước đi qua nhiều trạng thái trung gian: chưa ép bố cục
            // lúc này, đợi lượt SizeChanged khi form đã có kích thước thật.
            if (ClientSize.Width < 720 || ClientSize.Height < 400) return;
            int trai = Padding.Left;
            int phai = Padding.Right;
            int rongKhaDung = ClientSize.Width - trai - phai;
            if (rongKhaDung <= 0) return;
            const int khe = 16;
            int y = Padding.Top + pnlLoc.Height + khe;

            // Hàng KPI: 3 thẻ chia đều, thẻ cuối lấy phần lẻ khỏi hở mép.
            int rongThe = (rongKhaDung - khe * 2) / 3;
            int caoThe = theDoanhThu.Height;
            SuspendLayout();
            try
            {
                theDoanhThu.SetBounds(trai, y, rongThe, caoThe);
                theVe.SetBounds(trai + rongThe + khe, y, rongThe, caoThe);
                theLapDay.SetBounds(trai + (rongThe + khe) * 2, y,
                    rongKhaDung - (rongThe + khe) * 2, caoThe);
                y += caoThe + khe;

                // Hàng biểu đồ: 56% phần còn lại, lưới chiếm phần còn lại.
                int conLai = ClientSize.Height - Padding.Bottom - y;
                int caoBieuDo = (int)Math.Round((conLai - khe) * 0.56);
                if (caoBieuDo < 120) caoBieuDo = 120;
                int rongDuong = (int)Math.Round((rongKhaDung - khe) * 0.58);
                pnlDuong.SetBounds(trai, y, rongDuong, caoBieuDo);
                pnlTron.SetBounds(trai + rongDuong + khe, y,
                    rongKhaDung - rongDuong - khe, caoBieuDo);
                y += caoBieuDo + khe;

                int caoLuoi = ClientSize.Height - Padding.Bottom - y;
                if (caoLuoi < 60) caoLuoi = 60;
                pnlLuoi.SetBounds(trai, y, rongKhaDung, caoLuoi);
            }
            finally
            {
                ResumeLayout(true);
            }
            DieuChinhChuThichTron();
        }

        private void pnlDauLuoi_Resize(object sender, EventArgs e)
        {
            cboBang.Location = new Point(pnlDauLuoi.Width - 192, (pnlDauLuoi.Height - cboBang.Height) / 2);
        }

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
                    string.Format("{0:dd/MM/yyyy HH:mm} — {1}", batDau, row["TenSK"]));
            }
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
            return int.TryParse(cbo.SelectedValue.ToString(), out int v) ? v : 0;
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
            if (dangSapXep) return;
            if (pnlLoc == null || pnlNut == null || cboSuKien == null || cboSuatDien == null
                || lblSuKien == null || lblSuatDien == null) return;
            dangSapXep = true;
            try
            {
                // Nút Xuất luôn sát mép phải: trước đây chỉ tính nutLeft mà
                // không gán vị trí, nút đứng yên còn combo giãn đè lên nút,
                // phần đuôi combo lòi ra sau nút trông như một ô trống.
                int nutLeft = pnlLoc.ClientSize.Width - pnlNut.Width - 12;
                pnlNut.Location = new Point(nutLeft, 0);

                // Cụm nút bên phải: nút Xuất (pnlNut) + nút "In DS khách" bên trái nó.
                int inLeft = nutLeft;
                if (btnInDsKhach != null)
                {
                    inLeft = nutLeft - 150 - 10;
                    btnInDsKhach.SetBounds(inLeft, 11, 150, 40);
                }

                // Hẹp (kèm nút in mới) thì đẩy cụm Sự kiện + Suất diễn xuống
                // hàng hai, khỏi tràn lên nút. Ngưỡng 1200 tính cả nút in.
                bool haiHang = pnlLoc.ClientSize.Width < 1200;
                int caoMoi = haiHang ? 104 : 62;
                if (pnlLoc.Height != caoMoi) pnlLoc.Height = caoMoi;

                if (haiHang)
                {
                    int x = 16;
                    lblSuKien.Location = new Point(x, 64);
                    x += 54 + 8;
                    int conLai = inLeft - 12 - x - 84;
                    if (conLai < 160) conLai = 160;
                    int rong1 = conLai / 2;
                    int rong2 = conLai - rong1;
                    cboSuKien.SetBounds(x, 55, rong1, 36);
                    x += rong1 + 12;
                    lblSuatDien.Location = new Point(x, 64);
                    x += 64 + 8;
                    cboSuatDien.SetBounds(x, 55, rong2, 36);
                }
                else
                {
                    int xMot = 390;
                    lblSuKien.Location = new Point(xMot, 21);
                    xMot += 54 + 8;
                    // Mép phải combo chừa 12px trước nút in: 84 = khe 12 +
                    // nhãn 64 + khe 8. Công thức cũ thiếu 20px nên combo
                    // lấn 8px lên nút in.
                    int conLaiMot = inLeft - 12 - xMot - 84;
                    if (conLaiMot < 200) conLaiMot = 200;
                    int rong1Mot = conLaiMot / 2;
                    int rong2Mot = conLaiMot - rong1Mot;
                    cboSuKien.SetBounds(xMot, 13, rong1Mot, 36);
                    xMot += rong1Mot + 12;
                    lblSuatDien.Location = new Point(xMot, 21);
                    xMot += 64 + 8;
                    cboSuatDien.SetBounds(xMot, 13, rong2Mot, 36);
                }
                // Danh sách xổ xuống chỉ rộng bằng đúng combo, khỏi
                // lấn sang phải đè lên nút in.
                cboSuKien.DropDownWidth = cboSuKien.Width;
                cboSuatDien.DropDownWidth = cboSuatDien.Width;
            }
            finally
            {
                dangSapXep = false;
            }
            // Đổi chiều cao hàng lọc thì các khối dưới phải dời theo.
            SapXepCacKhoi();
        }

        private void NapBangChiTiet()
        {
            if (dangNap) return;
            int maSK = LayMaChon(cboSuKien);
            int maSuat = LayMaChon(cboSuatDien);
            if (cboBang.SelectedIndex == 1)
                bangHienTai = BaoCaoBLL.TyLeLapDayTheoKhuVuc(maSK, maSuat, dtpTu.Value.Date, dtpDen.Value.Date);
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
            float nhe = 1f - (1f - t) * (1f - t) * (1f - t);
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
                MarkerSize = 6,
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
                tu[i] = giaTriCu.TryGetValue(nhan[i], out double cu) ? cu : 0.0;
                s.Points.AddXY(nhan[i], tu[i]);
            }

            area.AxisX.Interval = Math.Max(1, (int)Math.Ceiling(dt.Rows.Count / 12.0));
            area.AxisX.LabelStyle.Angle = dt.Rows.Count > 12 ? -30 : 0;
            area.AxisY.Maximum = lonNhat > 0 ? lonNhat * 1.1 : double.NaN;
            area.RecalculateAxesScale();
            BatDauHoatHinh(s, tu, dich, true);
        }

        private static string DinhDangNhan(object nhan, string cheDo)
        {
            string s = Convert.ToString(nhan);
            if (cheDo == "Ngay" && DateTime.TryParse(s, out DateTime d)) return d.ToString("dd/MM");
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
            const int gioiHan = 5;
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
                tu[i] = giaTriCu.TryGetValue(nhan[i], out double cu) ? cu : 0.0;
                s.Points.AddXY(nhan[i], tu[i]);
            }
            CapNhatChuThichTron(nhan, dich);
            BatDauHoatHinh(s, tu, dich, false);
        }

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

        private static void LamMemThe(Guna.UI2.WinForms.Guna2Panel the, int boTron)
        {
            if (the == null) return;
            the.BorderRadius = boTron;
            the.FillColor = Color.White;
            the.ShadowDecoration.Enabled = true;
            the.ShadowDecoration.Color = Color.FromArgb(20, 15, 23, 42);
            the.ShadowDecoration.Depth = 10;
            the.ShadowDecoration.BorderRadius = boTron;
        }

        private static void DatChipIcon(Guna.UI2.WinForms.Guna2Panel chip, PictureBox anh, Color mau)
        {
            if (chip == null) return;
            chip.BorderRadius = 14;
            chip.FillColor = mau;
            if (anh == null) return;
            var elip = new Guna.UI2.WinForms.Guna2Elipse
            {
                TargetControl = anh,
                BorderRadius = 14
            };
        }

        private void XuatExcel()
        {
            if (bangHienTai == null || bangHienTai.Rows.Count == 0)
            {
                MessageBox.Show(this, "Không có dữ liệu để xuất.",
                    "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = "bao-cao.xlsx"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    GhiFileExcel(dlg.FileName, bangHienTai);
                    MessageBox.Show(this,
                        string.Format("Đã xuất {0:N0} dòng ra file Excel.", bangHienTai.Rows.Count),
                        "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Lỗi xuất file: " + ex.Message,
                        "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Ghi DataTable ra file .xlsx thật (định dạng Open XML), không cần
        /// cài thêm thư viện Excel nào và không cần thêm reference nào:
        /// file zip chứa các phần XML, các mục để ở dạng không nén.
        /// Số ghi dạng số, chữ ghi dạng chuỗi nội tuyến, ngày giờ ghi dạng
        /// chữ "dd/MM/yyyy HH:mm" như trước đây. Hàng đầu in đậm, có độ
        /// rộng cột vừa nội dung và cố định hàng tiêu đề.
        /// </summary>
        private static void GhiFileExcel(string duongDan, DataTable bang)
        {
            int soCot = bang.Columns.Count;

            // Độ rộng cột: theo chuỗi hiển thị dài nhất, kẹp trong 10..40.
            int[] rongCot = new int[soCot];
            for (int c = 0; c < soCot; c++)
            {
                int dai = bang.Columns[c].ColumnName.Length;
                foreach (DataRow row in bang.Rows)
                {
                    int n = ChuoiHienThiExcel(row[c]).Length;
                    if (n > dai) dai = n;
                }
                rongCot[c] = Math.Max(10, Math.Min(40, dai + 2));
            }

            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
            sb.Append("<sheetViews><sheetView workbookViewId=\"0\">"
                + "<pane ySplit=\"1\" topLeftCell=\"A2\" activePane=\"bottomLeft\" state=\"frozen\"/>"
                + "</sheetView></sheetViews>");
            sb.Append("<cols>");
            for (int c = 0; c < soCot; c++)
                sb.AppendFormat("<col min=\"{0}\" max=\"{0}\" width=\"{1}\" customWidth=\"1\"/>", c + 1, rongCot[c]);
            sb.Append("</cols><sheetData>");
            // Hàng tiêu đề.
            sb.Append("<row r=\"1\">");
            for (int c = 0; c < soCot; c++)
                sb.AppendFormat("<c r=\"{0}1\" t=\"inlineStr\" s=\"1\"><is><t>{1}</t></is></c>",
                    TenCotExcel(c), ThoatXml(bang.Columns[c].ColumnName));
            sb.Append("</row>");
            // Hàng dữ liệu.
            for (int r = 0; r < bang.Rows.Count; r++)
            {
                sb.AppendFormat("<row r=\"{0}\">", r + 2);
                for (int c = 0; c < soCot; c++)
                {
                    object v = bang.Rows[r][c];
                    string o = TenCotExcel(c) + (r + 2);
                    if (v == null || v == DBNull.Value)
                    {
                        sb.AppendFormat("<c r=\"{0}\"/>", o);
                    }
                    else if (v is DateTime d)
                    {
                        sb.AppendFormat("<c r=\"{0}\" t=\"inlineStr\"><is><t>{1}</t></is></c>",
                            o, ThoatXml(d.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)));
                    }
                    else if (v is sbyte || v is byte || v is short || v is ushort
                        || v is int || v is uint || v is long || v is ulong
                        || v is float || v is double || v is decimal)
                    {
                        sb.AppendFormat("<c r=\"{0}\"><v>{1}</v></c>",
                            o, Convert.ToString(v, CultureInfo.InvariantCulture));
                    }
                    else if (v is bool b)
                    {
                        sb.AppendFormat("<c r=\"{0}\" t=\"b\"><v>{1}</v></c>", o, b ? 1 : 0);
                    }
                    else
                    {
                        sb.AppendFormat("<c r=\"{0}\" t=\"inlineStr\"><is><t>{1}</t></is></c>",
                            o, ThoatXml(Convert.ToString(v, CultureInfo.InvariantCulture)));
                    }
                }
                sb.Append("</row>");
            }
            sb.Append("</sheetData></worksheet>");
            string sheetXml = sb.ToString();

            const string contentTypes =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n"
                + "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">"
                + "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>"
                + "<Default Extension=\"xml\" ContentType=\"application/xml\"/>"
                + "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>"
                + "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>"
                + "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>"
                + "</Types>";
            const string rels =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n"
                + "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">"
                + "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>"
                + "</Relationships>";
            const string workbook =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n"
                + "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">"
                + "<sheets><sheet name=\"BaoCao\" sheetId=\"1\" r:id=\"rId1\"/></sheets>"
                + "</workbook>";
            const string workbookRels =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n"
                + "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">"
                + "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>"
                + "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>"
                + "</Relationships>";
            const string styles =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n"
                + "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">"
                + "<fonts count=\"2\">"
                + "<font><sz val=\"11\"/><name val=\"Calibri\"/></font>"
                + "<font><b/><sz val=\"11\"/><name val=\"Calibri\"/></font>"
                + "</fonts>"
                + "<fills count=\"2\">"
                + "<fill><patternFill patternType=\"none\"/></fill>"
                + "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFD9E2EC\"/><bgColor rgb=\"FFD9E2EC\"/></patternFill></fill>"
                + "</fills>"
                + "<borders count=\"1\"><border><left/><right/><top/><bottom/><diagonal/></border></borders>"
                + "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>"
                + "<cellXfs count=\"2\">"
                + "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>"
                + "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"1\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\"/>"
                + "</cellXfs>"
                + "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>"
                + "</styleSheet>";

            // Ghi file .xlsx (thực chất là file zip chứa các phần XML) mà
            // không cần thêm reference nào: các mục để ở dạng không nén,
            // chỉ cần tính CRC32 và ghi đúng cấu trúc zip.
            GhiFileZipKhongNen(duongDan,
                new[] { "[Content_Types].xml", "_rels/.rels", "xl/workbook.xml",
                    "xl/_rels/workbook.xml.rels", "xl/styles.xml", "xl/worksheets/sheet1.xml" },
                new[] { contentTypes, rels, workbook, workbookRels, styles, sheetXml });
        }

        private static void GhiFileZipKhongNen(string duongDan, string[] tenMuc, string[] noiDungMuc)
        {
            DateTime bayGio = DateTime.Now;
            ushort gioDos = (ushort)((bayGio.Hour << 11) | (bayGio.Minute << 5) | (bayGio.Second / 2));
            ushort ngayDos = (ushort)(((bayGio.Year - 1980) << 9) | (bayGio.Month << 5) | bayGio.Day);
            var utf8 = new UTF8Encoding(false);
            using (var fs = new FileStream(duongDan, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                var viTri = new long[tenMuc.Length];
                var crcs = new uint[tenMuc.Length];
                var kichThuocs = new uint[tenMuc.Length];
                var tenBytesArr = new byte[tenMuc.Length][];
                for (int i = 0; i < tenMuc.Length; i++)
                {
                    byte[] duLieu = utf8.GetBytes(noiDungMuc[i]);
                    byte[] tenBytes = utf8.GetBytes(tenMuc[i]);
                    uint crc = TinhCrc32(duLieu);
                    viTri[i] = fs.Position;
                    crcs[i] = crc;
                    kichThuocs[i] = (uint)duLieu.Length;
                    tenBytesArr[i] = tenBytes;
                    bw.Write(0x04034b50u);   // chữ ký local header
                    bw.Write((ushort)20);    // phiên bản cần thiết
                    bw.Write((ushort)0);     // cờ
                    bw.Write((ushort)0);     // phương thức: 0 = không nén
                    bw.Write(gioDos);
                    bw.Write(ngayDos);
                    bw.Write(crc);
                    bw.Write((uint)duLieu.Length);
                    bw.Write((uint)duLieu.Length);
                    bw.Write((ushort)tenBytes.Length);
                    bw.Write((ushort)0);     // extra field
                    bw.Write(tenBytes);
                    bw.Write(duLieu);
                }
                bw.Flush();
                long viTriThuMuc = fs.Position;
                for (int i = 0; i < tenMuc.Length; i++)
                {
                    byte[] tenBytes = tenBytesArr[i];
                    bw.Write(0x02014b50u);   // chữ ký central header
                    bw.Write((ushort)20);    // phiên bản tạo bởi
                    bw.Write((ushort)20);    // phiên bản cần thiết
                    bw.Write((ushort)0);     // cờ
                    bw.Write((ushort)0);     // phương thức
                    bw.Write(gioDos);
                    bw.Write(ngayDos);
                    bw.Write(crcs[i]);
                    bw.Write(kichThuocs[i]);
                    bw.Write(kichThuocs[i]);
                    bw.Write((ushort)tenBytes.Length);
                    bw.Write((ushort)0);     // extra field
                    bw.Write((ushort)0);     // bình luận
                    bw.Write((ushort)0);     // đĩa bắt đầu
                    bw.Write((ushort)0);     // thuộc tính nội bộ
                    bw.Write(0u);            // thuộc tính ngoài
                    bw.Write((uint)viTri[i]);// vị trí local header
                    bw.Write(tenBytes);
                }
                long kichThuocThuMuc = fs.Position - viTriThuMuc;
                bw.Write(0x06054b50u);       // end of central directory
                bw.Write((ushort)0);         // đĩa này
                bw.Write((ushort)0);         // đĩa chứa central directory
                bw.Write((ushort)tenMuc.Length);
                bw.Write((ushort)tenMuc.Length);
                bw.Write((uint)kichThuocThuMuc);
                bw.Write((uint)viTriThuMuc);
                bw.Write((ushort)0);         // bình luận
            }
        }

        private static readonly uint[] BangCrc32 = TaoBangCrc32();

        private static uint[] TaoBangCrc32()
        {
            var bang = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint c = i;
                for (int k = 0; k < 8; k++)
                    c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
                bang[i] = c;
            }
            return bang;
        }

        private static uint TinhCrc32(byte[] duLieu)
        {
            uint crc = 0xFFFFFFFFu;
            foreach (byte b in duLieu)
                crc = BangCrc32[(crc ^ b) & 0xFF] ^ (crc >> 8);
            return crc ^ 0xFFFFFFFFu;
        }

        private static string ChuoiHienThiExcel(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            if (v is DateTime d) return d.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (v is decimal m) return m.ToString("#,##0", CultureInfo.InvariantCulture);
            if (v is double || v is float) return Convert.ToString(v, CultureInfo.InvariantCulture);
            return Convert.ToString(v, CultureInfo.InvariantCulture);
        }

        private static string TenCotExcel(int chiSo)
        {
            string ten = "";
            chiSo++;
            while (chiSo > 0)
            {
                int du = (chiSo - 1) % 26;
                ten = (char)('A' + du) + ten;
                chiSo = (chiSo - 1) / 26;
            }
            return ten;
        }

        private static string ThoatXml(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var sb = new StringBuilder(s.Length);
            foreach (char ch in s)
            {
                if (ch == '&') sb.Append("&amp;");
                else if (ch == '<') sb.Append("&lt;");
                else if (ch == '>') sb.Append("&gt;");
                else if (ch < 0x20 && ch != '\t' && ch != '\r' && ch != '\n') continue;
                else sb.Append(ch);
            }
            return sb.ToString();
        }

    }
}

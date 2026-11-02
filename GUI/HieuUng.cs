using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Guna.UI2.WinForms;

namespace QuanLySuKienVaBanVe.GUI
{
    /// <summary>
    /// Hiệu ứng dùng chung cho các form quản lý (Guna2). Gọi 1 dòng trong
    /// constructor (sau InitializeComponent) là đủ, KHÔNG sửa file Designer:
    ///     HieuUng.GanChoForm(this);   // form nào cũng dùng được
    ///     HieuUng.MoDan(this);        // chỉ form top-level (dialog), xem ghi chú dưới
    ///
    /// Có đúng 3 hiệu ứng, đều nhẹ và an toàn với theme Guna2 sẵn có:
    ///  1. Nút: rê chuột thì NHẤC LÊN 2px + đậm màu + hiện bóng đổ; nhấn thì
    ///     đậm thêm, thả ra thì hạ xuống. (Chỉ tô màu khi Designer chưa đặt
    ///     sẵn HoverState/PressedState, nên nút nào đã chỉnh màu hover thì
    ///     giữ nguyên như cũ.)
    ///  2. Lưới: rê chuột lên nút Sửa/Xóa thì ô đó đậm lên (vẽ trong
    ///     ManagementGridRenderer, hàm này chỉ lo vẽ lại khi chuột đổi ô).
    ///  3. Mở dần (fade-in): chỉ form top-level mới mờ được (Opacity là
    ///     thuộc tính của cửa sổ top-level). Form nhúng trong FrmMain có
    ///     TopLevel = false nên MoDan() tự bỏ qua, không lỗi gì cả.
    ///
    /// KHÔNG làm: trượt/slide form nhúng (giật, vỡ layout khi resize),
    /// ripple (Guna2 không hỗ trợ, tự vẽ dễ nhấp nháy), hover cả hàng
    /// trong lưới (đụng màu xen kẽ + màu đang chọn của Guna2).
    /// Màn hình đăng nhập đã có hiệu ứng riêng (FxButton/FxControls) nên
    /// không gọi 2 hàm này ở đó.
    /// </summary>
    internal static class HieuUng
    {
        /// <summary>Form hiện dần từ trong suốt (0 -&gt; 1) trong ~0,2 giây.</summary>
        internal static void MoDan(Form form)
        {
            if (form == null || !form.TopLevel) return;
            var timer = new Timer { Interval = 15 };
            form.Opacity = 0;
            timer.Tick += delegate
            {
                if (form.IsDisposed || form.Disposing) { timer.Stop(); timer.Dispose(); return; }
                form.Opacity = Math.Min(1.0, form.Opacity + 0.08);
                if (form.Opacity >= 1.0) { timer.Stop(); timer.Dispose(); }
            };
            form.Disposed += delegate { timer.Stop(); timer.Dispose(); };
            form.Shown += delegate { if (!timer.Enabled) timer.Start(); };
            if (form.Visible) timer.Start();
        }

        /// <summary>Gắn hiệu ứng nút + lưới cho mọi control trong form (đệ quy).</summary>
        internal static void GanChoForm(Form form)
        {
            if (form == null) return;
            GanChoControl(form);
        }

        private static void GanChoControl(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                Guna2Button nut = c as Guna2Button;
                if (nut != null) GanChoNut(nut);
                DataGridView luoi = c as DataGridView;
                if (luoi != null) GanChoLuoi(luoi);
                if (c.HasChildren) GanChoControl(c);
            }
        }

        private static void GanChoNut(Guna2Button nut)
        {
            Color goc = nut.FillColor;
            if (goc.A == 0) return; // nút nền trong suốt (vd nút nav): không động vào

            // Chỉ tô khi Designer chưa đặt sẵn, để màu hover đã chỉnh tay được giữ nguyên.
            if (nut.HoverState.FillColor.IsEmpty)
                nut.HoverState.FillColor = Fx.Lerp(goc, Color.Black, 0.12f);
            // Guna2 bản này không có PressedState: khi nhấn thì tự đậm màu
            // hover lên, thả chuột hoặc rê ra ngoài thì trả lại màu hover cũ.
            Color hoverGoc = nut.HoverState.FillColor;
            bool dangNhan = false;
            nut.MouseDown += delegate (object s, MouseEventArgs e2)
            {
                if (e2.Button != MouseButtons.Left || dangNhan) return;
                dangNhan = true;
                nut.HoverState.FillColor = Fx.Lerp(goc, Color.Black, 0.22f);
            };
            nut.MouseUp += delegate
            {
                if (!dangNhan) return;
                dangNhan = false;
                nut.HoverState.FillColor = hoverGoc;
            };

            // Bóng đổ chỉ hiện khi rê chuột (bật/tắt qua sự kiện, không tốn Timer).
            // Nút nào Designer đã bật sẵn bóng (vd nút Xuất báo cáo) thì giữ nguyên,
            // không tắt đi khi chuột rời.
            bool bongSan = nut.ShadowDecoration.Enabled;
            nut.ShadowDecoration.Color = Color.FromArgb(70, goc);
            nut.ShadowDecoration.Depth = 6;
            nut.ShadowDecoration.BorderRadius = nut.BorderRadius;
            if (!bongSan) nut.ShadowDecoration.Enabled = false;

            // Nhấc nút lên 2px khi rê chuột: lấy lại vị trí gốc mỗi lần vào
            // (layout có thể đã dời nút sau lần gắn), rời chuột thì trả về.
            bool dangNoi = false;
            Point gocViTri = nut.Location;
            nut.MouseEnter += delegate
            {
                if (dangNoi) return;
                gocViTri = nut.Location;
                dangNoi = true;
                nut.Location = new Point(gocViTri.X, gocViTri.Y - 2);
                if (!bongSan) nut.ShadowDecoration.Enabled = true;
                nut.Invalidate();
            };
            nut.MouseLeave += delegate
            {
                if (dangNhan) { dangNhan = false; nut.HoverState.FillColor = hoverGoc; }
                if (!dangNoi) return;
                dangNoi = false;
                nut.Location = gocViTri;
                if (!bongSan) nut.ShadowDecoration.Enabled = false;
                nut.Invalidate();
            };
        }

        /// <summary>
        /// Vẽ lại lưới mỗi khi chuột chuyển sang nút Sửa/Xóa khác, để
        /// ManagementGridRenderer tô đậm đúng ô đang rê chuột. Chỉ vẽ lại
        /// khi ô hover thay đổi nên không gây nhấp nháy.
        /// </summary>
        private static void GanChoLuoi(DataGridView luoi)
        {
            string dangHot = null;
            luoi.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                DataGridView.HitTestInfo hit = luoi.HitTest(e.X, e.Y);
                string hot = null;
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    Rectangle o = luoi.GetCellDisplayRectangle(hit.ColumnIndex, hit.RowIndex, false);
                    Point p = new Point(e.X, e.Y);
                    if (ManagementGridRenderer.GetActionBounds(luoi, o, false).Contains(p))
                        hot = hit.RowIndex + ",sua";
                    else if (ManagementGridRenderer.GetActionBounds(luoi, o, true).Contains(p))
                        hot = hit.RowIndex + ",xoa";
                }
                if (hot != dangHot) { dangHot = hot; luoi.Invalidate(); }
            };
            luoi.MouseLeave += delegate
            {
                if (dangHot != null) { dangHot = null; luoi.Invalidate(); }
            };
        }
    }

    /// <summary>
    /// Bộ máy animation cho biểu đồ (Chart): nội suy giá trị từ số đang hiển
    /// thị sang số mới trong ~0,36 giây (24 bước, ease-out). Một form chỉ cần
    /// MỘT đối tượng này, dùng chung cho mọi biểu đồ; mỗi biểu đồ là một
    /// "track" chạy song song. Timer nằm trong components của form nên form
    /// đóng là tự hủy.
    /// Cách dùng (form có biểu đồ):
    ///     private readonly HoatHinhBieuDo hoatHinh;
    ///     // trong constructor, sau InitializeComponent():
    ///     hoatHinh = new HoatHinhBieuDo(components);
    ///     // khi vẽ lại: dựng Series với giá trị BẮT ĐẦU (đọc bằng
    ///     // LayGiaTriHienTai), rồi gọi:
    ///     hoatHinh.Chay(chart, series, giaTriBatDau, giaTriDich, khiXong);
    /// </summary>
    internal sealed class HoatHinhBieuDo : IDisposable
    {
        private const int TongBuoc = 24;
        private readonly Timer timer;
        private int buoc;
        private readonly List<TheoDoi> dangChay = new List<TheoDoi>();

        private sealed class TheoDoi
        {
            public Chart Chart;
            public Series Chuoi;
            public double[] Tu;
            public double[] Den;
            public Action KhiXong;
        }

        public HoatHinhBieuDo(IContainer components)
        {
            timer = components != null ? new Timer(components) : new Timer();
            timer.Interval = 15;
            timer.Tick += Tick;
        }

        /// <summary>Đọc giá trị Y đang hiển thị của biểu đồ, tra theo nhãn.</summary>
        public Dictionary<string, double> LayGiaTriHienTai(Chart chart)
        {
            var kq = new Dictionary<string, double>();
            if (chart != null && chart.Series.Count > 0)
                foreach (DataPoint p in chart.Series[0].Points)
                    if (!kq.ContainsKey(p.AxisLabel)) kq[p.AxisLabel] = p.YValues[0];
            return kq;
        }

        /// <summary>Bắt đầu (hoặc chạy lại) animation cho một biểu đồ.</summary>
        public void Chay(Chart chart, Series chuoi, double[] tu, double[] den, Action khiXong)
        {
            if (chart == null || chuoi == null || tu == null || den == null || tu.Length != den.Length)
            {
                if (khiXong != null) khiXong();
                return;
            }
            dangChay.RemoveAll(t => t.Chart == chart);
            dangChay.Add(new TheoDoi { Chart = chart, Chuoi = chuoi, Tu = tu, Den = den, KhiXong = khiXong });
            buoc = 0;
            timer.Stop();
            timer.Start();
        }

        /// <summary>Hủy animation đang chạy của một biểu đồ (vd khi hết dữ liệu).</summary>
        public void Dung(Chart chart)
        {
            dangChay.RemoveAll(t => t.Chart == chart);
            if (dangChay.Count == 0) timer.Stop();
        }

        private void Tick(object sender, EventArgs e)
        {
            buoc++;
            float t = Math.Min(1f, buoc / (float)TongBuoc);
            float nhe = 1f - (1f - t) * (1f - t) * (1f - t); // ease-out: nhanh đầu, chậm cuối
            bool xong = t >= 1f;
            for (int k = dangChay.Count - 1; k >= 0; k--)
            {
                TheoDoi td = dangChay[k];
                if (td.Chart.IsDisposed || td.Chuoi.Points.Count != td.Den.Length)
                {
                    dangChay.RemoveAt(k);
                    continue;
                }
                for (int i = 0; i < td.Den.Length; i++)
                    td.Chuoi.Points[i].YValues[0] = xong ? td.Den[i] : td.Tu[i] + (td.Den[i] - td.Tu[i]) * nhe;
                td.Chart.Invalidate();
                if (xong)
                {
                    dangChay.RemoveAt(k);
                    if (td.KhiXong != null) td.KhiXong();
                }
            }
            if (dangChay.Count == 0) timer.Stop();
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}


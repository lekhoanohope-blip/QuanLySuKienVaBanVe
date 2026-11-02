using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    /// <summary>
    /// In danh sách khách tham dự của một suất diễn (xem trước rồi mới in).
    /// Cách gắn vào FrmQuanLySuatDien: thêm 1 nút "In DS khách" trên Designer rồi
    /// gọi InDanhSachKhach.In(maSuat, this) trong sự kiện Click (xem README_BaoCao.txt).
    /// Một dòng in = một vé hợp lệ (ChiTietVe HopLe, đơn DaThanhToan).
    /// </summary>
    public static class InDanhSachKhach
    {
        public static void In(int maSuat, IWin32Window owner)
        {
            DataTable bang;
            try
            {
                bang = BaoCaoBLL.DanhSachKhachTheoSuat(maSuat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner,
                    "Lỗi tải danh sách khách: " + ex.Message,
                    "In danh sách khách", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (bang.Rows.Count == 0)
            {
                MessageBox.Show(owner,
                    "Suất diễn này chưa có vé hợp lệ nào.",
                    "In danh sách khách", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var mayIn = new MayInDanhSach(bang);
            try
            {
                using (var xem = new PrintPreviewDialog
                {
                    Document = mayIn.TaiLieu,
                    WindowState = FormWindowState.Maximized
                })
                {
                    xem.ShowDialog(owner);
                }
            }
            finally
            {
                mayIn.TaiLieu.Dispose();
            }
        }

        private sealed class MayInDanhSach
        {
            public PrintDocument TaiLieu { get; }

            private readonly DataTable bang;
            private int dongHienTai;
            private int soTrang;

            public MayInDanhSach(DataTable bangIn)
            {
                bang = bangIn;
                TaiLieu = new PrintDocument();
                TaiLieu.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
                TaiLieu.PrintPage += InTrang;
            }

            private void InTrang(object sender, PrintPageEventArgs e)
            {
                Graphics g = e.Graphics;
                RectangleF vung = e.MarginBounds;
                float y = vung.Top;
                soTrang++;

                DataRow dau = bang.Rows[0];
                string tenSK = Convert.ToString(dau["TenSK"]);
                string diaDiem = Convert.ToString(dau["DiaDiem"]);
                DateTime batDau = Convert.ToDateTime(dau["ThoiGianBatDau"]);

                using (var fTieuDe = new Font("Segoe UI", 16f, FontStyle.Bold))
                using (var fPhu = new Font("Segoe UI", 11f))
                using (var fCot = new Font("Segoe UI", 10f, FontStyle.Bold))
                using (var fNoiDung = new Font("Segoe UI", 10f))
                using (var fChan = new Font("Segoe UI", 9f))
                {
                    VeGiua(g, "DANH SÁCH KHÁCH THAM DỰ", fTieuDe, vung, ref y);
                    VeGiua(g, tenSK, fPhu, vung, ref y);
                    VeGiua(g,
                        string.Format("Suất diễn: {0:dd/MM/yyyy HH:mm}  •  Địa điểm: {1}", batDau, diaDiem),
                        fPhu, vung, ref y);
                    VeGiua(g,
                        string.Format("Ngày in: {0:dd/MM/yyyy HH:mm}  •  Tổng: {1} vé", DateTime.Now, bang.Rows.Count),
                        fPhu, vung, ref y);
                    y += 10f;

                    // Độ rộng từng cột theo tỷ lệ vùng in: STT | Họ tên | SĐT | Ghế | Khu vực | Mã vé
                    float[] tyLe = { 0.06f, 0.30f, 0.16f, 0.10f, 0.18f, 0.20f };
                    float[] x = new float[tyLe.Length];
                    float tich = vung.Left;
                    for (int i = 0; i < tyLe.Length; i++)
                    {
                        x[i] = tich;
                        tich += vung.Width * tyLe[i];
                    }
                    string[] tieuDeCot = { "STT", "Họ tên", "SĐT", "Ghế", "Khu vực", "Mã vé" };
                    for (int i = 0; i < tieuDeCot.Length; i++)
                        g.DrawString(tieuDeCot[i], fCot, Brushes.Black, x[i], y);
                    y += fCot.Height + 6f;
                    g.DrawLine(Pens.Black, vung.Left, y, vung.Right, y);
                    y += 6f;

                    float caoDong = fNoiDung.Height + 8f;
                    var cat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter };
                    using (var ke = new Pen(Color.FromArgb(220, 220, 220)))
                    {
                        while (dongHienTai < bang.Rows.Count)
                        {
                            if (y + caoDong > vung.Bottom - 30f) break;
                            DataRow row = bang.Rows[dongHienTai];
                            string[] giaTri =
                            {
                                (dongHienTai + 1).ToString(),
                                Convert.ToString(row["TenKH"]),
                                Convert.ToString(row["SDT"]),
                                Convert.ToString(row["MaGheHienThi"]),
                                Convert.ToString(row["TenKV"]),
                                Convert.ToString(row["MaVeCode"])
                            };
                            for (int i = 0; i < giaTri.Length; i++)
                            {
                                var vungChu = new RectangleF(x[i], y, vung.Width * tyLe[i] - 6f, caoDong);
                                g.DrawString(giaTri[i], fNoiDung, Brushes.Black, vungChu, cat);
                            }
                            y += caoDong;
                            g.DrawLine(ke, vung.Left, y, vung.Right, y);
                            dongHienTai++;
                        }
                    }

                    string chan = "Trang " + soTrang;
                    SizeF coChan = g.MeasureString(chan, fChan);
                    g.DrawString(chan, fChan, Brushes.Gray,
                        vung.Left + (vung.Width - coChan.Width) / 2f, vung.Bottom - 20f);
                }

                e.HasMorePages = dongHienTai < bang.Rows.Count;
            }

            private static void VeGiua(Graphics g, string text, Font font, RectangleF vung, ref float y)
            {
                SizeF co = g.MeasureString(text, font);
                g.DrawString(text, font, Brushes.Black,
                    vung.Left + (vung.Width - co.Width) / 2f, y);
                y += co.Height + 2f;
            }
        }
    }
}

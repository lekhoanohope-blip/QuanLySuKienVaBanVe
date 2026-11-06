using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using QuanLySuKienVaBanVe.GUI.Helpers;

namespace QuanLySuKienVaBanVe.GUI
{
    public class FrmInVe : Form
    {
        private readonly List<ThongTinInVeDTO> _danhSachVe = new List<ThongTinInVeDTO>();
        private int _maDon;
        private int _indexInPrint;

        private PrintDocument _printDoc;

        private Panel pnlHeader;
        private Panel pnlToolbar;
        private Panel pnlCanvas;
        private Panel pnlVe;
        private readonly List<PictureBox> _picVe = new List<PictureBox>();
        private Label lblTitle;
        private Label lblPosition;
        private Label lblStatus;
        private TextBox txtThuMuc;
        private Button btnPrint, btnPdf, btnClose;

        private int _renderWidth = -1;
        private const int KheGiuaVe = 18;
        private const int ChenLe = 16;

        public static DialogResult InDanhSachVe(int maDon)
        {
            return InDanhSachVe(maDon, null);
        }

        public static DialogResult InDanhSachVe(int maDon, IWin32Window owner)
        {
            if (maDon <= 0)
            {
                MessageBox.Show(owner,
                    "Chưa có đơn vé nào vừa thanh toán trong phiên bán này.\n" +
                    "Hãy chọn khu vực, giữ ghế và hoàn tất thanh toán trước khi in vé.",
                    "Chưa có vé để in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return DialogResult.None;
            }

            List<ThongTinInVeDTO> dsVe;
            try
            {
                dsVe = DonVeBLL.LayDanhSachVeIn(maDon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner,
                    "Không đọc được dữ liệu vé từ cơ sở dữ liệu:\n\n" + ex.Message,
                    "Lỗi in vé", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.None;
            }

            if (dsVe.Count == 0)
            {
                MessageBox.Show(owner,
                    "Đơn vé #" + maDon + " không còn vé hợp lệ nào để in (đã hủy hoặc chưa phát hành).",
                    "Không có vé hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return DialogResult.None;
            }

            return MoForm(dsVe, maDon, owner);
        }

        public static DialogResult InDanhSachVe(int maDon, IList<ChiTietVeDTO> dsChiTietVe, IWin32Window owner)
        {
            if (dsChiTietVe == null || dsChiTietVe.Count == 0)
                return InDanhSachVe(maDon, owner);

            DataRow don = null;
            try { don = DonVeBLL.LayThongTinDonVeVaSuatDien(maDon); }
            catch { don = null; }

            List<ThongTinInVeDTO> dsVe = new List<ThongTinInVeDTO>();
            foreach (ChiTietVeDTO ct in dsChiTietVe)
            {
                if (ct == null) continue;
                ThongTinInVeDTO ve = new ThongTinInVeDTO
                {
                    MaDon = ct.MaDon,
                    MaCT = ct.MaCT,
                    MaGhe = ct.MaGhe,
                    MaVeCode = ct.MaVeCode,
                    Zone = ct.TenKV,
                    Hang = ct.Hang,
                    Cot = ct.Cot,
                    GiaVe = ct.GiaVe
                };

                if (don != null)
                {
                    ve.TenSuKien = LayCot(don, "TenSK");
                    ve.DiaDiem = LayCot(don, "DiaDiem");
                    ve.TenKhachHang = LayCot(don, "TenKH");
                    DateTime gio;
                    if (DateTime.TryParse(LayCot(don, "ThoiGianBatDau"), out gio))
                        ve.ThoiGianBatDau = gio;
                }

                if (ve.Cot == 0 && !string.IsNullOrWhiteSpace(ct.MaGheHienThi))
                {
                    string hienThi = ct.MaGheHienThi.Trim().ToUpperInvariant();
                    int i = 0;
                    while (i < hienThi.Length && !char.IsDigit(hienThi[i])) i++;
                    if (i > 0) ve.Hang = hienThi.Substring(0, i);
                    int cot;
                    if (i < hienThi.Length && int.TryParse(hienThi.Substring(i), out cot)) ve.Cot = cot;
                }

                dsVe.Add(ve);
            }

            if (dsVe.Count == 0)
            {
                MessageBox.Show(owner, "Danh sách vé rỗng, không có gì để in.",
                    "Không có vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return DialogResult.None;
            }

            return MoForm(dsVe, maDon, owner);
        }

        private static DialogResult MoForm(List<ThongTinInVeDTO> dsVe, int maDon, IWin32Window owner)
        {
            using (FrmInVe frm = new FrmInVe(dsVe, maDon))
                return frm.ShowDialog(owner);
        }

        private static string LayCot(DataRow row, string cot)
        {
            return row != null && row.Table.Columns.Contains(cot) && row[cot] != DBNull.Value
                ? row[cot].ToString() : string.Empty;
        }

        public FrmInVe()
            : this(new List<ThongTinInVeDTO>(), 0)
        {
        }

        public FrmInVe(IList<ThongTinInVeDTO> dsVe, int maDon)
        {
            if (dsVe != null) _danhSachVe.AddRange(dsVe);
            _maDon = maDon;

            Text = "In vé - EventTicket";
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(241, 243, 247);
            MinimumSize = new Size(900, 560);
            ClientSize = new Size(1180, 660);
            StartPosition = FormStartPosition.CenterParent;
            DoubleBuffered = true;

            TaoHeader();
            TaoToolbar();
            TaoCanvas();
            TaoStatusStrip();
            SapXepLayout();

            _printDoc = new PrintDocument();
            _printDoc.DocumentName = "EventTicket";
            CauHinhKhoGiay(_printDoc);
            _printDoc.PrintPage += PrintDoc_PrintPage;

            HienThi();
        }

        private void TaoHeader()
        {
            pnlHeader = new Panel { BackColor = Color.FromArgb(29, 95, 216), Dock = DockStyle.Top };
            pnlHeader.Height = 36;

            lblTitle = new Label
            {
                Text = "In vé vừa đặt",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(14, 9)
            };
            pnlHeader.Controls.Add(lblTitle);

            lblPosition = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 235, 255),
                AutoSize = true,
                Location = new Point(0, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            pnlHeader.Controls.Add(lblPosition);
            Controls.Add(pnlHeader);
        }

        private Button TaoNut(string text, int width, Color back)
        {
            Button b = new Button
            {
                Text = text,
                Size = new Size(width, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Margin = new Padding(6, 0, 0, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(back, 0.04f);
            return b;
        }

        private void TaoToolbar()
        {
            pnlToolbar = new Panel { BackColor = Color.White, Dock = DockStyle.Top };
            pnlToolbar.Height = 52;

            FlowLayoutPanel flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(12, 9, 12, 9),
                BackColor = Color.White
            };


            btnClose = TaoNut("Đóng", 80, Color.FromArgb(156, 163, 175));
            btnClose.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            flow.Controls.Add(btnClose);

            pnlToolbar.Controls.Add(flow);
            Controls.Add(pnlToolbar);
        }
        private void TaoCanvas()
        {
            pnlCanvas = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(31, 41, 55),
                AutoScroll = true
            };

            pnlVe = new Panel
            {
                BackColor = Color.FromArgb(31, 41, 55),
                Location = new Point(0, 0)
            };
            pnlCanvas.Controls.Add(pnlVe);

            for (int i = 0; i < _danhSachVe.Count; i++)
            {
                PictureBox pic = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.AutoSize,
                    BackColor = Color.Transparent
                };
                _picVe.Add(pic);
                pnlVe.Controls.Add(pic);
            }

            Controls.Add(pnlCanvas);
            pnlCanvas.BringToFront();
            pnlCanvas.Resize += (s, e) => VeLaiDanhSach();
        }

        private void TaoStatusStrip()
        {
            lblStatus = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 9f),
                Text = "Sẵn sàng."
            };
            Controls.Add(lblStatus);
        }

        private void SapXepLayout()
        {
            Controls.SetChildIndex(pnlCanvas, 0);
            Controls.SetChildIndex(lblStatus, 1);
            Controls.SetChildIndex(pnlToolbar, 2);
            Controls.SetChildIndex(pnlHeader, 3);
        }

        private void HienThi()
        {
            if (_danhSachVe.Count == 0)
            {
                lblTitle.Text = "In vé - Không có dữ liệu";
                lblPosition.Text = "";
                lblStatus.Text = "Không có vé nào để hiển thị.";
                btnPrint.Enabled = btnPdf.Enabled = false;
                return;
            }

            lblTitle.Text = "In vé vừa đặt - Đơn #" + _maDon;
            lblPosition.Text = _danhSachVe.Count + " vé";

            List<string> maVe = new List<string>();
            foreach (ThongTinInVeDTO ve in _danhSachVe)
                maVe.Add(ve.MaVeCode);

            lblStatus.Text = string.Format(
                "{0} vé - kéo lên / kéo xuống để xem tất cả   |   Mã vé: {1}   |   File PDF: {2}",
                _danhSachVe.Count, string.Join(", ", maVe.ToArray()),
                TicketPdfExporter.LayTenFile(_danhSachVe[0]));

            VeLaiDanhSach();
        }

        private void VeLaiDanhSach()
        {
            if (_danhSachVe.Count == 0) return;

            int w = pnlCanvas.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - ChenLe;
            if (w < 320) w = 320;
            if (w > TicketRenderer.PrintSizePx.Width) w = TicketRenderer.PrintSizePx.Width;

            if (w != _renderWidth)
            {
                _renderWidth = w;
                float dpi = DeviceDpi > 0 ? DeviceDpi : 96f;
                for (int i = 0; i < _picVe.Count; i++)
                {
                    Bitmap moi = TicketRenderer.Render(_danhSachVe[i], w, dpi);
                    Bitmap cu = _picVe[i].Image as Bitmap;
                    _picVe[i].Image = moi;
                    if (cu != null) cu.Dispose();
                }
            }

            SapXepChumVe();
        }

        private void SapXepChumVe()
        {
            pnlVe.Width = Math.Max(pnlCanvas.ClientSize.Width, 1);

            int y = ChenLe;
            foreach (PictureBox pic in _picVe)
            {
                pic.Location = new Point(Math.Max(ChenLe, (pnlVe.Width - pic.Width) / 2), y);
                y += pic.Height + KheGiuaVe;
            }
            pnlVe.Height = y + ChenLe;
        }

        private static void CauHinhKhoGiay(PrintDocument doc)
        {
            try
            {
                doc.DefaultPageSettings.Landscape = true;
                foreach (PaperSize ps in doc.PrinterSettings.PaperSizes)
                {
                    if (Math.Abs(ps.Width - 827) <= 6 && Math.Abs(ps.Height - 354) <= 6)
                    {
                        doc.DefaultPageSettings.PaperSize = ps;
                        doc.DefaultPageSettings.Landscape = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CauHinhKhoGiay: " + ex.Message);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_danhSachVe.Count == 0) return;

            using (Bitmap anh = TicketRenderer.RenderForPrint(_danhSachVe[_indexInPrint]))
            {
                float dpiX = e.Graphics.DpiX, dpiY = e.Graphics.DpiY;

                RectangleF area = e.MarginBounds.IsEmpty ? e.PageBounds : e.MarginBounds;
                RectangleF box = new RectangleF(
                    area.X * dpiX / 100f, area.Y * dpiY / 100f,
                    area.Width * dpiX / 100f, area.Height * dpiY / 100f);

                float ratio = TicketRenderer.DesignW / TicketRenderer.DesignH;
                float w = box.Width, h = w / ratio;
                if (h > box.Height) { h = box.Height; w = h * ratio; }

                RectangleF dest = new RectangleF(
                    box.X + (box.Width - w) / 2f,
                    box.Y + (box.Height - h) / 2f,
                    w, h);

                SmoothingMode oldSm = e.Graphics.SmoothingMode;
                InterpolationMode oldIn = e.Graphics.InterpolationMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(anh, dest);
                e.Graphics.SmoothingMode = oldSm;
                e.Graphics.InterpolationMode = oldIn;
            }

            _indexInPrint++;
            e.HasMorePages = _indexInPrint < _danhSachVe.Count;
            if (!e.HasMorePages) _indexInPrint = 0;
        }

        private void InThietBi()
        {
            if (_danhSachVe.Count == 0) return;

            using (PrintDialog dlg = new PrintDialog())
            {
                dlg.Document = _printDoc;
                dlg.AllowSomePages = false;
                dlg.UseEXDialog = true;
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                _indexInPrint = 0;
                try
                {
                    _printDoc.Print();
                    lblStatus.Text = string.Format("Đã gửi {0} vé tới máy in '{1}'.",
                        _danhSachVe.Count, _printDoc.PrinterSettings.PrinterName);
                }
                catch (Exception ex)
                {
                    _indexInPrint = 0;
                    MessageBox.Show(this, "In thất bại: " + ex.Message,
                        "Lỗi máy in", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void XuatPdf()
        {
            if (_danhSachVe.Count == 0) return;

            string thuMuc = (txtThuMuc.Text ?? string.Empty).Trim();
            if (thuMuc.Length == 0) thuMuc = TicketPdfExporter.ThuMucMacDinh;

            try
            {
                List<string> files = TicketPdfExporter.XuatDanhSach(_danhSachVe, thuMuc);
                string noiDung = files.Count == 1
                    ? files[0]
                    : string.Join(Environment.NewLine, files.ToArray());

                lblStatus.Text = string.Format("Đã xuất {0} file PDF tại {1}", files.Count, thuMuc);
                MessageBox.Show(this,
                    "Xuất PDF thành công (" + files.Count + " file, 300 DPI):\n\n" + noiDung,
                    "Xuất vé PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không xuất được PDF: " + ex.Message,
                    "Lỗi xuất PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (PictureBox pic in _picVe)
                {
                    Bitmap anh = pic.Image as Bitmap;
                    if (anh != null) anh.Dispose();
                    pic.Image = null;
                }
                _picVe.Clear();

                if (_printDoc != null) { _printDoc.Dispose(); _printDoc = null; }
            }
            base.Dispose(disposing);
        }
    }
}

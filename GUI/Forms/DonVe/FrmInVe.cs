using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using QuanLySuKienVaBanVe.GUI.Helpers;
using static QuanLySuKienVaBanVe.GUI.FrmInVeGdi;

namespace QuanLySuKienVaBanVe.GUI
{
    public class FrmInVe : Form
    {
        #region Fields & Properties
        private readonly List<ThongTinInVeDTO> _danhSachVe = new List<ThongTinInVeDTO>();
        private int _maDon;
        private int _indexInPrint;

        private PrintDocument _printDoc;
        private Icon _formIcon;

        private Panel pnlHeader;
        private Panel pnlPathBar;
        private Panel pnlPathCard;
        private Panel pnlLockChip;
        private Panel pnlStatus;
        private SurfaceXemTruoc pnlXemTruoc;
        private LocBanhXe _locBanhXe;
        private readonly List<Bitmap> _anhGoc = new List<Bitmap>();
        private PictureBox picThuMuc;
        private PictureBox picLock;
        private Label lblPathLabel;
        private Label lblLock;
        private Label lblStatus;
        private TextBox txtThuMuc;
        private NutBoGoc btnPrint;
        private NutBoGoc btnPdf;
        private NutBoGoc btnClose;

        private string _tieuDe = "In vé";
        private string _thongTinDon = "";
        private int _nutLeft;
        private float _dpiAnh = 96f;
        private EventHandler _napRanh;

        private const int ChenLe = 16;
        private const int CaoHeader = 104;
        private const int CaoPathBar = 66;
        private const string NhanBrand = "EventTicket - Hệ thống quản lý sự kiện";

        #endregion

        #region Public API & Entry Points
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

        private static DialogResult MoForm(List<ThongTinInVeDTO> dsVe, int maDon, IWin32Window owner)
        {
            using (FrmInVe frm = new FrmInVe(dsVe, maDon))
                return frm.ShowDialog(owner);
        }

        #endregion

        #region Constructors & Initialization
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
            BackColor = MatNen;
            MinimumSize = new Size(900, 560);
            ClientSize = new Size(1180, 660);
            StartPosition = FormStartPosition.CenterParent;
            DoubleBuffered = true;
            TaoBieuTuongCuaSo();

            TaoHeader();
            TaoPathBar();
            TaoCanvas();
            TaoStatusStrip();
            SapXepLayout();
            SapXepNut();

            _printDoc = new PrintDocument();
            _printDoc.DocumentName = "EventTicket";
            _printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            _printDoc.OriginAtMargins = false;
            _printDoc.BeginPrint += (s, e) => _indexInPrint = 0;
            _printDoc.PrintPage += PrintDoc_PrintPage;

            HienThi();
        }

        #endregion

        #region UI Setup & Custom Styling
        private void TaoHeader()
        {
            pnlHeader = new Panel
            {
                BackColor = DanMau[0],
                Dock = DockStyle.Top
            };
            pnlHeader.Height = CaoHeader;
            pnlHeader.Paint += VeHeader;
            pnlHeader.Resize += (s, e) => { SapXepNut(); pnlHeader.Invalidate(); };

            btnPrint = new NutBoGoc("In vé", 118, true);
            btnPrint.BieuTuong = TaoIconMayIn(18, Color.White);
            btnPrint.Click += (s, e) => InThietBi();

            btnPdf = new NutBoGoc("Xuất PDF", 132, false);
            btnPdf.BieuTuong = TaoIconFile(18, MauXanh);
            btnPdf.Click += (s, e) => XuatPdf();

            btnClose = new NutBoGoc("Đóng", 106, false);
            btnClose.BieuTuong = TaoIconDong(18, MauXanh);
            btnClose.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            pnlHeader.Controls.Add(btnPrint);
            pnlHeader.Controls.Add(btnPdf);
            pnlHeader.Controls.Add(btnClose);
            Controls.Add(pnlHeader);
        }

        private void TaoPathBar()
        {
            pnlPathBar = new Panel
            {
                BackColor = MatNen,
                Dock = DockStyle.Top,
                Padding = new Padding(ChenLe, 10, ChenLe, 8)
            };
            pnlPathBar.Height = CaoPathBar;

            pnlPathCard = new Panel
            {
                BackColor = MatNen,
                Dock = DockStyle.Fill
            };
            pnlPathCard.Paint += VeTheDuongDan;
            pnlPathCard.Resize += (s, e) => SapXepDuongDan();

            picThuMuc = new PictureBox
            {
                Image = TaoIconThuMuc(22, MauXanhNhat),
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = MauThe,
                Cursor = Cursors.Hand
            };
            picThuMuc.Click += (s, e) => ChonThuMucLuu();

            lblPathLabel = new Label
            {
                Text = "Thư mục lưu file PDF",
                AutoSize = true,
                Font = FntNhan,
                ForeColor = MauChu,
                BackColor = MauThe
            };

            txtThuMuc = new TextBox
            {
                BorderStyle = BorderStyle.None,
                ReadOnly = false,
                TabStop = true,
                BackColor = MauThe,
                ForeColor = MauChu,
                Font = FntDuongDan,
                Text = TicketPdfExporter.ThuMucMacDinh
            };
            txtThuMuc.DoubleClick += (s, e) => ChonThuMucLuu();

            pnlLockChip = new Panel
            {
                BackColor = MauThe,
                Size = new Size(112, 26),
                Cursor = Cursors.Hand
            };
            pnlLockChip.Paint += VeChip;
            pnlLockChip.MouseEnter += (s, e) => { _chipHovered = true; pnlLockChip.Invalidate(); };
            pnlLockChip.MouseLeave += (s, e) => { _chipHovered = false; pnlLockChip.Invalidate(); };
            pnlLockChip.Click += (s, e) => ChonThuMucLuu();

            picLock = new PictureBox
            {
                Image = TaoIconDuyet(14, MauXanhNhat),
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            picLock.MouseEnter += (s, e) => { _chipHovered = true; pnlLockChip.Invalidate(); };
            picLock.MouseLeave += (s, e) => { _chipHovered = false; pnlLockChip.Invalidate(); };
            picLock.Click += (s, e) => ChonThuMucLuu();

            lblLock = new Label
            {
                Text = "Thay đổi",
                AutoSize = true,
                Font = FntChip,
                ForeColor = MauXanhNhat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            lblLock.MouseEnter += (s, e) => { _chipHovered = true; pnlLockChip.Invalidate(); };
            lblLock.MouseLeave += (s, e) => { _chipHovered = false; pnlLockChip.Invalidate(); };
            lblLock.Click += (s, e) => ChonThuMucLuu();

            ToolTip tip = new ToolTip();
            tip.SetToolTip(pnlLockChip, "Nhấn để chọn thư mục lưu file PDF khác");
            tip.SetToolTip(picLock, "Nhấn để chọn thư mục lưu file PDF khác");
            tip.SetToolTip(lblLock, "Nhấn để chọn thư mục lưu file PDF khác");
            tip.SetToolTip(picThuMuc, "Nhấn để chọn thư mục lưu file PDF khác");
            tip.SetToolTip(txtThuMuc, "Nhấp đúp chuột để chọn thư mục lưu file PDF");

            pnlLockChip.Controls.Add(picLock);
            pnlLockChip.Controls.Add(lblLock);

            pnlPathCard.Controls.Add(txtThuMuc);
            pnlPathCard.Controls.Add(lblPathLabel);
            pnlPathCard.Controls.Add(picThuMuc);
            pnlPathCard.Controls.Add(pnlLockChip);
            pnlPathBar.Controls.Add(pnlPathCard);
            Controls.Add(pnlPathBar);
        }

        private void TaoCanvas()
        {
            pnlXemTruoc = new SurfaceXemTruoc { Dock = DockStyle.Fill };
            Controls.Add(pnlXemTruoc);

            _locBanhXe = new LocBanhXe(pnlXemTruoc);
            Application.AddMessageFilter(_locBanhXe);
        }

        private void TaoStatusStrip()
        {
            pnlStatus = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 34,
                BackColor = MauStatus
            };
            pnlStatus.Paint += VeDuoiStatus;

            lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 14, 0),
                BackColor = MauStatus,
                ForeColor = MauPhu,
                Font = FntTrangThai,
                Text = "Sẵn sàng."
            };
            pnlStatus.Controls.Add(lblStatus);
            Controls.Add(pnlStatus);
        }

        private void SapXepLayout()
        {
            Controls.SetChildIndex(pnlXemTruoc, 0);
            Controls.SetChildIndex(pnlStatus, 1);
            Controls.SetChildIndex(pnlPathBar, 2);
            Controls.SetChildIndex(pnlHeader, 3);
        }

        private void SapXepNut()
        {
            if (pnlHeader == null || btnClose == null) return;

            int w = pnlHeader.ClientSize.Width;
            int h = pnlHeader.ClientSize.Height;
            if (w < 40 || h < 40) return;

            int y = (h - CaoNut) / 2;
            int x = w - 22;
            btnClose.Location = new Point(x - btnClose.Width, y);
            btnPdf.Location = new Point(btnClose.Left - 10 - btnPdf.Width, y);
            btnPrint.Location = new Point(btnPdf.Left - 10 - btnPrint.Width, y);
            _nutLeft = btnPrint.Left;
        }

        private void SapXepDuongDan()
        {
            if (pnlPathCard == null || picThuMuc == null) return;

            int h = pnlPathCard.ClientSize.Height;
            int w = pnlPathCard.ClientSize.Width;
            if (h < 10 || w < 10) return;

            picThuMuc.Location = new Point(16, (h - picThuMuc.Height) / 2);
            lblPathLabel.Location = new Point(picThuMuc.Right + 10, (h - lblPathLabel.Height) / 2);

            bool vung = w - 14 - pnlLockChip.Width - 260 > lblPathLabel.Right;
            pnlLockChip.Visible = vung;
            if (vung)
            {
                pnlLockChip.Location = new Point(w - 14 - pnlLockChip.Width, (h - pnlLockChip.Height) / 2);
                int tongW = picLock.Width + 6 + lblLock.PreferredWidth;
                int startX = Math.Max(8, (pnlLockChip.Width - tongW) / 2);
                picLock.Location = new Point(startX, (pnlLockChip.Height - picLock.Height) / 2);
                lblLock.Location = new Point(picLock.Right + 6, (pnlLockChip.Height - lblLock.Height) / 2);
            }

            int trai = lblPathLabel.Right + 16;
            int phai = vung ? pnlLockChip.Left - 16 : w - 16;
            if (phai - trai < 120)
            {
                pnlLockChip.Visible = false;
                phai = w - 16;
            }

            txtThuMuc.Location = new Point(trai, (h - txtThuMuc.Height) / 2);
            txtThuMuc.Width = Math.Max(80, phai - trai);
        }

        private void ChonThuMucLuu()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Chọn thư mục để lưu các file vé PDF:";
                string hienTai = (txtThuMuc.Text ?? string.Empty).Trim();
                if (System.IO.Directory.Exists(hienTai))
                    dlg.SelectedPath = hienTai;
                else
                    dlg.SelectedPath = TicketPdfExporter.ThuMucMacDinh;

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
                {
                    txtThuMuc.Text = dlg.SelectedPath.Trim();
                    lblStatus.Text = string.Format("Đã chọn thư mục lưu PDF: {0}", txtThuMuc.Text);
                }
            }
        }

        private void TaoBieuTuongCuaSo()
        {
            try
            {
                Color nen = Color.FromArgb(22, 52, 145);
                using (Bitmap anh = new Bitmap(32, 32))
                {
                    using (Graphics g = Graphics.FromImage(anh))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        using (SolidBrush b = new SolidBrush(nen)) g.FillRectangle(b, 0, 0, 32, 32);

                        RectangleF rc = new RectangleF(4f, 10f, 24f, 12f);
                        using (SolidBrush trang = new SolidBrush(Color.White))
                        using (GraphicsPath path = TaoPathBoGoc(rc, 3f))
                            g.FillPath(trang, path);

                        using (SolidBrush b = new SolidBrush(nen))
                        {
                            g.FillEllipse(b, 1f, 13.6f, 5f, 5f);
                            g.FillEllipse(b, 26f, 13.6f, 5f, 5f);
                            using (Pen p = new Pen(nen, 1.6f))
                            {
                                p.DashStyle = DashStyle.Dash;
                                p.DashPattern = new float[] { 1.6f, 1.4f };
                                g.DrawLine(p, 21f, 11.5f, 21f, 20.5f);
                            }
                        }
                    }

                    IntPtr handle = anh.GetHicon();
                    try
                    {
                        using (Icon goc = Icon.FromHandle(handle))
                        {
                            _formIcon = (Icon)goc.Clone();
                            Icon = _formIcon;
                        }
                    }
                    finally
                    {
                        DestroyIcon(handle);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TaoBieuTuongCuaSo: " + ex.Message);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        #endregion

        #region Header & Card Painting
        private static void VeHuyHieuVe(Graphics g, Rectangle rc)
        {
            using (GraphicsPath path = TaoPathBoGoc(rc, 16f))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(22, 52, 145))) g.FillPath(b, path);
                using (Pen p = new Pen(Color.FromArgb(90, 255, 255, 255))) g.DrawPath(p, path);
            }

            RectangleF rcVe = new RectangleF(rc.X + 11f, rc.Y + (rc.Height - 26f) / 2f, rc.Width - 22f, 26f);
            VeBieuTuongVe(g, rcVe, Color.White, Color.FromArgb(22, 52, 145), 2.1f);
        }

        private void VeDamDong(Graphics g, int xTrai, int xPhai)
        {
            int rong = xPhai - xTrai;
            if (rong < 110) return;

            Rectangle vung = new Rectangle(xTrai, 0, rong, pnlHeader.ClientSize.Height);
            g.SetClip(vung);

            Color mau = Color.FromArgb(46, 255, 255, 255);
            float[] tyLe = new float[] { 0.92f, 1.16f, 1.0f, 1.3f, 0.86f, 1.22f, 0.98f, 1.1f };
            float buoc = rong / (float)(tyLe.Length - 1);

            using (SolidBrush b = new SolidBrush(mau))
            using (Pen p = new Pen(mau))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;

                for (int i = 0; i < tyLe.Length; i++)
                {
                    float s = tyLe[i];
                    float cx = xTrai + buoc * i + (i == 0 ? 6f : 0f);
                    float dau = 17f * s;
                    float dinhY = vung.Height - 42f * s - 2f;

                    float ngang = dau * 3f;
                    float vaiY = dinhY + dau * 0.72f;
                    RectangleF than = new RectangleF(cx - ngang / 2f, vaiY, ngang, vung.Height - vaiY + 14f);
                    using (GraphicsPath path = TaoPathBoGoc(than, dau * 0.72f))
                        g.FillPath(b, path);

                    g.FillEllipse(b, cx - dau / 2f, dinhY, dau, dau);

                    if (i % 2 == 0)
                    {
                        p.Width = dau * 0.3f;
                        g.DrawLine(p, cx - dau * 0.8f, vaiY + dau * 0.35f,
                            cx - dau * 1.55f, dinhY - dau * 0.95f);
                        g.DrawLine(p, cx + dau * 0.8f, vaiY + dau * 0.35f,
                            cx + dau * 1.5f, dinhY - dau * 1.25f);
                        if (i == 4)
                        {
                            RectangleF caiVe = new RectangleF(cx + dau * 1.5f - 11f,
                                dinhY - dau * 1.25f - 14f, 22f, 14f);
                            using (GraphicsPath path = TaoPathBoGoc(caiVe, 3f))
                                g.FillPath(b, path);
                        }
                    }
                }
            }

            g.ResetClip();
        }

        private void VeHeader(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = pnlHeader.ClientRectangle;
            if (rc.Width < 40 || rc.Height < 40) return;

            using (LinearGradientBrush dan = TaoBrushDan(rc))
                g.FillRectangle(dan, rc);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle hieu = new Rectangle(22, (rc.Height - 58) / 2, 58, 58);
            VeHuyHieuVe(g, hieu);

            int x = hieu.Right + 18;
            Size ktTieuDe = DoChu(_tieuDe, FntTieuDe, g);
            Size ktDon = DoChu(_thongTinDon, FntDon, g);
            int caoKhoi = ktTieuDe.Height + 4 + ktDon.Height;
            int y = (rc.Height - caoKhoi) / 2;

            TextRenderer.DrawText(g, _tieuDe, FntTieuDe, new Point(x, y), Color.White, TextFormatFlags.NoPrefix);
            if (_thongTinDon.Length > 0)
                TextRenderer.DrawText(g, _thongTinDon, FntDon,
                    new Point(x, y + ktTieuDe.Height + 2), Color.FromArgb(214, 228, 255), TextFormatFlags.NoPrefix);

            int gach = x + Math.Max(ktTieuDe.Width, ktDon.Width) + 22;
            if (gach > _nutLeft - 60) return;

            using (SolidBrush b = new SolidBrush(Color.FromArgb(96, 255, 255, 255)))
                g.FillRectangle(b, gach, (rc.Height - 44) / 2, 1, 44);

            Size ktBrand = DoChu(NhanBrand, FntBrand, g);
            TextRenderer.DrawText(g, NhanBrand, FntBrand,
                new Point(gach + 22, (rc.Height - ktBrand.Height) / 2),
                Color.FromArgb(236, 242, 255), TextFormatFlags.NoPrefix);

            VeDamDong(g, gach + 22 + ktBrand.Width + 28, _nutLeft - 14);
        }

        private void VeTheDuongDan(object sender, PaintEventArgs e)
        {
            RectangleF rc = new RectangleF(0.5f, 0.5f, pnlPathCard.ClientSize.Width - 1f,
                pnlPathCard.ClientSize.Height - 1f);
            if (rc.Width < 4 || rc.Height < 4) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = TaoPathBoGoc(rc, 12f))
            {
                using (SolidBrush b = new SolidBrush(MauThe)) e.Graphics.FillPath(b, path);
                using (Pen p = new Pen(MauVien)) e.Graphics.DrawPath(p, path);
            }
        }

        private bool _chipHovered;

        private void VeChip(object sender, PaintEventArgs e)
        {
            RectangleF rc = new RectangleF(0.5f, 0.5f, pnlLockChip.ClientSize.Width - 1f,
                pnlLockChip.ClientSize.Height - 1f);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = TaoPathBoGoc(rc, 8f))
            {
                Color bg = _chipHovered ? Color.FromArgb(220, 235, 255) : MauChip;
                Color border = _chipHovered ? MauXanh : MauChipVien;
                using (SolidBrush b = new SolidBrush(bg)) e.Graphics.FillPath(b, path);
                using (Pen p = new Pen(border)) e.Graphics.DrawPath(p, path);
            }
        }

        private void VeDuoiStatus(object sender, PaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(MauVien))
                e.Graphics.FillRectangle(b, 0, 0, pnlStatus.ClientSize.Width, 1);
        }

        private static Bitmap TaoIconMayIn(int size, Color mau)
        {
            Bitmap anh = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(anh))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float k = size / 24f;
                using (Pen p = new Pen(mau, 1.9f * k))
                using (SolidBrush b = new SolidBrush(mau))
                {
                    p.LineJoin = LineJoin.Round;
                    g.DrawLines(p, new PointF[] { Diem(k, 7.4f, 7.2f), Diem(k, 7.4f, 2.6f),
                        Diem(k, 16.6f, 2.6f), Diem(k, 16.6f, 7.2f) });

                    RectangleF than = new RectangleF(3f * k, 7.2f * k, 18f * k, 8.8f * k);
                    using (GraphicsPath path = TaoPathBoGoc(than, 2f * k)) g.DrawPath(p, path);

                    g.DrawLines(p, new PointF[] { Diem(k, 6.6f, 16f), Diem(k, 6.6f, 21.4f),
                        Diem(k, 17.4f, 21.4f), Diem(k, 17.4f, 16f) });
                    g.DrawLine(p, 8.6f * k, 18.6f * k, 15.4f * k, 18.6f * k);
                    g.FillEllipse(b, 17.4f * k, 9.2f * k, 1.8f * k, 1.8f * k);
                }
            }
            return anh;
        }

        private static Bitmap TaoIconFile(int size, Color mau)
        {
            Bitmap anh = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(anh))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float k = size / 24f;
                using (Pen p = new Pen(mau, 1.9f * k))
                {
                    p.LineJoin = LineJoin.Round;
                    g.DrawLines(p, new PointF[] { Diem(k, 5.6f, 2.6f), Diem(k, 14.2f, 2.6f),
                        Diem(k, 18.4f, 6.8f), Diem(k, 18.4f, 21.4f), Diem(k, 5.6f, 21.4f),
                        Diem(k, 5.6f, 2.6f) });
                    g.DrawLines(p, new PointF[] { Diem(k, 14.2f, 2.6f), Diem(k, 14.2f, 6.8f),
                        Diem(k, 18.4f, 6.8f) });
                    g.DrawLine(p, 8.6f * k, 11.4f * k, 15.4f * k, 11.4f * k);
                    g.DrawLine(p, 8.6f * k, 14.6f * k, 15.4f * k, 14.6f * k);
                    g.DrawLine(p, 8.6f * k, 17.8f * k, 12.8f * k, 17.8f * k);
                }
            }
            return anh;
        }

        private static Bitmap TaoIconDong(int size, Color mau)
        {
            Bitmap anh = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(anh))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float k = size / 24f;
                using (Pen p = new Pen(mau, 1.9f * k))
                {
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    g.DrawEllipse(p, 2.8f * k, 2.8f * k, 18.4f * k, 18.4f * k);
                    g.DrawLine(p, 8.8f * k, 8.8f * k, 15.2f * k, 15.2f * k);
                    g.DrawLine(p, 15.2f * k, 8.8f * k, 8.8f * k, 15.2f * k);
                }
            }
            return anh;
        }

        private static Bitmap TaoIconThuMuc(int size, Color mau)
        {
            Bitmap anh = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(anh))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float k = size / 24f;
                using (SolidBrush sau = new SolidBrush(mau))
                using (SolidBrush truoc = new SolidBrush(Color.FromArgb(120, mau)))
                {
                    g.FillPolygon(sau, new PointF[] { Diem(k, 2.4f, 6.4f), Diem(k, 9f, 6.4f),
                        Diem(k, 11.2f, 9.2f), Diem(k, 2.4f, 9.2f) });
                    RectangleF than = new RectangleF(2.4f * k, 6.4f * k, 19.2f * k, 13.6f * k);
                    using (GraphicsPath path = TaoPathBoGoc(than, 2.2f * k)) g.FillPath(sau, path);

                    RectangleF nap = new RectangleF(2.4f * k, 9.6f * k, 19.2f * k, 10.4f * k);
                    using (GraphicsPath path = TaoPathBoGoc(nap, 2.2f * k)) g.FillPath(truoc, path);
                }
            }
            return anh;
        }

        private static Bitmap TaoIconDuyet(int size, Color mau)
        {
            Bitmap anh = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(anh))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float k = size / 24f;
                using (Pen p = new Pen(mau, 1.8f * k))
                {
                    p.LineJoin = LineJoin.Round;
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    g.DrawLines(p, new PointF[] {
                        Diem(k, 3f, 6.5f), Diem(k, 9f, 6.5f), Diem(k, 11f, 9.5f),
                        Diem(k, 21f, 9.5f), Diem(k, 21f, 19.5f), Diem(k, 3f, 19.5f), Diem(k, 3f, 6.5f)
                    });
                    g.DrawLine(p, 3f * k, 12f * k, 21f * k, 12f * k);
                }
            }
            return anh;
        }

        #endregion

        #region Ticket Preview & Layout
        private void HienThi()
        {
            if (_danhSachVe.Count == 0)
            {
                _tieuDe = "In vé";
                _thongTinDon = "Chưa có vé nào để hiển thị";
                lblStatus.Text = "Không có vé nào để hiển thị.";
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                pnlHeader.Invalidate();
                pnlXemTruoc.CapNhat(_anhGoc, "Xem trước vé tại đây...");
                return;
            }

            _tieuDe = "In vé";
            _thongTinDon = string.Format("Đơn #{0}   ·   {1} vé vừa thanh toán", _maDon, _danhSachVe.Count);

            List<string> maVe = new List<string>();
            foreach (ThongTinInVeDTO ve in _danhSachVe)
                maVe.Add(ve.MaVeCode);

            lblStatus.Text = string.Format(
                "{0} vé   ·   Mã vé: {1}   ·   File PDF: {2}   ·   Kéo lên / kéo xuống để xem tất cả",
                _danhSachVe.Count, string.Join(", ", maVe.ToArray()),
                TicketPdfExporter.LayTenFile(_danhSachVe[0]));

            _dpiAnh = DeviceDpi > 0 ? DeviceDpi : 96f;
            while (_anhGoc.Count > _danhSachVe.Count)
            {
                int cuoi = _anhGoc.Count - 1;
                _anhGoc[cuoi].Dispose();
                _anhGoc.RemoveAt(cuoi);
            }

            pnlHeader.Invalidate();
            NapAnhDau();
        }

        private void NapAnhDau()
        {
            DungNapAnh();
            int soDau = Math.Min(2, _danhSachVe.Count);
            while (_anhGoc.Count < soDau)
                _anhGoc.Add(TicketRenderer.Render(_danhSachVe[_anhGoc.Count], 1280, _dpiAnh));

            pnlXemTruoc.CapNhat(_anhGoc, "Xem trước vé tại đây...");

            if (_anhGoc.Count < _danhSachVe.Count)
            {
                _napRanh = delegate { NapAnhTiep(); };
                Application.Idle += _napRanh;
            }
        }

        private void NapAnhTiep()
        {
            if (_anhGoc.Count >= _danhSachVe.Count)
            {
                DungNapAnh();
                return;
            }

            _anhGoc.Add(TicketRenderer.Render(_danhSachVe[_anhGoc.Count], 1280, _dpiAnh));
            pnlXemTruoc.XacNhatNoiDung();

            if (_anhGoc.Count >= _danhSachVe.Count) DungNapAnh();
        }

        private void DungNapAnh()
        {
            if (_napRanh == null) return;
            Application.Idle -= _napRanh;
            _napRanh = null;
        }
        #endregion

        #region Printing & PDF Export
        private static void CauHinhKhoGiay(PrintDocument doc)
        {
            try
            {
                doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                doc.OriginAtMargins = false;

                const int ticketW = 827;
                const int ticketH = 354;

                if (doc.PrinterSettings != null && doc.PrinterSettings.PaperSizes != null)
                {
                    foreach (PaperSize ps in doc.PrinterSettings.PaperSizes)
                    {
                        if (Math.Abs(ps.Width - ticketW) <= 8 && Math.Abs(ps.Height - ticketH) <= 8)
                        {
                            doc.DefaultPageSettings.PaperSize = ps;
                            doc.DefaultPageSettings.Landscape = false;
                            return;
                        }
                        if (Math.Abs(ps.Width - ticketH) <= 8 && Math.Abs(ps.Height - ticketW) <= 8)
                        {
                            doc.DefaultPageSettings.PaperSize = ps;
                            doc.DefaultPageSettings.Landscape = true;
                            return;
                        }
                    }
                }

                try
                {
                    PaperSize custom = new PaperSize("EventTicket 210x90mm", ticketW, ticketH);
                    doc.DefaultPageSettings.PaperSize = custom;
                    doc.DefaultPageSettings.Landscape = false;
                }
                catch
                {
                    doc.DefaultPageSettings.Landscape = true;
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
                RectangleF area = e.MarginBounds.IsEmpty ? e.PageBounds : e.MarginBounds;

                float ratio = TicketRenderer.DesignW / TicketRenderer.DesignH;
                float targetW = 827f;
                float targetH = targetW / ratio;

                float w = targetW;
                float h = targetH;

                if (w > area.Width || h > area.Height)
                {
                    w = area.Width;
                    h = w / ratio;
                    if (h > area.Height)
                    {
                        h = area.Height;
                        w = h * ratio;
                    }
                }

                RectangleF dest = new RectangleF(
                    area.X + (area.Width - w) / 2f,
                    area.Y + (area.Height - h) / 2f,
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

                CauHinhKhoGiay(_printDoc);

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
            if (string.IsNullOrEmpty(thuMuc))
            {
                thuMuc = TicketPdfExporter.ThuMucMacDinh;
                txtThuMuc.Text = thuMuc;
            }

            try
            {
                List<string> files = TicketPdfExporter.XuatDanhSach(_danhSachVe, thuMuc);
                string noiDung = files.Count == 1
                    ? files[0]
                    : string.Join(Environment.NewLine, files.ToArray());

                lblStatus.Text = string.Format("Đã xuất {0} file PDF vào {1}", files.Count, thuMuc);
                DialogResult dr = MessageBox.Show(this,
                    "Xuất PDF thành công (" + files.Count + " file, 300 DPI):\n\n" + noiDung +
                    "\n\nBạn có muốn mở thư mục chứa file PDF vừa xuất không?",
                    "Xuất vé PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes && System.IO.Directory.Exists(thuMuc))
                {
                    try
                    {
                        if (files.Count > 0 && System.IO.File.Exists(files[0]))
                            System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + files[0] + "\"");
                        else
                            System.Diagnostics.Process.Start("explorer.exe", "\"" + thuMuc + "\"");
                    }
                    catch (Exception exMo)
                    {
                        System.Diagnostics.Debug.WriteLine("MoThuMuc: " + exMo.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không xuất được PDF: " + ex.Message,
                    "Lỗi xuất PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DungNapAnh();
            GoBoLocBanhXe();
            base.OnFormClosed(e);
        }

        private void GoBoLocBanhXe()
        {
            if (_locBanhXe != null)
            {
                Application.RemoveMessageFilter(_locBanhXe);
                _locBanhXe = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DungNapAnh();
                GoBoLocBanhXe();

                foreach (Bitmap anh in _anhGoc)
                    if (anh != null) anh.Dispose();
                _anhGoc.Clear();

                if (btnPrint != null && btnPrint.BieuTuong != null) btnPrint.BieuTuong.Dispose();
                if (btnPdf != null && btnPdf.BieuTuong != null) btnPdf.BieuTuong.Dispose();
                if (btnClose != null && btnClose.BieuTuong != null) btnClose.BieuTuong.Dispose();
                if (picThuMuc != null && picThuMuc.Image != null) picThuMuc.Image.Dispose();
                if (picLock != null && picLock.Image != null) picLock.Image.Dispose();

                if (_printDoc != null) { _printDoc.Dispose(); _printDoc = null; }
                if (_formIcon != null)
                {
                    Icon = null;
                    _formIcon.Dispose();
                    _formIcon = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}

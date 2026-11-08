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

namespace QuanLySuKienVaBanVe.GUI
{
    public class FrmInVe : Form
    {
        #region Fields & Properties
        private readonly List<ThongTinInVeDTO> _danhSachVe = new List<ThongTinInVeDTO>();
        private int _maDon;
        private int _indexInPrint;

        private PrintDocument _printDoc;
        private Icon _hieuUng;

        private Panel pnlHeader;
        private Panel pnlPathBar;
        private Panel pnlPathCard;
        private Panel pnlLockChip;
        private Panel pnlStatus;
        private Panel pnlCanvas;
        private Panel pnlVe;
        private readonly List<PictureBox> _picVe = new List<PictureBox>();
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
        private int _cardW = 800;

        private const int KheGiuaVe = 18;
        private const int ChenLe = 16;
        private const int CaoHeader = 104;
        private const int CaoPathBar = 66;
        private const int CaoNut = 38;
        private const int MaxVeTrenManHinh = 1180;
        private const string NhanBrand = "EventTicket - Hệ thống quản lý sự kiện";

        private static readonly Color MatNen = Color.FromArgb(244, 246, 251);
        private static readonly Color MauThe = Color.White;
        private static readonly Color MauStatus = Color.FromArgb(248, 250, 253);
        private static readonly Color MauVien = Color.FromArgb(225, 230, 240);
        private static readonly Color MauChu = Color.FromArgb(30, 41, 59);
        private static readonly Color MauPhu = Color.FromArgb(100, 116, 139);
        private static readonly Color MauXanh = Color.FromArgb(29, 78, 216);
        private static readonly Color MauXanhNhat = Color.FromArgb(37, 99, 235);
        private static readonly Color MauChip = Color.FromArgb(233, 240, 254);
        private static readonly Color MauChipVien = Color.FromArgb(191, 214, 251);
        private static readonly Color[] DanMau = new Color[]
        {
            Color.FromArgb(26, 58, 160),
            Color.FromArgb(53, 95, 220),
            Color.FromArgb(163, 176, 240)
        };
        private static readonly float[] DanViTri = new float[] { 0f, 0.48f, 1f };

        private static readonly Font FntTieuDe = new Font("Segoe UI", 20f, FontStyle.Bold);
        private static readonly Font FntDon = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        private static readonly Font FntBrand = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        private static readonly Font FntNut = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        private static readonly Font FntNhan = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        private static readonly Font FntDuongDan = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        private static readonly Font FntChip = new Font("Segoe UI", 8.25f, FontStyle.Bold);
        private static readonly Font FntRong = new Font("Segoe UI", 12f, FontStyle.Bold);
        private static readonly Font FntTrangThai = new Font("Segoe UI", 9f, FontStyle.Regular);

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
            CaoBieuTuongCuaSo();

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
            CauHinhKhoGiay(_printDoc);
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
                BackColor = MauThe
            };

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
                ReadOnly = true,
                TabStop = false,
                BackColor = MauThe,
                ForeColor = MauPhu,
                Font = FntDuongDan,
                Text = TicketPdfExporter.ThuMucMacDinh
            };

            pnlLockChip = new Panel
            {
                BackColor = MauThe,
                Size = new Size(104, 26)
            };
            pnlLockChip.Paint += VeChip;

            picLock = new PictureBox
            {
                Image = TaoIconO(14, MauXanhNhat, MauChip),
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = MauChip
            };
            lblLock = new Label
            {
                Text = "Cố định",
                AutoSize = true,
                Font = FntChip,
                ForeColor = MauXanhNhat,
                BackColor = MauChip
            };
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
            pnlCanvas = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = MatNen,
                AutoScroll = true
            };

            pnlVe = new Panel
            {
                BackColor = MatNen,
                Location = new Point(0, 0),
                Size = new Size(800, 400)
            };
            pnlVe.Paint += VeTheXemTruoc;
            pnlCanvas.Controls.Add(pnlVe);

            for (int i = 0; i < _danhSachVe.Count; i++)
            {
                PictureBox pic = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = MauThe
                };
                _picVe.Add(pic);
                pnlVe.Controls.Add(pic);
            }

            Controls.Add(pnlCanvas);
            pnlCanvas.Resize += (s, e) => VeLaiDanhSach();
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
            Controls.SetChildIndex(pnlCanvas, 0);
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

            bool vung = w - 14 - pnlLockChip.Width - 320 > lblPathLabel.Right;
            pnlLockChip.Visible = vung;
            if (vung)
            {
                pnlLockChip.Location = new Point(w - 14 - pnlLockChip.Width, (h - pnlLockChip.Height) / 2);
                picLock.Location = new Point(10, (pnlLockChip.Height - picLock.Height) / 2);
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

        private void CaoBieuTuongCuaSo()
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
                            _hieuUng = (Icon)goc.Clone();
                            Icon = _hieuUng;
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
                System.Diagnostics.Debug.WriteLine("CaoBieuTuongCuaSo: " + ex.Message);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        #endregion

        #region Header & Card Painting
        private static LinearGradientBrush TaoBrushDan(Rectangle bounds)
        {
            LinearGradientBrush brush = new LinearGradientBrush(bounds, DanMau[0], DanMau[DanMau.Length - 1],
                LinearGradientMode.Horizontal);
            ColorBlend nen = new ColorBlend();
            nen.Colors = DanMau;
            nen.Positions = DanViTri;
            brush.InterpolationColors = nen;
            return brush;
        }

        private static GraphicsPath TaoPathBoGoc(RectangleF rc, float banKinh)
        {
            GraphicsPath path = new GraphicsPath();
            float d = Math.Min(banKinh * 2f, Math.Min(rc.Width, rc.Height));
            if (d <= 0.5f)
            {
                path.AddRectangle(rc);
                return path;
            }
            path.AddArc(rc.X, rc.Y, d, d, 180f, 90f);
            path.AddArc(rc.Right - d, rc.Y, d, d, 270f, 90f);
            path.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0f, 90f);
            path.AddArc(rc.X, rc.Bottom - d, d, d, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath TaoSao(float tamX, float tamY, float rNgoai, float rTrong)
        {
            GraphicsPath path = new GraphicsPath();
            PointF[] dinh = new PointF[10];
            for (int i = 0; i < 10; i++)
            {
                double goc = -Math.PI / 2d + i * Math.PI / 5d;
                float r = (i % 2 == 0) ? rNgoai : rTrong;
                dinh[i] = new PointF(tamX + (float)(r * Math.Cos(goc)), tamY + (float)(r * Math.Sin(goc)));
            }
            path.AddPolygon(dinh);
            return path;
        }

        private static Size DoChu(string text, Font font, Graphics g)
        {
            if (string.IsNullOrEmpty(text)) return Size.Empty;
            return TextRenderer.MeasureText(g, text, font, Size.Empty, TextFormatFlags.NoPrefix);
        }

        private static PointF Diem(float k, float x, float y)
        {
            return new PointF(x * k, y * k);
        }

        private static void VeBieuTuongVe(Graphics g, RectangleF rc, Color mau, Color mauCat, float net)
        {
            using (GraphicsPath path = TaoPathBoGoc(rc, Math.Min(rc.Height * 0.26f, rc.Width * 0.05f)))
            using (Pen pen = new Pen(mau, net))
            {
                pen.LineJoin = LineJoin.Round;
                g.DrawPath(pen, path);
            }

            float het = net * 2.4f;
            using (SolidBrush cat = new SolidBrush(mauCat))
            {
                g.FillEllipse(cat, rc.X - het / 2f, rc.Y + rc.Height / 2f - het / 2f, het, het);
                g.FillEllipse(cat, rc.Right - het / 2f, rc.Y + rc.Height / 2f - het / 2f, het, het);
            }

            float xTai = rc.X + rc.Width * 0.68f;
            using (Pen pen = new Pen(mau, net * 0.85f))
            {
                pen.DashStyle = DashStyle.Dash;
                pen.DashPattern = new float[] { 2.4f, 2.2f };
                g.DrawLine(pen, xTai, rc.Y + net * 2.4f, xTai, rc.Bottom - net * 2.4f);
            }

            using (SolidBrush b = new SolidBrush(mau))
            using (GraphicsPath sao = TaoSao(rc.X + rc.Width * 0.26f, rc.Y + rc.Height / 2f, net * 3.6f, net * 1.5f))
                g.FillPath(b, sao);
        }

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

        private void VeChip(object sender, PaintEventArgs e)
        {
            RectangleF rc = new RectangleF(0.5f, 0.5f, pnlLockChip.ClientSize.Width - 1f,
                pnlLockChip.ClientSize.Height - 1f);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = TaoPathBoGoc(rc, 8f))
            {
                using (SolidBrush b = new SolidBrush(MauChip)) e.Graphics.FillPath(b, path);
                using (Pen p = new Pen(MauChipVien)) e.Graphics.DrawPath(p, path);
            }
        }

        private void VeDuoiStatus(object sender, PaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(MauVien))
                e.Graphics.FillRectangle(b, 0, 0, pnlStatus.ClientSize.Width, 1);
        }

        private void VeTheXemTruoc(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            float cao = Math.Max(ChenLe * 2f + 120f, pnlVe.Height - ChenLe * 2f);
            RectangleF the = new RectangleF(ChenLe, ChenLe, Math.Max(120f, _cardW - 1f), cao);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = TaoPathBoGoc(the, 18f))
            {
                using (SolidBrush b = new SolidBrush(MauThe)) g.FillPath(b, path);
                using (Pen p = new Pen(MauVien)) g.DrawPath(p, path);
            }

            if (_danhSachVe.Count > 0) return;

            string rong = "Xem trước vé tại đây...";
            Size kt = DoChu(rong, FntRong, g);
            int iconR = 132;
            int iconC = 78;
            int y = (int)(the.Top + (the.Height - (iconC + 22 + kt.Height)) / 2f);
            if (y < the.Top + 24) y = (int)the.Top + 24;

            RectangleF rc = new RectangleF(the.X + (the.Width - iconR) / 2f, y, iconR, iconC);
            VeBieuTuongVe(g, rc, Color.FromArgb(186, 210, 246), MauThe, 5f);
            TextRenderer.DrawText(g, rong, FntRong,
                new Point((int)(the.X + (the.Width - kt.Width) / 2f), y + iconC + 22),
                Color.FromArgb(148, 163, 184), TextFormatFlags.NoPrefix);
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

        private static Bitmap TaoIconO(int size, Color mau, Color mauNen)
        {
            Bitmap anh = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(anh))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float k = size / 24f;
                using (Pen p = new Pen(mau, 2f * k))
                using (SolidBrush b = new SolidBrush(mau))
                using (SolidBrush lo = new SolidBrush(mauNen))
                {
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    g.DrawArc(p, 7.2f * k, 3.4f * k, 9.6f * k, 10f * k, 180f, 180f);
                    g.DrawLine(p, 7.2f * k, 8.4f * k, 7.2f * k, 11.6f * k);
                    g.DrawLine(p, 16.8f * k, 8.4f * k, 16.8f * k, 11.6f * k);

                    RectangleF than = new RectangleF(4.6f * k, 11.4f * k, 14.8f * k, 9.6f * k);
                    using (GraphicsPath path = TaoPathBoGoc(than, 2.2f * k)) g.FillPath(b, path);
                    g.FillEllipse(lo, 11.1f * k, 14.6f * k, 1.8f * k, 1.8f * k);
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
                VeLaiDanhSach();
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

            float dpi = DeviceDpi > 0 ? DeviceDpi : 96f;
            for (int i = 0; i < _picVe.Count; i++)
            {
                if (_picVe[i].Image == null)
                    _picVe[i].Image = TicketRenderer.Render(_danhSachVe[i], 1280, dpi);
            }

            pnlHeader.Invalidate();
            VeLaiDanhSach();
        }

        private void VeLaiDanhSach()
        {
            int viewport = Math.Max(200, pnlCanvas.ClientSize.Height);
            int w = Math.Max(320, pnlCanvas.ClientSize.Width - ChenLe * 2);
            int cao = SapXepChumVe(w);

            if (cao > viewport)
            {
                int wHetKhe = Math.Max(320, w - SystemInformation.VerticalScrollBarWidth);
                if (wHetKhe < w) cao = SapXepChumVe(wHetKhe);
            }

            pnlVe.Height = Math.Max(cao, viewport);
            pnlVe.Invalidate();
        }

        private int SapXepChumVe(int cardW)
        {
            _cardW = cardW;
            pnlVe.Width = Math.Max(pnlCanvas.ClientSize.Width, cardW + ChenLe * 2);

            int trong = cardW - ChenLe * 4;
            if (trong > MaxVeTrenManHinh) trong = MaxVeTrenManHinh;
            if (trong < 260) trong = 260;
            int cao = (int)Math.Round(trong * TicketRenderer.DesignH / TicketRenderer.DesignW);

            int x = ChenLe + (cardW - trong) / 2;
            int y = ChenLe + 8;
            foreach (PictureBox pic in _picVe)
            {
                pic.Size = new Size(trong, cao);
                pic.Location = new Point(x, y);
                y += cao + KheGiuaVe;
            }

            if (_picVe.Count == 0) return ChenLe * 2 + 260;
            return y - KheGiuaVe + 8 + ChenLe;
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

            string thuMuc = TicketPdfExporter.ThuMucMacDinh;
            txtThuMuc.Text = thuMuc;

            try
            {
                List<string> files = TicketPdfExporter.XuatDanhSach(_danhSachVe, thuMuc);
                string noiDung = files.Count == 1
                    ? files[0]
                    : string.Join(Environment.NewLine, files.ToArray());

                lblStatus.Text = string.Format("Đã xuất {0} file PDF vào {1}", files.Count, thuMuc);
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

                if (btnPrint != null && btnPrint.BieuTuong != null) btnPrint.BieuTuong.Dispose();
                if (btnPdf != null && btnPdf.BieuTuong != null) btnPdf.BieuTuong.Dispose();
                if (btnClose != null && btnClose.BieuTuong != null) btnClose.BieuTuong.Dispose();
                if (picThuMuc != null && picThuMuc.Image != null) picThuMuc.Image.Dispose();
                if (picLock != null && picLock.Image != null) picLock.Image.Dispose();

                if (_printDoc != null) { _printDoc.Dispose(); _printDoc = null; }
                if (_hieuUng != null)
                {
                    Icon = null;
                    _hieuUng.Dispose();
                    _hieuUng = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        private sealed class NutBoGoc : Button
        {
            public Bitmap BieuTuong;

            private readonly bool _chinh;
            private bool _gac;
            private bool _nhan;

            public NutBoGoc(string text, int width, bool chinh)
            {
                _chinh = chinh;
                Text = text;
                Size = new Size(width, CaoNut);
                Font = FntNut;
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                Cursor = Cursors.Hand;
                BackColor = MatNen;

                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                    ControlStyles.Selectable, true);

                MouseEnter += (s, e) => { _gac = true; Invalidate(); };
                MouseLeave += (s, e) => { _gac = false; _nhan = false; Invalidate(); };
                MouseDown += (s, e) => { _nhan = true; Invalidate(); };
                MouseUp += (s, e) => { _nhan = false; Invalidate(); };
                GotFocus += (s, e) => Invalidate();
                LostFocus += (s, e) => Invalidate();
                EnabledChanged += (s, e) => Invalidate();
            }

            private Color MauNen()
            {
                if (!Enabled) return _chinh ? Color.FromArgb(110, 255, 255, 255) : Color.FromArgb(232, 238, 246);
                if (_nhan) return _chinh ? Color.FromArgb(15, 47, 137) : Color.FromArgb(222, 232, 248);
                if (_gac) return _chinh ? Color.FromArgb(21, 60, 175) : Color.FromArgb(238, 244, 255);
                return _chinh ? MauXanh : Color.White;
            }

            private Color MauChu()
            {
                if (!Enabled) return _chinh ? Color.FromArgb(198, 255, 255, 255) : Color.FromArgb(148, 163, 184);
                return _chinh ? Color.White : Color.FromArgb(30, 64, 175);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                Rectangle rc = ClientRectangle;

                Control cha = Parent;
                if (cha != null && cha.ClientSize.Width > 0 && cha.ClientSize.Height > 0)
                {
                    Rectangle nen = new Rectangle(-Left, -Top, cha.ClientSize.Width, cha.ClientSize.Height);
                    using (LinearGradientBrush dan = TaoBrushDan(nen))
                        g.FillRectangle(dan, rc);
                }
                else
                {
                    g.Clear(MatNen);
                }

                g.SmoothingMode = SmoothingMode.AntiAlias;
                RectangleF khuon = new RectangleF(0.6f, 0.6f, Width - 1.2f, Height - 1.2f);
                using (GraphicsPath path = TaoPathBoGoc(khuon, 9f))
                {
                    using (SolidBrush b = new SolidBrush(MauNen())) g.FillPath(b, path);
                    using (Pen p = new Pen(_chinh ? Color.FromArgb(70, 255, 255, 255) : Color.FromArgb(203, 213, 225)))
                        g.DrawPath(p, path);
                }

                Color chu = MauChu();
                Size kt = DoChu(Text, FntNut, g);
                int iconW = BieuTuong != null ? 18 : 0;
                int khe = BieuTuong != null ? 8 : 0;
                int x = (Width - (iconW + khe + kt.Width)) / 2;

                if (BieuTuong != null)
                {
                    Rectangle o = new Rectangle(x, (Height - iconW) / 2, iconW, iconW);
                    if (Enabled)
                    {
                        g.DrawImage(BieuTuong, o);
                    }
                    else
                    {
                        using (ImageAttributes attr = new ImageAttributes())
                        {
                            ColorMatrix lop = new ColorMatrix();
                            lop.Matrix33 = 0.55f;
                            attr.SetColorMatrix(lop);
                            g.DrawImage(BieuTuong, o, 0, 0, BieuTuong.Width, BieuTuong.Height,
                                GraphicsUnit.Pixel, attr);
                        }
                    }
                }

                TextRenderer.DrawText(g, Text, FntNut,
                    new Rectangle(x + iconW + khe, 0, kt.Width + 10, Height), chu,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }
        }
    }
}

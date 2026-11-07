using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.DTO;
using ZXing;
using ZXing.Rendering;

namespace QuanLySuKienVaBanVe.GUI.Helpers
{
    public static class TicketRenderer
    {
        public const float DesignW = 2100f;
        public const float DesignH = 900f;
        public const float StubW = 525f;
        public const float BodyW = DesignW - StubW;
        public const float TearX = BodyW;

        public const int PrintDpi = 300;
        public static readonly Size PrintSizePx = new Size(2480, 1063);
        public static readonly SizeF PrintSizeMm = new SizeF(210f, 90f);

        private const float NotchR = 20f;

        private static string _fontFamily;
        private static FontFamily _customFontFamily;
        private static PrivateFontCollection _fontCollection;
        private static readonly object _fontLock = new object();

        private static void EnsureCustomFonts()
        {
            if (_fontCollection != null) return;
            lock (_fontLock)
            {
                if (_fontCollection != null) return;
                var coll = new PrivateFontCollection();
                foreach (string root in CandidateFolders())
                {
                    string fontDir = Path.Combine(root, "Resources", "Fonts");
                    if (Directory.Exists(fontDir))
                    {
                        foreach (string fontFile in Directory.GetFiles(fontDir, "*.ttf"))
                        {
                            try { coll.AddFontFile(fontFile); }
                            catch { }
                        }
                    }
                }

                if (coll.Families.Length > 0)
                {
                    foreach (FontFamily fam in coll.Families)
                    {
                        if (string.Equals(fam.Name, "Poppins", StringComparison.OrdinalIgnoreCase))
                        {
                            _customFontFamily = fam;
                            break;
                        }
                    }
                    if (_customFontFamily == null)
                        _customFontFamily = coll.Families[0];
                }
                _fontCollection = coll;
            }
        }

        public static string FontFamily
        {
            get
            {
                if (_fontFamily == null)
                {
                    EnsureCustomFonts();
                    if (_customFontFamily != null)
                    {
                        _fontFamily = _customFontFamily.Name;
                    }
                    else
                    {
                        _fontFamily = "Segoe UI";
                        foreach (string ten in new[] { "Poppins", "Montserrat", "Nunito", "Segoe UI" })
                        {
                            try
                            {
                                using (Font f = new Font(ten, 10f))
                                {
                                    if (string.Equals(f.FontFamily.Name, ten, StringComparison.OrdinalIgnoreCase))
                                    {
                                        _fontFamily = ten;
                                        break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
                return _fontFamily;
            }
            set { _fontFamily = value; }
        }

        private static Font CreateFont(float sizePx, FontStyle style)
        {
            EnsureCustomFonts();
            if (_customFontFamily != null)
            {
                try { return new Font(_customFontFamily, sizePx, style, GraphicsUnit.Pixel); }
                catch { }
            }
            return new Font(FontFamily, sizePx, style, GraphicsUnit.Pixel);
        }

        public static string BackgroundFolder = null;

        private static readonly Dictionary<string, Image> _imageCache =
            new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        private static readonly object _imageLock = new object();

        private static readonly Color InkWhite = Color.White;
        private static readonly Color BoxBorder = Color.FromArgb(150, 255, 255, 255);

        private static readonly Color[] BodyStops =
        {
            Color.FromArgb(0xC4, 0x0F, 0x63),
            Color.FromArgb(0xA9, 0x14, 0xC8),
            Color.FromArgb(0x5B, 0x17, 0x9C),
            Color.FromArgb(0x2B, 0x3D, 0xB8),
            Color.FromArgb(0x14, 0xB2, 0xEE)
        };

        private static readonly Color[] StubStops =
        {
            Color.FromArgb(0xA8, 0x0A, 0x5C),
            Color.FromArgb(0x8E, 0x0E, 0x86),
            Color.FromArgb(0x54, 0x16, 0x92),
            Color.FromArgb(0x33, 0x1B, 0x8C),
            Color.FromArgb(0x1B, 0x2A, 0x74)
        };

        private static readonly Color ZoneTop = Color.FromArgb(0xE0, 0x1D, 0x8C);
        private static readonly Color ZoneBottom = Color.FromArgb(0x7B, 0x1F, 0xA8);
        private static readonly Color RowSeatTop = Color.FromArgb(0x6D, 0x5A, 0xD4);
        private static readonly Color RowSeatBottom = Color.FromArgb(0x2F, 0x3F, 0x9E);
        private static readonly Color TimeTop = Color.FromArgb(0x38, 0xB6, 0xF6);
        private static readonly Color TimeBottom = Color.FromArgb(0x15, 0x65, 0xC9);

        public static Bitmap Render(ThongTinInVeDTO ve, int widthPx, float dpi)
        {
            if (ve == null) throw new ArgumentNullException("ve");
            if (widthPx < 60) widthPx = 60;

            int heightPx = (int)Math.Round(widthPx * DesignH / DesignW);
            Bitmap bmp = new Bitmap(widthPx, heightPx, PixelFormat.Format32bppPArgb);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Clear(Color.FromArgb(0x12, 0x12, 0x1C));

                float k = widthPx / DesignW;
                g.ScaleTransform(k, k);

                DrawBody(g, ve);
                DrawStub(g, ve);
                DrawTear(g);
            }

            bmp.SetResolution(dpi, dpi);
            return bmp;
        }

        public static Bitmap RenderForPrint(ThongTinInVeDTO ve)
        {
            return Render(ve, PrintSizePx.Width, PrintDpi);
        }

        private static void DrawBody(Graphics g, ThongTinInVeDTO ve)
        {
            DrawRegionBackground(g, new RectangleF(0f, 0f, BodyW, DesignH), "ticket_bg", BodyStops);

            DrawLogo(g, new RectangleF(90f, 66f, 430f, 100f));

            DrawFittedText(g, Co(ve.TenSuKien, "Sự kiện chưa đặt tên"),
                new RectangleF(90f, 209f, 1040f, 150f),
                112f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleLeft);

            DrawFittedText(g, "Location: " + Co(ve.DiaDiem, "Chưa cập nhật địa điểm"),
                new RectangleF(90f, 353f, 1040f, 60f),
                40f, FontStyle.Regular, InkWhite, ContentAlignment.MiddleLeft);

            DrawInfoBox(g, new RectangleF(87f, 487f, 298f, 205f), ZoneTop, ZoneBottom, "Zone", ve.ZoneIn);
            DrawRowSeatBox(g, new RectangleF(412f, 487f, 362f, 205f), RowSeatTop, RowSeatBottom, ve.HangIn, ve.SoGheIn);
            DrawInfoBox(g, new RectangleF(801f, 487f, 348f, 205f), TimeTop, TimeBottom, "Time:", ve.GioIn);

            DrawFittedText(g, ve.NgayIn,
                new RectangleF(100f, 715f, 1100f, 185f),
                150f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter);

            DrawImageAsset(g, "MusicNote", new RectangleF(1304f, 90f, 120f, 299f));
        }

        private static void DrawStub(Graphics g, ThongTinInVeDTO ve)
        {
            DrawRegionBackground(g, new RectangleF(TearX, 0f, StubW, DesignH), "stub_bg", StubStops);

            DrawLogo(g, new RectangleF(TearX + 62f, 55f, 400f, 93f));

            DrawFittedText(g, ve.NgayIn,
                new RectangleF(TearX + 20f, 195f, 485f, 110f),
                84f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter);

            DrawStackedPair(g, new RectangleF(TearX + 45f, 300f, 200f, 136f), "Row", ve.HangIn);
            DrawStackedPair(g, new RectangleF(TearX + 255f, 300f, 200f, 136f), "Seat", ve.SoGheIn);

            DrawFittedText(g, Co(ve.TenSuKien, "Sự kiện chưa đặt tên"),
                new RectangleF(TearX + 30f, 465f, 465f, 170f),
                62f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter, maxLines: 3);

            DrawBarcode(g, new RectangleF(TearX + 50f, 676f, 430f, 175f), ve.MaBarcode);
        }

        private static void DrawStackedPair(Graphics g, RectangleF r, string label, string value)
        {
            DrawText(g, label, new RectangleF(r.X, r.Y, r.Width, 55f),
                36f, FontStyle.Regular, Color.FromArgb(235, 255, 255, 255), ContentAlignment.MiddleCenter);
            DrawFittedText(g, value, new RectangleF(r.X, r.Y + 58f, r.Width, 78f),
                62f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter);
        }

        private static void DrawRegionBackground(Graphics g, RectangleF r, string tenAnh, Color[] stops)
        {
            Image bg = LoadImage(tenAnh);
            if (bg != null)
            {
                g.DrawImage(bg, r);
                return;
            }

            using (GraphicsPath clip = RoundPath(r, 0f))
            using (var br = new LinearGradientBrush(
                new PointF(r.Left, r.Top), new PointF(r.Right, r.Bottom), Color.Transparent, Color.Transparent))
            {
                br.InterpolationColors = new ColorBlend
                {
                    Positions = new[] { 0f, 0.26f, 0.52f, 0.76f, 1f },
                    Colors = stops
                };
                g.FillPath(br, clip);
            }
        }

        private static void DrawTear(Graphics g)
        {
            using (var pen = new Pen(Color.FromArgb(215, 255, 255, 255), 3f))
            {
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[] { 14f, 11f };
                g.DrawLine(pen, TearX, NotchR * 0.9f, TearX, DesignH - NotchR * 0.9f);
            }

            using (var hole = new SolidBrush(Color.FromArgb(235, 10, 10, 18)))
            {
                g.FillEllipse(hole, TearX - NotchR, -NotchR, NotchR * 2f, NotchR * 2f);
                g.FillEllipse(hole, TearX - NotchR, DesignH - NotchR, NotchR * 2f, NotchR * 2f);
            }
        }

        private static void DrawLogo(Graphics g, RectangleF r)
        {
            Image logo = LoadImage("logoEventTicket");
            if (logo != null)
            {
                DrawPreserveAspect(g, logo, r);
                return;
            }

            DrawFittedText(g, "EventTicket", r, r.Height * 0.80f,
                FontStyle.Bold | FontStyle.Italic, InkWhite, ContentAlignment.MiddleCenter);
        }

        private static void DrawImageAsset(Graphics g, string ten, RectangleF r)
        {
            Image anh = LoadImage(ten);
            if (anh != null) DrawPreserveAspect(g, anh, r);
        }

        private static void DrawPreserveAspect(Graphics g, Image anh, RectangleF r)
        {
            if (anh.Width <= 0 || anh.Height <= 0) return;

            float k = Math.Min(r.Width / anh.Width, r.Height / anh.Height);
            float w = anh.Width * k;
            float h = anh.Height * k;
            g.DrawImage(anh, new RectangleF(r.X + (r.Width - w) / 2f, r.Y + (r.Height - h) / 2f, w, h));
        }

        private static void DrawInfoBox(Graphics g, RectangleF r, Color top, Color bottom, string title, string value)
        {
            float h = r.Height;

            using (GraphicsPath p = RoundPath(r, h * 0.16f))
            using (var br = new LinearGradientBrush(r, top, bottom, 90f))
                g.FillPath(br, p);

            using (GraphicsPath p = RoundPath(r, h * 0.16f))
            using (var pen = new Pen(BoxBorder, 2.5f))
                g.DrawPath(pen, p);

            DrawText(g, title, new RectangleF(r.X, r.Y + 0.08f * h, r.Width, 0.33f * h),
                40f, FontStyle.Regular, Color.FromArgb(240, 255, 255, 255), ContentAlignment.MiddleCenter);

            DrawFittedText(g, value, new RectangleF(r.X + 10f, r.Y + 0.42f * h, r.Width - 20f, 0.46f * h),
                76f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter);
        }

        private static void DrawRowSeatBox(Graphics g, RectangleF r, Color top, Color bottom, string row, string seat)
        {
            float h = r.Height;

            using (GraphicsPath p = RoundPath(r, h * 0.16f))
            using (var br = new LinearGradientBrush(r, top, bottom, 90f))
                g.FillPath(br, p);

            using (GraphicsPath p = RoundPath(r, h * 0.16f))
            using (var pen = new Pen(BoxBorder, 2.5f))
                g.DrawPath(pen, p);

            float half = r.Width / 2f;
            RectangleF left = new RectangleF(r.X, r.Y, half, h);
            RectangleF right = new RectangleF(r.X + half, r.Y, half, h);

            DrawText(g, "Row", new RectangleF(left.X, left.Y + 0.08f * h, left.Width, 0.30f * h),
                40f, FontStyle.Regular, Color.FromArgb(240, 255, 255, 255), ContentAlignment.MiddleCenter);
            DrawFittedText(g, row, new RectangleF(left.X, left.Y + 0.40f * h, left.Width, 0.54f * h),
                76f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter);

            DrawText(g, "Seat", new RectangleF(right.X, right.Y + 0.08f * h, right.Width, 0.30f * h),
                40f, FontStyle.Regular, Color.FromArgb(240, 255, 255, 255), ContentAlignment.MiddleCenter);
            DrawFittedText(g, seat, new RectangleF(right.X, right.Y + 0.40f * h, right.Width, 0.54f * h),
                76f, FontStyle.Bold, InkWhite, ContentAlignment.MiddleCenter);

            using (var pen = new Pen(Color.FromArgb(225, 255, 255, 255), 2.5f))
                g.DrawLine(pen, r.X + half, r.Y + 0.16f * h, r.X + half, r.Bottom - 0.16f * h);
        }

        private static void DrawBarcode(Graphics g, RectangleF area, string content)
        {
            using (GraphicsPath frame = RoundPath(area, area.Height * 0.10f))
            using (var white = new SolidBrush(Color.White))
                g.FillPath(white, frame);

            RectangleF inner = RectangleF.Inflate(area, -area.Width * 0.05f, -area.Height * 0.14f);

            Bitmap bc = CreateBarcodeBitmap(content, (int)inner.Width, (int)inner.Height);
            if (bc == null) return;

            using (bc)
            {
                InterpolationMode old = g.InterpolationMode;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(bc, inner);
                g.InterpolationMode = old;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }
        }

        public static Bitmap CreateBarcodeBitmap(string content, int widthPx, int heightPx)
        {
            if (string.IsNullOrEmpty(content)) return null;
            if (widthPx < 8 || heightPx < 8) return null;

            try
            {
                BarcodeWriter writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = widthPx,
                        Height = heightPx,
                        Margin = 0,
                        NoPadding = true,
                        PureBarcode = true
                    },
                    Renderer = new BitmapRenderer
                    {
                        Foreground = Color.Black,
                        Background = Color.White
                    }
                };
                return writer.Write(content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TicketRenderer barcode: " + ex.Message);
                return null;
            }
        }

        private static string Co(string giaTri, string khiTrong)
        {
            return string.IsNullOrWhiteSpace(giaTri) ? khiTrong : giaTri.Trim();
        }

        private static GraphicsPath RoundPath(RectangleF r, float radius)
        {
            return QuanLySuKienVaBanVe.GUI.Fx.RoundRect(r, radius);
        }

        private static void DrawText(Graphics g, string text, RectangleF r, float sizePx,
            FontStyle style, Color color, ContentAlignment align)
        {
            using (Font f = CreateFont(sizePx, style))
            using (var br = new SolidBrush(color))
            using (StringFormat sf = AlignFormat(align))
                g.DrawString(text ?? string.Empty, f, br, r, sf);
        }

        private static void DrawFittedText(Graphics g, string text, RectangleF r, float sizePx,
            FontStyle style, Color color, ContentAlignment align, int maxLines = 1)
        {
            text = text ?? string.Empty;

            StringFormat draw = AlignFormat(align);
            draw.Trimming = StringTrimming.EllipsisCharacter;
            if (maxLines == 1)
            {
                // Private fonts can report glyph bounds smaller than the full line.
                draw.FormatFlags &= ~StringFormatFlags.LineLimit;
                draw.FormatFlags |= StringFormatFlags.NoWrap;
            }
            else
            {
                draw.FormatFlags &= ~StringFormatFlags.NoWrap;
                draw.LineAlignment = align == ContentAlignment.MiddleCenter
                    ? StringAlignment.Center : StringAlignment.Near;
            }

            try
            {
                float size = sizePx;
                while (size > 14f)
                {
                    using (Font f = CreateFont(size, style))
                    {
                        bool fits;
                        if (maxLines == 1)
                        {
                            SizeF need = g.MeasureString(text, f, PointF.Empty, draw);
                            float lineHeight = Math.Max(need.Height, f.GetHeight(g));
                            fits = need.Width <= r.Width && lineHeight <= r.Height;
                        }
                        else
                        {
                            StringFormat probe = AlignFormat(align);
                            probe.Trimming = StringTrimming.None;
                            probe.FormatFlags &= ~StringFormatFlags.NoWrap;
                            SizeF need;
                            using (probe)
                                need = g.MeasureString(text, f, new SizeF(r.Width, r.Height * 4f), probe);
                            fits = need.Height <= r.Height + 0.5f;
                        }

                        if (fits)
                        {
                            using (var br = new SolidBrush(color))
                                g.DrawString(text, f, br, r, draw);
                            return;
                        }
                    }
                    size -= Math.Max(1.5f, size * 0.05f);
                }

                using (Font f = CreateFont(14f, style))
                using (var br = new SolidBrush(color))
                    g.DrawString(text, f, br, r, draw);
            }
            finally
            {
                draw.Dispose();
            }
        }

        private static StringFormat AlignFormat(ContentAlignment align)
        {
            StringFormat sf = new StringFormat();
            switch (align)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near; sf.LineAlignment = StringAlignment.Near; break;
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center; sf.LineAlignment = StringAlignment.Near; break;
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far; sf.LineAlignment = StringAlignment.Near; break;
                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near; sf.LineAlignment = StringAlignment.Center; break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center; sf.LineAlignment = StringAlignment.Center; break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far; sf.LineAlignment = StringAlignment.Center; break;
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near; sf.LineAlignment = StringAlignment.Far; break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center; sf.LineAlignment = StringAlignment.Far; break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far; sf.LineAlignment = StringAlignment.Far; break;
            }
            return sf;
        }

        private static Image LoadImage(string ten)
        {
            lock (_imageLock)
            {
                Image cached;
                if (_imageCache.TryGetValue(ten, out cached)) return cached;

                Image found = null;
                foreach (string root in CandidateFolders())
                {
                    foreach (string ext in new[] { ".png", ".jpg", ".jpeg" })
                    {
                        string path = Path.Combine(root, "Resources", "Ticket", ten + ext);
                        if (File.Exists(path))
                        {
                            try
                            {
                                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                using (var tmp = Image.FromStream(fs))
                                {
                                    found = new Bitmap(tmp);
                                }
                            }
                            catch { found = null; }
                            break;
                        }
                    }
                    if (found != null) break;
                }

                _imageCache[ten] = found;
                return found;
            }
        }

        private static IEnumerable<string> CandidateFolders()
        {
            if (!string.IsNullOrWhiteSpace(BackgroundFolder))
                yield return BackgroundFolder;

            yield return Application.StartupPath;
            yield return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static void ResetBackgroundCache()
        {
            lock (_imageLock)
            {
                foreach (Image img in _imageCache.Values)
                    if (img != null) img.Dispose();
                _imageCache.Clear();
            }
        }
    }
}

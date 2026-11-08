using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    internal static class FrmInVeGdi
    {
        public const int CaoNut = 38;

        public static readonly Color MatNen = Color.FromArgb(244, 246, 251);
        public static readonly Color MauThe = Color.White;
        public static readonly Color MauStatus = Color.FromArgb(248, 250, 253);
        public static readonly Color MauVien = Color.FromArgb(225, 230, 240);
        public static readonly Color MauChu = Color.FromArgb(30, 41, 59);
        public static readonly Color MauPhu = Color.FromArgb(100, 116, 139);
        public static readonly Color MauXanh = Color.FromArgb(29, 78, 216);
        public static readonly Color MauXanhNhat = Color.FromArgb(37, 99, 235);
        public static readonly Color MauChip = Color.FromArgb(233, 240, 254);
        public static readonly Color MauChipVien = Color.FromArgb(191, 214, 251);

        public static readonly Color[] DanMau = new Color[]
        {
            Color.FromArgb(26, 58, 160),
            Color.FromArgb(53, 95, 220),
            Color.FromArgb(163, 176, 240)
        };
        public static readonly float[] DanViTri = new float[] { 0f, 0.48f, 1f };

        public static readonly Font FntTieuDe = new Font("Segoe UI", 20f, FontStyle.Bold);
        public static readonly Font FntDon = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font FntBrand = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        public static readonly Font FntNut = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font FntNhan = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font FntDuongDan = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font FntChip = new Font("Segoe UI", 8.25f, FontStyle.Bold);
        public static readonly Font FntRong = new Font("Segoe UI", 12f, FontStyle.Bold);
        public static readonly Font FntTrangThai = new Font("Segoe UI", 9f, FontStyle.Regular);

        public static LinearGradientBrush TaoBrushDan(Rectangle bounds)
        {
            LinearGradientBrush brush = new LinearGradientBrush(bounds, DanMau[0], DanMau[DanMau.Length - 1],
                LinearGradientMode.Horizontal);
            ColorBlend nen = new ColorBlend();
            nen.Colors = DanMau;
            nen.Positions = DanViTri;
            brush.InterpolationColors = nen;
            return brush;
        }

        public static GraphicsPath TaoPathBoGoc(RectangleF rc, float banKinh)
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

        public static GraphicsPath TaoSao(float tamX, float tamY, float rNgoai, float rTrong)
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

        public static Size DoChu(string text, Font font, Graphics g)
        {
            if (string.IsNullOrEmpty(text)) return Size.Empty;
            return TextRenderer.MeasureText(g, text, font, Size.Empty, TextFormatFlags.NoPrefix);
        }

        public static PointF Diem(float k, float x, float y)
        {
            return new PointF(x * k, y * k);
        }

        public static void VeBieuTuongVe(Graphics g, RectangleF rc, Color mau, Color mauCat, float net)
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
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QuanLySuKienVaBanVe.GUI
{
    /// <summary>Bảng màu + hàm vẽ/animation dùng chung cho các control hiệu ứng.</summary>
    public static class Fx
    {
        // ---- Bảng màu (lấy theo ảnh mẫu: nền hồng, nút hồng, link xanh) ----
        public static readonly Color Pink = Color.FromArgb(255, 45, 150);
        public static readonly Color PinkDark = Color.FromArgb(214, 16, 116);
        public static readonly Color PinkPale = Color.FromArgb(255, 200, 232);
        public static readonly Color Blue = Color.FromArgb(29, 95, 216);
        public static readonly Color Field = Color.FromArgb(243, 244, 246);
        public static readonly Color FieldHover = Color.FromArgb(230, 233, 240);
        public static readonly Color Ink = Color.FromArgb(17, 24, 39);
        public static readonly Color Muted = Color.FromArgb(107, 114, 128);
        public static readonly Color Danger = Color.FromArgb(220, 38, 38);
        public static readonly Color Success = Color.FromArgb(22, 163, 74);

        // ---- Tiện ích ----
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        /// <summary>Hình chữ nhật bo góc (GDI+ không có sẵn).</summary>
        public static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            float d = Math.Min(radius * 2f, Math.Min(r.Width, r.Height));
            GraphicsPath p = new GraphicsPath();
            if (d <= 0f) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // ---- Hàm easing (t chạy từ 0 đến 1) ----

        /// <summary>Rơi xuống rồi nảy vài lần rồi nằm yên: dùng cho "bounce".</summary>
        public static float EaseOutBounce(float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            const float n1 = 7.5625f, d1 = 2.75f;
            if (t < 1f / d1) return n1 * t * t;
            if (t < 2f / d1) { t -= 1.5f / d1; return n1 * t * t + 0.75f; }
            if (t < 2.5f / d1) { t -= 2.25f / d1; return n1 * t * t + 0.9375f; }
            t -= 2.625f / d1; return n1 * t * t + 0.984375f;
        }

        /// <summary>Lò xo tắt dần: bắt đầu = 1, dao động quanh 0 rồi về 0 (hiệu ứng "nảy" khi bấm).</summary>
        public static float Spring(float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return (float)(Math.Exp(-5.0 * t) * Math.Cos(11.0 * t));
        }

        // ---- Icon vẽ bằng GDI+ (không cần file ảnh) ----

        public static void IconUser(Graphics g, RectangleF r, Color c)
        {
            float s = Math.Min(r.Width, r.Height);
            using (Pen p = new Pen(c, 1.8f))
            {
                g.DrawEllipse(p, r.X + s * 0.31f, r.Y + s * 0.06f, s * 0.38f, s * 0.38f);
                g.DrawArc(p, r.X + s * 0.1f, r.Y + s * 0.55f, s * 0.8f, s * 0.8f, 200, 140);
            }
        }

        public static void IconLock(Graphics g, RectangleF r, Color c)
        {
            float s = Math.Min(r.Width, r.Height);
            using (Pen p = new Pen(c, 1.8f))
            using (GraphicsPath body = RoundRect(new RectangleF(r.X + s * 0.14f, r.Y + s * 0.44f, s * 0.72f, s * 0.5f), 3f))
            {
                g.DrawPath(p, body);
                g.DrawArc(p, r.X + s * 0.28f, r.Y + s * 0.06f, s * 0.44f, s * 0.66f, 180, 180);
                g.FillEllipse(new SolidBrush(c), r.X + s * 0.44f, r.Y + s * 0.62f, s * 0.12f, s * 0.12f);
            }
        }

        public static void IconMail(Graphics g, RectangleF r, Color c)
        {
            float s = Math.Min(r.Width, r.Height);
            using (Pen p = new Pen(c, 1.8f))
            using (GraphicsPath body = RoundRect(new RectangleF(r.X, r.Y + s * 0.2f, s, s * 0.62f), 3f))
            {
                g.DrawPath(p, body);
                g.DrawLines(p, new[]
                {
                    new PointF(r.X + s * 0.05f, r.Y + s * 0.28f),
                    new PointF(r.X + s * 0.5f, r.Y + s * 0.58f),
                    new PointF(r.X + s * 0.95f, r.Y + s * 0.28f)
                });
            }
        }

        /// <summary>Con mắt: slashed = true là "đang ẩn mật khẩu" (mắt gạch chéo).</summary>
        public static void IconEye(Graphics g, RectangleF r, Color c, bool slashed)
        {
            float s = Math.Min(r.Width, r.Height);
            using (Pen p = new Pen(c, 1.8f))
            {
                g.DrawEllipse(p, r.X, r.Y + s * 0.22f, s, s * 0.56f);
                g.FillEllipse(new SolidBrush(c), r.X + s * 0.36f, r.Y + s * 0.36f, s * 0.28f, s * 0.28f);
                if (slashed) g.DrawLine(p, r.X + s * 0.12f, r.Y + s * 0.9f, r.X + s * 0.88f, r.Y + s * 0.1f);
            }
        }

        public static void IconTick(Graphics g, RectangleF box, float scale, Color c)
        {
            float cx = box.X + box.Width / 2f, cy = box.Y + box.Height / 2f;
            PointF[] pts =
            {
                new PointF(-0.27f, 0.02f), new PointF(-0.06f, 0.22f), new PointF(0.3f, -0.2f)
            };
            for (int i = 0; i < pts.Length; i++)
                pts[i] = new PointF(cx + pts[i].X * box.Width * scale, cy + pts[i].Y * box.Height * scale);
            using (Pen p = new Pen(c, 2.4f) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round })
                g.DrawLines(p, pts);
        }

        public static void IconChevron(Graphics g, PointF center, Color c)
        {
            using (Pen p = new Pen(c, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                g.DrawLines(p, new[]
                {
                    new PointF(center.X - 4.5f, center.Y - 2.2f),
                    new PointF(center.X, center.Y + 2.4f),
                    new PointF(center.X + 4.5f, center.Y - 2.2f)
                });
        }

        /// <summary>Cờ tròn nhỏ: VN (nền đỏ, sao vàng) hoặc EN (nền xanh, chữ thập).</summary>
        public static void IconFlag(Graphics g, RectangleF r, Ngon ngon)
        {
            GraphicsState st = g.Save();
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(r);
                g.SetClip(gp, CombineMode.Intersect);
            }
            if (ngon == Ngon.VN)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(218, 37, 29)), r);
                using (Font f = new Font("Segoe UI Symbol", r.Height * 0.5f, FontStyle.Regular, GraphicsUnit.Pixel))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString("★", f, new SolidBrush(Color.FromArgb(255, 235, 0)), r, sf);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(1, 33, 105)), r);
                float cx = r.X + r.Width / 2f, cy = r.Y + r.Height / 2f;
                using (Pen w = new Pen(Color.White, r.Height * 0.2f))
                using (Pen red = new Pen(Color.FromArgb(200, 16, 46), r.Height * 0.1f))
                {
                    g.DrawLine(w, r.X, r.Y, r.Right, r.Bottom);
                    g.DrawLine(w, r.Right, r.Y, r.X, r.Bottom);
                    g.DrawLine(w, cx, r.Y, cx, r.Bottom);
                    g.DrawLine(w, r.X, cy, r.Right, cy);
                    g.DrawLine(red, cx, r.Y, cx, r.Bottom);
                    g.DrawLine(red, r.X, cy, r.Right, cy);
                }
            }
            g.Restore(st);
        }
    }
}

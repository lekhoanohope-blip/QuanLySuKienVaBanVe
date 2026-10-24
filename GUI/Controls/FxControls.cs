using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;   // tránh mơ hồ với System.Threading.Timer khi project bật ImplicitUsings

namespace QuanLySuKienVaBanVe.GUI
{
    // =====================================================================
    //  FxControl: lớp cha của mọi control có hiệu ứng.
    //  - Tự vẽ bằng GDI+ (UserPaint) nên bo góc/đổ bóng được.
    //  - Có biến Hover chạy mượt từ 0 -> 1 khi rê chuột vào (và 1 -> 0 khi rời đi).
    //    Các lớp con dùng Hover để pha màu: đó chính là "Background-color hover effect".
    //  - Một Timer 15ms chỉ chạy khi còn animation, xong là tự dừng (không tốn CPU).
    // =====================================================================
    public abstract class FxControl : Control
    {
        private readonly Timer timer = new Timer { Interval = 15 };
        private bool hovering;

        protected float Hover;                       // 0 = bình thường, 1 = đang rê chuột

        protected FxControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);
            BackColor = Color.Transparent;
            timer.Tick += delegate
            {
                if (!OnTick()) timer.Stop();
                Invalidate();
            };
        }

        protected void Kick() { if (!timer.Enabled) timer.Start(); }

        protected void SetHover(bool value) { hovering = value; Kick(); }

        /// <summary>Kiểm tra lại chuột còn nằm trong control không (cần khi control có control con).</summary>
        protected void ReevaluateHover()
        {
            hovering = ClientRectangle.Contains(PointToClient(Cursor.Position));
            Kick();
        }

        /// <summary>Mỗi 15ms: trả true nếu còn animation cần chạy tiếp.</summary>
        protected virtual bool OnTick()
        {
            float target = (hovering && Enabled) ? 1f : 0f;
            if (Math.Abs(Hover - target) < 0.001f) { Hover = target; return false; }
            Hover += Math.Sign(target - Hover) * 0.14f;
            Hover = Math.Max(0f, Math.Min(1f, Hover));
            return true;
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); SetHover(true); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); ReevaluateHover(); }
        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Kick(); Invalidate(); }

        // Control tự vẽ (UserPaint) KHÔNG tự vẽ lại khi đổi Text/Font: phải gọi Invalidate thủ công,
        // nếu thiếu thì đổi ngôn ngữ xong nút vẫn hiện chữ cũ.
        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }
        protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); Invalidate(); }

        protected override void Dispose(bool disposing)
        {
            if (disposing) timer.Dispose();
            base.Dispose(disposing);
        }
    }

    // =====================================================================
    //  FxButton: nút bo góc.
    //   - Hover: đổi màu nền mượt + nổi lên 2px + quầng sáng hồng.
    //   - Nhấn: co lại 4%. Thả chuột: "nảy" kiểu lò xo (Bounce animation).
    //   - Enabled = false: hiện màu hồng nhạt như ảnh mẫu.
    // =====================================================================
    public class FxButton : FxControl
    {
        private bool pressed;
        private float bounceT = -1f;
        private DateTime bounceStart;

        public Color BaseColor = Fx.Pink;
        public Color HoverColor = Fx.PinkDark;
        public Color DisabledColor = Fx.PinkPale;
        public int Radius = 16;

        public FxButton()
        {
            Size = new Size(388, 62);        // rộng hơn ô nhập 8px, 4px mỗi bên là vùng đệm cho hiệu ứng nổi/quầng sáng
            Font = new Font("Segoe UI Semibold", 11.5f);
            ForeColor = Color.White;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && Enabled) { pressed = true; Invalidate(); }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (pressed)
            {
                pressed = false;
                bounceT = 0f;
                bounceStart = DateTime.Now;
                Kick();
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            base.OnEnabledChanged(e);
        }

        protected override bool OnTick()
        {
            bool again = base.OnTick();
            if (bounceT >= 0f)
            {
                bounceT = (float)((DateTime.Now - bounceStart).TotalMilliseconds / 650.0);
                if (bounceT >= 1f) bounceT = -1f; else again = true;
            }
            return again;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            float scale = pressed ? 0.96f : (bounceT >= 0f ? 1f - 0.07f * Fx.Spring(bounceT) : 1f);
            float lift = -2f * Hover;
            RectangleF rect = new RectangleF(4, 4, Width - 8, Height - 8);
            float cx = Width / 2f, cy = Height / 2f;

            GraphicsState st = g.Save();
            g.TranslateTransform(cx, cy + lift);
            g.ScaleTransform(scale, scale);
            g.TranslateTransform(-cx, -cy);

            Color fill = Enabled ? Fx.Lerp(BaseColor, HoverColor, Hover) : DisabledColor;

            if (Enabled && Hover > 0.01f)       // quầng sáng khi hover
            {
                using (GraphicsPath glow = Fx.RoundRect(RectangleF.Inflate(rect, 3f * Hover, 3f * Hover), Radius + 2))
                using (SolidBrush gb = new SolidBrush(Color.FromArgb((int)(60 * Hover), BaseColor)))
                    g.FillPath(gb, glow);
            }
            using (GraphicsPath path = Fx.RoundRect(rect, Radius))
            using (SolidBrush b = new SolidBrush(fill))
                g.FillPath(b, path);

            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (SolidBrush tb = new SolidBrush(ForeColor))
                g.DrawString(Text, Font, tb, rect, sf);

            g.Restore(st);
        }
    }

    // =====================================================================
    //  FxLink: chữ liên kết. Hover: chữ chuyển màu hồng + gạch chân.
    // =====================================================================
    public class FxLink : FxControl
    {
        public Color NormalColor = Fx.Blue;
        public Color HoverColor = Fx.PinkDark;

        public FxLink()
        {
            Font = new Font("Segoe UI Semibold", 10f);
            Cursor = Cursors.Hand;
        }

        private void Fit()
        {
            Size s = TextRenderer.MeasureText(Text ?? "", Font);
            Size = new Size(s.Width + 4, s.Height + 2);
        }

        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Fit(); }
        protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); Fit(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (Font f = new Font(Font, Hover > 0.5f ? Font.Style | FontStyle.Underline : Font.Style))
            using (SolidBrush b = new SolidBrush(Fx.Lerp(NormalColor, HoverColor, Hover)))
                g.DrawString(Text, f, b, 0, 0);
        }
    }

    // =====================================================================
    //  FxCheck: ô tick bo góc. Khi tick, dấu ✓ "nảy" lên (spring) như ảnh mẫu.
    // =====================================================================
    public class FxCheck : FxControl
    {
        private bool isChecked;
        private float tickT = -1f;
        private DateTime tickStart;

        public event EventHandler CheckedChanged;

        public FxCheck()
        {
            Font = new Font("Segoe UI", 10f);
            Cursor = Cursors.Hand;
            Height = 28;
        }

        public bool Checked
        {
            get { return isChecked; }
            set
            {
                if (isChecked == value) return;
                isChecked = value;
                tickT = 0f; tickStart = DateTime.Now; Kick();
                Invalidate();
                EventHandler h = CheckedChanged;
                if (h != null) h(this, EventArgs.Empty);
            }
        }

        private void Fit()
        {
            Size s = TextRenderer.MeasureText(Text ?? "", Font);
            Width = 30 + s.Width + 2;
        }

        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Fit(); }
        protected override void OnClick(EventArgs e) { base.OnClick(e); Checked = !Checked; }

        protected override bool OnTick()
        {
            bool again = base.OnTick();
            if (tickT >= 0f)
            {
                tickT = (float)((DateTime.Now - tickStart).TotalMilliseconds / 500.0);
                if (tickT >= 1f) tickT = -1f; else again = true;
            }
            return again;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            RectangleF box = new RectangleF(2, (Height - 20) / 2f, 20, 20);
            Color fill = isChecked ? Fx.Blue : Fx.Lerp(Color.White, Color.FromArgb(222, 233, 255), Hover);
            using (GraphicsPath p = Fx.RoundRect(box, 5f))
            using (SolidBrush b = new SolidBrush(fill))
            using (Pen pen = new Pen(Fx.Blue, 1.6f))
            {
                g.FillPath(b, p);
                g.DrawPath(pen, p);
            }
            if (isChecked)
            {
                float k = tickT >= 0f ? 1f - 0.6f * Fx.Spring(tickT) : 1f;     // dấu tick nảy
                Fx.IconTick(g, box, k, Color.White);
            }
            using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center })
            using (SolidBrush tb = new SolidBrush(Fx.Ink))
                g.DrawString(Text, Font, tb, new RectangleF(30, 0, Width - 30, Height), sf);
        }
    }

    public enum FxIcon { None, User, Lock, Mail }

    // =====================================================================
    //  RoundedTextBox: ô nhập bo góc = 1 TextBox không viền đặt trong control tự vẽ.
    //   - Hover: nền đậm lên mượt. Focus: viền xanh. Lỗi: viền đỏ.
    //   - Ô mật khẩu có con mắt để hiện/ẩn.
    //   - Placeholder dùng EM_SETCUEBANNER của Windows (chữ mờ biến mất khi gõ).
    // =====================================================================
    public class RoundedTextBox : FxControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private readonly TextBox tb = new TextBox();
        private readonly FxIcon icon;
        private readonly bool password;
        private bool showPassword, focused, error, eyeHot;
        private string cue = "";

        public event EventHandler ValueChanged;

        public RoundedTextBox(FxIcon icon, bool password)
        {
            this.icon = icon;
            this.password = password;
            Size = new Size(380, 54);
            Font = new Font("Segoe UI", 11f);

            tb.BorderStyle = BorderStyle.None;
            tb.Font = Font;
            tb.ForeColor = Fx.Ink;
            tb.BackColor = Fx.Field;
            tb.MaxLength = 64;
            tb.UseSystemPasswordChar = password;
            tb.GotFocus += delegate { focused = true; Invalidate(); };
            tb.LostFocus += delegate { focused = false; Invalidate(); };
            tb.MouseEnter += delegate { SetHover(true); };
            tb.MouseLeave += delegate { ReevaluateHover(); };
            tb.HandleCreated += delegate { ApplyCue(); };
            tb.TextChanged += delegate
            {
                if (error) error = false;
                Invalidate();
                EventHandler h = ValueChanged;
                if (h != null) h(this, EventArgs.Empty);
            };
            Controls.Add(tb);
            LayoutInner();
        }

        public string Value { get { return tb.Text; } set { tb.Text = value ?? ""; } }
        public string Cue { get { return cue; } set { cue = value ?? ""; ApplyCue(); } }
        public bool HasError { get { return error; } set { error = value; Invalidate(); } }
        public void FocusInput() { tb.Focus(); }

        private void ApplyCue()
        {
            try
            {
                if (tb.IsHandleCreated) SendMessage(tb.Handle, EM_SETCUEBANNER, (IntPtr)1, cue);
            }
            catch (Exception) { /* hệ điều hành không hỗ trợ placeholder: bỏ qua, form vẫn dùng bình thường */ }
        }

        private Rectangle EyeRect { get { return new Rectangle(Width - 46, (Height - 30) / 2, 30, 30); } }

        private void LayoutInner()
        {
            int left = icon == FxIcon.None ? 18 : 52;
            int right = password ? 52 : 18;
            tb.Left = left;
            tb.Width = Math.Max(20, Width - left - right);
            tb.Top = (Height - tb.Height) / 2;
        }

        protected override void OnResize(EventArgs e) { base.OnResize(e); LayoutInner(); }
        protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); tb.Font = Font; LayoutInner(); }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool hot = password && EyeRect.Contains(e.Location);
            if (hot != eyeHot) { eyeHot = hot; Cursor = hot ? Cursors.Hand : Cursors.Default; Invalidate(); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (eyeHot) { showPassword = !showPassword; tb.UseSystemPasswordChar = !showPassword; Invalidate(); }
            else tb.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fill = Fx.Lerp(Fx.Field, Fx.FieldHover, Hover);
            if (tb.BackColor != fill) tb.BackColor = fill;

            using (GraphicsPath path = Fx.RoundRect(new RectangleF(1, 1, Width - 2, Height - 2), 14f))
            using (SolidBrush b = new SolidBrush(fill))
            {
                g.FillPath(b, path);
                if (error || focused)
                    using (Pen p = new Pen(error ? Fx.Danger : Fx.Blue, 1.8f)) g.DrawPath(p, path);
            }

            Color ic = error ? Fx.Danger : (focused ? Fx.Blue : Fx.Muted);
            RectangleF ir = new RectangleF(17, (Height - 22) / 2f, 22, 22);
            if (icon == FxIcon.User) Fx.IconUser(g, ir, ic);
            else if (icon == FxIcon.Lock) Fx.IconLock(g, ir, ic);
            else if (icon == FxIcon.Mail) Fx.IconMail(g, ir, ic);

            if (password)
            {
                Rectangle er = EyeRect;
                Fx.IconEye(g, new RectangleF(er.X + 4, er.Y + 4, 22, 22), eyeHot ? Fx.Blue : Fx.Muted, !showPassword);
            }
        }
    }

    // =====================================================================
    //  LangChip: nút chọn ngôn ngữ góc phải (cờ + "VN" + mũi tên) như ảnh mẫu.
    // =====================================================================
    public class LangChip : FxControl
    {
        public LangChip()
        {
            Size = new Size(98, 46);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Semibold", 11f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            RectangleF r = new RectangleF(3, 3 - 1.5f * Hover, Width - 6, Height - 8);
            using (GraphicsPath sh = Fx.RoundRect(new RectangleF(r.X, r.Y + 2, r.Width, r.Height), 13))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                g.FillPath(sb, sh);
            using (GraphicsPath p = Fx.RoundRect(r, 13))
            using (SolidBrush b = new SolidBrush(Fx.Lerp(Color.White, Color.FromArgb(255, 235, 247), Hover)))
                g.FillPath(b, p);

            Fx.IconFlag(g, new RectangleF(r.X + 12, r.Y + (r.Height - 24) / 2f, 24, 24), Lang.Hien);
            using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center })
            using (SolidBrush tb = new SolidBrush(Fx.Ink))
                g.DrawString(Lang.Hien == Ngon.VN ? "VN" : "EN", Font, tb, new RectangleF(r.X + 44, r.Y, 36, r.Height), sf);
            Fx.IconChevron(g, new PointF(r.Right - 14, r.Y + r.Height / 2f), Fx.Ink);
        }
    }

    // =====================================================================
    //  CardPanel: thẻ trắng bo góc có đổ bóng mềm. Bóng + nền trắng được vẽ
    //  1 lần vào bitmap (cache) nên rê chuột lên các control con vẫn mượt.
    // =====================================================================
    public class CardPanel : Panel
    {
        public const int Pad = 24;      // vùng đệm để chứa bóng đổ
        private Bitmap cache;

        public CardPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
        }

        private void BuildCache()
        {
            if (cache != null) cache.Dispose();
            cache = null;
            if (Width <= 2 * Pad || Height <= 2 * Pad) return;
            cache = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(cache))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                RectangleF r = new RectangleF(Pad, Pad, Width - 2 * Pad, Height - 2 * Pad);
                for (int i = Pad - 2; i >= 1; i--)          // bóng: nhiều vòng viền mờ dần
                {
                    float k = 1f - i / (float)Pad;
                    int alpha = (int)(16 * k * k);
                    RectangleF rr = RectangleF.Inflate(r, i * 0.7f, i * 0.7f);
                    rr.Offset(0, 6);
                    using (GraphicsPath gp = Fx.RoundRect(rr, 22 + i * 0.7f))
                    using (Pen p = new Pen(Color.FromArgb(alpha, 90, 10, 60), 1.6f))
                        g.DrawPath(p, gp);
                }
                using (GraphicsPath card = Fx.RoundRect(r, 22))
                    g.FillPath(Brushes.White, card);
            }
        }

        protected override void OnResize(EventArgs eventargs) { base.OnResize(eventargs); BuildCache(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (cache == null) BuildCache();
            if (cache != null) e.Graphics.DrawImageUnscaled(cache, 0, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && cache != null) cache.Dispose();
            base.Dispose(disposing);
        }
    }
}

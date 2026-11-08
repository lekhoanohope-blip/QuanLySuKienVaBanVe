using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static QuanLySuKienVaBanVe.GUI.FrmInVeGdi;

namespace QuanLySuKienVaBanVe.GUI
{
    internal sealed class NutBoGoc : Button
    {
        public Bitmap BieuTuong;

        private readonly bool _isPrimary;
        private bool _isHovered;
        private bool _isPressed;

        public NutBoGoc(string text, int width, bool isPrimary)
        {
            _isPrimary = isPrimary;
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

            MouseEnter += (s, e) => { _isHovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _isHovered = false; _isPressed = false; Invalidate(); };
            MouseDown += (s, e) => { _isPressed = true; Invalidate(); };
            MouseUp += (s, e) => { _isPressed = false; Invalidate(); };
            GotFocus += (s, e) => Invalidate();
            LostFocus += (s, e) => Invalidate();
            EnabledChanged += (s, e) => Invalidate();
        }

        private Color MauNen()
        {
            if (!Enabled) return _isPrimary ? Color.FromArgb(110, 255, 255, 255) : Color.FromArgb(232, 238, 246);
            if (_isPressed) return _isPrimary ? Color.FromArgb(15, 47, 137) : Color.FromArgb(222, 232, 248);
            if (_isHovered) return _isPrimary ? Color.FromArgb(21, 60, 175) : Color.FromArgb(238, 244, 255);
            return _isPrimary ? MauXanh : Color.White;
        }

        private Color MauChu()
        {
            if (!Enabled) return _isPrimary ? Color.FromArgb(198, 255, 255, 255) : Color.FromArgb(148, 163, 184);
            return _isPrimary ? Color.White : Color.FromArgb(30, 64, 175);
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
                using (Pen p = new Pen(_isPrimary ? Color.FromArgb(70, 255, 255, 255) : Color.FromArgb(203, 213, 225)))
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

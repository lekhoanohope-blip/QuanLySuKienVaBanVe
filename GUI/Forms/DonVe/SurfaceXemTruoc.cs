using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.GUI.Helpers;
using static QuanLySuKienVaBanVe.GUI.FrmInVeGdi;

namespace QuanLySuKienVaBanVe.GUI
{
    internal sealed class SurfaceXemTruoc : Control
    {
        private const int Kenh = 16;
        private const int Khe = 18;
        private const int Dem = 8;
        private const int MaxRong = 1180;
        private const int KheLuot = 14;

        private List<Bitmap> _anhGoc = new List<Bitmap>();
        private readonly List<Bitmap> _anhLop = new List<Bitmap>();
        private string _thongBao = "";
        private int _scrollY;
        private int _maxScroll;
        private int _contentH;
        private int _rongAnh;
        private int _caoAnh;
        private int _rongThe;
        private int _rongAnhLuu;
        private readonly Timer _dongHo;
        private readonly Timer _henLop;
        private Rectangle _nut = Rectangle.Empty;
        private int _mucTieu;
        private bool _keo;
        private bool _gac;
        private int _keoY;
        private int _keoScroll;

        public SurfaceXemTruoc()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = MatNen;
            TabStop = false;

            _dongHo = new Timer { Interval = 12 };
            _dongHo.Tick += ChayBuoc;

            _henLop = new Timer { Interval = 90 };
            _henLop.Tick += TaiLop;
        }

        public void CapNhat(List<Bitmap> anhGoc, string thongBao)
        {
            DungDong();
            _scrollY = 0;
            _mucTieu = 0;
            _anhGoc = anhGoc;
            _thongBao = thongBao;
            SapXep();
        }

        public void XacNhatNoiDung()
        {
            SapXep();
        }

        public void XoayBanh(int delta)
        {
            if (_maxScroll <= 0) return;
            int buoc = Math.Max(90, ClientSize.Height / 5);
            DatMucTieu(_mucTieu + (delta < 0 ? buoc : -buoc), true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SapXep();
        }

        private void SapXep()
        {
            int viewport = Math.Max(160, ClientSize.Height);
            int w = Math.Max(300, ClientSize.Width - Kenh * 2);
            DatKichThuoc(w);
            if (ChieuCao() > viewport)
            {
                int het = Math.Max(300, w - KheLuot);
                if (het < w)
                {
                    w = het;
                    DatKichThuoc(w);
                }
            }

            _rongThe = w;
            _contentH = Math.Max(ChieuCao(), viewport);
            _maxScroll = Math.Max(0, ChieuCao() - viewport);
            TaiAnhLop();
            if (_scrollY > _maxScroll) _scrollY = _maxScroll;
            if (_mucTieu > _maxScroll) _mucTieu = _maxScroll;
            Invalidate();
        }

        private void DatKichThuoc(int w)
        {
            int trong = w - Kenh * 2 - Dem * 2;
            if (trong > MaxRong) trong = MaxRong;
            if (trong < 240) trong = 240;
            _rongAnh = trong;
            _caoAnh = (int)Math.Round(trong * TicketRenderer.DesignH / TicketRenderer.DesignW);
        }

        private int ChieuCao()
        {
            int n = _anhGoc.Count;
            if (n == 0) return Kenh * 2 + 260;
            return Kenh * 2 + Dem * 2 + n * _caoAnh + (n - 1) * Khe;
        }

        private void TaiAnhLop()
        {
            if (_rongAnh <= 0) return;
            if (_rongAnh == _rongAnhLuu && _anhLop.Count == _anhGoc.Count) return;

            if (_anhLop.Count != _anhGoc.Count)
            {
                if (_rongAnh == _rongAnhLuu)
                {
                    for (int i = _anhLop.Count; i < _anhGoc.Count; i++)
                        _anhLop.Add(PhongAnh(_anhGoc[i], _rongAnh, _caoAnh));
                    return;
                }
                TaiLopAnh();
                return;
            }
            if (Math.Abs(_rongAnh - _rongAnhLuu) < 10) return;

            _henLop.Stop();
            _henLop.Start();
        }

        private void TaiLop(object sender, EventArgs e)
        {
            TaiLopAnh();
        }

        private void TaiLopAnh()
        {
            _henLop.Stop();
            _rongAnhLuu = _rongAnh;

            foreach (Bitmap cu in _anhLop) if (cu != null) cu.Dispose();
            _anhLop.Clear();

            for (int i = 0; i < _anhGoc.Count; i++)
                _anhLop.Add(PhongAnh(_anhGoc[i], _rongAnh, _caoAnh));

            Invalidate();
        }

        private static Bitmap PhongAnh(Bitmap goc, int w, int h)
        {
            Bitmap kq = new Bitmap(w, h, PixelFormat.Format32bppPArgb);
            using (Graphics g = Graphics.FromImage(kq))
            using (ImageAttributes attr = new ImageAttributes())
            {
                attr.SetWrapMode(WrapMode.TileFlipXY);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(goc, new Rectangle(0, 0, w, h), 0, 0, goc.Width, goc.Height,
                    GraphicsUnit.Pixel, attr);
            }
            return kq;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int viewport = ClientSize.Height;
            g.Clear(MatNen);

            float x = Kenh;
            float y = Kenh - _scrollY;
            float w = _rongThe - 1f;
            float h = _contentH - Kenh * 2f;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = TaoPathBoGoc(new RectangleF(x, y, w, h), 18f))
            {
                using (SolidBrush b = new SolidBrush(MauThe)) g.FillPath(b, path);
                using (Pen p = new Pen(MauVien)) g.DrawPath(p, path);
            }

            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            for (int i = 0; i < _anhLop.Count; i++)
            {
                Bitmap anh = _anhLop[i];
                int vy = Kenh + Dem + i * (_caoAnh + Khe) - _scrollY;
                if (vy + _caoAnh < 0 || vy > viewport) continue;

                int vx = Kenh + (_rongThe - _rongAnh) / 2;
                if (anh.Width == _rongAnh && anh.Height == _caoAnh)
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImageUnscaled(anh, vx, vy);
                }
                else
                {
                    g.InterpolationMode = InterpolationMode.Bilinear;
                    g.DrawImage(anh, new Rectangle(vx, vy, _rongAnh, _caoAnh));
                }
            }

            if (_anhGoc.Count == 0) VeTrangThaiRong(g, viewport);
            VeThanhLuot(g, viewport);
        }

        private void VeTrangThaiRong(Graphics g, int viewport)
        {
            if (string.IsNullOrEmpty(_thongBao)) return;

            Size kt = DoChu(_thongBao, FntRong, g);
            int iconR = 132;
            int iconC = 78;
            int caoThe = Math.Max(_contentH, viewport) - Kenh * 2;
            int y = (int)(Kenh - _scrollY + (caoThe - (iconC + 22 + kt.Height)) / 2f);
            int san = Kenh + 24 - _scrollY;
            if (y < san) y = san;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            VeBieuTuongVe(g, new RectangleF(Kenh + (_rongThe - iconR) / 2f, y, iconR, iconC),
                Color.FromArgb(186, 210, 246), MauThe, 5f);
            g.SmoothingMode = SmoothingMode.None;

            TextRenderer.DrawText(g, _thongBao, FntRong,
                new Point(Kenh + (_rongThe - kt.Width) / 2, y + iconC + 22),
                Color.FromArgb(148, 163, 184), TextFormatFlags.NoPrefix);
        }

        private void VeThanhLuot(Graphics g, int viewport)
        {
            if (_maxScroll <= 0)
            {
                _nut = Rectangle.Empty;
                return;
            }

            int x = ClientSize.Width - KheLuot + 4;
            int yT = Kenh;
            int caoTrack = viewport - Kenh * 2;
            if (caoTrack < 40)
            {
                _nut = Rectangle.Empty;
                return;
            }

            int caoNut = Math.Max(48, (int)((long)caoTrack * viewport / Math.Max(1, _contentH)));
            if (caoNut > caoTrack) caoNut = caoTrack;
            int yNut = yT + (int)((long)(caoTrack - caoNut) * _scrollY / _maxScroll);
            _nut = new Rectangle(x, yNut, 6, caoNut);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath nen = TaoPathBoGoc(new RectangleF(x, yT, 6, caoTrack), 3f))
            using (SolidBrush bNen = new SolidBrush(Color.FromArgb(236, 239, 246)))
                g.FillPath(bNen, nen);

            using (GraphicsPath duong = TaoPathBoGoc(
                new RectangleF(_nut.X, _nut.Y, _nut.Width, _nut.Height), 3f))
            using (SolidBrush bNut = new SolidBrush(
                _keo || _gac ? Color.FromArgb(148, 163, 184) : Color.FromArgb(191, 203, 218)))
                g.FillPath(bNut, duong);

            g.SmoothingMode = SmoothingMode.None;
        }

        private Rectangle VungNut()
        {
            if (_nut.IsEmpty) return Rectangle.Empty;
            return new Rectangle(_nut.X - 5, _nut.Y - 3, _nut.Width + 10, _nut.Height + 6);
        }

        private void DatMucTieu(int v, bool tuTu)
        {
            v = Math.Max(0, Math.Min(_maxScroll, v));
            _mucTieu = v;

            if (!tuTu)
            {
                DungDong();
                if (v == _scrollY) return;
                _scrollY = v;
                Invalidate();
                return;
            }

            if (v == _scrollY)
            {
                DungDong();
                return;
            }
            if (!_dongHo.Enabled) _dongHo.Start();
        }

        private void DungDong()
        {
            if (_dongHo.Enabled) _dongHo.Stop();
        }

        private void ChayBuoc(object sender, EventArgs e)
        {
            int con = _mucTieu - _scrollY;
            if (con == 0)
            {
                DungDong();
                return;
            }

            int dai = Math.Abs(con);
            int buoc = (int)Math.Ceiling(dai * 0.34);
            if (buoc < 2) buoc = 2;
            if (buoc > dai) buoc = dai;

            _scrollY += con > 0 ? buoc : -buoc;
            Invalidate();

            if (_scrollY == _mucTieu) DungDong();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_maxScroll <= 0) return;

            if (VungNut().Contains(e.Location))
            {
                DungDong();
                _keo = true;
                _keoY = e.Y;
                _keoScroll = _scrollY;
                Capture = true;
                Invalidate();
                return;
            }

            int viewport = ClientSize.Height;
            if (e.Y < _nut.Top) DatMucTieu(_mucTieu - viewport + 60, true);
            else if (e.Y > _nut.Bottom) DatMucTieu(_mucTieu + viewport - 60, true);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_keo)
            {
                int caoTrack = Math.Max(1, ClientSize.Height - Kenh * 2);
                int vuong = Math.Max(1, caoTrack - _nut.Height);
                DatMucTieu(_keoScroll + (int)((long)(e.Y - _keoY) * _maxScroll / vuong), false);
                return;
            }

            bool gac = VungNut().Contains(e.Location);
            if (gac != _gac)
            {
                _gac = gac;
                Cursor = gac ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!_keo) return;
            _keo = false;
            Capture = false;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_gac) return;
            _gac = false;
            Cursor = Cursors.Default;
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            XoayBanh(e.Delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dongHo.Stop();
                _dongHo.Tick -= ChayBuoc;
                _dongHo.Dispose();

                _henLop.Stop();
                _henLop.Tick -= TaiLop;
                _henLop.Dispose();

                foreach (Bitmap anh in _anhLop) if (anh != null) anh.Dispose();
                _anhLop.Clear();
                _anhGoc = new List<Bitmap>();
            }
            base.Dispose(disposing);
        }
    }

    internal sealed class LocBanhXe : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;
        private readonly SurfaceXemTruoc _surface;

        public LocBanhXe(SurfaceXemTruoc surface)
        {
            _surface = surface;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL || _surface == null || !_surface.IsHandleCreated || !_surface.Visible)
                return false;

            Form form = _surface.FindForm();
            if (form == null || Form.ActiveForm != form) return false;

            Point goc = _surface.PointToClient(Cursor.Position);
            if (!_surface.ClientRectangle.Contains(goc)) return false;

            _surface.XoayBanh((short)((m.WParam.ToInt64() >> 16) & 0xFFFF));
            return true;
        }
    }
}

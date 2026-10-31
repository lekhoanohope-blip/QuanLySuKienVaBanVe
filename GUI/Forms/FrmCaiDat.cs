using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public class FrmCaiDat : Form
    {
        private const int W = 460, H = 430, FIELD_X = 40, FIELD_W = 380, TOP = 24;

        private Bitmap bg;
        private CardPanel card;
        private Label lblTitle, lblMsg;
        private RoundedTextBox txtOld, txtNew, txtConfirm;
        private FxButton btnChange;

        public FrmCaiDat()
        {
            Text = Lang.T("m_settings");
            Font = new Font("Segoe UI", 9f);
            DoubleBuffered = true;
            AutoScroll = true;
            KeyPreview = true;

            BuildCard();
            ApplyLanguage();
            LayoutCard();
            txtOld.FocusInput();
        }

        private Label MakeLabel(float size, FontStyle style, Color color, ContentAlignment align)
        {
            return new Label
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                ForeColor = color,
                Font = new Font("Segoe UI", size, style),
                TextAlign = align
            };
        }

        private void BuildCard()
        {
            card = new CardPanel { Size = new Size(W + 2 * CardPanel.Pad, H + 2 * CardPanel.Pad) };
            Controls.Add(card);
            Panel layer = new Panel
            {
                BackColor = Color.White,
                Location = new Point(CardPanel.Pad, CardPanel.Pad),
                Size = new Size(W, H)
            };
            card.Controls.Add(layer);

            lblTitle = MakeLabel(22f, FontStyle.Bold, Fx.Ink, ContentAlignment.MiddleCenter);
            lblTitle.SetBounds(0, 36, W, 52);

            txtOld = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 106) };
            txtNew = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 174) };
            txtConfirm = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 242) };

            lblMsg = MakeLabel(9.5f, FontStyle.Regular, Fx.Danger, ContentAlignment.MiddleLeft);
            lblMsg.SetBounds(FIELD_X + 4, 302, FIELD_W - 8, 34);

            btnChange = new FxButton { Location = new Point(FIELD_X - 4, 350), Enabled = false };
            btnChange.Click += delegate { DoChangePassword(); };

            EventHandler changed = delegate { lblMsg.Text = ""; UpdateButton(); };
            txtOld.ValueChanged += changed;
            txtNew.ValueChanged += changed;
            txtConfirm.ValueChanged += changed;

            layer.Controls.AddRange(new Control[] { lblTitle, txtOld, txtNew, txtConfirm, lblMsg, btnChange });
        }

        private void ApplyLanguage()
        {
            lblTitle.Text = Lang.T("cp_title");
            txtOld.Cue = Lang.T("ph_old");
            txtNew.Cue = Lang.T("ph_new");
            txtConfirm.Cue = Lang.T("ph_new_confirm");
            btnChange.Text = Lang.T("btn_changepw");
        }

        private void LayoutCard()
        {
            if (card == null) return;
            int x = Math.Max(0, (ClientSize.Width - card.Width) / 2);
            int y = Math.Max(TOP, TOP + (ClientSize.Height - TOP - card.Height) / 2);
            card.Location = new Point(x, y);
            AutoScrollMinSize = new Size(0, TOP + card.Height + 12);
        }

        private void UpdateButton()
        {
            btnChange.Enabled = txtOld.Value.Length > 0 && txtNew.Value.Length > 0 && txtConfirm.Value.Length > 0;
        }

        private void ShowMsg(string text, bool ok)
        {
            lblMsg.ForeColor = ok ? Fx.Success : Fx.Danger;
            lblMsg.Text = text;
        }

        private void DoChangePassword()
        {
            if (!btnChange.Enabled) return;

            KetQuaDoiMatKhau kq = TaiKhoanBLL.DoiMatKhau(txtOld.Value, txtNew.Value, txtConfirm.Value);
            if (kq == KetQuaDoiMatKhau.ThanhCong)
            {
                txtOld.Value = ""; txtNew.Value = ""; txtConfirm.Value = "";
                ShowMsg(Lang.T("cp_ok"), true);
                return;
            }

            if (kq == KetQuaDoiMatKhau.SaiMatKhauCu || kq == KetQuaDoiMatKhau.TamKhoa) txtOld.Value = "";

            txtOld.HasError = kq == KetQuaDoiMatKhau.SaiMatKhauCu || kq == KetQuaDoiMatKhau.TamKhoa;
            txtNew.HasError = kq == KetQuaDoiMatKhau.MatKhauMoiNgan || kq == KetQuaDoiMatKhau.TrungMatKhauCu;
            txtConfirm.HasError = kq == KetQuaDoiMatKhau.XacNhanKhongKhop;

            switch (kq)
            {
                case KetQuaDoiMatKhau.SaiMatKhauCu: ShowMsg(Lang.T("err_old_wrong"), false); break;
                case KetQuaDoiMatKhau.MatKhauMoiNgan: ShowMsg(Lang.T("err_pass_short"), false); break;
                case KetQuaDoiMatKhau.XacNhanKhongKhop: ShowMsg(Lang.T("err_confirm"), false); break;
                case KetQuaDoiMatKhau.TrungMatKhauCu: ShowMsg(Lang.T("err_new_same"), false); break;
                case KetQuaDoiMatKhau.TamKhoa: ShowMsg(Lang.T("err_temp", TaiKhoanBLL.GiayKhoaConLai), false); break;
                case KetQuaDoiMatKhau.ChuaDangNhap: ShowMsg(Lang.T("err_not_login"), false); break;
                default: ShowMsg(Lang.T("err_sys"), false); break;
            }
        }

        private void RebuildBackground()
        {
            if (bg != null) { bg.Dispose(); bg = null; }
            bg = Fx.CreateBackground(ClientSize.Width, ClientSize.Height);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (bg == null) RebuildBackground();
            if (bg != null) e.Graphics.DrawImageUnscaled(bg, 0, 0); else base.OnPaintBackground(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RebuildBackground();
            LayoutCard();
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
                DoChangePassword();
                return;
            }
            base.OnKeyDown(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (bg != null) bg.Dispose();
            base.OnFormClosed(e);
        }
    }
}

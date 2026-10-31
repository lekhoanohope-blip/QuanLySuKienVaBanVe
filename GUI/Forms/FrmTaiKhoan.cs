using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public class FrmTaiKhoan : Form
    {
        private const int W = 460, H = 430, FIELD_X = 40, FIELD_W = 380;

        private readonly TaiKhoanDTO editing;
        private Bitmap bg;
        private CardPanel card;
        private Label lblTitle, lblMsg;
        private RoundedTextBox txtUser, txtPass, txtConfirm;
        private FxButton btnSubmit;

        public string TenDaLuu { get; private set; }

        public FrmTaiKhoan() : this(null) { }

        public FrmTaiKhoan(TaiKhoanDTO taiKhoan)
        {
            editing = taiKhoan;
            Font = new Font("Segoe UI", 9f);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;
            DoubleBuffered = true;
            ClientSize = new Size(W + 2 * CardPanel.Pad, H + 2 * CardPanel.Pad);

            BuildCard();
            ApplyMode();
            UpdateButton();
        }

        private bool IsEdit { get { return editing != null; } }

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
            card = new CardPanel { Size = ClientSize, Location = Point.Empty };
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

            txtUser = new RoundedTextBox(FxIcon.User, false) { Location = new Point(FIELD_X, 106) };
            txtPass = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 174) };
            txtConfirm = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 242) };

            lblMsg = MakeLabel(9.5f, FontStyle.Regular, Fx.Danger, ContentAlignment.MiddleLeft);
            lblMsg.SetBounds(FIELD_X + 4, 302, FIELD_W - 8, 34);

            btnSubmit = new FxButton { Location = new Point(FIELD_X - 4, 350), Enabled = false };
            btnSubmit.Click += delegate { Submit(); };

            EventHandler changed = delegate { lblMsg.Text = ""; UpdateButton(); };
            txtUser.ValueChanged += changed;
            txtPass.ValueChanged += changed;
            txtConfirm.ValueChanged += changed;

            layer.Controls.AddRange(new Control[] { lblTitle, txtUser, txtPass, txtConfirm, lblMsg, btnSubmit });
        }

        private void ApplyMode()
        {
            if (IsEdit)
            {
                Text = Lang.T("emp_edit_title");
                lblTitle.Text = Lang.T("emp_edit_title");
                txtUser.Value = editing.TenDangNhap;
                txtUser.Enabled = false;
                txtPass.Cue = Lang.T("ph_new");
                txtConfirm.Cue = Lang.T("ph_new_confirm");
                btnSubmit.Text = Lang.T("btn_save");
            }
            else
            {
                Text = Lang.T("emp_add_title");
                lblTitle.Text = Lang.T("reg_title");
                txtUser.Cue = Lang.T("ph_user");
                txtPass.Cue = Lang.T("ph_pass");
                txtConfirm.Cue = Lang.T("ph_confirm");
                btnSubmit.Text = Lang.T("btn_register");
            }
        }

        private void UpdateButton()
        {
            bool passwords = txtPass.Value.Length > 0 && txtConfirm.Value.Length > 0;
            btnSubmit.Enabled = IsEdit ? passwords : passwords && txtUser.Value.Trim().Length > 0;
        }

        private void ShowError(string text)
        {
            lblMsg.ForeColor = Fx.Danger;
            lblMsg.Text = text;
        }

        private void Submit()
        {
            if (!btnSubmit.Enabled) return;
            if (IsEdit) SubmitEdit(); else SubmitCreate();
        }

        private void SubmitCreate()
        {
            KetQuaDangKy kq = TaiKhoanBLL.DangKy(txtUser.Value, txtPass.Value, txtConfirm.Value);
            if (kq == KetQuaDangKy.ThanhCong)
            {
                TenDaLuu = txtUser.Value.Trim();
                DialogResult = DialogResult.OK;
                return;
            }

            txtUser.HasError = kq == KetQuaDangKy.TenKhongHopLe || kq == KetQuaDangKy.TrungTen;
            txtPass.HasError = kq == KetQuaDangKy.MatKhauNgan;
            txtConfirm.HasError = kq == KetQuaDangKy.XacNhanKhongKhop;

            switch (kq)
            {
                case KetQuaDangKy.TenKhongHopLe: ShowError(Lang.T("err_user_fmt")); break;
                case KetQuaDangKy.MatKhauNgan: ShowError(Lang.T("err_pass_short")); break;
                case KetQuaDangKy.XacNhanKhongKhop: ShowError(Lang.T("err_confirm")); break;
                case KetQuaDangKy.TrungTen: ShowError(Lang.T("err_exists")); break;
                case KetQuaDangKy.KhongCoQuyen: ShowError(Lang.T("err_no_perm")); break;
                default: ShowError(Lang.T("err_sys")); break;
            }
        }

        private void SubmitEdit()
        {
            KetQuaTaiKhoan kq = TaiKhoanBLL.DatLaiMatKhau(editing.MaTK, txtPass.Value, txtConfirm.Value);
            if (kq == KetQuaTaiKhoan.ThanhCong)
            {
                TenDaLuu = editing.TenDangNhap;
                DialogResult = DialogResult.OK;
                return;
            }

            txtPass.HasError = kq == KetQuaTaiKhoan.MatKhauNgan;
            txtConfirm.HasError = kq == KetQuaTaiKhoan.XacNhanKhongKhop;

            switch (kq)
            {
                case KetQuaTaiKhoan.MatKhauNgan: ShowError(Lang.T("err_pass_short")); break;
                case KetQuaTaiKhoan.XacNhanKhongKhop: ShowError(Lang.T("err_confirm")); break;
                case KetQuaTaiKhoan.KhongCoQuyen: ShowError(Lang.T("err_no_perm")); break;
                default: ShowError(Lang.T("err_sys")); break;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (bg == null) bg = Fx.CreateBackground(ClientSize.Width, ClientSize.Height);
            if (bg != null) e.Graphics.DrawImageUnscaled(bg, 0, 0); else base.OnPaintBackground(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (IsEdit) txtPass.FocusInput(); else txtUser.FocusInput();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
                Submit();
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = e.SuppressKeyPress = true;
                DialogResult = DialogResult.Cancel;
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

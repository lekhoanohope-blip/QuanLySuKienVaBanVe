using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;   // tránh mơ hồ với System.Threading.Timer khi project bật ImplicitUsings
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    /// <summary>
    /// Màn hình Đăng nhập + Đăng ký (cùng 1 form, chuyển qua lại bằng link).
    /// Giao diện được dựng hoàn toàn bằng code (không dùng Designer) để cả nhóm copy là chạy.
    /// Đăng nhập thành công: DialogResult = OK và PhienDangNhap đã có thông tin người dùng.
    /// </summary>
    public class FrmDangNhap : Form
    {
        // Kích thước vùng nội dung của thẻ (chưa tính phần đệm bóng đổ)
        private const int W = 460, H = 490, FIELD_X = 40, FIELD_W = 380;

        private enum Mode { Login, Register }
        private enum Anim { None, Drop, Shake }

        private Mode mode = Mode.Login;

        // --- khung ---
        private CardPanel card;
        private Panel layerLogin, layerReg;
        private LangChip chip;
        private ContextMenuStrip langMenu;
        private ToolTip tip = new ToolTip();

        // --- đăng nhập ---
        private Label lblLoginTitle, lblLoginMsg, lblNoAcc;
        private RoundedTextBox txtUser, txtPass;
        private FxCheck chkRemember;
        private FxLink lnkForgot, lnkToReg;
        private FxButton btnLogin;

        // --- đăng ký ---
        private Label lblRegTitle, lblRegMsg, lblHaveAcc;
        private RoundedTextBox txtRegUser, txtRegPass, txtRegConfirm;
        private FxCheck chkAgree;
        private FxLink lnkTerms, lnkToLogin;
        private FxButton btnRegister;

        // --- nền + animation ---
        private Bitmap bg;
        private readonly Timer animTimer = new Timer { Interval = 15 };
        private readonly Stopwatch sw = new Stopwatch();
        private Anim anim = Anim.None;
        private int dropHeight = 160, baseX, baseY;

        public FrmDangNhap()
        {
            Text = Lang.T("app_title");
            Font = new Font("Segoe UI", 9f);
            ClientSize = new Size(1000, 700);
            MinimumSize = new Size(660, 740);
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;
            DoubleBuffered = true;

            BuildCard();
            BuildLanguageChip();
            animTimer.Tick += delegate { AnimTick(); };
            Lang.Changed += OnLangChanged;

            ApplyLanguage();
            ShowMode(Mode.Login, false);
            LoadRemembered();
        }

        // Giảm nhấp nháy khi control trong suốt di chuyển (WS_EX_COMPOSITED)
        protected override CreateParams CreateParams
        {
            get { CreateParams cp = base.CreateParams; cp.ExStyle |= 0x02000000; return cp; }
        }

        // =================================================================
        //  1. DỰNG GIAO DIỆN
        // =================================================================
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

        private Panel MakeLayer()
        {
            Panel p = new Panel { BackColor = Color.Transparent, Location = new Point(CardPanel.Pad, CardPanel.Pad), Size = new Size(W, H) };
            card.Controls.Add(p);
            return p;
        }

        private void BuildCard()
        {
            card = new CardPanel { Size = new Size(W + 2 * CardPanel.Pad, H + 2 * CardPanel.Pad) };
            Controls.Add(card);
            layerLogin = MakeLayer();
            layerReg = MakeLayer();
            BuildLoginLayer();
            BuildRegisterLayer();
        }

        private void BuildLoginLayer()
        {
            lblLoginTitle = MakeLabel(22f, FontStyle.Bold, Fx.Ink, ContentAlignment.MiddleCenter);
            lblLoginTitle.SetBounds(0, 36, W, 52);

            txtUser = new RoundedTextBox(FxIcon.User, false) { Location = new Point(FIELD_X, 116) };
            txtPass = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 186) };

            lblLoginMsg = MakeLabel(9.5f, FontStyle.Regular, Fx.Danger, ContentAlignment.MiddleLeft);
            lblLoginMsg.SetBounds(FIELD_X + 4, 248, FIELD_W - 8, 38);

            chkRemember = new FxCheck { Location = new Point(FIELD_X, 294) };
            lnkForgot = new FxLink { Top = 296 };
            lnkForgot.Click += delegate { ShowMsg(lblLoginMsg, Lang.T("forgot_msg"), true); };

            btnLogin = new FxButton { Location = new Point(FIELD_X - 4, 336), Enabled = false };
            btnLogin.Click += delegate { DoLogin(); };

            lblNoAcc = MakeLabel(10f, FontStyle.Regular, Fx.Ink, ContentAlignment.MiddleLeft);
            lnkToReg = new FxLink();
            lnkToReg.Click += delegate { ShowMode(Mode.Register, true); };

            txtUser.ValueChanged += delegate { lblLoginMsg.Text = ""; UpdateButtons(); };
            txtPass.ValueChanged += delegate { lblLoginMsg.Text = ""; UpdateButtons(); };

            layerLogin.Controls.AddRange(new Control[]
            {
                lblLoginTitle, txtUser, txtPass, lblLoginMsg, chkRemember, lnkForgot, btnLogin, lblNoAcc, lnkToReg
            });
        }

        private void BuildRegisterLayer()
        {
            lblRegTitle = MakeLabel(22f, FontStyle.Bold, Fx.Ink, ContentAlignment.MiddleCenter);
            lblRegTitle.SetBounds(0, 36, W, 52);

            txtRegUser = new RoundedTextBox(FxIcon.User, false) { Location = new Point(FIELD_X, 106) };
            txtRegPass = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 174) };
            txtRegConfirm = new RoundedTextBox(FxIcon.Lock, true) { Location = new Point(FIELD_X, 242) };

            lblRegMsg = MakeLabel(9.5f, FontStyle.Regular, Fx.Danger, ContentAlignment.MiddleLeft);
            lblRegMsg.SetBounds(FIELD_X + 4, 302, FIELD_W - 8, 34);

            chkAgree = new FxCheck { Location = new Point(FIELD_X, 342) };
            lnkTerms = new FxLink { Top = 344 };
            lnkTerms.Click += delegate { MessageBox.Show(Lang.T("terms_text"), Lang.T("terms"), MessageBoxButtons.OK, MessageBoxIcon.Information); };

            btnRegister = new FxButton { Location = new Point(FIELD_X - 4, 380), Enabled = false };
            btnRegister.Click += delegate { DoRegister(); };

            lblHaveAcc = MakeLabel(10f, FontStyle.Regular, Fx.Ink, ContentAlignment.MiddleLeft);
            lnkToLogin = new FxLink();
            lnkToLogin.Click += delegate { ShowMode(Mode.Login, true); };

            EventHandler changed = delegate { lblRegMsg.Text = ""; UpdateButtons(); };
            txtRegUser.ValueChanged += changed;
            txtRegPass.ValueChanged += changed;
            txtRegConfirm.ValueChanged += changed;
            chkAgree.CheckedChanged += changed;

            layerReg.Controls.AddRange(new Control[]
            {
                lblRegTitle, txtRegUser, txtRegPass, txtRegConfirm, lblRegMsg,
                chkAgree, lnkTerms, btnRegister, lblHaveAcc, lnkToLogin
            });
        }

        private void BuildLanguageChip()
        {
            chip = new LangChip { Anchor = AnchorStyles.Top | AnchorStyles.Right };
            chip.Location = new Point(ClientSize.Width - chip.Width - 24, 18);
            Controls.Add(chip);

            langMenu = new ContextMenuStrip { Font = new Font("Segoe UI", 10.5f), ShowImageMargin = false };
            ToolStripMenuItem vi = new ToolStripMenuItem(Lang.T("lang_vi")) { Tag = Ngon.VN };
            ToolStripMenuItem en = new ToolStripMenuItem(Lang.T("lang_en")) { Tag = Ngon.EN };
            vi.Click += delegate { Lang.Set(Ngon.VN); };
            en.Click += delegate { Lang.Set(Ngon.EN); };
            langMenu.Items.Add(vi);
            langMenu.Items.Add(en);
            langMenu.Opening += delegate
            {
                foreach (ToolStripMenuItem it in langMenu.Items) it.Checked = ((Ngon)it.Tag) == Lang.Hien;
            };
            chip.Click += delegate { langMenu.Show(chip, new Point(0, chip.Height + 2)); };
            chip.BringToFront();
        }

        // =================================================================
        //  2. ĐA NGÔN NGỮ
        // =================================================================
        private void OnLangChanged(object sender, EventArgs e)
        {
            ApplyLanguage();
            chip.Invalidate();
        }

        private void ApplyLanguage()
        {
            Text = Lang.T("app_title");

            lblLoginTitle.Text = Lang.T("login_title");
            txtUser.Cue = Lang.T("ph_user");
            txtPass.Cue = Lang.T("ph_pass");
            chkRemember.Text = Lang.T("remember");
            lnkForgot.Text = Lang.T("forgot");
            lnkForgot.Left = FIELD_X + FIELD_W - lnkForgot.Width;
            btnLogin.Text = Lang.T("btn_login");
            lblNoAcc.Text = Lang.T("no_acc");
            lnkToReg.Text = Lang.T("reg_now");
            CenterPair(lblNoAcc, lnkToReg, 428);

            lblRegTitle.Text = Lang.T("reg_title");
            txtRegUser.Cue = Lang.T("ph_user");
            txtRegPass.Cue = Lang.T("ph_pass");
            txtRegConfirm.Cue = Lang.T("ph_confirm");
            chkAgree.Text = Lang.T("agree");
            lnkTerms.Text = Lang.T("terms");
            lnkTerms.Left = chkAgree.Right + 1;
            btnRegister.Text = Lang.T("btn_register");
            lblHaveAcc.Text = Lang.T("have_acc");
            lnkToLogin.Text = Lang.T("login_link");
            CenterPair(lblHaveAcc, lnkToLogin, 454);

            lblLoginMsg.Text = "";
            lblRegMsg.Text = "";
        }

        /// <summary>Đặt 1 nhãn + 1 link nằm cạnh nhau và canh giữa thẻ.</summary>
        private void CenterPair(Label lbl, FxLink lnk, int y)
        {
            Size sz = TextRenderer.MeasureText(lbl.Text, lbl.Font);
            int total = sz.Width + 6 + lnk.Width;
            int x = (W - total) / 2;
            lbl.SetBounds(x, y, sz.Width + 2, 26);
            lnk.Location = new Point(x + sz.Width + 6, y + (26 - lnk.Height) / 2 + 1);
        }

        // =================================================================
        //  3. XỬ LÝ NGHIỆP VỤ (chỉ gọi BLL, không đụng DAL/SQL)
        // =================================================================
        private void UpdateButtons()
        {
            btnLogin.Enabled = txtUser.Value.Trim().Length > 0 && txtPass.Value.Length > 0;
            btnRegister.Enabled = txtRegUser.Value.Trim().Length > 0 && txtRegPass.Value.Length > 0 &&
                                  txtRegConfirm.Value.Length > 0 && chkAgree.Checked;
        }

        private void ShowMsg(Label lbl, string text, bool ok)
        {
            lbl.ForeColor = ok ? Fx.Success : Fx.Danger;
            lbl.Text = text;
        }

        private void DoLogin()
        {
            if (!btnLogin.Enabled || mode != Mode.Login) return;

            TaiKhoanDTO tk;
            KetQuaDangNhap kq = TaiKhoanBLL.DangNhap(txtUser.Value, txtPass.Value, out tk);

            // Xóa ô mật khẩu TRƯỚC khi hiện lỗi (vì gõ/xóa chữ sẽ tự xóa thông báo lỗi và viền đỏ)
            if (kq != KetQuaDangNhap.ThanhCong) txtPass.Value = "";

            switch (kq)
            {
                case KetQuaDangNhap.ThanhCong:
                    SaveRemembered();
                    PhienDangNhap.BatDau(tk);
                    DialogResult = DialogResult.OK;     // ShowDialog() trả về OK và form tự đóng
                    return;
                case KetQuaDangNhap.ThieuThongTin: ShowMsg(lblLoginMsg, Lang.T("err_empty"), false); break;
                case KetQuaDangNhap.SaiThongTin: ShowMsg(lblLoginMsg, Lang.T("err_wrong"), false); break;
                case KetQuaDangNhap.TaiKhoanBiKhoa: ShowMsg(lblLoginMsg, Lang.T("err_locked"), false); break;
                case KetQuaDangNhap.TamKhoa: ShowMsg(lblLoginMsg, Lang.T("err_temp", TaiKhoanBLL.GiayKhoaConLai), false); break;
                default:
                    ShowMsg(lblLoginMsg, Lang.T("err_sys"), false);
                    // Khong lo thong tin loi noi bo he thong ra UI
                    break;
            }
            txtPass.HasError = true;
            txtPass.FocusInput();
        }

        private void DoRegister()
        {
            if (!btnRegister.Enabled || mode != Mode.Register) return;

            KetQuaDangKy kq = TaiKhoanBLL.DangKy(txtRegUser.Value, txtRegPass.Value, txtRegConfirm.Value);
            if (kq == KetQuaDangKy.ThanhCong)
            {
                string ten = txtRegUser.Value.Trim();
                txtRegUser.Value = ""; txtRegPass.Value = ""; txtRegConfirm.Value = ""; chkAgree.Checked = false;
                ShowMode(Mode.Login, true);
                txtUser.Value = ten;
                txtPass.Value = "";
                ShowMsg(lblLoginMsg, Lang.T("reg_ok"), true);
                txtPass.FocusInput();
                return;
            }

            txtRegUser.HasError = kq == KetQuaDangKy.TenKhongHopLe || kq == KetQuaDangKy.TrungTen;
            txtRegPass.HasError = kq == KetQuaDangKy.MatKhauNgan;
            txtRegConfirm.HasError = kq == KetQuaDangKy.XacNhanKhongKhop;

            switch (kq)
            {
                case KetQuaDangKy.TenKhongHopLe: ShowMsg(lblRegMsg, Lang.T("err_user_fmt"), false); break;
                case KetQuaDangKy.MatKhauNgan: ShowMsg(lblRegMsg, Lang.T("err_pass_short"), false); break;
                case KetQuaDangKy.XacNhanKhongKhop: ShowMsg(lblRegMsg, Lang.T("err_confirm"), false); break;
                case KetQuaDangKy.TrungTen: ShowMsg(lblRegMsg, Lang.T("err_exists"), false); break;
                default:
                    ShowMsg(lblRegMsg, Lang.T("err_sys"), false);
                    // Khong lo thong tin loi noi bo he thong ra UI
                    break;
            }
        }

        private void ShowMode(Mode m, bool animate)
        {
            mode = m;
            layerLogin.Visible = m == Mode.Login;
            layerReg.Visible = m == Mode.Register;
            lblLoginMsg.Text = ""; lblRegMsg.Text = "";
            txtPass.HasError = false;
            UpdateButtons();
            CenterCard();
            if (m == Mode.Login) txtUser.FocusInput(); else txtRegUser.FocusInput();
        }

        // Ghi nhớ tên đăng nhập (KHÔNG lưu mật khẩu - lưu mật khẩu vào file là lỗi bảo mật)
        private static string RememberFile
        {
            get
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QuanLySuKienVaBanVe");
                return Path.Combine(dir, "remember.txt");
            }
        }

        private void LoadRemembered()
        {
            try
            {
                if (File.Exists(RememberFile))
                {
                    string name = File.ReadAllText(RememberFile).Trim();
                    if (name.Length > 0) { txtUser.Value = name; chkRemember.Checked = true; txtPass.FocusInput(); }
                }
            }
            catch { /* không đọc được thì bỏ qua, không ảnh hưởng đăng nhập */ }
        }

        private void SaveRemembered()
        {
            try
            {
                if (chkRemember.Checked)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RememberFile));
                    File.WriteAllText(RememberFile, txtUser.Value.Trim());
                }
                else if (File.Exists(RememberFile)) File.Delete(RememberFile);
            }
            catch { }
        }

        // =================================================================
        //  4. NỀN GRADIENT + ANIMATION THẺ
        // =================================================================
        private void BuildBackground()
        {
            if (ClientSize.Width < 10 || ClientSize.Height < 10) return;
            if (bg != null) bg.Dispose();
            bg = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (Graphics g = Graphics.FromImage(bg))
            {
                Rectangle r = new Rectangle(0, 0, bg.Width, bg.Height);
                using (LinearGradientBrush br = new LinearGradientBrush(r, Color.White, Color.White, 20f))
                {
                    br.InterpolationColors = new ColorBlend
                    {
                        Positions = new[] { 0f, 0.3f, 0.65f, 1f },
                        Colors = new[]
                        {
                            Color.FromArgb(255, 255, 255), Color.FromArgb(255, 222, 242),
                            Color.FromArgb(255, 140, 203), Color.FromArgb(255, 20, 147)
                        }
                    };
                    g.FillRectangle(br, r);
                }
                // lưới ô vuông mờ như ảnh mẫu
                const int cell = 128;
                Random rnd = new Random(7);
                using (SolidBrush sq = new SolidBrush(Color.FromArgb(34, 255, 255, 255)))
                using (Pen line = new Pen(Color.FromArgb(46, 255, 255, 255), 1f))
                {
                    for (int x = 0; x < bg.Width; x += cell)
                        for (int y = 0; y < bg.Height; y += cell)
                            if (rnd.Next(5) == 0) g.FillRectangle(sq, x, y, cell, cell);
                    for (int x = 0; x < bg.Width; x += cell) g.DrawLine(line, x, 0, x, bg.Height);
                    for (int y = 0; y < bg.Height; y += cell) g.DrawLine(line, 0, y, bg.Width, y);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (bg == null) BuildBackground();
            if (bg != null) e.Graphics.DrawImageUnscaled(bg, 0, 0); else base.OnPaintBackground(e);
        }

        private void CenterCard()
        {
            baseX = (ClientSize.Width - card.Width) / 2;
            baseY = Math.Max(10, (ClientSize.Height - card.Height) / 2);
            card.Location = new Point(baseX, baseY);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (card == null) return;
            BuildBackground();
            CenterCard();
            Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BuildBackground();
            CenterCard();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Đã nhớ tên đăng nhập thì nhảy thẳng vào ô mật khẩu, chưa thì vào ô tên
            if (txtUser.Value.Length > 0) txtPass.FocusInput(); else txtUser.FocusInput();
        }

        private void StartAnim(Anim a, int amplitude = 0)
        {
            anim = a;
            if (amplitude > 0) dropHeight = amplitude;
            sw.Restart();
            animTimer.Start();
            AnimTick();
        }

        private void AnimTick()
        {
            float dur = anim == Anim.Drop ? 850f : 450f;
            float t = (float)(sw.ElapsedMilliseconds / dur);
            if (t >= 1f || anim == Anim.None)
            {
                animTimer.Stop();
                anim = Anim.None;
                card.Location = new Point(baseX, baseY);
                return;
            }
            if (anim == Anim.Drop)
                card.Location = new Point(baseX, baseY - (int)(dropHeight * (1f - Fx.EaseOutBounce(t))));   // BOUNCE
            else
                card.Location = new Point(baseX + (int)(16 * Math.Exp(-4 * t) * Math.Sin(t * 38)), baseY);   // lắc khi sai
        }

        // =================================================================
        //  5. BÀN PHÍM + DỌN DẸP
        // =================================================================
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
                if (mode == Mode.Login) DoLogin(); else DoRegister();
                return;
            }
            base.OnKeyDown(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Lang.Changed -= OnLangChanged;      // bắt buộc gỡ, nếu không form cũ vẫn bị giữ trong bộ nhớ
            animTimer.Dispose();
            if (bg != null) bg.Dispose();
            base.OnFormClosed(e);
        }
    }
}

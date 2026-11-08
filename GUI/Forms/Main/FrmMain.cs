using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmMain : Form
    {
        #region Fields & Properties
        public bool DangXuat { get; private set; }
        private Form currentFormChild;
        private bool adjustingWindow;
        private bool compactLogo;
        private Image brandLogo;
        private Image brandLogoSource;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void DragWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        #endregion

        #region Constructors & Initialization
        public FrmMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            resizeBox.BringToFront();
            if (btnDangXuat != null) btnDangXuat.Click += btnDangXuat_Click;
            if (btnCaiDat != null) btnCaiDat.Click += btnCaiDat_Click;
            ApplyPhanQuyen();
            TaiLogoBrand();

            Shown += FrmMain_Shown;
            SizeChanged += (s, e) => { LayoutMainHeader(); LayoutSidebar(); };
            LocationChanged += (s, e) =>
            {
                if (!adjustingWindow && WindowState == FormWindowState.Normal)
                {
                    Rectangle workingArea = Screen.FromControl(this).WorkingArea;
                    if (MaximizedBounds != workingArea)
                    {
                        MaximizedBounds = workingArea;
                        MaximumSize = workingArea.Size;
                    }
                }
            };
            DpiChanged += (s, e) =>
            {
                FitToWorkingArea();
                LayoutMainHeader();
                LayoutSidebar();
            };

            pnlHeader.MouseDown += DragWindow_MouseDown;
            pnlLogo.MouseDown += DragWindow_MouseDown;
            lblLogo.MouseDown += DragWindow_MouseDown;
            picLogo.MouseDown += DragWindow_MouseDown;
            lblSubtitle.MouseDown += DragWindow_MouseDown;

            pnlHeader.DoubleClick += (s, e) =>
            {
                this.WindowState = (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
            };

            controlBoxClose.Click += (s, e) => Application.Exit();

        }

        #endregion

        #region UI Setup & Custom Styling (Guna2)
        private int Px(int logicalPixels)
        {
            return (int)Math.Round(logicalPixels * DeviceDpi / 96f);
        }

        private void TaiLogoBrand()
        {
            if (lblLogo == null) return;
            if (brandLogoSource == null)
            {
                string duongDan = System.IO.Path.Combine(Application.StartupPath, @"Resources\Ticket\logoEventTicket.png");
                if (System.IO.File.Exists(duongDan))
                {
                    try
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(duongDan);
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                        using (Image tam = Image.FromStream(ms))
                            brandLogoSource = new Bitmap(tam);
                    }
                    catch { brandLogoSource = null; }
                }

                if (brandLogoSource == null)
                {
                    lblLogo.Image = null;
                    lblLogo.Text = "EventTicket";
                    return;
                }
            }

            int w = lblLogo.Width, h = lblLogo.Height;
            if (w <= 1 || h <= 1) return;

            float k = Math.Min(w / (float)brandLogoSource.Width, h / (float)brandLogoSource.Height);
            int dw = Math.Max(1, (int)Math.Round(brandLogoSource.Width * k));
            int dh = Math.Max(1, (int)Math.Round(brandLogoSource.Height * k));
            if (brandLogo != null && brandLogo.Width == dw && brandLogo.Height == dh) return;

            Bitmap moi = new Bitmap(dw, dh, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (Graphics g = Graphics.FromImage(moi))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(brandLogoSource, new Rectangle(0, 0, dw, dh));
            }

            Image cu = brandLogo;
            brandLogo = moi;
            lblLogo.Text = "";
            lblLogo.Image = moi;
            if (cu != null) cu.Dispose();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            FitToWorkingArea();
            LayoutMainHeader();
            LayoutSidebar();
            MoMucDauTien();
        }

        private void FitToWorkingArea()
        {
            if (adjustingWindow) return;
            adjustingWindow = true;
            try
            {
                Rectangle workingArea = Screen.FromControl(this).WorkingArea;
                MaximizedBounds = workingArea;
                MinimumSize = new Size(Math.Min(Px(900), workingArea.Width),
                    Math.Min(Px(620), workingArea.Height));
                MaximumSize = workingArea.Size;

                if (WindowState == FormWindowState.Normal)
                {
                    int width = Math.Min(Width, workingArea.Width);
                    int height = Math.Min(Height, workingArea.Height);
                    SetBounds(workingArea.Left + (workingArea.Width - width) / 2,
                        workingArea.Top + (workingArea.Height - height) / 2, width, height);
                }
            }
            finally { adjustingWindow = false; }
        }

        private void LayoutMainHeader()
        {
            if (pnlHeader == null || txtTimKiem == null) return;
            int gap = Px(16);
            bool showUser = pnlHeader.ClientSize.Width >= Px(540);
            lblUser.Visible = showUser;
            ptbAvatar.Visible = showUser;
            btnThongBao.Visible = showUser;

            txtTimKiem.Visible = false;
        }

        private void LayoutSidebar()
        {
            if (pnlSidebar == null) return;
            int sidebarWidth = Width < Px(1050) ? Px(200) : Px(250);
            if (pnlSidebar.Width != sidebarWidth) pnlSidebar.Width = sidebarWidth;

            pnlSidebar.AutoScroll = true;
            btnDangXuat.Dock = DockStyle.None;
            btnCaiDat.Dock = DockStyle.None;
            if (picCrowd != null) picCrowd.Dock = DockStyle.None;

            var navButtons = DanhSachNutDieuHuong();
            int navBottom = Px(140) + navButtons.Count(b => b != null && b.Visible) * Px(60) + Px(10);
            int footerH = (picCrowd != null && picCrowd.Height > 0) ? picCrowd.Height : Px(150);
            int linkTop = Math.Max(navBottom, pnlSidebar.ClientSize.Height - Px(140) - footerH);
            int totalContentHeight = linkTop + Px(140) + footerH;

            pnlSidebar.AutoScrollMinSize = new Size(0, totalContentHeight);
            int viewportWidth = sidebarWidth -
                (totalContentHeight > pnlSidebar.ClientSize.Height ? SystemInformation.VerticalScrollBarWidth : 0);
            int currentY = Px(140);

            foreach (var btn in navButtons)
            {
                if (btn != null)
                {
                    btn.Width = viewportWidth;
                    if (btn.Visible)
                    {
                        btn.SetBounds(0, currentY, viewportWidth, Px(50));
                        currentY += Px(60);
                    }
                }
            }

            btnDangXuat.Width = viewportWidth;
            btnCaiDat.Width = viewportWidth;
            if (btnCaiDat.Visible)
            {
                btnDangXuat.SetBounds(0, linkTop, viewportWidth, Px(70));
                btnCaiDat.SetBounds(0, linkTop + Px(70), viewportWidth, Px(70));
            }
            else
            {
                btnDangXuat.SetBounds(0, linkTop + Px(70), viewportWidth, Px(70));
            }

            if (picCrowd != null)
            {
                picCrowd.SetBounds(0, linkTop + Px(140), viewportWidth, footerH);
            }

            bool compact = sidebarWidth < Px(220);
            if (compact != compactLogo)
            {
                compactLogo = compact;
                lblLogo.Font = new Font("Segoe UI", compact ? 14f : 18f, FontStyle.Bold);
            }
            // Logo kiểu mới: icon vé + ảnh logo + dòng phụ, bám theo chiều rộng sidebar.
            int iconSize = Px(compact ? 36 : 44);
            picLogo.SetBounds(Px(compact ? 14 : 20), Px(38), iconSize, iconSize);
            lblSubtitle.Location = new Point(Px(compact ? 58 : 72), Px(compact ? 64 : 68));
            lblSubtitle.Visible = !compact;

            int logoLeft = Px(compact ? 58 : 72);
            int logoTop = Px(compact ? 34 : 32);
            int logoBottom = !compact ? lblSubtitle.Top - Px(4) : logoTop + Px(32);
            int logoWidth = viewportWidth - logoLeft - Px(compact ? 12 : 16);
            lblLogo.SetBounds(logoLeft, logoTop,
                Math.Max(Px(60), logoWidth), Math.Max(Px(18), logoBottom - logoTop));
            TaiLogoBrand();
        }

        #endregion

        #region Navigation & Child Forms
        public void OpenChildForm(Form childForm, Guna.UI2.WinForms.Guna2Button btnActive)
        {
            if (childForm == null) return;
            if (currentFormChild != null)
            {
                var oldForm = currentFormChild;
                oldForm.Close();
                if (!oldForm.IsDisposed)
                {
                    childForm.Dispose();
                    return;
                }
                pnlContent.Controls.Remove(oldForm);
                currentFormChild = null;
            }

            // Đổi màu menu và vẽ lại ngay TRƯỚC khi Show: Show() chạy Load của
            // form con, nếu Load nặng thì menu cũ vẫn xanh suốt thời gian đó.
            ResetButtonColors();
            if (btnActive != null) { btnActive.FillColor = Color.FromArgb(20, 184, 166); btnActive.ForeColor = Color.White; }
            Update();

            currentFormChild = childForm;
            LayoutMainHeader();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
            resizeBox.BringToFront();
        }

        private Guna.UI2.WinForms.Guna2Button[] DanhSachNutDieuHuong()
        {
            return new Guna.UI2.WinForms.Guna2Button[] {
                btnSuKien, btnSuatDien, btnKhachHang, btnTaiKhoan, btnBaoCao,
                btnBanVe, btnDonVe
            };
        }

        private void MoMucDauTien()
        {
            foreach (var nut in DanhSachNutDieuHuong())
            {
                if (nut != null && nut.Visible)
                {
                    nut.PerformClick();
                    return;
                }
            }
        }

        private void ResetButtonColors()
        {
            Color defaultColor = Color.Transparent;
            Color defaultTextColor = Color.FromArgb(203, 213, 225);
            if (btnSuKien != null) { btnSuKien.FillColor = defaultColor; btnSuKien.ForeColor = defaultTextColor; }
            if (btnSuatDien != null) { btnSuatDien.FillColor = defaultColor; btnSuatDien.ForeColor = defaultTextColor; }
            if (btnKhuVuc != null) { btnKhuVuc.FillColor = defaultColor; btnKhuVuc.ForeColor = defaultTextColor; }
            if (btnKhachHang != null) { btnKhachHang.FillColor = defaultColor; btnKhachHang.ForeColor = defaultTextColor; }
            if (btnBanVe != null) { btnBanVe.FillColor = defaultColor; btnBanVe.ForeColor = defaultTextColor; }
            if (btnDonVe != null) { btnDonVe.FillColor = defaultColor; btnDonVe.ForeColor = defaultTextColor; }
            if (btnTaiKhoan != null) { btnTaiKhoan.FillColor = defaultColor; btnTaiKhoan.ForeColor = defaultTextColor; }
            if (btnBaoCao != null) { btnBaoCao.FillColor = defaultColor; btnBaoCao.ForeColor = defaultTextColor; }
            if (btnCaiDat != null) { btnCaiDat.FillColor = defaultColor; btnCaiDat.ForeColor = defaultTextColor; }
        }

        #endregion

        #region Event Handlers
        public void btnSuKien_Click(object sender, EventArgs e)
        {
            if (!PhienDangNhap.LaQuanLy)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Quản lý Sự kiện!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenChildForm(new FrmQuanLySuKien(), btnSuKien);
        }

        public void btnSuatDien_Click(object sender, EventArgs e)
        {
            if (!PhienDangNhap.LaQuanLy)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Quản lý Suất diễn!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenChildForm(new FrmQuanLySuatDien(), btnSuatDien);
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmKhachHang(), btnKhachHang);
        }

        private void btnKhuVuc_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmQuanLyKhuVuc(), btnKhuVuc);
        }

        private void btnBanVe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmBanVe(), btnBanVe);
        }

        private void btnDonVe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmQuanLyDonVe(), btnDonVe);
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmNhanVien(), btnTaiKhoan);
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmBaoCao(), btnBaoCao);
        }


        private void btnCaiDat_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmCaiDat(), btnCaiDat);
        }


        private void ApplyPhanQuyen()
        {
            if (PhienDangNhap.DaDangNhap)
            {
                var tk = PhienDangNhap.TaiKhoan;
                if (lblUser != null)
                {
                    lblUser.Text = $"{tk.TenDangNhap} ({(PhienDangNhap.LaQuanLy ? "Quản lý" : "Nhân viên")})";
                }

                bool isAdmin = PhienDangNhap.LaQuanLy;

                // 1. ADMIN
                if (btnSuKien != null) btnSuKien.Visible = isAdmin;
                if (btnSuatDien != null) btnSuatDien.Visible = isAdmin;
                if (btnKhachHang != null) btnKhachHang.Visible = isAdmin;
                if (btnTaiKhoan != null) btnTaiKhoan.Visible = isAdmin;
                if (btnBaoCao != null) btnBaoCao.Visible = isAdmin;

                // 2. STAFF
                if (btnBanVe != null) btnBanVe.Visible = !isAdmin;
                if (btnDonVe != null) btnDonVe.Visible = !isAdmin;

                // 3. ẨN CÁC NÚT CŨ
                if (btnKhuVuc != null) btnKhuVuc.Visible = false;

                LayoutSidebar();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                if (!currentFormChild.IsDisposed)
                {
                    e.Cancel = true;
                    return;
                }
            }
            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (brandLogo != null)
            {
                brandLogo.Dispose();
                brandLogo = null;
            }
            if (brandLogoSource != null)
            {
                brandLogoSource.Dispose();
                brandLogoSource = null;
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (currentFormChild != null)
                {
                    currentFormChild.Close();
                    if (!currentFormChild.IsDisposed) return;
                }
                DangXuat = true;
                PhienDangNhap.KetThuc();
                Close();
            }
        }
        #endregion
    }
}

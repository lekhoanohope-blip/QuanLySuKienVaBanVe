using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmMain : Form
    {
        public bool DangXuat { get; private set; }
        private Form currentFormChild;
        private bool adjustingWindow;
        private bool compactLogo;

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

        public FrmMain()
        {
            InitializeComponent();
            HieuUng.GanChoForm(this);
            this.DoubleBuffered = true;
            resizeBox.BringToFront();
            if (btnDangXuat != null) btnDangXuat.Click += btnDangXuat_Click;
            if (btnCaiDat != null) btnCaiDat.Click += btnCaiDat_Click;
            ApplyPhanQuyen();

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

        private int Px(int logicalPixels)
        {
            return (int)Math.Round(logicalPixels * DeviceDpi / 96f);
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

            int rightEdge = showUser ? btnThongBao.Left : controlBoxMinimize.Left;
            int searchWidth = Math.Min(Px(450), rightEdge - txtTimKiem.Left - gap);
            txtTimKiem.Visible = searchWidth >= Px(160);
            if (txtTimKiem.Visible) txtTimKiem.Width = searchWidth;
        }

        private void LayoutSidebar()
        {
            if (pnlSidebar == null) return;
            int sidebarWidth = Width < Px(1050) ? Px(200) : Px(250);
            if (pnlSidebar.Width != sidebarWidth) pnlSidebar.Width = sidebarWidth;

            pnlSidebar.AutoScroll = true;
            btnDangXuat.Dock = DockStyle.None;
            btnCaiDat.Dock = DockStyle.None;
            // Cập nhật mảng này để bao gồm tất cả các nút có thể hiển thị
            Control[] navButtons = DanhSachNutDieuHuong();
            int navBottom = Px(140) + navButtons.Count(b => b != null && b.Visible) * Px(60) + Px(10);
            // Chừa 150px dưới đáy cho ảnh đám đông cố định, nút dưới cùng không đè lên ảnh.
            int footerH = Px(150);
            int linkTop = Math.Max(navBottom, pnlSidebar.ClientSize.Height - Px(140) - footerH);
            pnlSidebar.AutoScrollMinSize = new Size(0, linkTop + Px(140));
            int viewportWidth = sidebarWidth -
                (linkTop + Px(140) > pnlSidebar.ClientSize.Height ? SystemInformation.VerticalScrollBarWidth : 0);
            int currentY = Px(140);

            foreach (Control btn in navButtons)
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

            bool compact = sidebarWidth < Px(220);
            if (compact != compactLogo)
            {
                compactLogo = compact;
                lblLogo.Font = new Font("Segoe UI", compact ? 14f : 18f, FontStyle.Bold);
            }
            // Logo kiểu mới: icon vé + tên + dòng phụ, bám theo chiều rộng sidebar.
            int iconSize = Px(compact ? 36 : 44);
            picLogo.SetBounds(Px(compact ? 14 : 20), Px(38), iconSize, iconSize);
            lblLogo.Location = new Point(Px(compact ? 58 : 72), Px(compact ? 42 : 40));
            lblSubtitle.Location = new Point(Px(compact ? 58 : 72), Px(compact ? 64 : 68));
            lblSubtitle.Visible = !compact;
        }

        public void OpenChildForm(Form childForm, Guna.UI2.WinForms.Guna2Button btnActive)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                if (!currentFormChild.IsDisposed)
                {
                    childForm.Dispose();
                    return;
                }
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
            resizeBox.BringToFront();

            ResetButtonColors();
            if (btnActive != null) { btnActive.FillColor = Color.FromArgb(20, 184, 166); btnActive.ForeColor = Color.White; }
        }

        private Control[] DanhSachNutDieuHuong()
        {
            return new Control[] {
                btnSuKien, btnSuatDien, btnKhachHang, btnTaiKhoan, btnBaoCao,
                btnNhatKy, btnBanVe, btnDonVe
            };
        }

        private void MoMucDauTien()
        {
            foreach (Control nut in DanhSachNutDieuHuong())
            {
                if (nut != null && nut.Visible)
                {
                    ((Guna.UI2.WinForms.Guna2Button)nut).PerformClick();
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
            if (btnNhatKy != null) { btnNhatKy.FillColor = defaultColor; btnNhatKy.ForeColor = defaultTextColor; }
            if (btnCaiDat != null) { btnCaiDat.FillColor = defaultColor; btnCaiDat.ForeColor = defaultTextColor; }
        }

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
            OpenChildForm(new FrmQuanLyKhuVuc(), btnBanVe);
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

        private void btnNhatKy_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmNhatKy(), btnNhatKy);
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
                if (btnNhatKy != null) btnNhatKy.Visible = isAdmin;

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
    }
}
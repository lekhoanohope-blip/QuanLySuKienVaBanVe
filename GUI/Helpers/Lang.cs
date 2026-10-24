using System;
using System.Collections.Generic;

namespace QuanLySuKienVaBanVe.GUI
{
    public enum Ngon { VN = 0, EN = 1 }

    /// <summary>
    /// Đa ngôn ngữ đơn giản: mỗi khóa có {tiếng Việt, tiếng Anh}.
    /// Dùng:  Lang.T("btn_login")   hoặc   Lang.T("err_temp", 30)
    /// Khi gọi Lang.Set(...) sự kiện Changed bắn ra để các form tự đổi chữ.
    /// Muốn thêm chữ mới: thêm 1 dòng vào từ điển bên dưới.
    /// </summary>
    public static class Lang
    {
        public static Ngon Hien { get; private set; }
        public static event EventHandler Changed;

        static Lang() { Hien = Ngon.VN; }

        public static void Set(Ngon n)
        {
            if (n == Hien) return;
            Hien = n;
            EventHandler h = Changed;
            if (h != null) h(null, EventArgs.Empty);
        }

        public static string T(string key, params object[] args)
        {
            string[] v;
            if (!D.TryGetValue(key, out v)) return key;     // thiếu khóa -> hiện luôn tên khóa để dễ phát hiện
            string s = v[(int)Hien];
            return args != null && args.Length > 0 ? string.Format(s, args) : s;
        }

        private static readonly Dictionary<string, string[]> D = new Dictionary<string, string[]>
        {
            // ---- Chung ----
            { "app_title",   new[] { "Quản lý sự kiện và bán vé", "Event and Ticket Management" } },
            { "lang_vi",     new[] { "Tiếng Việt (VN)", "Tiếng Việt (VN)" } },
            { "lang_en",     new[] { "English (EN)", "English (EN)" } },

            // ---- Đăng nhập / đăng ký ----
            { "login_title", new[] { "Đăng nhập", "Sign in" } },
            { "reg_title",   new[] { "Đăng ký tài khoản", "Create account" } },
            { "ph_user",     new[] { "Nhập tên đăng nhập", "Enter username" } },
            { "ph_pass",     new[] { "Nhập mật khẩu", "Enter password" } },
            { "ph_confirm",  new[] { "Xác nhận mật khẩu", "Confirm password" } },
            { "remember",    new[] { "Ghi nhớ tên đăng nhập", "Remember my username" } },
            { "forgot",      new[] { "Quên mật khẩu?", "Forgot password?" } },
            { "btn_login",   new[] { "Đăng nhập", "Sign in" } },
            { "btn_register",new[] { "Tạo tài khoản", "Create account" } },
            { "no_acc",      new[] { "Bạn chưa có tài khoản?", "Don't have an account?" } },
            { "reg_now",     new[] { "Đăng ký ngay", "Sign up" } },
            { "have_acc",    new[] { "Bạn đã có tài khoản?", "Already have an account?" } },
            { "login_link",  new[] { "Đăng nhập", "Sign in" } },
            { "agree",       new[] { "Đồng ý với", "I agree to the" } },
            { "terms",       new[] { "Điều kiện và điều khoản", "Terms and conditions" } },
            { "terms_text",  new[] {
                "1. Tài khoản đăng ký mới có vai trò Nhân viên bán vé.\n2. Không chia sẻ mật khẩu cho người khác.\n3. Mọi thao tác bán vé, hủy vé đều được ghi nhật ký.\n4. Quản lý có quyền khóa tài khoản vi phạm.",
                "1. New accounts are created with the Ticket staff role.\n2. Do not share your password with anyone.\n3. All sales and cancellations are logged.\n4. Managers may lock accounts that break the rules." } },

            { "err_empty",   new[] { "Vui lòng nhập tên đăng nhập và mật khẩu.", "Please enter your username and password." } },
            { "err_wrong",   new[] { "Sai tên đăng nhập hoặc mật khẩu.", "Incorrect username or password." } },
            { "err_locked",  new[] { "Tài khoản đã bị khóa. Vui lòng liên hệ Quản lý.", "This account is locked. Please contact the Manager." } },
            { "err_temp",    new[] { "Sai quá nhiều lần. Thử lại sau {0} giây.", "Too many failed attempts. Try again in {0} s." } },
            { "err_sys",     new[] { "Không kết nối được CSDL. Kiểm tra App.config và SQL Server.", "Cannot reach the database. Check App.config and SQL Server." } },
            { "err_user_fmt",new[] { "Tên đăng nhập 4-30 ký tự: chữ, số, dấu _ hoặc .", "Username: 4-30 characters, letters, digits, _ or ." } },
            { "err_pass_short", new[] { "Mật khẩu tối thiểu 6 ký tự.", "Password must be at least 6 characters." } },
            { "err_confirm", new[] { "Mật khẩu xác nhận không khớp.", "Passwords do not match." } },
            { "err_exists",  new[] { "Tên đăng nhập đã tồn tại.", "This username already exists." } },
            { "reg_ok",      new[] { "Tạo tài khoản thành công. Hãy đăng nhập.", "Account created. Please sign in." } },
            { "forgot_msg",  new[] { "Vui lòng liên hệ Quản lý để được cấp lại mật khẩu.", "Please contact the Manager to reset your password." } },

            // ---- Form chính ----
            { "m_system",    new[] { "Hệ thống", "System" } },
            { "m_changepw",  new[] { "Đổi mật khẩu", "Change password" } },
            { "m_language",  new[] { "Ngôn ngữ", "Language" } },
            { "m_logout",    new[] { "Đăng xuất", "Sign out" } },
            { "m_exit",      new[] { "Thoát", "Exit" } },
            { "m_catalog",   new[] { "Danh mục", "Catalog" } },
            { "m_events",    new[] { "Sự kiện", "Events" } },
            { "m_showtimes", new[] { "Suất diễn", "Showtimes" } },
            { "m_zones",     new[] { "Khu vực và giá vé", "Zones and prices" } },
            { "m_customers", new[] { "Khách hàng", "Customers" } },
            { "m_sales",     new[] { "Bán vé", "Ticketing" } },
            { "m_pos",       new[] { "Bán vé tại quầy", "Box-office sales" } },
            { "m_orders",    new[] { "Quản lý đơn vé", "Orders" } },
            { "m_reports",   new[] { "Báo cáo", "Reports" } },
            { "m_revenue",   new[] { "Thống kê doanh thu", "Revenue statistics" } },
            { "m_audit",     new[] { "Nhật ký hệ thống", "Audit log" } },
            { "m_help",      new[] { "Trợ giúp", "Help" } },
            { "m_about",     new[] { "Giới thiệu", "About" } },
            { "s_user",      new[] { "Người dùng", "User" } },
            { "s_role",      new[] { "Vai trò", "Role" } },
            { "role_QuanLy", new[] { "Quản lý sự kiện", "Event manager" } },
            { "role_NhanVien", new[] { "Nhân viên bán vé", "Ticket staff" } },
            { "not_ready",   new[] { "Màn hình \"{0}\" chưa được tích hợp (do thành viên khác phụ trách).", "The \"{0}\" screen is not integrated yet." } },
            { "confirm_logout", new[] { "Bạn có chắc muốn đăng xuất?", "Are you sure you want to sign out?" } },
            { "confirm_exit",   new[] { "Bạn có chắc muốn thoát chương trình?", "Are you sure you want to exit?" } },
            { "about_text",  new[] { "Quản lý sự kiện và bán vé\nĐề tài 20 - Nhóm 3 - COMP1019\nWinForms + SQL Server", "Event and Ticket Management\nTopic 20 - Group 3 - COMP1019\nWinForms + SQL Server" } },
        };
    }
}

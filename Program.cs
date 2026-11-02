using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.GUI;

namespace QuanLySuKienVaBanVe
{
    internal static class Program
    {
        /// <summary>
        /// Vòng lặp: Đăng nhập -> Form chính -> (Đăng xuất) -> quay lại Đăng nhập ...
        /// Đóng cửa sổ đăng nhập hoặc thoát form chính (không đăng xuất) thì kết thúc chương trình.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                using (FrmDangNhap login = new FrmDangNhap())
                {
                    if (login.ShowDialog() != DialogResult.OK) break;
                }

                bool dangXuat = false;
                using (FrmMain main = new FrmMain())
                {
                    Application.Run(main);
                    dangXuat = main.DangXuat;
                }

                if (!dangXuat) break;
            }
        }
    }
}

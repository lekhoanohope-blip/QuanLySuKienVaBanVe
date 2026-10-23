using System;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.GUI; 

namespace QuanLySuKienVaBanVe 
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FrmQuanLySuKien());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI.Helpers
{
    public static class TicketPdfExporter
    {
        public const string TenFileDinhDang = "EventTicket_mave_{0}.pdf";

        public static string ThuMucMacDinh
        {
            get { return LayDownloads(); }
        }

        private static string LayDownloads()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders"))
                {
                    if (key != null)
                    {
                        object raw = key.GetValue("{374DE290-123F-4565-9164-39C4925E467B}");
                        if (raw != null)
                        {
                            string path = Environment.ExpandEnvironmentVariables(raw.ToString());
                            if (!string.IsNullOrWhiteSpace(path)) return path.Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LayDownloads: " + ex.Message);
            }

            string profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!string.IsNullOrWhiteSpace(profile))
                return Path.Combine(profile, "Downloads");

            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!string.IsNullOrWhiteSpace(docs))
                return Path.Combine(docs, "Downloads");

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads");
        }

        public static string LayTenFile(ThongTinInVeDTO ve)
        {
            string so = ve != null ? ve.SoMaVe : "0";
            return string.Format(CultureInfo.InvariantCulture, TenFileDinhDang, LamSach(so));
        }

        public static string Xuat(ThongTinInVeDTO ve, string thuMuc)
        {
            if (ve == null) throw new ArgumentNullException("ve");
            return XuatDanhSach(new List<ThongTinInVeDTO> { ve }, thuMuc)[0];
        }

        public static List<string> XuatDanhSach(IEnumerable<ThongTinInVeDTO> dsVe, string thuMuc)
        {
            if (dsVe == null) throw new ArgumentNullException("dsVe");

            if (string.IsNullOrWhiteSpace(thuMuc)) thuMuc = ThuMucMacDinh;
            Directory.CreateDirectory(thuMuc);

            List<string> daGhi = new List<string>();
            foreach (ThongTinInVeDTO ve in dsVe)
            {
                if (ve == null) continue;
                string duongDan = Path.Combine(thuMuc, LayTenFile(ve));
                using (FileStream fs = new FileStream(duongDan, FileMode.Create, FileAccess.Write))
                    XuatVaoStream(fs, new List<ThongTinInVeDTO> { ve });
                daGhi.Add(duongDan);
            }
            return daGhi;
        }

        public static void XuatVaoStream(Stream outStream, IList<ThongTinInVeDTO> dsVe)
        {
            if (outStream == null) throw new ArgumentNullException("outStream");
            if (dsVe == null || dsVe.Count == 0)
                throw new ArgumentException("Danh sách vé rỗng, không có gì để xuất PDF.", "dsVe");

            PdfDocument doc = new PdfDocument();
            try
            {
                doc.Info.Title = "EventTicket - In ve";
                doc.Info.Author = "QuanLySuKienVaBanVe";
                doc.Info.Creator = "EventTicket TV3";

                for (int i = 0; i < dsVe.Count; i++)
                {
                    PdfPage page = doc.AddPage();
                    page.Width = XUnit.FromMillimeter(TicketRenderer.PrintSizeMm.Width);
                    page.Height = XUnit.FromMillimeter(TicketRenderer.PrintSizeMm.Height);

                    using (Bitmap anh = TicketRenderer.RenderForPrint(dsVe[i]))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        anh.Save(ms, ImageFormat.Png);
                        ms.Position = 0;

                        using (XImage img = XImage.FromStream(ms))
                        using (XGraphics gfx = XGraphics.FromPdfPage(page))
                            gfx.DrawImage(img, 0, 0, page.Width.Point, page.Height.Point);
                    }
                }

                doc.Save(outStream, false);
            }
            finally
            {
                doc.Close();
            }
        }

        private static string LamSach(string chuoi)
        {
            if (string.IsNullOrEmpty(chuoi)) return "0";
            char[] invalid = Path.GetInvalidFileNameChars();
            char[] kyTu = new char[chuoi.Length];
            int n = 0;
            foreach (char c in chuoi)
                kyTu[n++] = Array.IndexOf(invalid, c) >= 0 ? '_' : c;
            string kq = new string(kyTu, 0, n).Trim();
            return kq.Length == 0 ? "0" : kq;
        }
    }
}

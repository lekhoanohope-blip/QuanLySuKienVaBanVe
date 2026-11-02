using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace QuanLySuKienVaBanVe.GUI.Helpers
{
    public static class IconHelper
    {
        private static readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>();

        public static Image GetIcon(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return null;

            if (_cache.ContainsKey(filename)) return _cache[filename];

            string[] possiblePaths = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", filename),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Icons", filename),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Resources", "Icons", filename),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", filename)
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            Image img = Image.FromStream(fs);
                            _cache[filename] = img;
                            return img;
                        }
                    }
                    catch
                    {
                        // Bỏ qua lỗi đọc file nếu đang lock
                    }
                }
            }

            return null;
        }
    }
}

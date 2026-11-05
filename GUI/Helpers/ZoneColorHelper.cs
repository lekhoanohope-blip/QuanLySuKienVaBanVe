using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace QuanLySuKienVaBanVe.GUI.Helpers
{
    public static class ZoneColorHelper
    {
        private static readonly Regex HexColorRegex = new Regex("^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

        public static bool IsValidHex(string hex)
        {
            return !string.IsNullOrWhiteSpace(hex) && HexColorRegex.IsMatch(hex.Trim());
        }

        public static Color Resolve(string hex, string maVung = null, Color? fallback = null)
        {
            if (IsValidHex(hex))
            {
                try
                {
                    return ColorTranslator.FromHtml(hex.Trim());
                }
                catch
                {
                    // Ignore parse error and fall through
                }
            }

            if (!string.IsNullOrEmpty(maVung))
            {
                return ByCode(maVung);
            }

            return fallback ?? Color.FromArgb(100, 116, 139);
        }

        public static Color ByCode(string maVung)
        {
            switch (maVung)
            {
                case "VIP_A": return Color.FromArgb(217, 119, 6);
                case "VIP_B": return Color.FromArgb(202, 138, 4);
                case "GA_A": return Color.FromArgb(37, 99, 235);
                case "GA_B": return Color.FromArgb(5, 150, 105);
                case "CAT": return Color.FromArgb(124, 58, 237);
                default: return Color.FromArgb(100, 116, 139);
            }
        }

        public static string DefaultHexFor(string maVung)
        {
            switch (maVung)
            {
                case "VIP_A": return "#D97706";
                case "VIP_B": return "#CA8A04";
                case "GA_A": return "#2563EB";
                case "GA_B": return "#059669";
                case "CAT": return "#7C3AED";
                default: return "#64748B";
            }
        }
    }
}

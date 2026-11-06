using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuanLySuKienVaBanVe.GUI.Helpers
{
    public static class ZoneColorHelper
    {
        private static readonly Regex HexColorRegex = new Regex("^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> DefaultColors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "VIP_A", "#D97706" }, { "VIP_B", "#CA8A04" },
            { "VIP_C", "#DC2626" }, { "VIP_D", "#DB2777" },
            { "GA_A", "#2563EB" }, { "GA_B", "#0D9488" },
            { "GA_C", "#4F46E5" }, { "GA_D", "#0284C7" },
            { "GA_E", "#059669" }, { "GA_F", "#9333EA" },
            { "GA_G", "#9F1239" }, { "GA_H", "#475569" },
            { "CAT", "#7C3AED" }, { "CAT_A", "#6D28D9" },
            { "CAT_B", "#7C2D12" }, { "CAT_C", "#1E3A8A" }, { "CAT_D", "#831843" }
        };

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

            string defaultHex;
            if (DefaultColors.TryGetValue(maVung?.Trim() ?? string.Empty, out defaultHex))
                return ColorTranslator.FromHtml(defaultHex);

            return fallback ?? Color.FromArgb(100, 116, 139);
        }

        public static Color ByCode(string maVung)
        {
            return ColorTranslator.FromHtml(DefaultHexFor(maVung));
        }

        public static string DefaultHexFor(string maVung)
        {
            string hex;
            return DefaultColors.TryGetValue(maVung?.Trim() ?? string.Empty, out hex) ? hex : "#64748B";
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    internal static class ManagementGridRenderer
    {
        private static readonly Image IconXoa = global::QuanLySuKienVaBanVe.Properties.Resources.icon_xoa;
        private static readonly Image IconSua = global::QuanLySuKienVaBanVe.Properties.Resources.icon_sua;
        internal static Rectangle GetActionBounds(DataGridView grid, Rectangle cell, bool delete)
        {
            int gap = Scale(grid, 8);
            int size = Math.Min(Scale(grid, 32), Math.Min(cell.Height - Scale(grid, 16),
                (cell.Width - Scale(grid, 16) - gap) / 2));
            if (size <= 0) return Rectangle.Empty;

            int left = cell.Left + (cell.Width - size * 2 - gap) / 2;
            return new Rectangle(left + (delete ? size + gap : 0), cell.Top + (cell.Height - size) / 2, size, size);
        }

        internal static void PaintStatus(DataGridView grid, DataGridViewCellPaintingEventArgs e,
            string text, Color background, Color foreground)
        {
            GraphicsState state = e.Graphics.Save();
            try
            {
                PaintBackground(e);
                int width = Math.Min(e.CellBounds.Width - Scale(grid, 24),
                    TextRenderer.MeasureText(e.Graphics, text, e.CellStyle.Font, Size.Empty,
                        TextFormatFlags.NoPadding | TextFormatFlags.SingleLine).Width + Scale(grid, 24));
                int height = Math.Min(Scale(grid, 28), e.CellBounds.Height - Scale(grid, 16));
                if (width <= 0 || height <= 0) return;

                Rectangle badge = new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - width) / 2,
                    e.CellBounds.Top + (e.CellBounds.Height - height) / 2, width, height);
                FillRoundedRectangle(e.Graphics, badge, height / 2, background);
                TextRenderer.DrawText(e.Graphics, text, e.CellStyle.Font, badge, foreground,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsClipping);
            }
            finally
            {
                e.Graphics.Restore(state);
                e.Handled = true;
            }
        }

        internal static void PaintActions(DataGridView grid, DataGridViewCellPaintingEventArgs e)
        {
            GraphicsState state = e.Graphics.Save();
            try
            {
                PaintBackground(e);
                Point chuot = grid.PointToClient(Cursor.Position);
                Rectangle nutSua = GetActionBounds(grid, e.CellBounds, false);
                Rectangle nutXoa = GetActionBounds(grid, e.CellBounds, true);
                DrawAction(e.Graphics, nutSua, false, nutSua.Contains(chuot));
                DrawAction(e.Graphics, nutXoa, true, nutXoa.Contains(chuot));
            }
            finally
            {
                e.Graphics.Restore(state);
                e.Handled = true;
            }
        }

        private static int Scale(Control control, int pixels)
        {
            return (int)Math.Round(pixels * control.DeviceDpi / 96f);
        }

        private static void PaintBackground(DataGridViewCellPaintingEventArgs e)
        {
            // Keep the same row separator as ordinary cells; clip custom drawing to this cell.
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background |
                DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Border);
            e.Graphics.SetClip(Rectangle.Intersect(e.CellBounds, e.ClipBounds), CombineMode.Intersect);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        private static void DrawAction(Graphics graphics, Rectangle bounds, bool delete, bool hot)
        {
            if (bounds.IsEmpty) return;

            // Nút Sửa/Xóa "nổi lên" khi rê chuột: vẽ cao hơn ~2px + bóng đổ
            // mờ bên dưới. Chỉ thay đổi chỗ VẼ, vùng bắt chuột (bounds gốc)
            // giữ nguyên nên không bị nhấp nháy khi rê ở mép nút.
            int noi = hot ? Math.Max(1, bounds.Height / 11) : 0;
            Rectangle rectVe = new Rectangle(bounds.X, bounds.Y - noi, bounds.Width, bounds.Height);
            int radius = Math.Max(1, bounds.Height / 5);
            if (hot)
            {
                FillRoundedRectangle(graphics,
                    new Rectangle(rectVe.X, rectVe.Y + 2, rectVe.Width, rectVe.Height),
                    radius, Color.FromArgb(35, 0, 0, 0));
                FillRoundedRectangle(graphics,
                    new Rectangle(rectVe.X, rectVe.Y + 4, rectVe.Width, rectVe.Height),
                    radius, Color.FromArgb(18, 0, 0, 0));
            }

            Color background = delete
                ? (hot ? Color.FromArgb(254, 226, 226) : Color.FromArgb(254, 242, 242))
                : (hot ? Color.FromArgb(219, 234, 254) : Color.FromArgb(239, 246, 255));
            FillRoundedRectangle(graphics, rectVe, radius, background);

            // Icon thật trong Resources (icon_sua.png / icon_xoa.png, nền trong suốt),
            // vẽ vừa khung, giữ nét ở DPI cao nhờ ảnh gốc 64x64.
            Image icon = delete ? IconXoa : IconSua;
            if (icon == null) return;
            float iconSize = rectVe.Height / 2f;
            RectangleF dich = new RectangleF(
                rectVe.Left + (rectVe.Width - iconSize) / 2f,
                rectVe.Top + (rectVe.Height - iconSize) / 2f,
                iconSize, iconSize);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(icon, dich);
        }

        internal static void FillRoundedRectangle(Graphics graphics, Rectangle bounds, int radius, Color color)
        {
            int diameter = Math.Max(1, Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height)));
            using (GraphicsPath path = new GraphicsPath())
            using (SolidBrush brush = new SolidBrush(color))
            {
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                graphics.FillPath(brush, path);
            }
        }
    }
}
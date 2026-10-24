using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    internal static class ManagementGridRenderer
    {
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
                DrawAction(e.Graphics, GetActionBounds(grid, e.CellBounds, false), false);
                DrawAction(e.Graphics, GetActionBounds(grid, e.CellBounds, true), true);
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

        private static void DrawAction(Graphics graphics, Rectangle bounds, bool delete)
        {
            if (bounds.IsEmpty) return;
            Color background = delete ? Color.FromArgb(254, 242, 242) : Color.FromArgb(239, 246, 255);
            Color foreground = delete ? Color.FromArgb(185, 28, 28) : Color.FromArgb(37, 99, 235);
            FillRoundedRectangle(graphics, bounds, Math.Max(1, bounds.Height / 5), background);

            GraphicsState state = graphics.Save();
            try
            {
                float iconSize = bounds.Height / 2f;
                graphics.TranslateTransform(bounds.Left + (bounds.Width - iconSize) / 2,
                    bounds.Top + (bounds.Height - iconSize) / 2);
                graphics.ScaleTransform(iconSize / 16f, iconSize / 16f);
                using (Pen pen = new Pen(foreground, 1.5f))
                {
                    pen.LineJoin = LineJoin.Round;
                    if (delete)
                    {
                        graphics.DrawLine(pen, 3, 4, 13, 4);
                        graphics.DrawLine(pen, 6, 2, 10, 2);
                        graphics.DrawRectangle(pen, 4, 4, 8, 10);
                        graphics.DrawLine(pen, 7, 7, 7, 11);
                        graphics.DrawLine(pen, 10, 7, 10, 11);
                    }
                    else
                    {
                        graphics.DrawPolygon(pen, new[] { new Point(3, 10), new Point(10, 3),
                            new Point(13, 6), new Point(6, 13), new Point(3, 13) });
                        graphics.DrawLine(pen, 9, 4, 12, 7);
                    }
                }
            }
            finally { graphics.Restore(state); }
        }

        private static void FillRoundedRectangle(Graphics graphics, Rectangle bounds, int radius, Color color)
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

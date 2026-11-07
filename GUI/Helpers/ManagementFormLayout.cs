using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    internal static class ManagementFormLayout
    {
        internal static void Attach(Control header, Control search, Control add, Control edit, Control delete)
        {
            var primary = add as Guna.UI2.WinForms.Guna2Button;
            if (primary != null)
            {
                primary.FillColor = ManagementUi.Primary;
                primary.HoverState.FillColor = Color.FromArgb(6, 95, 70);
            }
            search.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            add.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            edit.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            delete.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            header.SizeChanged += (sender, args) => Apply(header, search, add, edit, delete);
            Apply(header, search, add, edit, delete);
        }

        private static void Apply(Control header, Control search, Control add, Control edit, Control delete)
        {
            float scale = header.DeviceDpi / 96f;
            Func<int, int> px = value => (int)Math.Round(value * scale);
            int gap = px(10);
            int actionsWidth = add.Width + edit.Width + delete.Width + gap * 2;
            int availableWidth = header.ClientSize.Width;
            bool stacked = availableWidth < actionsWidth + px(190);
            int headerHeight = stacked ? px(112) : px(72);
            if (header.Height != headerHeight) header.Height = headerHeight;

            search.Location = new Point(0, stacked ? px(12) : px(20));
            search.Width = Math.Max(px(120), Math.Min(px(350),
                availableWidth - (stacked ? 0 : actionsWidth + px(20))));

            int actionX = Math.Max(0, availableWidth - actionsWidth);
            int actionY = stacked ? px(66) : px(20);
            add.Location = new Point(actionX, actionY);
            edit.Location = new Point(actionX + add.Width + gap, actionY);
            delete.Location = new Point(actionX + add.Width + edit.Width + gap * 2, actionY);
        }
    }
    internal static class ManagementUi
    {
        internal static readonly Color Primary = Color.FromArgb(4, 120, 87);
        internal static readonly Color MutedText = Color.FromArgb(71, 85, 105);
        internal static readonly Color DangerText = Color.FromArgb(185, 28, 28);
        internal static int Px(Control control, int value) => (int)Math.Round(value * control.DeviceDpi / 96f);

        internal static string StatusText(string status)
        {
            switch (status)
            {
                case "SapDien": return "Sắp diễn ra";
                case "DangDien": return "Đang diễn ra";
                case "DaKetThuc": return "Đã kết thúc";
                case "DaHuy": return "Đã hủy";
                case "MoCuaBanVe": return "Mở bán vé";
                case "DongBanVe": return "Đóng bán vé";
                default: return status;
            }
        }

        internal static bool Matches(string keyword, params string[] values)
        {
            string normalized = BLL.KhachHangBLL.LoaiBoDauTiengViet(keyword.Trim());
            foreach (string value in values)
                if (BLL.KhachHangBLL.LoaiBoDauTiengViet(value ?? "")
                    .IndexOf(normalized, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        internal static Action AttachListState(DataGridView grid, Control search, Control edit, Control delete)
        {
            var message = new Panel { BackColor = Color.White, Visible = false, Height = 100 };
            var label = new Label { Dock = DockStyle.Top, Height = 52, TextAlign = ContentAlignment.MiddleCenter, ForeColor = MutedText };
            var clear = new Button { Text = "Xóa bộ lọc", Dock = DockStyle.Bottom, Height = 36 };
            clear.Click += (sender, args) => { search.Text = ""; search.Focus(); };
            message.Controls.Add(label);
            message.Controls.Add(clear);
            grid.Controls.Add(message);
            Action layout = () => message.SetBounds(12, Math.Max(grid.ColumnHeadersHeight + 8, (grid.ClientSize.Height - 100) / 2), Math.Max(1, grid.ClientSize.Width - 24), 100);
            grid.SizeChanged += (sender, args) => layout();
            Action update = () =>
            {
                bool empty = grid.Rows.Count == 0;
                edit.Enabled = delete.Enabled = !empty && grid.CurrentRow != null;
                label.Text = string.IsNullOrWhiteSpace(search.Text)
                    ? "Chưa có dữ liệu. Chọn Thêm mới để tạo bản ghi đầu tiên."
                    : "Không tìm thấy kết quả. Thử từ khóa khác hoặc xóa bộ lọc.";
                clear.Visible = !string.IsNullOrWhiteSpace(search.Text);
                message.Visible = empty;
                layout();
                if (empty) message.BringToFront();
            };
            grid.SelectionChanged += (sender, args) => update();
            return update;
        }
    }

}

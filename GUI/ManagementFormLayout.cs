using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    internal static class ManagementFormLayout
    {
        internal static void Attach(Control header, Control search, Control add, Control edit, Control delete)
        {
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
}

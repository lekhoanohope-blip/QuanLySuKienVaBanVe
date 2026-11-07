using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLyDonVe
    {
        private void InitializeOrderSpacing()
        {
            foreach (var grid in new[] { dgvDonVe, dgvChiTietVe })
            {
                grid.ScrollBars = ScrollBars.Vertical;
                grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            }
            lblDetailBadge.AutoSize = false;
            lblDetailBadge.TextAlign = ContentAlignment.MiddleCenter;
            lblDetailBadge.UseMnemonic = false;
            lblDetailTitle.UseMnemonic = false;
            foreach (var label in new[] { lblDetailBadge, lblCustomerName, lblCustomerPhone,
                lblCustomerEmail, lblDetailEventName, lblDetailEventTime, lblRuleBadge })
            {
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                label.TextChanged += PnlHeader_SizeChanged;
                label.VisibleChanged += PnlHeader_SizeChanged;
            }
            pnlDetailInfo.SizeChanged += PnlHeader_SizeChanged;
            pnlDetailActions.SizeChanged += PnlHeader_SizeChanged;
            pnlDetailDivider.Dock = lblTicketsTitle.Dock = DockStyle.None;
            foreach (var button in new[] { btnInVe, btnDoiVe, btnHuyVe, btnCopyMaVe })
                button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void ApplyResponsiveLayout(float scale)
        {
            if (updatingLayout || IsDisposed || Disposing) return;
            updatingLayout = true;
            try
            {
                Func<int, int> px = value => ScalePixels(value, scale);
                int gap = px(12);
                int fieldHeight = px(40);
                int width = pnlHeader.ClientSize.Width;
                int filterWidth = px(190);
                int buttonWidth = px(100);
                bool wrapToolbar = width < px(570);
                int searchWidth = wrapToolbar ? Math.Max(1, width) :
                    Math.Max(px(160), Math.Min(px(420), width - filterWidth - buttonWidth - gap * 2));
                txtTimKiem.SetBounds(0, px(8), searchWidth, fieldHeight);
                int filterTop = wrapToolbar ? txtTimKiem.Bottom + gap : px(8);
                cboLocTrangThai.ItemHeight = Math.Max(1, fieldHeight - 6);
                cboLocTrangThai.SetBounds(wrapToolbar ? 0 : txtTimKiem.Right + gap,
                    filterTop, Math.Min(filterWidth, width), fieldHeight);
                btnLamMoi.SetBounds(Math.Max(0, width - buttonWidth), filterTop, buttonWidth, fieldHeight);
                pnlHeader.Height = btnLamMoi.Bottom + px(16);

                int available = splitContainer.Width - splitContainer.SplitterWidth;
                if (available > 0)
                {
                    int minMaster = Math.Min(px(260), available / 2);
                    int minDetail = Math.Min(px(260), available / 2);
                    int desiredMaster = (int)Math.Round(available * preferredMasterRatio);
                    splitContainer.SplitterDistance = Math.Max(minMaster, Math.Min(desiredMaster, available - minDetail));
                }

                SetRowHeight(dgvDonVe, px(112));
                SetRowHeight(dgvChiTietVe, px(60));
                LayoutOrderDetails(scale);
                LayoutOrderActions(scale);

                int minBody = pnlDetailInfo.Height + pnlDetailActions.Height + px(120);
                pnlPageContent.Height = Math.Max(ClientSize.Height - Padding.Vertical,
                    (pnlTitle.Visible ? pnlTitle.Height : 0) + pnlHeader.Height + minBody);
            }
            finally { updatingLayout = false; }
        }

        private static void SetRowHeight(DataGridView grid, int height)
        {
            if (grid.RowTemplate.Height != height)
                grid.RowTemplate.Height = height;

            if (grid is Guna.UI2.WinForms.Guna2DataGridView gunaGrid)
            {
                if (gunaGrid.ThemeStyle.RowsStyle.Height != height)
                    gunaGrid.ThemeStyle.RowsStyle.Height = height;
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Height != height)
                    row.Height = height;
            }
        }

        private void LayoutOrderDetails(float scale)
        {
            Func<int, int> px = value => ScalePixels(value, scale);
            int pad = px(20);
            int width = Math.Max(1, pnlDetailInfo.ClientSize.Width - pad * 2);
            int y = px(20);
            Size badgeText = TextRenderer.MeasureText(lblDetailBadge.Text, lblDetailBadge.Font,
                Size.Empty, TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            int badgeWidth = string.IsNullOrEmpty(lblDetailBadge.Text) ? 0 : badgeText.Width + px(24);
            int badgeHeight = Math.Max(lblDetailBadge.Font.Height + px(10), px(28));
            lblDetailTitle.Location = new Point(pad, y + px(2));
            bool stacked = lblDetailTitle.PreferredWidth + badgeWidth + px(16) > width;
            lblDetailBadge.SetBounds(pad + Math.Max(0, width - badgeWidth),
                stacked ? lblDetailTitle.Bottom + px(8) : y, Math.Min(width, badgeWidth), badgeHeight);
            y = Math.Max(lblDetailTitle.Bottom, lblDetailBadge.Bottom) + px(20);

            y = PlaceDetailLabel(lblCustomerName, pad, y, width, px(10), false);
            lblCustomerPhone.Visible = !string.IsNullOrWhiteSpace(lblCustomerPhone.Text) && lblCustomerPhone.Text != "—";
            if (lblCustomerPhone.Visible)
                y = PlaceDetailLabel(lblCustomerPhone, pad, y, width, px(8), false);
            if (!string.IsNullOrWhiteSpace(lblCustomerEmail.Text) && lblCustomerEmail.Visible)
                y = PlaceDetailLabel(lblCustomerEmail, pad, y, width, px(8), false);
            y += px(12);
            y = PlaceDetailLabel(lblDetailEventName, pad, y, width, px(10), true);
            y = PlaceDetailLabel(lblDetailEventTime, pad, y, width, px(12), false);
            y = PlaceDetailLabel(lblRuleBadge, pad, y, width, px(20), true);
            pnlDetailDivider.SetBounds(pad, y, width, 1);
            y += px(16);
            int copyWidth = px(110);
            int titleHeight = Math.Max(lblTicketsTitle.Font.Height + px(8), px(30));
            lblTicketsTitle.SetBounds(pad, y, Math.Max(1, width - copyWidth - px(8)), titleHeight);
            btnCopyMaVe.SetBounds(pad + width - copyWidth, y + px(2), copyWidth, px(28));
            pnlDetailInfo.Height = Math.Max(lblTicketsTitle.Bottom, btnCopyMaVe.Bottom) + px(12);
        }

        private static int PlaceDetailLabel(Label label, int x, int y, int width, int gap, bool wrap)
        {
            int height = label.Font.Height + 4;
            if (wrap)
                height = TextRenderer.MeasureText(label.Text, label.Font, new Size(width, int.MaxValue),
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix).Height + 4;
            label.UseMnemonic = false;
            label.SetBounds(x, y, width, height);
            return label.Bottom + gap;
        }

        private void LayoutOrderActions(float scale)
        {
            Func<int, int> px = value => ScalePixels(value, scale);
            int width = pnlDetailActions.ClientSize.Width;
            int pad = px(16), gap = px(10), height = px(40);
            int printWidth = px(95), exchangeWidth = px(120), cancelWidth = px(100);

            bool allOneLine = width >= pad * 2 + printWidth + exchangeWidth + cancelWidth + gap * 2;
            if (allOneLine)
            {
                btnInVe.SetBounds(pad, px(12), printWidth, height);
                btnDoiVe.SetBounds(btnInVe.Right + gap, px(12), exchangeWidth, height);
                btnHuyVe.SetBounds(Math.Max(pad, width - pad - cancelWidth), px(12), cancelWidth, height);
                pnlDetailActions.Height = btnHuyVe.Bottom + px(12);
            }
            else
            {
                bool twoOnFirstLine = width >= pad * 2 + printWidth + exchangeWidth + gap;
                if (twoOnFirstLine)
                {
                    btnInVe.SetBounds(pad, px(12), printWidth, height);
                    btnDoiVe.SetBounds(btnInVe.Right + gap, px(12), exchangeWidth, height);
                    btnHuyVe.SetBounds(Math.Max(pad, width - pad - cancelWidth), btnDoiVe.Bottom + gap, cancelWidth, height);
                    pnlDetailActions.Height = btnHuyVe.Bottom + px(12);
                }
                else
                {
                    int fullWidth = Math.Max(1, width - pad * 2);
                    btnInVe.SetBounds(pad, px(12), fullWidth, height);
                    btnDoiVe.SetBounds(pad, btnInVe.Bottom + gap, fullWidth, height);
                    btnHuyVe.SetBounds(pad, btnDoiVe.Bottom + gap, fullWidth, height);
                    pnlDetailActions.Height = btnHuyVe.Bottom + px(12);
                }
            }
        }
    }
}

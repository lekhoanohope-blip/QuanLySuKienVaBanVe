using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmDoiVe
    {
        private System.ComponentModel.IContainer components;
        private Panel header, body, seatCard, summaryCard, summaryContent, actions;
        private Label title, customer, eventName, location, count, oldTicket, newTicket, difference, methodLabel, stateLabel;
        private Guna2Button customerDetails, retry, btnXacNhan, btnDong;
        private Guna2ComboBox cboSuatMoi, cboKhuVucMoi, cboPhuongThuc;
        private Guna2TextBox txtTimGhe;
        private Label showLabel, zoneLabel, searchLabel, oldHeading, newHeading, moneyHeading;
        private DataGridView dgvGheTrong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SuspendLayout();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 9.5F);
            BackColor = Color.FromArgb(248, 250, 252);
            Text = "Đổi vé";
            ClientSize = new Size(1000, 700);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;
            header = new Panel(); body = new Panel { AutoScroll = true };
            seatCard = new Panel { BackColor = Color.White };
            summaryCard = new Panel { BackColor = Color.White, AutoScroll = true };
            summaryContent = new Panel { BackColor = Color.White };
            actions = new Panel();
            title = MakeLabel("Đổi vé", 18, true);
            customer = MakeLabel("", 10, true); eventName = MakeLabel("");
            customerDetails = MakeButton("Chi tiết khách hàng", false);
            customerDetails.FillColor = BackColor;
            customerDetails.Click += (s, e) => {
                string info = Convert.ToString(donVeRow["TenKH"]) + "\nSĐT: " + Convert.ToString(donVeRow["SDT"]);
                string email = donVeRow.Table.Columns.Contains("Email") ? Convert.ToString(donVeRow["Email"]) : "";
                if (!string.IsNullOrWhiteSpace(email)) info += "\nEmail: " + email;
                MessageBox.Show(this, info, "Chi tiết khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            showLabel = MakeLabel("Suất diễn", 11, true);
            zoneLabel = MakeLabel("Khu vực"); searchLabel = MakeLabel("Tìm ghế");
            cboSuatMoi = MakeCombo("Suất diễn mới"); cboKhuVucMoi = MakeCombo("Lọc khu vực");
            cboPhuongThuc = MakeCombo("Phương thức thu hoặc hoàn tiền");
            txtTimGhe = new Guna2TextBox { BorderRadius = 6, Font = Font,
                PlaceholderText = "Số ghế, khu vực hoặc hạng vé", MaxLength = 100, AccessibleName = "Tìm ghế" };
            txtTimGhe.FocusedState.BorderColor = Color.FromArgb(13, 148, 136);
            location = MakeLabel(""); count = MakeLabel(""); stateLabel = MakeLabel("Đang tải suất diễn…");
            retry = MakeButton("Thử lại", false); retry.Visible = false;
            retry.Click += async (s, e) => { if (shows == null) await LoadShowsAsync(); else await LoadSeatsAsync(); };
            dgvGheTrong = new DataGridView {
                AutoGenerateColumns = false, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false, ReadOnly = true, MultiSelect = false,
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(241,245,249), EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ScrollBars = ScrollBars.Vertical, AccessibleName = "Danh sách ghế trống"
            };
            dgvGheTrong.DefaultCellStyle = new DataGridViewCellStyle { Font = Font,
                ForeColor = Color.FromArgb(15,23,42), SelectionBackColor = Color.FromArgb(236,253,245),
                SelectionForeColor = Color.FromArgb(15,23,42), Padding = new Padding(10,4,10,4) };
            dgvGheTrong.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font(Font,FontStyle.Bold),
                BackColor = Color.FromArgb(241,245,249), ForeColor = Color.FromArgb(51,65,85), Padding = new Padding(10,0,10,0) };
            dgvGheTrong.Columns.Add(new DataGridViewTextBoxColumn { Name="Seat", DataPropertyName="MaGheHienThi", HeaderText="Ghế", FillWeight=22 });
            dgvGheTrong.Columns.Add(new DataGridViewTextBoxColumn { Name="Zone", DataPropertyName="ZoneDisplay", HeaderText="Khu vực / Hạng", FillWeight=48 });
            dgvGheTrong.Columns.Add(new DataGridViewTextBoxColumn { Name="Price", DataPropertyName="GiaVe", HeaderText="Giá vé", FillWeight=30 });
            dgvGheTrong.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGheTrong.Columns["Price"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGheTrong.CellFormatting += (s,e) => {
                if (e.RowIndex >= 0 && e.ColumnIndex == 2 && e.Value != null) {
                    e.Value = Money(Convert.ToDecimal(e.Value)); e.FormattingApplied = true;
                }
            };
            dgvGheTrong.CurrentCellChanged += (s,e) => { if (!bindingSeats) UpdateSelection(); };
            dgvGheTrong.MouseDown += (s,e) => {
                if (dgvGheTrong.HitTest(e.X,e.Y).Type == DataGridViewHitTestType.None) {
                    dgvGheTrong.ClearSelection(); dgvGheTrong.CurrentCell = null; UpdateSelection();
                }
            };
            oldHeading = MakeLabel("Vé hiện tại",11,true); newHeading = MakeLabel("Vé mới",11,true);
            moneyHeading = MakeLabel("Khoản thu / hoàn",11,true);
            oldTicket = MakeLabel(""); newTicket = MakeLabel("Chọn một ghế bên trái");
            difference = MakeLabel("Hiện sau khi chọn ghế",11,true); methodLabel = MakeLabel("");
            btnDong = MakeButton("Đóng",false); btnXacNhan = MakeButton("Xác nhận đổi vé",true);
            btnXacNhan.Enabled = false;
            btnDong.Click += (s,e) => { if (!dangDoiVe) { DialogResult = DialogResult.Cancel; Close(); } };
            btnXacNhan.Click += ConfirmExchange;
            cboSuatMoi.SelectedIndexChanged += async (s,e) => { if (!bindingShows) await LoadSeatsAsync(); };
            cboKhuVucMoi.SelectedIndexChanged += (s,e) => { if (!bindingSeats) FilterSeats(); };
            txtTimGhe.TextChanged += (s,e) => { if (!bindingSeats) FilterSeats(); };
            header.Controls.AddRange(new Control[] { title, customer, eventName, customerDetails });
            seatCard.Controls.AddRange(new Control[] { showLabel,cboSuatMoi,location,searchLabel,txtTimGhe,zoneLabel,cboKhuVucMoi,dgvGheTrong,count,stateLabel,retry });
            summaryContent.Controls.AddRange(new Control[] {oldHeading,oldTicket,newHeading,newTicket,moneyHeading,difference,methodLabel,cboPhuongThuc});
            summaryCard.Controls.Add(summaryContent);
            actions.Controls.AddRange(new Control[] {btnDong,btnXacNhan});
            body.Controls.AddRange(new Control[] {seatCard,summaryCard});
            Controls.AddRange(new Control[] {header,body,actions});
            AcceptButton = btnXacNhan; CancelButton = btnDong;
            Load += FrmDoiVe_Load;
            ClientSizeChanged += (s,e) => LayoutExchange();
            DpiChanged += (s,e) => LayoutExchange();
            summaryCard.ClientSizeChanged += (s,e) => LayoutSummary();
            ResumeLayout(false);
        }
    }
}

namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmThanhToan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlContainer = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.lblThanhToanNote = new System.Windows.Forms.Label();
            this.btnThanhToan = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuyVe = new Guna.UI2.WinForms.Guna2Button();
            this.pnlSummaryCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTongTienVal = new System.Windows.Forms.Label();
            this.lblTongTienLabel = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.lblSeatNote = new System.Windows.Forms.Label();
            this.pnlGheBadges = new System.Windows.Forms.FlowLayoutPanel();
            this.lblViTriGheLabel = new System.Windows.Forms.Label();
            this.lblSoLuongVal = new System.Windows.Forms.Label();
            this.lblSoLuongLabel = new System.Windows.Forms.Label();
            this.lblKhuVucVal = new System.Windows.Forms.Label();
            this.lblKhuVucLabel = new System.Windows.Forms.Label();
            this.lblSuatVal = new System.Windows.Forms.Label();
            this.lblSuatLabel = new System.Windows.Forms.Label();
            this.lblSKVal = new System.Windows.Forms.Label();
            this.lblSKLabel = new System.Windows.Forms.Label();
            this.lblSummaryTitle = new System.Windows.Forms.Label();
            this.pnlCustomerCard = new Guna.UI2.WinForms.Guna2Panel();
            this.radVisa = new Guna.UI2.WinForms.Guna2RadioButton();
            this.radATM = new Guna.UI2.WinForms.Guna2RadioButton();
            this.radTienMat = new Guna.UI2.WinForms.Guna2RadioButton();
            this.radChuyenKhoan = new Guna.UI2.WinForms.Guna2RadioButton();
            this.lblPaymentMethodTitle = new System.Windows.Forms.Label();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtSDT = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtTenKH = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTenKH = new System.Windows.Forms.Label();
            this.cboKhachHang = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblChonKH = new System.Windows.Forms.Label();
            this.lblCustomerCardTitle = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlSummaryCard.SuspendLayout();
            this.pnlCustomerCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTop.BorderThickness = 1;
            this.pnlTop.Controls.Add(this.lblHeaderSub);
            this.pnlTop.Controls.Add(this.lblHeaderTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 75);
            this.pnlTop.TabIndex = 0;
            // 
            // lblHeaderSub
            // 
            this.lblHeaderSub.AutoSize = true;
            this.lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblHeaderSub.Location = new System.Drawing.Point(22, 42);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Size = new System.Drawing.Size(0, 21);
            this.lblHeaderSub.TabIndex = 1;
            this.lblHeaderSub.Click += new System.EventHandler(this.lblHeaderSub_Click);
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(20, 10);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(169, 37);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "Thanh toán ";
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.pnlBottomButtons);
            this.pnlContainer.Controls.Add(this.pnlSummaryCard);
            this.pnlContainer.Controls.Add(this.pnlCustomerCard);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 75);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContainer.Size = new System.Drawing.Size(1200, 725);
            this.pnlContainer.TabIndex = 1;
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.lblThanhToanNote);
            this.pnlBottomButtons.Controls.Add(this.btnThanhToan);
            this.pnlBottomButtons.Controls.Add(this.btnHuyVe);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(20, 620);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(1160, 85);
            this.pnlBottomButtons.TabIndex = 2;
            // 
            // lblThanhToanNote
            // 
            this.lblThanhToanNote.AutoSize = true;
            this.lblThanhToanNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblThanhToanNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblThanhToanNote.Location = new System.Drawing.Point(742, 68);
            this.lblThanhToanNote.Name = "lblThanhToanNote";
            this.lblThanhToanNote.Size = new System.Drawing.Size(0, 20);
            this.lblThanhToanNote.TabIndex = 2;
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BorderRadius = 8;
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.No;
            this.btnThanhToan.Enabled = false;
            this.btnThanhToan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(740, 18);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(390, 48);
            this.btnThanhToan.TabIndex = 1;
            this.btnThanhToan.Text = "XÁC NHẬN THANH TOÁN ";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // btnHuyVe
            // 
            this.btnHuyVe.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnHuyVe.BorderRadius = 8;
            this.btnHuyVe.BorderThickness = 1;
            this.btnHuyVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuyVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnHuyVe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnHuyVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnHuyVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnHuyVe.Location = new System.Drawing.Point(20, 18);
            this.btnHuyVe.Name = "btnHuyVe";
            this.btnHuyVe.Size = new System.Drawing.Size(200, 48);
            this.btnHuyVe.TabIndex = 0;
            this.btnHuyVe.Text = "← HỦY VÉ";
            // 
            // pnlSummaryCard
            // 
            this.pnlSummaryCard.BackColor = System.Drawing.Color.White;
            this.pnlSummaryCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSummaryCard.BorderRadius = 10;
            this.pnlSummaryCard.BorderThickness = 1;
            this.pnlSummaryCard.Controls.Add(this.lblTongTienVal);
            this.pnlSummaryCard.Controls.Add(this.lblTongTienLabel);
            this.pnlSummaryCard.Controls.Add(this.pnlDivider);
            this.pnlSummaryCard.Controls.Add(this.lblSeatNote);
            this.pnlSummaryCard.Controls.Add(this.pnlGheBadges);
            this.pnlSummaryCard.Controls.Add(this.lblViTriGheLabel);
            this.pnlSummaryCard.Controls.Add(this.lblSoLuongVal);
            this.pnlSummaryCard.Controls.Add(this.lblSoLuongLabel);
            this.pnlSummaryCard.Controls.Add(this.lblKhuVucVal);
            this.pnlSummaryCard.Controls.Add(this.lblKhuVucLabel);
            this.pnlSummaryCard.Controls.Add(this.lblSuatVal);
            this.pnlSummaryCard.Controls.Add(this.lblSuatLabel);
            this.pnlSummaryCard.Controls.Add(this.lblSKVal);
            this.pnlSummaryCard.Controls.Add(this.lblSKLabel);
            this.pnlSummaryCard.Controls.Add(this.lblSummaryTitle);
            this.pnlSummaryCard.Location = new System.Drawing.Point(560, 20);
            this.pnlSummaryCard.Name = "pnlSummaryCard";
            this.pnlSummaryCard.Padding = new System.Windows.Forms.Padding(18);
            this.pnlSummaryCard.Size = new System.Drawing.Size(615, 580);
            this.pnlSummaryCard.TabIndex = 1;
            // 
            // lblTongTienVal
            // 
            this.lblTongTienVal.AutoSize = true;
            this.lblTongTienVal.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTongTienVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblTongTienVal.Location = new System.Drawing.Point(18, 460);
            this.lblTongTienVal.Name = "lblTongTienVal";
            this.lblTongTienVal.Size = new System.Drawing.Size(209, 46);
            this.lblTongTienVal.TabIndex = 14;
            this.lblTongTienVal.Text = "1.000.000 đ";
            // 
            // lblTongTienLabel
            // 
            this.lblTongTienLabel.AutoSize = true;
            this.lblTongTienLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongTienLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTongTienLabel.Location = new System.Drawing.Point(18, 430);
            this.lblTongTienLabel.Name = "lblTongTienLabel";
            this.lblTongTienLabel.Size = new System.Drawing.Size(259, 28);
            this.lblTongTienLabel.TabIndex = 13;
            this.lblTongTienLabel.Text = "TỔNG GIÁ VÉ CUỐI CÙNG:";
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlDivider.Location = new System.Drawing.Point(20, 410);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(570, 1);
            this.pnlDivider.TabIndex = 12;
            // 
            // lblSeatNote
            // 
            this.lblSeatNote.AutoSize = true;
            this.lblSeatNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblSeatNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSeatNote.Location = new System.Drawing.Point(18, 380);
            this.lblSeatNote.Name = "lblSeatNote";
            this.lblSeatNote.Size = new System.Drawing.Size(0, 20);
            this.lblSeatNote.TabIndex = 11;
            // 
            // pnlGheBadges
            // 
            this.pnlGheBadges.AutoScroll = true;
            this.pnlGheBadges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlGheBadges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGheBadges.Location = new System.Drawing.Point(20, 298);
            this.pnlGheBadges.Name = "pnlGheBadges";
            this.pnlGheBadges.Padding = new System.Windows.Forms.Padding(6);
            this.pnlGheBadges.Size = new System.Drawing.Size(570, 75);
            this.pnlGheBadges.TabIndex = 10;
            // 
            // lblViTriGheLabel
            // 
            this.lblViTriGheLabel.AutoSize = true;
            this.lblViTriGheLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblViTriGheLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblViTriGheLabel.Location = new System.Drawing.Point(18, 275);
            this.lblViTriGheLabel.Name = "lblViTriGheLabel";
            this.lblViTriGheLabel.Size = new System.Drawing.Size(125, 21);
            this.lblViTriGheLabel.TabIndex = 9;
            this.lblViTriGheLabel.Text = "Vị trí ghế cụ thể";
            // 
            // lblSoLuongVal
            // 
            this.lblSoLuongVal.AutoSize = true;
            this.lblSoLuongVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSoLuongVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSoLuongVal.Location = new System.Drawing.Point(18, 240);
            this.lblSoLuongVal.Name = "lblSoLuongVal";
            this.lblSoLuongVal.Size = new System.Drawing.Size(50, 25);
            this.lblSoLuongVal.TabIndex = 8;
            this.lblSoLuongVal.Text = "2 Vé";
            // 
            // lblSoLuongLabel
            // 
            this.lblSoLuongLabel.AutoSize = true;
            this.lblSoLuongLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSoLuongLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSoLuongLabel.Location = new System.Drawing.Point(18, 220);
            this.lblSoLuongLabel.Name = "lblSoLuongLabel";
            this.lblSoLuongLabel.Size = new System.Drawing.Size(101, 21);
            this.lblSoLuongLabel.TabIndex = 7;
            this.lblSoLuongLabel.Text = "Số lượng vé:";
            // 
            // lblKhuVucVal
            // 
            this.lblKhuVucVal.AutoSize = true;
            this.lblKhuVucVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblKhuVucVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(51)))), ((int)(((byte)(234)))));
            this.lblKhuVucVal.Location = new System.Drawing.Point(18, 185);
            this.lblKhuVucVal.Name = "lblKhuVucVal";
            this.lblKhuVucVal.Size = new System.Drawing.Size(309, 25);
            this.lblKhuVucVal.TabIndex = 6;
            this.lblKhuVucVal.Text = "VIP A (Hạng VIP - 500.000 đ / vé)";
            // 
            // lblKhuVucLabel
            // 
            this.lblKhuVucLabel.AutoSize = true;
            this.lblKhuVucLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblKhuVucLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKhuVucLabel.Location = new System.Drawing.Point(18, 165);
            this.lblKhuVucLabel.Name = "lblKhuVucLabel";
            this.lblKhuVucLabel.Size = new System.Drawing.Size(147, 21);
            this.lblKhuVucLabel.TabIndex = 5;
            this.lblKhuVucLabel.Text = "Khu vực / Hạng vé:";
            // 
            // lblSuatVal
            // 
            this.lblSuatVal.AutoSize = true;
            this.lblSuatVal.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblSuatVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblSuatVal.Location = new System.Drawing.Point(18, 130);
            this.lblSuatVal.Name = "lblSuatVal";
            this.lblSuatVal.Size = new System.Drawing.Size(543, 23);
            this.lblSuatVal.TabIndex = 4;
            this.lblSuatVal.Text = "30/10/2026 19:30 - Hội trường A (280 An Dương Vương, Q.5, TP.HCM)";
            // 
            // lblSuatLabel
            // 
            this.lblSuatLabel.AutoSize = true;
            this.lblSuatLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSuatLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSuatLabel.Location = new System.Drawing.Point(18, 110);
            this.lblSuatLabel.Name = "lblSuatLabel";
            this.lblSuatLabel.Size = new System.Drawing.Size(154, 21);
            this.lblSuatLabel.TabIndex = 3;
            this.lblSuatLabel.Text = "Suất diễn & Địa điểm:";
            // 
            // lblSKVal
            // 
            this.lblSKVal.AutoSize = true;
            this.lblSKVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSKVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSKVal.Location = new System.Drawing.Point(18, 75);
            this.lblSKVal.Name = "lblSKVal";
            this.lblSKVal.Size = new System.Drawing.Size(323, 25);
            this.lblSKVal.TabIndex = 2;
            this.lblSKVal.Text = "Live Concert: HCMUE Melody 2026";
            // 
            // lblSKLabel
            // 
            this.lblSKLabel.AutoSize = true;
            this.lblSKLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSKLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSKLabel.Location = new System.Drawing.Point(18, 55);
            this.lblSKLabel.Name = "lblSKLabel";
            this.lblSKLabel.Size = new System.Drawing.Size(94, 21);
            this.lblSKLabel.TabIndex = 1;
            this.lblSKLabel.Text = "Tên sự kiện:";
            // 
            // lblSummaryTitle
            // 
            this.lblSummaryTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSummaryTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSummaryTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSummaryTitle.Location = new System.Drawing.Point(18, 18);
            this.lblSummaryTitle.Name = "lblSummaryTitle";
            this.lblSummaryTitle.Size = new System.Drawing.Size(579, 28);
            this.lblSummaryTitle.TabIndex = 0;
            this.lblSummaryTitle.Text = "3. THÔNG TIN VÉ - VỊ TRÍ GHẾ ";
            // 
            // pnlCustomerCard
            // 
            this.pnlCustomerCard.BackColor = System.Drawing.Color.White;
            this.pnlCustomerCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlCustomerCard.BorderRadius = 10;
            this.pnlCustomerCard.BorderThickness = 1;
            this.pnlCustomerCard.Controls.Add(this.radVisa);
            this.pnlCustomerCard.Controls.Add(this.radATM);
            this.pnlCustomerCard.Controls.Add(this.radTienMat);
            this.pnlCustomerCard.Controls.Add(this.radChuyenKhoan);
            this.pnlCustomerCard.Controls.Add(this.lblPaymentMethodTitle);
            this.pnlCustomerCard.Controls.Add(this.txtEmail);
            this.pnlCustomerCard.Controls.Add(this.lblEmail);
            this.pnlCustomerCard.Controls.Add(this.txtSDT);
            this.pnlCustomerCard.Controls.Add(this.lblSDT);
            this.pnlCustomerCard.Controls.Add(this.txtTenKH);
            this.pnlCustomerCard.Controls.Add(this.lblTenKH);
            this.pnlCustomerCard.Controls.Add(this.cboKhachHang);
            this.pnlCustomerCard.Controls.Add(this.lblChonKH);
            this.pnlCustomerCard.Controls.Add(this.lblCustomerCardTitle);
            this.pnlCustomerCard.Location = new System.Drawing.Point(20, 20);
            this.pnlCustomerCard.Name = "pnlCustomerCard";
            this.pnlCustomerCard.Padding = new System.Windows.Forms.Padding(18);
            this.pnlCustomerCard.Size = new System.Drawing.Size(520, 580);
            this.pnlCustomerCard.TabIndex = 0;
            // 
            // radVisa
            // 
            this.radVisa.AutoSize = true;
            this.radVisa.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radVisa.CheckedState.BorderThickness = 0;
            this.radVisa.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radVisa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radVisa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.radVisa.Location = new System.Drawing.Point(25, 495);
            this.radVisa.Name = "radVisa";
            this.radVisa.Size = new System.Drawing.Size(273, 27);
            this.radVisa.TabIndex = 13;
            this.radVisa.Text = "Thẻ Quốc tế (Visa / Mastercard)";
            this.radVisa.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.radVisa.UncheckedState.BorderThickness = 2;
            this.radVisa.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // radATM
            // 
            this.radATM.AutoSize = true;
            this.radATM.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radATM.CheckedState.BorderThickness = 0;
            this.radATM.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radATM.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radATM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.radATM.Location = new System.Drawing.Point(25, 455);
            this.radATM.Name = "radATM";
            this.radATM.Size = new System.Drawing.Size(218, 27);
            this.radATM.TabIndex = 12;
            this.radATM.Text = "Thẻ ATM nội địa (Napas)";
            this.radATM.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.radATM.UncheckedState.BorderThickness = 2;
            this.radATM.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // radTienMat
            // 
            this.radTienMat.AutoSize = true;
            this.radTienMat.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radTienMat.CheckedState.BorderThickness = 0;
            this.radTienMat.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radTienMat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radTienMat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.radTienMat.Location = new System.Drawing.Point(25, 415);
            this.radTienMat.Name = "radTienMat";
            this.radTienMat.Size = new System.Drawing.Size(174, 27);
            this.radTienMat.TabIndex = 11;
            this.radTienMat.Text = "Tiền mặt (tại quầy)";
            this.radTienMat.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.radTienMat.UncheckedState.BorderThickness = 2;
            this.radTienMat.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // radChuyenKhoan
            // 
            this.radChuyenKhoan.AutoSize = true;
            this.radChuyenKhoan.Checked = true;
            this.radChuyenKhoan.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radChuyenKhoan.CheckedState.BorderThickness = 0;
            this.radChuyenKhoan.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.radChuyenKhoan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radChuyenKhoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.radChuyenKhoan.Location = new System.Drawing.Point(25, 375);
            this.radChuyenKhoan.Name = "radChuyenKhoan";
            this.radChuyenKhoan.Size = new System.Drawing.Size(312, 27);
            this.radChuyenKhoan.TabIndex = 10;
            this.radChuyenKhoan.TabStop = true;
            this.radChuyenKhoan.Text = "Chuyển khoản ngân hàng (QR Code)";
            this.radChuyenKhoan.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.radChuyenKhoan.UncheckedState.BorderThickness = 2;
            this.radChuyenKhoan.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // lblPaymentMethodTitle
            // 
            this.lblPaymentMethodTitle.AutoSize = true;
            this.lblPaymentMethodTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethodTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblPaymentMethodTitle.Location = new System.Drawing.Point(18, 340);
            this.lblPaymentMethodTitle.Name = "lblPaymentMethodTitle";
            this.lblPaymentMethodTitle.Size = new System.Drawing.Size(308, 25);
            this.lblPaymentMethodTitle.TabIndex = 9;
            this.lblPaymentMethodTitle.Text = "2. PHƯƠNG THỨC THANH TOÁN";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtEmail.BorderRadius = 6;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtEmail.Location = new System.Drawing.Point(20, 288);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "Nhập địa chỉ email nhận vé điện tử...";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(480, 34);
            this.txtEmail.TabIndex = 8;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblEmail.Location = new System.Drawing.Point(18, 265);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(52, 21);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Email:";
            // 
            // txtSDT
            // 
            this.txtSDT.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtSDT.BorderRadius = 6;
            this.txtSDT.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSDT.DefaultText = "";
            this.txtSDT.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSDT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtSDT.Location = new System.Drawing.Point(20, 218);
            this.txtSDT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.PlaceholderText = "Nhập số điện thoại (ví dụ: 0901234567)...";
            this.txtSDT.SelectedText = "";
            this.txtSDT.Size = new System.Drawing.Size(480, 34);
            this.txtSDT.TabIndex = 6;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblSDT.Location = new System.Drawing.Point(18, 195);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(110, 21);
            this.lblSDT.TabIndex = 5;
            this.lblSDT.Text = "Số điện thoại:";
            // 
            // txtTenKH
            // 
            this.txtTenKH.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtTenKH.BorderRadius = 6;
            this.txtTenKH.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenKH.DefaultText = "";
            this.txtTenKH.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtTenKH.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTenKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtTenKH.Location = new System.Drawing.Point(20, 148);
            this.txtTenKH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTenKH.Name = "txtTenKH";
            this.txtTenKH.PlaceholderText = "Nhập họ tên khách hàng...";
            this.txtTenKH.SelectedText = "";
            this.txtTenKH.Size = new System.Drawing.Size(480, 34);
            this.txtTenKH.TabIndex = 4;
            // 
            // lblTenKH
            // 
            this.lblTenKH.AutoSize = true;
            this.lblTenKH.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblTenKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTenKH.Location = new System.Drawing.Point(18, 125);
            this.lblTenKH.Name = "lblTenKH";
            this.lblTenKH.Size = new System.Drawing.Size(154, 21);
            this.lblTenKH.TabIndex = 3;
            this.lblTenKH.Text = "Họ & tên khách hàng:";
            // 
            // cboKhachHang
            // 
            this.cboKhachHang.BackColor = System.Drawing.Color.Transparent;
            this.cboKhachHang.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.cboKhachHang.BorderRadius = 6;
            this.cboKhachHang.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboKhachHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhachHang.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.cboKhachHang.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.cboKhachHang.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.cboKhachHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cboKhachHang.ItemHeight = 28;
            this.cboKhachHang.Location = new System.Drawing.Point(20, 78);
            this.cboKhachHang.Name = "cboKhachHang";
            this.cboKhachHang.Size = new System.Drawing.Size(480, 34);
            this.cboKhachHang.TabIndex = 2;
            // 
            // lblChonKH
            // 
            this.lblChonKH.AutoSize = true;
            this.lblChonKH.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblChonKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblChonKH.Location = new System.Drawing.Point(18, 55);
            this.lblChonKH.Name = "lblChonKH";
            this.lblChonKH.Size = new System.Drawing.Size(309, 21);
            this.lblChonKH.TabIndex = 1;
            this.lblChonKH.Text = "Chọn khách hàng có sẵn (hoặc nhập mới):";
            // 
            // lblCustomerCardTitle
            // 
            this.lblCustomerCardTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCustomerCardTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCustomerCardTitle.Location = new System.Drawing.Point(18, 18);
            this.lblCustomerCardTitle.Name = "lblCustomerCardTitle";
            this.lblCustomerCardTitle.Size = new System.Drawing.Size(484, 28);
            this.lblCustomerCardTitle.TabIndex = 0;
            this.lblCustomerCardTitle.Text = "1. THÔNG TIN KHÁCH HÀNG ";
            this.lblCustomerCardTitle.Click += new System.EventHandler(this.lblCustomerCardTitle_Click);
            // 
            // FrmThanhToan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmThanhToan";
            this.Text = "Trang Thanh toán";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlContainer.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlBottomButtons.PerformLayout();
            this.pnlSummaryCard.ResumeLayout(false);
            this.pnlSummaryCard.PerformLayout();
            this.pnlCustomerCard.ResumeLayout(false);
            this.pnlCustomerCard.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSub;
        private Guna.UI2.WinForms.Guna2Panel pnlContainer;
        private Guna.UI2.WinForms.Guna2Panel pnlCustomerCard;
        private System.Windows.Forms.Label lblCustomerCardTitle;
        private System.Windows.Forms.Label lblChonKH;
        private Guna.UI2.WinForms.Guna2ComboBox cboKhachHang;
        private System.Windows.Forms.Label lblTenKH;
        private Guna.UI2.WinForms.Guna2TextBox txtTenKH;
        private System.Windows.Forms.Label lblSDT;
        private Guna.UI2.WinForms.Guna2TextBox txtSDT;
        private System.Windows.Forms.Label lblEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private System.Windows.Forms.Label lblPaymentMethodTitle;
        private Guna.UI2.WinForms.Guna2RadioButton radChuyenKhoan;
        private Guna.UI2.WinForms.Guna2RadioButton radTienMat;
        private Guna.UI2.WinForms.Guna2RadioButton radATM;
        private Guna.UI2.WinForms.Guna2RadioButton radVisa;
        private Guna.UI2.WinForms.Guna2Panel pnlSummaryCard;
        private System.Windows.Forms.Label lblSummaryTitle;
        private System.Windows.Forms.Label lblSKLabel;
        private System.Windows.Forms.Label lblSKVal;
        private System.Windows.Forms.Label lblSuatLabel;
        private System.Windows.Forms.Label lblSuatVal;
        private System.Windows.Forms.Label lblKhuVucLabel;
        private System.Windows.Forms.Label lblKhuVucVal;
        private System.Windows.Forms.Label lblSoLuongLabel;
        private System.Windows.Forms.Label lblSoLuongVal;
        private System.Windows.Forms.Label lblViTriGheLabel;
        private System.Windows.Forms.FlowLayoutPanel pnlGheBadges;
        private System.Windows.Forms.Label lblSeatNote;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Label lblTongTienLabel;
        private System.Windows.Forms.Label lblTongTienVal;
        private System.Windows.Forms.Panel pnlBottomButtons;
        private Guna.UI2.WinForms.Guna2Button btnHuyVe;
        private Guna.UI2.WinForms.Guna2Button btnThanhToan;
        private System.Windows.Forms.Label lblThanhToanNote;
    }
}

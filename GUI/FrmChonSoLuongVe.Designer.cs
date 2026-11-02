namespace QuanLySuKienVaBanVe.GUI
{
    partial class FrmChonSoLuongVe
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
            this.pnlCard = new Guna.UI2.WinForms.Guna2Panel();
            this.btnTiepTuc = new Guna.UI2.WinForms.Guna2Button();
            this.lblChonKhuVucKhac = new System.Windows.Forms.Label();
            this.pnlGiftBox = new Guna.UI2.WinForms.Guna2Panel();
            this.lblGiftContent = new System.Windows.Forms.Label();
            this.lblGiftIcon = new System.Windows.Forms.Label();
            this.pnlQtyRow = new System.Windows.Forms.Panel();
            this.btnTang = new Guna.UI2.WinForms.Guna2Button();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.btnGiam = new Guna.UI2.WinForms.Guna2Button();
            this.lblTenKV = new System.Windows.Forms.Label();
            this.lblConLai = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.pnlGiftBox.SuspendLayout();
            this.pnlQtyRow.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCard
            // 
            this.pnlCard.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlCard.BorderRadius = 12;
            this.pnlCard.BorderThickness = 1;
            this.pnlCard.Controls.Add(this.btnTiepTuc);
            this.pnlCard.Controls.Add(this.lblChonKhuVucKhac);
            this.pnlCard.Controls.Add(this.pnlGiftBox);
            this.pnlCard.Controls.Add(this.pnlQtyRow);
            this.pnlCard.Controls.Add(this.lblWarning);
            this.pnlCard.Controls.Add(this.pnlDivider);
            this.pnlCard.Controls.Add(this.btnClose);
            this.pnlCard.Controls.Add(this.lblTitle);
            this.pnlCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard.FillColor = System.Drawing.Color.White;
            this.pnlCard.Location = new System.Drawing.Point(0, 0);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.ShadowDecoration.BorderRadius = 12;
            this.pnlCard.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlCard.ShadowDecoration.Depth = 15;
            this.pnlCard.ShadowDecoration.Enabled = true;
            this.pnlCard.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(4);
            this.pnlCard.Size = new System.Drawing.Size(460, 360);
            this.pnlCard.TabIndex = 0;
            // 
            // btnTiepTuc
            // 
            this.btnTiepTuc.BorderRadius = 6;
            this.btnTiepTuc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTiepTuc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTiepTuc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTiepTuc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTiepTuc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTiepTuc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(194)))), ((int)(((byte)(117)))));
            this.btnTiepTuc.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTiepTuc.ForeColor = System.Drawing.Color.White;
            this.btnTiepTuc.Location = new System.Drawing.Point(24, 292);
            this.btnTiepTuc.Name = "btnTiepTuc";
            this.btnTiepTuc.Size = new System.Drawing.Size(412, 44);
            this.btnTiepTuc.TabIndex = 7;
            this.btnTiepTuc.Text = "Tiếp tục - 0 đ ≫";
            this.btnTiepTuc.Click += new System.EventHandler(this.btnTiepTuc_Click);
            // 
            // lblChonKhuVucKhac
            // 
            this.lblChonKhuVucKhac.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblChonKhuVucKhac.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Underline);
            this.lblChonKhuVucKhac.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblChonKhuVucKhac.Location = new System.Drawing.Point(24, 258);
            this.lblChonKhuVucKhac.Name = "lblChonKhuVucKhac";
            this.lblChonKhuVucKhac.Size = new System.Drawing.Size(412, 24);
            this.lblChonKhuVucKhac.TabIndex = 6;
            this.lblChonKhuVucKhac.Text = "Chọn khu vực khác";
            this.lblChonKhuVucKhac.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblChonKhuVucKhac.Click += new System.EventHandler(this.lblChonKhuVucKhac_Click);
            // 
            // pnlGiftBox
            // 
            this.pnlGiftBox.BackColor = System.Drawing.Color.Transparent;
            this.pnlGiftBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlGiftBox.BorderRadius = 8;
            this.pnlGiftBox.BorderThickness = 1;
            this.pnlGiftBox.Controls.Add(this.lblGiftContent);
            this.pnlGiftBox.Controls.Add(this.lblGiftIcon);
            this.pnlGiftBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlGiftBox.Location = new System.Drawing.Point(24, 185);
            this.pnlGiftBox.Name = "pnlGiftBox";
            this.pnlGiftBox.Size = new System.Drawing.Size(412, 58);
            this.pnlGiftBox.TabIndex = 5;
            // 
            // lblGiftContent
            // 
            this.lblGiftContent.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGiftContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblGiftContent.Location = new System.Drawing.Point(40, 8);
            this.lblGiftContent.Name = "lblGiftContent";
            this.lblGiftContent.Size = new System.Drawing.Size(360, 42);
            this.lblGiftContent.TabIndex = 1;
            this.lblGiftContent.Text = "Bao gồm: Lightstick + Vòng tay vải + Sticker + Lanyard + Khăn Twilly";
            this.lblGiftContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGiftIcon
            // 
            this.lblGiftIcon.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGiftIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblGiftIcon.Location = new System.Drawing.Point(10, 14);
            this.lblGiftIcon.Name = "lblGiftIcon";
            this.lblGiftIcon.Size = new System.Drawing.Size(26, 28);
            this.lblGiftIcon.TabIndex = 0;
            this.lblGiftIcon.Text = "ⓘ";
            this.lblGiftIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlQtyRow
            // 
            this.pnlQtyRow.Controls.Add(this.btnTang);
            this.pnlQtyRow.Controls.Add(this.lblSoLuong);
            this.pnlQtyRow.Controls.Add(this.btnGiam);
            this.pnlQtyRow.Controls.Add(this.lblTenKV);
            this.pnlQtyRow.Controls.Add(this.lblConLai);
            this.pnlQtyRow.Location = new System.Drawing.Point(24, 118);
            this.pnlQtyRow.Name = "pnlQtyRow";
            this.pnlQtyRow.Size = new System.Drawing.Size(412, 52);
            this.pnlQtyRow.TabIndex = 4;
            // 
            // btnTang
            // 
            this.btnTang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTang.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnTang.BorderRadius = 4;
            this.btnTang.BorderThickness = 1;
            this.btnTang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTang.FillColor = System.Drawing.Color.White;
            this.btnTang.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnTang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnTang.Location = new System.Drawing.Point(374, 9);
            this.btnTang.Name = "btnTang";
            this.btnTang.Size = new System.Drawing.Size(34, 34);
            this.btnTang.TabIndex = 4;
            this.btnTang.Text = "+";
            this.btnTang.Click += new System.EventHandler(this.btnTang_Click);
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSoLuong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSoLuong.Location = new System.Drawing.Point(324, 9);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(44, 34);
            this.lblSoLuong.TabIndex = 3;
            this.lblSoLuong.Text = "1";
            this.lblSoLuong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGiam
            // 
            this.btnGiam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGiam.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnGiam.BorderRadius = 4;
            this.btnGiam.BorderThickness = 1;
            this.btnGiam.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGiam.FillColor = System.Drawing.Color.White;
            this.btnGiam.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGiam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnGiam.Location = new System.Drawing.Point(284, 9);
            this.btnGiam.Name = "btnGiam";
            this.btnGiam.Size = new System.Drawing.Size(34, 34);
            this.btnGiam.TabIndex = 2;
            this.btnGiam.Text = "-";
            this.btnGiam.Click += new System.EventHandler(this.btnGiam_Click);
            // 
            // lblTenKV
            // 
            this.lblTenKV.AutoSize = true;
            this.lblTenKV.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTenKV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblTenKV.Location = new System.Drawing.Point(0, 5);
            this.lblTenKV.Name = "lblTenKV";
            this.lblTenKV.Size = new System.Drawing.Size(126, 25);
            this.lblTenKV.TabIndex = 0;
            this.lblTenKV.Text = "Niên Thiếu 1";
            // 
            // lblConLai
            // 
            this.lblConLai.AutoSize = true;
            this.lblConLai.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblConLai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblConLai.Location = new System.Drawing.Point(0, 30);
            this.lblConLai.Name = "lblConLai";
            this.lblConLai.Size = new System.Drawing.Size(117, 20);
            this.lblConLai.TabIndex = 1;
            this.lblConLai.Text = "Còn trống: 20 vé";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblWarning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
            this.lblWarning.Location = new System.Drawing.Point(24, 82);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(296, 20);
            this.lblWarning.TabIndex = 3;
            this.lblWarning.Text = "Lưu ý: Bạn chỉ có thể chọn vé trong 1 khu vực";
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlDivider.Location = new System.Drawing.Point(24, 64);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(412, 1);
            this.pnlDivider.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BorderRadius = 4;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnClose.Location = new System.Drawing.Point(416, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(32, 32);
            this.btnClose.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(24, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(386, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Khu NIEN_THIEU_1";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmChonSoLuongVe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(460, 360);
            this.Controls.Add(this.pnlCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmChonSoLuongVe";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn số lượng vé";
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.pnlGiftBox.ResumeLayout(false);
            this.pnlQtyRow.ResumeLayout(false);
            this.pnlQtyRow.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlCard;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Panel pnlQtyRow;
        private System.Windows.Forms.Label lblTenKV;
        private System.Windows.Forms.Label lblConLai;
        private Guna.UI2.WinForms.Guna2Button btnGiam;
        private System.Windows.Forms.Label lblSoLuong;
        private Guna.UI2.WinForms.Guna2Button btnTang;
        private Guna.UI2.WinForms.Guna2Panel pnlGiftBox;
        private System.Windows.Forms.Label lblGiftIcon;
        private System.Windows.Forms.Label lblGiftContent;
        private System.Windows.Forms.Label lblChonKhuVucKhac;
        private Guna.UI2.WinForms.Guna2Button btnTiepTuc;
    }
}

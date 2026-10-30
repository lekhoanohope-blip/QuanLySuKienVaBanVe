using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using Guna.UI2.WinForms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLyKhuVuc : Form
    {
        public int? MaSuatCanChon { get; set; }

        private int? _editingMaKV = null;
        private Panel seatMapWrapper;
        private bool _isPopulating = false;
        private bool _isDirty = false;
        private bool _isLayingOut = false;
        private Guna.UI2.WinForms.Guna2Button _selectedZoneButton = null;

        // Lưu thông tin mapping giữa Guna2Button trên sơ đồ và dữ liệu KhuVuc
        private readonly Dictionary<Guna.UI2.WinForms.Guna2Button, ZoneButtonMeta> _zoneMetaMap
            = new Dictionary<Guna.UI2.WinForms.Guna2Button, ZoneButtonMeta>();

        private class ZoneButtonMeta
        {
            public ZoneDefinition Definition { get; set; }
            public DataRow DataRow { get; set; }
        }

        private class ZoneDefinition
        {
            public string Code { get; set; }
            public string DisplayName { get; set; }
            public string Category { get; set; }
            public Rectangle Bounds { get; set; }
        }

        public FrmQuanLyKhuVuc(int? maSuat = null)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MaSuatCanChon = maSuat;

            WireDirtyTracking();
            txtTimKiemKV.TextChanged += txtTimKiemKV_TextChanged;
            pnlTopAction.SizeChanged += FrmQuanLyKhuVuc_SizeChanged;
            // Font/DPI autoscaling finishes during layout; measuring earlier can
            // let WinForms scale our freshly calculated bounds a second time.
            Layout += (s, e) => LayoutKhuVucForm();
        }

        private void WireDirtyTracking()
        {
            txtTenKV.TextChanged += (s, e) => { if (!_isPopulating) _isDirty = true; };
            cboLoaiKV.SelectedIndexChanged += (s, e) => { if (!_isPopulating) _isDirty = true; };
            txtSucChua.TextChanged += (s, e) => { if (!_isPopulating) _isDirty = true; };
            txtGiaVe.TextChanged += (s, e) => { if (!_isPopulating) _isDirty = true; };
            toggleBanVe.CheckedChanged += (s, e) => { if (!_isPopulating) _isDirty = true; };
        }

        private void FrmQuanLyKhuVuc_Load(object sender, EventArgs e)
        {
            ApplyPhanQuyen();
            LoadComboBoxSuKien();
            LayoutKhuVucForm();
        }

        private void FrmQuanLyKhuVuc_SizeChanged(object sender, EventArgs e)
        {
            LayoutKhuVucForm();
        }

        private void ApplyPhanQuyen()
        {
            if (!PhienDangNhap.LaQuanLy)
            {
                btnThemKhuVuc.Visible = false;
                pnlRightInfo.Visible = false;
                btnToggleInfo.Visible = false;

                if (dgvKhuVuc.Columns["colThaoTac"] != null)
                    dgvKhuVuc.Columns["colThaoTac"].Visible = false;
            }
        }

        // ============================================================
        // BỐ CỤC CO GIÃN (RESPONSIVE LAYOUT)
        // ============================================================
        private void LayoutKhuVucForm()
        {
            if (_isLayingOut || pnlCards == null || ClientSize.Width <= 0) return;
            _isLayingOut = true;
            SuspendLayout();
            try
            {
                // Match the content inset of the table instead of mixing scaled designer
                // coordinates with fixed runtime pixels.
                int inset = pnlGrid.Padding.Left;
                LayoutTopActions(inset);
                LayoutSummaryCards(inset);
                AdjustGridHeight();
            }
            finally
            {
                ResumeLayout(true);
                _isLayingOut = false;
            }
        }

        private int UiPx(int value) => (int)Math.Round(value * DeviceDpi / 96f);

        private void LayoutTopActions(int inset)
        {
            int gap = UiPx(16), smallGap = UiPx(8);
            int width = Math.Max(1, pnlTopAction.ClientSize.Width - inset * 2);
            int right = inset + width;
            int buttonWidth = Math.Max(UiPx(140), btnThemKhuVuc.PreferredSize.Width + UiPx(24));
            int buttonHeight = Math.Max(UiPx(36), btnThemKhuVuc.Font.Height + UiPx(16));
            bool addBesideTitle = !btnThemKhuVuc.Visible ||
                lblTitle.GetPreferredSize(Size.Empty).Width + gap + buttonWidth <= width;
            int titleWidth = Math.Max(1, width - (addBesideTitle && btnThemKhuVuc.Visible ? buttonWidth + gap : 0));
            lblTitle.AutoSize = false;
            lblTitle.SetBounds(inset, UiPx(8), titleWidth,
                lblTitle.GetPreferredSize(new Size(titleWidth, 0)).Height);
            btnThemKhuVuc.SetBounds(right - buttonWidth, UiPx(8), buttonWidth, buttonHeight);

            int subtitleY = Math.Max(lblTitle.Bottom,
                addBesideTitle && btnThemKhuVuc.Visible ? btnThemKhuVuc.Bottom : lblTitle.Bottom) + smallGap;
            lblSubTitle.AutoSize = false;
            lblSubTitle.SetBounds(inset, subtitleY, width,
                lblSubTitle.GetPreferredSize(new Size(width, 0)).Height);

            int filterY = lblSubTitle.Bottom + gap;
            int controlHeight = Math.Max(UiPx(36), Math.Max(cboSuKien.Height, cboSuatDien.Height));
            int searchWidth = Math.Min(UiPx(240), Math.Max(UiPx(160), width / 5));
            int labelsWidth = lblSuKien.PreferredWidth + lblSuatDien.PreferredWidth + smallGap * 2;
            bool separateSearch = !addBesideTitle || width < labelsWidth + UiPx(440) + searchWidth + gap * 2;
            int filterWidth = width - (separateSearch ? 0 : searchWidth + gap);
            bool stackFilters = filterWidth < labelsWidth + UiPx(300) + gap;
            int comboWidth = Math.Max(1, stackFilters ? filterWidth - Math.Max(lblSuKien.PreferredWidth,
                lblSuatDien.PreferredWidth) - smallGap : (filterWidth - labelsWidth - gap) / 2);

            lblSuKien.Location = new Point(inset, filterY + (controlHeight - lblSuKien.Height) / 2);
            cboSuKien.SetBounds(lblSuKien.Right + smallGap, filterY, comboWidth, controlHeight);
            int showY = stackFilters ? filterY + controlHeight + gap : filterY;
            int showX = stackFilters ? inset : cboSuKien.Right + gap;
            lblSuatDien.Location = new Point(showX, showY + (controlHeight - lblSuatDien.Height) / 2);
            cboSuatDien.SetBounds(lblSuatDien.Right + smallGap, showY, comboWidth, controlHeight);

            int searchY = separateSearch ? showY + controlHeight + gap : filterY;
            if (!addBesideTitle && btnThemKhuVuc.Visible)
            {
                btnThemKhuVuc.Location = new Point(right - buttonWidth, searchY);
                searchWidth = Math.Max(1, Math.Min(searchWidth, width - buttonWidth - gap));
                txtTimKiemKV.SetBounds(right - buttonWidth - gap - searchWidth, searchY, searchWidth, controlHeight);
            }
            else
                txtTimKiemKV.SetBounds(right - searchWidth, searchY, searchWidth, controlHeight);
            pnlTopAction.Height = Math.Max(cboSuatDien.Bottom, txtTimKiemKV.Bottom) + gap;
        }

        private void LayoutSummaryCards(int inset)
        {
            var cards = new[] { pnlCard1, pnlCard2, pnlCard3, pnlCard4 };
            var titles = new[] { lblCard1Title, lblCard2Title, lblCard3Title, lblCard4Title };
            var values = new[] { lblCard1Value, lblCard2Value, lblCard3Value, lblCard4Value };
            int gap = UiPx(16), padding = UiPx(16), top = UiPx(12);
            int width = Math.Max(1, pnlCards.ClientSize.Width - inset * 2);
            int minimumWidth = Math.Max(UiPx(180), values.Max(label => label.GetPreferredSize(Size.Empty).Width) + padding * 2);
            int columns = width >= minimumWidth * 4 + gap * 3 ? 4 : width >= minimumWidth * 2 + gap ? 2 : 1;
            int cardWidth = Math.Max(1, (width - gap * (columns - 1)) / columns);
            int textWidth = Math.Max(1, cardWidth - padding * 2);
            int titleHeight = titles.Max(label => label.GetPreferredSize(new Size(textWidth, 0)).Height);
            int valueHeight = values.Max(label => label.GetPreferredSize(new Size(textWidth, 0)).Height);
            int valueTop = top + titleHeight + UiPx(8);
            int cardHeight = Math.Max(UiPx(88), valueTop + valueHeight + padding);
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].SetBounds(inset + (i % columns) * (cardWidth + gap),
                    UiPx(8) + (i / columns) * (cardHeight + gap), cardWidth, cardHeight);
                titles[i].AutoSize = false;
                titles[i].SetBounds(padding, top, textWidth, titleHeight);
                values[i].AutoSize = false;
                values[i].SetBounds(padding, valueTop, textWidth, valueHeight);
            }
            int rows = (cards.Length + columns - 1) / columns;
            pnlCards.Height = UiPx(8) + rows * cardHeight + (rows - 1) * gap + UiPx(20);
        }

        private void AdjustGridHeight()
        {
            if (dgvKhuVuc == null || pnlGrid == null) return;
            int rows = dgvKhuVuc.Rows.Count;
            int needed = 30 + 36 + Math.Max(1, rows) * 40 + 22;
            int h = Math.Min(240, Math.Max(140, needed));
            if (pnlGrid.Height != h)
            {
                pnlGrid.Height = h;
            }
        }

        private void ScrollToBottomPanel()
        {
            try
            {
                this.AutoScrollPosition = new Point(0, pnlBottom.Top);
            }
            catch { }
        }

        // ============================================================
        // LOAD DỮ LIỆU DROPDOWN
        // ============================================================
        private void LoadComboBoxSuKien()
        {
            try
            {
                DataTable dt = SuKienBLL.LayDanhSach();

                cboSuKien.SelectedIndexChanged -= cboSuKien_SelectedIndexChanged;
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
                cboSuKien.DataSource = dt;

                AdjustDropDownWidth(cboSuKien, dt, "TenSK");

                int selectedMaSK = 0;
                if (MaSuatCanChon.HasValue)
                {
                    SuatDienDTO sd = SuatDienBLL.LayTheoMa(MaSuatCanChon.Value);
                    if (sd != null) selectedMaSK = sd.MaSK;
                }

                if (selectedMaSK > 0)
                {
                    cboSuKien.SelectedValue = selectedMaSK;
                }
                else if (cboSuKien.Items.Count > 0)
                {
                    cboSuKien.SelectedIndex = 0;
                }

                cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;
                LoadComboBoxSuatDien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!PromptDiscardChanges())
            {
                // Revert selected index nếu người dùng chọn Cancel
                return;
            }
            LoadComboBoxSuatDien();
        }

        private void LoadComboBoxSuatDien()
        {
            if (cboSuKien.SelectedValue == null || cboSuKien.SelectedValue is DataRowView)
            {
                cboSuatDien.DataSource = null;
                ResetForEmptyShowtime();
                return;
            }

            try
            {
                int maSK = Convert.ToInt32(cboSuKien.SelectedValue);
                DataTable dt = SuatDienBLL.LayDanhSachTheoSuKien(maSK);

                cboSuatDien.SelectedIndexChanged -= cboSuatDien_SelectedIndexChanged;
                cboSuatDien.DataSource = null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    cboSuatDien.DisplayMember = "HienThiSuat";
                    cboSuatDien.ValueMember = "MaSuat";
                    cboSuatDien.DataSource = dt;
                    AdjustDropDownWidth(cboSuatDien, dt, "HienThiSuat");

                    if (MaSuatCanChon.HasValue && dt.AsEnumerable().Any(r => Convert.ToInt32(r["MaSuat"]) == MaSuatCanChon.Value))
                    {
                        cboSuatDien.SelectedValue = MaSuatCanChon.Value;
                        MaSuatCanChon = null; // dùng xong reset
                    }
                    else
                    {
                        cboSuatDien.SelectedIndex = 0;
                    }

                    cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;
                    LoadData();
                }
                else
                {
                    cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;
                    ResetForEmptyShowtime();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách suất diễn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSuatDien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!PromptDiscardChanges()) return;
            LoadData();
        }

        private void AdjustDropDownWidth(ComboBox cbo, DataTable dt, string columnName)
        {
            if (dt == null || dt.Rows.Count == 0 || string.IsNullOrEmpty(columnName)) return;
            int maxWidth = cbo.Width;
            using (Graphics g = cbo.CreateGraphics())
            {
                foreach (DataRow row in dt.Rows)
                {
                    string text = row[columnName]?.ToString() ?? "";
                    int w = (int)g.MeasureString(text, cbo.Font).Width + 35;
                    if (w > maxWidth) maxWidth = w;
                }
            }
            cbo.DropDownWidth = maxWidth;
        }

        private void ResetForEmptyShowtime()
        {
            dgvKhuVuc.Rows.Clear();
            if (seatMapWrapper != null) seatMapWrapper.Controls.Clear();
            _zoneMetaMap.Clear();
            pnlLegend.Visible = false;

            lblCard1Value.Text = "0";
            lblCard2Value.Text = "0";
            lblCard3Value.Text = "0";
            lblCard4Value.Text = "0 đ";

            ClearInput(force: true);
            lblTrangThaiForm.Text = "Sự kiện chưa có suất diễn hợp lệ";
            btnLuu.Enabled = false;
        }

        // ============================================================
        // NẠP DỮ LIỆU CHÍNH (LOAD DATA)
        // ============================================================
        private void LoadData()
        {
            if (cboSuatDien.SelectedValue == null || cboSuatDien.SelectedValue is DataRowView)
            {
                ResetForEmptyShowtime();
                return;
            }

            try
            {
                int maSuat = Convert.ToInt32(cboSuatDien.SelectedValue);

                // 1. Lấy thông tin suất diễn để vẽ sơ đồ
                SuatDienDTO sd = SuatDienBLL.LayTheoMa(maSuat);
                int loaiSoDo = sd != null ? sd.LoaiSoDo : 1;

                // 2. Lấy dữ liệu khu vực từ BLL
                DataTable dt = KhuVucBLL.LayDanhSachTheoSuat(maSuat);

                // 3. Vẽ sơ đồ ghế & ánh xạ vùng
                DrawSeatMap(loaiSoDo, dt);

                // 4. Nạp dữ liệu bảng
                PopulateGrid(dt);

                // 5. Cập nhật thẻ thống kê & chú thích
                UpdateCardsAndLegend(dt);
                LayoutKhuVucForm();

                btnLuu.Enabled = PhienDangNhap.LaQuanLy;

                // 6. Xóa form nhập và sẵn sàng
                ClearInput(force: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải khu vực: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateGrid(DataTable dt)
        {
            dgvKhuVuc.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                int idx = dgvKhuVuc.Rows.Add();
                dgvKhuVuc.Rows[idx].Cells["colMaKV"].Value = row["MaKV"];
                dgvKhuVuc.Rows[idx].Cells["colMaVung"].Value = row["MaVung"];
                dgvKhuVuc.Rows[idx].Cells["colTenKV"].Value = row["TenKV"];
                dgvKhuVuc.Rows[idx].Cells["colLoaiKV"].Value = row["HangVe"];
                dgvKhuVuc.Rows[idx].Cells["colSucChua"].Value = Convert.ToInt32(row["TongSoGhe"]).ToString("N0");
                dgvKhuVuc.Rows[idx].Cells["colGiaVe"].Value = Convert.ToDecimal(row["GiaVe"]).ToString("N0") + " đ";
                dgvKhuVuc.Rows[idx].Cells["colDaBan"].Value = row["DaBan"];
                dgvKhuVuc.Rows[idx].Cells["colDangGiu"].Value = row["DangGiu"];
                dgvKhuVuc.Rows[idx].Cells["colConLai"].Value = row["ConLai"];

                bool choPhep = Convert.ToBoolean(row["ChoPhepBanVe"]);
                dgvKhuVuc.Rows[idx].Cells["colTrangThai"].Value = choPhep ? "Đang mở bán" : "Đang đóng";
                dgvKhuVuc.Rows[idx].Tag = row;
            }

            dgvKhuVuc.ClearSelection();
        }

        // ============================================================
        // VẼ SƠ ĐỒ PHÂN KHU & ÁNH XẠ VÙNG
        // ============================================================
        private void DrawSeatMap(int loaiSoDo, DataTable dtKhuVuc)
        {
            if (seatMapWrapper == null)
            {
                seatMapWrapper = new Panel { BackColor = Color.White };
                pnlSeatMap.Controls.Add(seatMapWrapper);
            }

            seatMapWrapper.Controls.Clear();
            _zoneMetaMap.Clear();
            _selectedZoneButton = null;

            List<ZoneDefinition> definitions = GetZoneDefinitions(loaiSoDo);

            // Nạp danh sách vùng vào combobox trên form
            _isPopulating = true;
            cboMaVung.DataSource = null;
            cboMaVung.DisplayMember = "DisplayName";
            cboMaVung.ValueMember = "Code";
            cboMaVung.DataSource = definitions;
            _isPopulating = false;

            // 1. Thêm các khối sân khấu & lối đi (Chỉ trang trí tĩnh, không click)
            DrawStagesAndPassways(loaiSoDo);

            // 2. Thêm các nút vùng
            foreach (var zone in definitions)
            {
                // Tìm dòng dữ liệu đã cấu hình tương ứng
                DataRow matchedRow = null;
                foreach (DataRow row in dtKhuVuc.Rows)
                {
                    string dbMaVung = row["MaVung"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(dbMaVung) && dbMaVung.Equals(zone.Code, StringComparison.OrdinalIgnoreCase))
                    {
                        matchedRow = row;
                        break;
                    }
                }

                // Fallback nếu dữ liệu cũ chưa có MaVung: so sánh tên hoặc hạng vé
                if (matchedRow == null)
                {
                    foreach (DataRow row in dtKhuVuc.Rows)
                    {
                        string ten = row["TenKV"]?.ToString() ?? "";
                        if (ten.IndexOf(zone.DisplayName, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matchedRow = row;
                            break;
                        }
                    }
                }

                float fontSize = zone.Bounds.Width < 80 ? 8.5F : 9.5F;
                Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button
                {
                    Location = zone.Bounds.Location,
                    Size = zone.Bounds.Size,
                    BorderRadius = 6,
                    Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                    Tag = zone.Code
                };

                // Lấy mã Vùng / tên ngắn gọn làm tiêu đề trên sơ đồ để tránh rối mắt
                string vungName = zone.DisplayName;
                if (matchedRow != null && matchedRow["MaVung"] != DBNull.Value)
                {
                    string mv = matchedRow["MaVung"].ToString().Trim();
                    if (!string.IsNullOrEmpty(mv))
                        vungName = mv.Replace("_", " ");
                }

                if (matchedRow != null)
                {
                    // Vùng đã cấu hình: tô màu theo hạng vé
                    string hangVe = matchedRow["HangVe"]?.ToString() ?? "VIP";
                    Color baseColor = GetCategoryColor(hangVe);
                    decimal gia = Convert.ToDecimal(matchedRow["GiaVe"]);
                    int conLai = Convert.ToInt32(matchedRow["ConLai"]);
                    bool choPhep = Convert.ToBoolean(matchedRow["ChoPhepBanVe"]);

                    btn.FillColor = baseColor;
                    btn.ForeColor = Color.White;
                    btn.BorderColor = Color.Transparent;
                    btn.BorderThickness = 0;

                    string statusSuffix = choPhep ? "" : " [Đóng]";
                    string giaFormatted = zone.Bounds.Width < 80 ? $"{gia:N0}đ" : $"{gia:N0} đ";
                    btn.Text = $"{vungName}\n{giaFormatted}{statusSuffix}";

                    // Tooltip hiển thị đầy đủ thông tin khi rê chuột
                    ToolTip tt = new ToolTip();
                    string tenKV = matchedRow["TenKV"]?.ToString() ?? "";
                    tt.SetToolTip(btn, $"{vungName}: {tenKV}\nHạng vé: {hangVe}\nGiá vé: {gia:N0} đ\nCòn trống: {conLai} ghế");
                }
                else
                {
                    // Vùng chưa cấu hình: hiển thị màu nhạt, viền nét mỏng
                    btn.FillColor = Color.FromArgb(248, 250, 252);
                    btn.ForeColor = Color.FromArgb(100, 116, 139);
                    btn.BorderColor = Color.FromArgb(203, 213, 225);
                    btn.BorderThickness = 1;
                    btn.Text = $"{vungName}\n(Chưa thiết lập)";

                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(btn, $"{vungName} (Chưa thiết lập)\nHạng vé đề xuất: {zone.Category}");
                }

                btn.Click += Zone_Click;
                seatMapWrapper.Controls.Add(btn);

                _zoneMetaMap[btn] = new ZoneButtonMeta
                {
                    Definition = zone,
                    DataRow = matchedRow
                };
            }

            seatMapWrapper.Controls.Add(pnlLegend);
            pnlLegend.Visible = true;

            PositionLegendAndResize();
            CenterSeatMap();
        }

        private void DrawStagesAndPassways(int loaiSoDo)
        {
            Color clrStage = Color.FromArgb(226, 232, 240);
            Color clrText = Color.FromArgb(71, 85, 105);

            if (loaiSoDo == 1)
            {
                AddDecorativeBlock(93, 39, 413, 80, "SÂN KHẤU", clrStage, clrText);
                AddDecorativeBlock(256, 120, 80, 215, "LỐI ĐI", clrStage, clrText);
                AddDecorativeBlock(128, 335, 329, 65, "SÂN KHẤU PHỤ", clrStage, clrText);
            }
            else if (loaiSoDo == 2)
            {
                AddDecorativeBlock(130, 30, 400, 60, "SÂN KHẤU", clrStage, clrText);
            }
            else if (loaiSoDo == 3)
            {
                AddDecorativeBlock(150, 10, 400, 60, "SÂN KHẤU", clrStage, clrText);
            }
        }

        private void AddDecorativeBlock(int x, int y, int w, int h, string text, Color fill, Color fore)
        {
            Label lbl = new Label
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = fill,
                ForeColor = fore,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BorderStyle = BorderStyle.None
            };
            seatMapWrapper.Controls.Add(lbl);
        }

        private List<ZoneDefinition> GetZoneDefinitions(int loaiSoDo)
        {
            var list = new List<ZoneDefinition>();
            if (loaiSoDo == 1)
            {
                list.Add(new ZoneDefinition { Code = "VIP_A", DisplayName = "VIP A", Category = "VIP", Bounds = new Rectangle(93, 160, 142, 129) });
                list.Add(new ZoneDefinition { Code = "VIP_B", DisplayName = "VIP B", Category = "VIP", Bounds = new Rectangle(362, 160, 132, 129) });
                list.Add(new ZoneDefinition { Code = "GA_A", DisplayName = "GA A", Category = "Phổ thông", Bounds = new Rectangle(17, 96, 70, 278) });
                list.Add(new ZoneDefinition { Code = "GA_B", DisplayName = "GA B", Category = "Phổ thông", Bounds = new Rectangle(514, 96, 70, 278) });
                list.Add(new ZoneDefinition { Code = "CAT", DisplayName = "CAT", Category = "Thường", Bounds = new Rectangle(69, 410, 464, 66) });
            }
            else if (loaiSoDo == 2)
            {
                list.Add(new ZoneDefinition { Code = "GA_A", DisplayName = "GA A", Category = "Phổ thông", Bounds = new Rectangle(40, 120, 80, 60) });
                list.Add(new ZoneDefinition { Code = "VIP_B", DisplayName = "VIP B", Category = "VIP", Bounds = new Rectangle(140, 120, 80, 60) });
                list.Add(new ZoneDefinition { Code = "VIP_A", DisplayName = "VIP A", Category = "VIP", Bounds = new Rectangle(240, 120, 180, 60) });
                list.Add(new ZoneDefinition { Code = "VIP_C", DisplayName = "VIP C", Category = "VIP", Bounds = new Rectangle(440, 120, 80, 60) });
                list.Add(new ZoneDefinition { Code = "GA_B", DisplayName = "GA B", Category = "Phổ thông", Bounds = new Rectangle(540, 120, 80, 60) });
                list.Add(new ZoneDefinition { Code = "GA_C", DisplayName = "GA C", Category = "Phổ thông", Bounds = new Rectangle(40, 200, 180, 70) });
                list.Add(new ZoneDefinition { Code = "VIP_D", DisplayName = "VIP D", Category = "VIP", Bounds = new Rectangle(240, 200, 180, 60) });
                list.Add(new ZoneDefinition { Code = "GA_D", DisplayName = "GA D", Category = "Phổ thông", Bounds = new Rectangle(440, 200, 180, 70) });
                list.Add(new ZoneDefinition { Code = "CAT_B", DisplayName = "CAT B", Category = "Thường", Bounds = new Rectangle(40, 290, 170, 80) });
                list.Add(new ZoneDefinition { Code = "GA_E", DisplayName = "GA E", Category = "Phổ thông", Bounds = new Rectangle(240, 280, 180, 60) });
                list.Add(new ZoneDefinition { Code = "CAT_C", DisplayName = "CAT C", Category = "Thường", Bounds = new Rectangle(440, 290, 170, 80) });
                list.Add(new ZoneDefinition { Code = "CAT_A", DisplayName = "CAT A", Category = "Thường", Bounds = new Rectangle(240, 360, 180, 40) });
            }
            else
            {
                list.Add(new ZoneDefinition { Code = "VIP_A", DisplayName = "VIP A", Category = "VIP", Bounds = new Rectangle(150, 90, 90, 35) });
                list.Add(new ZoneDefinition { Code = "VIP_B", DisplayName = "VIP B", Category = "VIP", Bounds = new Rectangle(250, 90, 90, 35) });
                list.Add(new ZoneDefinition { Code = "VIP_C", DisplayName = "VIP C", Category = "VIP", Bounds = new Rectangle(350, 90, 90, 35) });
                list.Add(new ZoneDefinition { Code = "VIP_D", DisplayName = "VIP D", Category = "VIP", Bounds = new Rectangle(450, 90, 90, 35) });

                list.Add(new ZoneDefinition { Code = "GA_A", DisplayName = "GA A", Category = "Phổ thông", Bounds = new Rectangle(150, 140, 90, 60) });
                list.Add(new ZoneDefinition { Code = "GA_B", DisplayName = "GA B", Category = "Phổ thông", Bounds = new Rectangle(250, 140, 90, 60) });
                list.Add(new ZoneDefinition { Code = "GA_C", DisplayName = "GA C", Category = "Phổ thông", Bounds = new Rectangle(350, 140, 90, 60) });
                list.Add(new ZoneDefinition { Code = "GA_D", DisplayName = "GA D", Category = "Phổ thông", Bounds = new Rectangle(450, 140, 90, 60) });

                list.Add(new ZoneDefinition { Code = "GA_E", DisplayName = "GA E", Category = "Phổ thông", Bounds = new Rectangle(150, 220, 90, 60) });
                list.Add(new ZoneDefinition { Code = "GA_F", DisplayName = "GA F", Category = "Phổ thông", Bounds = new Rectangle(250, 220, 90, 60) });
                list.Add(new ZoneDefinition { Code = "GA_G", DisplayName = "GA G", Category = "Phổ thông", Bounds = new Rectangle(350, 220, 90, 60) });
                list.Add(new ZoneDefinition { Code = "GA_H", DisplayName = "GA H", Category = "Phổ thông", Bounds = new Rectangle(450, 220, 90, 60) });

                list.Add(new ZoneDefinition { Code = "CAT_C", DisplayName = "CAT C", Category = "Thường", Bounds = new Rectangle(20, 90, 100, 190) });
                list.Add(new ZoneDefinition { Code = "CAT_D", DisplayName = "CAT D", Category = "Thường", Bounds = new Rectangle(580, 90, 100, 190) });
                list.Add(new ZoneDefinition { Code = "CAT_A", DisplayName = "CAT A", Category = "Thường", Bounds = new Rectangle(150, 300, 190, 70) });
                list.Add(new ZoneDefinition { Code = "CAT_B", DisplayName = "CAT B", Category = "Thường", Bounds = new Rectangle(360, 300, 190, 70) });
            }
            return list;
        }

        private Color GetCategoryColor(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Color.FromArgb(245, 158, 11);

            string cat = category.Trim();

            // 1. VIP (Tím)
            if (cat.IndexOf("VIP", StringComparison.OrdinalIgnoreCase) >= 0)
                return Color.FromArgb(147, 51, 234);

            // 2. Phổ thông / GA (Xanh dương) - kiểm tra cả có dấu lẫn không dấu
            if (cat.IndexOf("Phổ thông", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("Pho thong", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("GA", StringComparison.OrdinalIgnoreCase) >= 0)
                return Color.FromArgb(56, 189, 248);

            // 3. Thường / CAT (Cam)
            return Color.FromArgb(245, 158, 11);
        }

        private void PositionLegendAndResize()
        {
            int maxRight = 0, maxBottom = 0;
            foreach (Control c in seatMapWrapper.Controls)
            {
                if (c == pnlLegend) continue;
                maxRight = Math.Max(maxRight, c.Right);
                maxBottom = Math.Max(maxBottom, c.Bottom);
            }

            pnlLegend.Location = new Point(maxRight + 25, 10);
            pnlLegend.Size = new Size(220, 500);

            int wrapperWidth = maxRight + 25 + 220 + 20;
            int wrapperHeight = Math.Max(maxBottom + 20, 500);
            seatMapWrapper.Size = new Size(wrapperWidth, wrapperHeight);
        }

        private void CenterSeatMap()
        {
            if (seatMapWrapper == null || pnlSeatMap == null) return;

            int offsetX = (pnlSeatMap.ClientSize.Width - seatMapWrapper.Width) / 2;
            int offsetY = (pnlSeatMap.ClientSize.Height - seatMapWrapper.Height) / 2;
            if (offsetX < 10) offsetX = 10;
            if (offsetY < 10) offsetY = 10;

            seatMapWrapper.Location = new Point(offsetX, offsetY);
        }

        private void pnlSeatMap_Resize(object sender, EventArgs e)
        {
            CenterSeatMap();
        }

        // ============================================================
        // CLICK VÙNG TRÊN SƠ ĐỒ & ĐỒNG BỘ BẢNG - FORM
        // ============================================================
        private void Zone_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as Guna.UI2.WinForms.Guna2Button;
            if (clickedBtn == null) return;

            if (_isDirty && !PromptDiscardChanges()) return;

            SelectZoneButton(clickedBtn);

            if (!_zoneMetaMap.TryGetValue(clickedBtn, out ZoneButtonMeta meta)) return;

            if (meta.DataRow != null)
            {
                // Vùng ĐÃ CẤU HÌNH -> Chế độ cập nhật
                int maKV = Convert.ToInt32(meta.DataRow["MaKV"]);
                SelectGridRowByMaKV(maKV);
                PopulateInputsForEdit(meta.DataRow, meta.Definition);
            }
            else
            {
                // Vùng CHƯA CẤU HÌNH -> Chế độ thiết lập mới
                dgvKhuVuc.ClearSelection();
                PopulateInputsForNew(meta.Definition);
            }

            ScrollToBottomPanel();
        }

        private void SelectZoneButton(Guna2Button btn)
        {
            if (seatMapWrapper != null)
            {
                foreach (Control ctrl in seatMapWrapper.Controls)
                {
                    if (ctrl is Guna2Button b && _zoneMetaMap.ContainsKey(b))
                    {
                        var m = _zoneMetaMap[b];
                        b.BorderColor = m.DataRow != null ? Color.Transparent : Color.FromArgb(203, 213, 225);
                        b.BorderThickness = m.DataRow != null ? 0 : 1;
                    }
                }
            }

            _selectedZoneButton = btn;
            if (btn != null)
            {
                btn.BorderColor = Color.FromArgb(16, 185, 129);
                btn.BorderThickness = 3;
            }
        }

        private void SelectGridRowByMaKV(int maKV)
        {
            foreach (DataGridViewRow row in dgvKhuVuc.Rows)
            {
                if (row.Cells["colMaKV"].Value != null &&
                    Convert.ToInt32(row.Cells["colMaKV"].Value) == maKV)
                {
                    row.Selected = true;
                    dgvKhuVuc.CurrentCell = row.Cells["colTenKV"];
                    break;
                }
            }
        }

        private void dgvKhuVuc_SelectionChanged(object sender, EventArgs e)
        {
            if (_isPopulating) return;
            if (dgvKhuVuc.CurrentRow == null || dgvKhuVuc.CurrentRow.Index < 0) return;

            DataRow row = dgvKhuVuc.CurrentRow.Tag as DataRow;
            if (row == null) return;

            string maVung = row["MaVung"]?.ToString() ?? "";

            // Tìm nút tương ứng trên sơ đồ để làm nổi bật
            Guna2Button matchedBtn = null;
            foreach (var kvp in _zoneMetaMap)
            {
                if (kvp.Value.DataRow != null && Convert.ToInt32(kvp.Value.DataRow["MaKV"]) == Convert.ToInt32(row["MaKV"]))
                {
                    matchedBtn = kvp.Key;
                    break;
                }
            }

            SelectZoneButton(matchedBtn);

            ZoneDefinition def = matchedBtn != null && _zoneMetaMap.ContainsKey(matchedBtn) ? _zoneMetaMap[matchedBtn].Definition : null;
            PopulateInputsForEdit(row, def);
        }

        private void PopulateInputsForEdit(DataRow row, ZoneDefinition def)
        {
            _isPopulating = true;
            _editingMaKV = Convert.ToInt32(row["MaKV"]);

            lblTrangThaiForm.Text = $"Cập nhật khu vực: {row["TenKV"]} ({row["MaVung"]})";
            btnLuu.Text = "Lưu thay đổi";

            string maVung = row["MaVung"]?.ToString() ?? (def != null ? def.Code : "");
            cboMaVung.SelectedValue = maVung;
            txtTenKV.Text = row["TenKV"]?.ToString() ?? "";
            cboLoaiKV.Text = row["HangVe"]?.ToString() ?? "VIP";

            int tongSoGhe = Convert.ToInt32(row["TongSoGhe"]);
            int daBan = Convert.ToInt32(row["DaBan"]);
            int dangGiu = Convert.ToInt32(row["DangGiu"]);
            int conLai = Convert.ToInt32(row["ConLai"]);

            txtSucChua.Text = tongSoGhe.ToString();
            txtSucChua.Enabled = PhienDangNhap.LaQuanLy;

            lblSucChuaHint.Text = $"Hiện có {tongSoGhe} ghế: {daBan} đã bán, {dangGiu} đang giữ, {conLai} trống";

            decimal gia = Convert.ToDecimal(row["GiaVe"]);
            txtGiaVe.Text = gia.ToString("N0");

            toggleBanVe.Checked = Convert.ToBoolean(row["ChoPhepBanVe"]);

            _isPopulating = false;
            _isDirty = false;
        }

        private void PopulateInputsForNew(ZoneDefinition def)
        {
            _isPopulating = true;
            _editingMaKV = null;

            string code = def != null ? def.Code : "";
            string name = def != null ? def.DisplayName : "";
            string cat = def != null ? def.Category : "VIP";

            lblTrangThaiForm.Text = $"Thiết lập khu vực: {name}";
            btnLuu.Text = "Tạo khu vực và sinh ghế";

            cboMaVung.SelectedValue = code;
            txtTenKV.Text = name;
            cboLoaiKV.Text = cat;
            txtSucChua.Text = "20";
            txtSucChua.Enabled = PhienDangNhap.LaQuanLy;
            lblSucChuaHint.Text = "Tự động sinh ma trận ghế (10 ghế mỗi hàng)";
            txtGiaVe.Text = "";
            toggleBanVe.Checked = true;

            _isPopulating = false;
            _isDirty = false;

            txtGiaVe.Focus();
        }

        private void cboMaVung_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isPopulating) return;
            if (cboMaVung.SelectedValue == null) return;

            string selectedCode = cboMaVung.SelectedValue.ToString();

            // Tìm nút trên sơ đồ tương ứng với mã vùng này
            foreach (var kvp in _zoneMetaMap)
            {
                if (kvp.Value.Definition.Code.Equals(selectedCode, StringComparison.OrdinalIgnoreCase))
                {
                    Zone_Click(kvp.Key, EventArgs.Empty);
                    break;
                }
            }
        }

        // ============================================================
        // NÚT THÊM / LƯU / HỦY
        // ============================================================
        private void btnThemKhuVuc_Click(object sender, EventArgs e)
        {
            if (_isDirty && !PromptDiscardChanges()) return;

            // 1. Mở panel thông tin nếu đang đóng
            if (!pnlRightInfo.Visible)
            {
                pnlRightInfo.Visible = true;
                btnToggleInfo.Text = ">";
                LayoutKhuVucForm();
            }

            // 2. Tìm vùng chưa cấu hình đầu tiên trên sơ đồ
            Guna2Button firstEmpty = null;
            foreach (var kvp in _zoneMetaMap)
            {
                if (kvp.Value.DataRow == null)
                {
                    firstEmpty = kvp.Key;
                    break;
                }
            }

            if (firstEmpty != null)
            {
                Zone_Click(firstEmpty, EventArgs.Empty);
                ScrollToBottomPanel();
            }
            else
            {
                MessageBox.Show("Tất cả các vùng trên sơ đồ của suất diễn này đã được thiết lập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboSuatDien.SelectedValue == null || cboSuatDien.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn suất diễn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKV.Text))
            {
                MessageBox.Show("Tên khu vực không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKV.Focus();
                return;
            }

            string maVung = cboMaVung.SelectedValue?.ToString() ?? "";

            decimal giaVe;
            string cleanGia = txtGiaVe.Text.Replace(".", "").Replace(",", "").Replace("đ", "").Trim();
            if (!decimal.TryParse(cleanGia, out giaVe) || giaVe < 0)
            {
                MessageBox.Show("Giá vé không hợp lệ (phải là số >= 0).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaVe.Focus();
                return;
            }

            int sucChua;
            if (!int.TryParse(txtSucChua.Text.Trim(), out sucChua) || sucChua <= 0 || sucChua > 2600)
            {
                MessageBox.Show("Sức chứa phải là số nguyên dương từ 1 đến 2.600 ghế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSucChua.Focus();
                return;
            }

            int maSuat = Convert.ToInt32(cboSuatDien.SelectedValue);
            bool choPhepBanVe = toggleBanVe.Checked;

            KhuVucDTO kv = new KhuVucDTO
            {
                MaKV = _editingMaKV ?? 0,
                MaSuat = maSuat,
                MaVung = maVung,
                TenKV = txtTenKV.Text.Trim(),
                HangVe = cboLoaiKV.Text,
                GiaVe = giaVe,
                TongSoGhe = sucChua,
                ChoPhepBanVe = choPhepBanVe
            };

            string result = _editingMaKV == null ? KhuVucBLL.Them(kv) : KhuVucBLL.Sua(kv);
            if (result == "Success")
            {
                MessageBox.Show(_editingMaKV == null
                    ? "Thêm khu vực và sinh ghế thành công."
                    : "Cập nhật khu vực thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _isDirty = false;
                string currentVung = maVung;
                LoadData();

                // Tự động chọn lại vùng vừa lưu
                foreach (var kvp in _zoneMetaMap)
                {
                    if (kvp.Value.Definition.Code.Equals(currentVung, StringComparison.OrdinalIgnoreCase))
                    {
                        Zone_Click(kvp.Key, EventArgs.Empty);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearInput(force: true);
        }

        private void ClearInput(bool force = false)
        {
            if (!force && _isDirty && !PromptDiscardChanges()) return;

            if (_selectedZoneButton != null && _zoneMetaMap.TryGetValue(_selectedZoneButton, out ZoneButtonMeta meta))
            {
                if (meta.DataRow != null)
                {
                    PopulateInputsForEdit(meta.DataRow, meta.Definition);
                    return;
                }
                else
                {
                    PopulateInputsForNew(meta.Definition);
                    return;
                }
            }

            _isPopulating = true;
            _editingMaKV = null;
            lblTrangThaiForm.Text = "Thiết lập khu vực mới";
            btnLuu.Text = "Tạo khu vực và sinh ghế";

            if (cboMaVung.Items.Count > 0) cboMaVung.SelectedIndex = 0;
            txtTenKV.Text = "";
            cboLoaiKV.SelectedIndex = 0;
            txtSucChua.Text = "20";
            txtSucChua.Enabled = PhienDangNhap.LaQuanLy;
            lblSucChuaHint.Text = "Tự động sinh ma trận ghế (10 ghế mỗi hàng)";
            txtGiaVe.Text = "";
            toggleBanVe.Checked = true;

            SelectZoneButton(null);
            _isPopulating = false;
            _isDirty = false;
        }

        private bool PromptDiscardChanges()
        {
            if (!_isDirty) return true;

            DialogResult dr = MessageBox.Show(
                "Bạn có thay đổi chưa lưu. Bạn có muốn lưu thay đổi trước không?",
                "Xác nhận", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                btnLuu_Click(this, EventArgs.Empty);
                return !_isDirty;
            }
            else if (dr == DialogResult.No)
            {
                _isDirty = false;
                return true;
            }
            return false;
        }

        // ============================================================
        // GRID EVENTS (PAINTING & ACTIONS)
        // ============================================================
        private void dgvKhuVuc_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dgvKhuVuc.Columns[e.ColumnIndex].Name;
            if (colName == "colTrangThai")
            {
                string status = Convert.ToString(e.Value);
                Color badgeBg = status == "Đang mở bán" ? Color.FromArgb(209, 250, 229) : Color.FromArgb(254, 243, 199);
                Color badgeFore = status == "Đang mở bán" ? Color.FromArgb(6, 95, 70) : Color.FromArgb(146, 64, 14);

                ManagementGridRenderer.PaintStatus(dgvKhuVuc, e, status, badgeBg, badgeFore);
            }
            else if (colName == "colThaoTac")
            {
                ManagementGridRenderer.PaintActions(dgvKhuVuc, e);
            }
        }

        private void dgvKhuVuc_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Left) return;
            if (dgvKhuVuc.Columns[e.ColumnIndex].Name != "colThaoTac") return;

            dgvKhuVuc.CurrentCell = dgvKhuVuc.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle cell = new Rectangle(Point.Empty,
                dgvKhuVuc.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Size);
            Point location = new Point(e.X, e.Y);

            int maKV = Convert.ToInt32(dgvKhuVuc.Rows[e.RowIndex].Cells["colMaKV"].Value);
            string tenKV = dgvKhuVuc.Rows[e.RowIndex].Cells["colTenKV"].Value?.ToString() ?? "";

            if (ManagementGridRenderer.GetActionBounds(dgvKhuVuc, cell, false).Contains(location))
            {
                // Sửa
                dgvKhuVuc.Rows[e.RowIndex].Selected = true;
                DataRow row = dgvKhuVuc.Rows[e.RowIndex].Tag as DataRow;
                if (row != null)
                {
                    PopulateInputsForEdit(row, null);
                }
            }
            else if (ManagementGridRenderer.GetActionBounds(dgvKhuVuc, cell, true).Contains(location))
            {
                // Xóa
                if (MessageBox.Show(
                    $"Xóa khu vực '{tenKV}' sẽ xóa toàn bộ ghế bên trong. Bạn có chắc chắn muốn tiếp tục?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                string result = KhuVucBLL.Xoa(maKV);
                if (result == "Success")
                {
                    MessageBox.Show("Xóa khu vực thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // ============================================================
        // TÌM KIẾM
        // ============================================================
        private void txtTimKiemKV_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiemKV.Text.Trim().ToLower();
            CurrencyManager cm = (CurrencyManager)BindingContext[dgvKhuVuc.DataSource ?? dgvKhuVuc.Rows];
            cm.SuspendBinding();

            foreach (DataGridViewRow row in dgvKhuVuc.Rows)
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    row.Visible = true;
                    continue;
                }

                string maKV = row.Cells["colMaKV"].Value?.ToString().ToLower() ?? "";
                string maVung = row.Cells["colMaVung"].Value?.ToString().ToLower() ?? "";
                string tenKV = row.Cells["colTenKV"].Value?.ToString().ToLower() ?? "";
                string loaiKV = row.Cells["colLoaiKV"].Value?.ToString().ToLower() ?? "";

                bool match = maKV.Contains(keyword) || maVung.Contains(keyword) ||
                             tenKV.Contains(keyword) || loaiKV.Contains(keyword);

                row.Visible = match;
            }

            cm.ResumeBinding();
        }

        // ============================================================
        // CARDS & CHÚ THÍCH (LEGEND)
        // ============================================================
        private void UpdateCardsAndLegend(DataTable dt)
        {
            int tongKV = dt.Rows.Count;
            int tongGhe = 0;
            decimal minPrice = 0;
            int soKVBanVe = 0;

            foreach (DataRow row in dt.Rows)
            {
                tongGhe += Convert.ToInt32(row["TongSoGhe"]);
                decimal gia = Convert.ToDecimal(row["GiaVe"]);
                if (minPrice == 0 || gia < minPrice) minPrice = gia;
                if (Convert.ToBoolean(row["ChoPhepBanVe"])) soKVBanVe++;
            }

            lblCard1Value.Text = tongKV.ToString("N0");
            lblCard2Value.Text = tongGhe.ToString("N0");
            lblCard3Value.Text = soKVBanVe.ToString("N0");
            lblCard4Value.Text = minPrice > 0 ? minPrice.ToString("N0") + " đ" : "0 đ";

            pnlLegend.Controls.Clear();
            pnlLegend.Controls.Add(lblLegendTitle);

            // Gom nhóm chú thích theo hạng vé thực tế trên sơ đồ
            var legendItems = new List<LegendItemInfo>
            {
                new LegendItemInfo { Name = "Hạng VIP", Color = Color.FromArgb(147, 51, 234) },
                new LegendItemInfo { Name = "Hạng Phổ thông", Color = Color.FromArgb(56, 189, 248) },
                new LegendItemInfo { Name = "Hạng Thường", Color = Color.FromArgb(245, 158, 11) }
            };

            int yOffset = 30;
            foreach (var item in legendItems)
            {
                // Tìm khoảng giá cho hạng vé này
                var rows = dt.AsEnumerable().Where(r => r["HangVe"].ToString().IndexOf(item.Name.Replace("Hạng ", ""), StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                string priceText;
                if (rows.Count > 0)
                {
                    decimal pMin = rows.Min(r => Convert.ToDecimal(r["GiaVe"]));
                    decimal pMax = rows.Max(r => Convert.ToDecimal(r["GiaVe"]));
                    priceText = pMin == pMax ? $"{pMin:N0} đ" : $"{pMin:N0} - {pMax:N0} đ";
                }
                else
                {
                    priceText = "Chưa thiết lập";
                }

                Panel pnlItem = new Panel { Size = new Size(190, 42), Location = new Point(5, yOffset) };
                Guna.UI2.WinForms.Guna2CirclePictureBox dot = new Guna.UI2.WinForms.Guna2CirclePictureBox
                {
                    FillColor = item.Color,
                    Size = new Size(12, 12),
                    Location = new Point(2, 6)
                };
                Label lblName = new Label
                {
                    Text = item.Name,
                    AutoSize = false,
                    Width = 160,
                    Height = 18,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 41, 59),
                    Location = new Point(20, 2)
                };
                Label lblPrice = new Label
                {
                    Text = priceText,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Location = new Point(20, 20)
                };

                pnlItem.Controls.Add(dot);
                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(lblPrice);
                pnlLegend.Controls.Add(pnlItem);
                yOffset += 46;
            }
        }

        private class LegendItemInfo
        {
            public string Name { get; set; }
            public Color Color { get; set; }
        }

        private void btnToggleInfo_Click(object sender, EventArgs e)
        {
            pnlRightInfo.Visible = !pnlRightInfo.Visible;
            btnToggleInfo.Text = pnlRightInfo.Visible ? ">" : "<";
            LayoutKhuVucForm();
        }
    }
}

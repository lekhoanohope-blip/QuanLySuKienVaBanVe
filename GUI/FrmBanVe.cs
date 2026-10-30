using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmBanVe : Form
    {
        private Panel seatMapWrapper;
        private int _currentMaSK = 0;
        private int _currentMaSuat = 0;
        private int _currentLoaiSoDo = 1;
        private DataTable _dtKhuVucCurrent = null;

        private Guna2Button _selectedZoneButton = null;
        private ZoneDefinition _selectedZoneDef = null;
        private DataRow _selectedZoneRow = null;

        private int _soLuong = 1;
        private int _maxSoLuong = 10;
        private decimal _donGiaHienTai = 0m;

        // Lưu thông tin ánh xạ button trên sơ đồ
        private readonly Dictionary<Guna2Button, ZoneButtonMeta> _zoneMetaMap
            = new Dictionary<Guna2Button, ZoneButtonMeta>();

        private class ZoneButtonMeta
        {
            public ZoneDefinition Definition { get; set; }
            public DataRow DataRow { get; set; }
            public Color BaseColor { get; set; }
        }

        private class ZoneDefinition
        {
            public string Code { get; set; }
            public string DisplayName { get; set; }
            public string Category { get; set; } // VIP, Thường, Phổ thông
            public Rectangle Bounds { get; set; }
        }

        public FrmBanVe()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Wire up event handlers
            this.Load += FrmBanVe_Load;
            this.SizeChanged += FrmBanVe_SizeChanged;
            pnlMainBody.SizeChanged += (s, e) => ResponsiveLayout();
            pnlSeatMapCanvas.Resize += (s, e) => CenterSeatMap();

            cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;
            cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;

            btnGiam.Click += btnGiam_Click;
            btnTang.Click += btnTang_Click;
            btnDatVe.Click += btnDatVe_Click;

            InitSummaryGrid();
            InitLegendDots();
        }

        private void FrmBanVe_Load(object sender, EventArgs e)
        {
            // Bạn có thể giữ lại đoạn kiểm tra kết nối CSDL ngầm nếu cần, 
            // nhưng xóa bỏ 2 dòng gán Text và ForeColor cho lblDataBadge đi:
            bool isDb = MockDataService.KiemTraKetNoiCSDL();
            // (Đã xóa lblDataBadge.Text và lblDataBadge.ForeColor ở đây)
            LoadSuKien();
            ResponsiveLayout();
        }

        private void FrmBanVe_SizeChanged(object sender, EventArgs e)
        {
            ResponsiveLayout();
        }

        #region Bố cục 3/5 và 2/5 Responsive
        /*
        private void ResponsiveLayout()
        {
            if (pnlMainBody.ClientSize.Width <= 0) return;

            int padding = 16;
            int gap = 16;
            int totalWidth = pnlMainBody.ClientSize.Width - padding * 2;
            int totalHeight = pnlMainBody.ClientSize.Height - padding * 2;

            if (totalWidth <= 400) return;

            // 3/5 diện tích cho Sơ đồ Seatmap (60%), 2/5 diện tích cho Bảng điều khiển (40%)
            int leftWidth = (int)(totalWidth * 0.58f);
            int rightWidth = totalWidth - leftWidth - gap;

            pnlSeatMapContainer.SetBounds(padding, padding, leftWidth, totalHeight);
            pnlRightAction.SetBounds(padding + leftWidth + gap, padding, rightWidth, totalHeight);

            // Cập nhật chiều cao và vị trí các card bên phải
            int cardW = pnlRightAction.ClientSize.Width;
            pnlLegendCard.Width = cardW;
            pnlTicketInfoCard.Width = cardW;
            pnlQuantityCard.Width = cardW;
            pnlPaymentCard.Width = cardW;

            btnDatVe.Width = Math.Max(200, cardW - 36);

            CenterSeatMap();
        }
        */

        private void ResponsiveLayout()
        {
            if (pnlMainBody.ClientSize.Width <= 0) return;

            int padding = 16;
            int gap = 16;
            int totalWidth = pnlMainBody.ClientSize.Width - padding * 2;
            int totalHeight = pnlMainBody.ClientSize.Height - padding * 2;

            if (totalWidth <= 400) return;

            int leftWidth = (int)(totalWidth * 0.58f);
            int rightWidth = totalWidth - leftWidth - gap;

            pnlSeatMapContainer.SetBounds(padding, padding, leftWidth, totalHeight);
            pnlRightAction.SetBounds(padding + leftWidth + gap, padding, rightWidth, totalHeight);

            int cardW = pnlRightAction.ClientSize.Width;

            // CHỈNH SỬA CHIỀU CAO CHO pnlLegendCard Ở ĐÂY:
            // Cho phép chiều cao tự động co giãn dựa vào vị trí của bảng (dgvSummary) hoặc các chú thích bên trong
            int legendCardHeight = Math.Max(320, dgvSummary.Bottom + 16);
            pnlLegendCard.SetBounds(0, 0, cardW, legendCardHeight);

            // Cập nhật lại vị trí các card phía dưới dịch xuống theo pnlLegendCard mới
            int currentY = pnlLegendCard.Bottom + gap;

            pnlTicketInfoCard.SetBounds(0, currentY, cardW, pnlTicketInfoCard.Height);
            currentY += pnlTicketInfoCard.Height + gap;

            pnlQuantityCard.SetBounds(0, currentY, cardW, pnlQuantityCard.Height);
            currentY += pnlQuantityCard.Height + gap;

            pnlPaymentCard.SetBounds(0, currentY, cardW, pnlPaymentCard.Height);

            btnDatVe.Width = Math.Max(200, cardW - 36);

            CenterSeatMap();
        }

        #endregion

        #region 1. Load Sự kiện & Suất diễn
        private void LoadSuKien()
        {
            try
            {
                DataTable dtSK = MockDataService.LayDanhSachSuKien();
                cboSuKien.SelectedIndexChanged -= cboSuKien_SelectedIndexChanged;
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
                cboSuKien.DataSource = dtSK;
                cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;

                if (cboSuKien.Items.Count > 0)
                {
                    cboSuKien.SelectedIndex = 0;
                    if (cboSuKien.SelectedValue != null && int.TryParse(cboSuKien.SelectedValue.ToString(), out int skId))
                    {
                        _currentMaSK = skId;
                    }
                }
                LoadSuatDien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sự kiện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue != null && int.TryParse(cboSuKien.SelectedValue.ToString(), out int skId))
            {
                _currentMaSK = skId;
                LoadSuatDien();
            }
        }

        private void LoadSuatDien()
        {
            try
            {
                DataTable dtSuat = MockDataService.LayDanhSachSuatDien(_currentMaSK);
                cboSuatDien.SelectedIndexChanged -= cboSuatDien_SelectedIndexChanged;
                cboSuatDien.DisplayMember = "HienThiSuat";
                cboSuatDien.ValueMember = "MaSuat";
                cboSuatDien.DataSource = dtSuat;
                cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;

                if (dtSuat != null && dtSuat.Rows.Count > 0)
                {
                    cboSuatDien.SelectedIndex = 0;
                    ReadCurrentSuatInfo();
                }
                else
                {
                    _currentMaSuat = 0;
                    _currentLoaiSoDo = 1;
                    ClearSeatMap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải suất diễn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboSuatDien_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadCurrentSuatInfo();
        }

        private void ReadCurrentSuatInfo()
        {
            if (cboSuatDien.SelectedItem is DataRowView drv)
            {
                _currentMaSuat = Convert.ToInt32(drv["MaSuat"]);
                _currentLoaiSoDo = drv.Row.Table.Columns.Contains("LoaiSoDo") && drv["LoaiSoDo"] != DBNull.Value
                    ? Convert.ToInt32(drv["LoaiSoDo"]) : 1;

                string tenSoDo = _currentLoaiSoDo == 1 ? "Sơ đồ 1 (Chữ T)"
                    : _currentLoaiSoDo == 2 ? "Sơ đồ 2 (Hình chữ nhật)" : "Sơ đồ 3 (Lưới)";
                lblSeatMapTitle.Text = $"SƠ ĐỒ SEATMAP: {tenSoDo.ToUpper()}";

                LoadKhuVucVaVe();
            }
        }
        #endregion

        #region 2. Load Khu vực & Vẽ Sơ đồ Seatmap
        private void LoadKhuVucVaVe()
        {
            if (_currentMaSuat <= 0) return;

            _dtKhuVucCurrent = MockDataService.LayDanhSachKhuVuc(_currentMaSuat, _currentLoaiSoDo);

            // Nạp bảng tóm tắt danh sách khu vực và giá vé ở 2.1
            BindSummaryGrid(_dtKhuVucCurrent);

            // Vẽ sơ đồ ghế theo loại sơ đồ được chọn
            DrawSeatMap(_currentLoaiSoDo, _dtKhuVucCurrent);

            // Reset thông tin vé đã chọn
            ResetTicketSelection();
        }

        private void ClearSeatMap()
        {
            if (seatMapWrapper != null)
                seatMapWrapper.Controls.Clear();
            _zoneMetaMap.Clear();
            _selectedZoneButton = null;
            ResetTicketSelection();
        }

        private void DrawSeatMap(int loaiSoDo, DataTable dtKhuVuc)
        {
            if (seatMapWrapper == null)
            {
                seatMapWrapper = new Panel { BackColor = Color.White };
                pnlSeatMapCanvas.Controls.Add(seatMapWrapper);
            }

            seatMapWrapper.Controls.Clear();
            _zoneMetaMap.Clear();
            _selectedZoneButton = null;

            List<ZoneDefinition> definitions = GetZoneDefinitions(loaiSoDo);

            // 1. Vẽ các khối sân khấu & lối đi trang trí (không bấm được)
            DrawStagesAndCatwalks(loaiSoDo);

            // 2. Thêm các Button khu vực có thể tương tác
            HashSet<int> matchedMaKVs = new HashSet<int>();
            foreach (var zone in definitions)
            {
                DataRow matchedRow = null;

                // Khớp theo MaVung trước
                foreach (DataRow r in dtKhuVuc.Rows)
                {
                    string dbMaVung = r["MaVung"]?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(dbMaVung) && dbMaVung.Equals(zone.Code, StringComparison.OrdinalIgnoreCase))
                    {
                        matchedRow = r;
                        matchedMaKVs.Add(Convert.ToInt32(r["MaKV"]));
                        break;
                    }
                }

                // Fallback theo tên khu vực
                if (matchedRow == null)
                {
                    foreach (DataRow r in dtKhuVuc.Rows)
                    {
                        int id = Convert.ToInt32(r["MaKV"]);
                        if (matchedMaKVs.Contains(id)) continue;

                        string ten = r["TenKV"]?.ToString() ?? "";
                        if (ten.IndexOf(zone.DisplayName, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matchedRow = r;
                            matchedMaKVs.Add(id);
                            break;
                        }
                    }
                }

                float fontSize = zone.Bounds.Width < 80 ? 8.5F : 9.5F;
                Guna2Button btn = new Guna2Button
                {
                    Location = zone.Bounds.Location,
                    Size = zone.Bounds.Size,
                    BorderRadius = 6,
                    Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                    Tag = zone.Code,
                    Cursor = Cursors.Hand
                };

                string hangVe = matchedRow != null ? (matchedRow["HangVe"]?.ToString() ?? "VIP") : zone.Category;
                Color baseColor = GetCategoryColor(hangVe);

                if (matchedRow != null)
                {
                    decimal gia = Convert.ToDecimal(matchedRow["GiaVe"]);
                    int conLai = matchedRow.Table.Columns.Contains("ConLai") ? Convert.ToInt32(matchedRow["ConLai"]) : 50;
                    bool choPhep = !matchedRow.Table.Columns.Contains("ChoPhepBanVe") || Convert.ToBoolean(matchedRow["ChoPhepBanVe"]);

                    btn.FillColor = choPhep ? baseColor : Color.FromArgb(203, 213, 225);
                    btn.ForeColor = Color.White;
                    btn.BorderColor = Color.Transparent;
                    btn.BorderThickness = 0;

                    string giaStr = $"{gia:N0} đ";
                    string statusSuffix = choPhep ? (conLai > 0 ? "" : "\n[HẾT VÉ]") : "\n[KHÓA]";
                    btn.Text = $"{zone.DisplayName}\n{giaStr}{statusSuffix}";

                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(btn, $"Khu vực: {zone.DisplayName}\nHạng vé: {hangVe}\nĐơn giá: {gia:N0} đ\nCòn trống: {conLai} ghế\nClick để chọn khu vực này");

                    if (!choPhep || conLai <= 0)
                    {
                        btn.Cursor = Cursors.No;
                    }
                }
                else
                {
                    btn.FillColor = Color.FromArgb(241, 245, 249);
                    btn.ForeColor = Color.FromArgb(100, 116, 139);
                    btn.BorderColor = Color.FromArgb(203, 213, 225);
                    btn.BorderThickness = 1;
                    btn.Text = $"{zone.DisplayName}\n(Chưa có giá)";
                }

                btn.Click += ZoneButton_Click;
                seatMapWrapper.Controls.Add(btn);

                _zoneMetaMap[btn] = new ZoneButtonMeta
                {
                    Definition = zone,
                    DataRow = matchedRow,
                    BaseColor = baseColor
                };
            }

            ComputeWrapperSize();
            CenterSeatMap();
        }

        private void DrawStagesAndCatwalks(int loaiSoDo)
        {
            Color clrStage = Color.FromArgb(226, 232, 240);
            Color clrText = Color.FromArgb(71, 85, 105);

            if (loaiSoDo == 1) // Sơ đồ chữ T
            {
                AddBlock(93, 39, 413, 80, "SÂN KHẤU (STAGE)", clrStage, clrText);
                AddBlock(256, 120, 80, 215, "LỐI ĐI", clrStage, clrText);
                AddBlock(128, 335, 329, 65, "SÂN KHẤU PHỤ", clrStage, clrText);
            }
            else if (loaiSoDo == 2) // Sơ đồ hình chữ nhật
            {
                AddBlock(130, 30, 400, 60, "SÂN KHẤU CHÍNH", clrStage, clrText);
            }
            else // Sơ đồ lưới
            {
                AddBlock(150, 10, 400, 60, "SÂN KHẤU CHÍNH", clrStage, clrText);
            }
        }

        private void AddBlock(int x, int y, int w, int h, string text, Color fill, Color fore)
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
            if (loaiSoDo == 1) // Sơ đồ chữ T (Ảnh 3)
            {
                list.Add(new ZoneDefinition { Code = "VIP_A", DisplayName = "VIP A", Category = "VIP", Bounds = new Rectangle(93, 160, 142, 129) });
                list.Add(new ZoneDefinition { Code = "VIP_B", DisplayName = "VIP B", Category = "VIP", Bounds = new Rectangle(362, 160, 132, 129) });
                list.Add(new ZoneDefinition { Code = "GA_A", DisplayName = "GA A", Category = "Phổ thông", Bounds = new Rectangle(17, 96, 70, 278) });
                list.Add(new ZoneDefinition { Code = "GA_B", DisplayName = "GA B", Category = "Phổ thông", Bounds = new Rectangle(514, 96, 70, 278) });
                list.Add(new ZoneDefinition { Code = "CAT", DisplayName = "CAT", Category = "Thường", Bounds = new Rectangle(69, 410, 464, 66) });
            }
            else if (loaiSoDo == 2) // Sơ đồ hình chữ nhật (Ảnh 2)
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
            else // Sơ đồ lưới (Ảnh 1)
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
            // 1. VIP (Tím #9333EA)
            if (cat.IndexOf("VIP", StringComparison.OrdinalIgnoreCase) >= 0)
                return Color.FromArgb(147, 51, 234);

            // 2. Phổ thông / GA (Xanh dương #38BDF8)
            if (cat.IndexOf("Phổ thông", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("Pho thong", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("GA", StringComparison.OrdinalIgnoreCase) >= 0)
                return Color.FromArgb(56, 189, 248);

            // 3. Thường / CAT (Cam #F59E0B)
            return Color.FromArgb(245, 158, 11);
        }

        private void ComputeWrapperSize()
        {
            int maxX = 680, maxY = 500;
            foreach (Control c in seatMapWrapper.Controls)
            {
                if (c.Right > maxX) maxX = c.Right;
                if (c.Bottom > maxY) maxY = c.Bottom;
            }
            seatMapWrapper.Size = new Size(maxX + 20, maxY + 20);
        }

        private void CenterSeatMap()
        {
            if (seatMapWrapper == null || pnlSeatMapCanvas == null) return;
            int offsetX = Math.Max(0, (pnlSeatMapCanvas.ClientSize.Width - seatMapWrapper.Width) / 2);
            int offsetY = Math.Max(0, (pnlSeatMapCanvas.ClientSize.Height - seatMapWrapper.Height) / 2);
            seatMapWrapper.Location = new Point(offsetX, offsetY);
        }
        #endregion

        #region 3. Tương tác Click Sơ đồ & Chọn khu vực
        private void ZoneButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Guna2Button clickedBtn)) return;
            if (!_zoneMetaMap.ContainsKey(clickedBtn)) return;

            ZoneButtonMeta meta = _zoneMetaMap[clickedBtn];

            // Kiểm tra trạng thái khóa hoặc hết vé
            if (meta.DataRow != null)
            {
                bool choPhep = !meta.DataRow.Table.Columns.Contains("ChoPhepBanVe") || Convert.ToBoolean(meta.DataRow["ChoPhepBanVe"]);
                int conLai = meta.DataRow.Table.Columns.Contains("ConLai") ? Convert.ToInt32(meta.DataRow["ConLai"]) : 50;

                if (!choPhep)
                {
                    MessageBox.Show("Khu vực này hiện đang đóng bán vé!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (conLai <= 0)
                {
                    MessageBox.Show("Khu vực này đã hết vé trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 3.1. Hiệu ứng: Chuyển khu vực được click sang màu XANH LÁ CÂY, trả các khu vực khác về màu cũ
            if (_selectedZoneButton != null && _selectedZoneButton != clickedBtn)
            {
                if (_zoneMetaMap.ContainsKey(_selectedZoneButton))
                {
                    _selectedZoneButton.FillColor = _zoneMetaMap[_selectedZoneButton].BaseColor;
                    _selectedZoneButton.BorderThickness = 0;
                }
            }

            _selectedZoneButton = clickedBtn;
            _selectedZoneDef = meta.Definition;
            _selectedZoneRow = meta.DataRow;

            // Đánh dấu chọn bằng màu xanh lá cây (#10B981 - Emerald Green)
            clickedBtn.FillColor = Color.FromArgb(16, 185, 129);
            clickedBtn.BorderColor = Color.White;
            clickedBtn.BorderThickness = 2;

            // Cập nhật thông tin chi tiết sang mục 2.2
            UpdateSelectedTicketInfo(meta);
        }

        private void UpdateSelectedTicketInfo(ZoneButtonMeta meta)
        {
            string tenKV = meta.DataRow != null ? (meta.DataRow["TenKV"]?.ToString() ?? meta.Definition.DisplayName) : meta.Definition.DisplayName;
            string hangVe = meta.DataRow != null ? (meta.DataRow["HangVe"]?.ToString() ?? meta.Definition.Category) : meta.Definition.Category;
            decimal giaVe = meta.DataRow != null ? Convert.ToDecimal(meta.DataRow["GiaVe"]) : 0m;
            int tongSoGhe = meta.DataRow != null && meta.DataRow.Table.Columns.Contains("TongSoGhe") ? Convert.ToInt32(meta.DataRow["TongSoGhe"]) : 50;
            int conLai = meta.DataRow != null && meta.DataRow.Table.Columns.Contains("ConLai") ? Convert.ToInt32(meta.DataRow["ConLai"]) : tongSoGhe;

            _donGiaHienTai = giaVe;
            _maxSoLuong = Math.Max(1, Math.Min(20, conLai));

            // Cập nhật hiển thị Card 2.2
            lblSelectedTenKV.Text = $"Khu vực: {tenKV}";
            lblSelectedHangVe.Text = $"Hạng vé: {hangVe}";
            lblSelectedSucChua.Text = $"Còn trống: {conLai} / {tongSoGhe} ghế";
            lblSelectedGiaVe.Text = $"Đơn giá: {giaVe:N0} đ";

            // Reset bộ đếm số lượng về 1
            _soLuong = 1;
            lblSoLuong.Text = _soLuong.ToString();
            lblGhiChuSoLuong.Text = $"(Tối đa: {_maxSoLuong} vé/lần)";

            // Cập nhật tổng tiền (2.4)
            RecalculateTotal();

            // Đồng bộ chọn dòng tương ứng trên DataGridView ở 2.1
            HighlightGridRow(tenKV);
        }

        private void ResetTicketSelection()
        {
            _selectedZoneButton = null;
            _selectedZoneDef = null;
            _selectedZoneRow = null;
            _donGiaHienTai = 0m;
            _soLuong = 1;

            lblSelectedTenKV.Text = "Khu vực: (Vui lòng click chọn trên sơ đồ)";
            lblSelectedHangVe.Text = "Hạng vé: --";
            lblSelectedSucChua.Text = "Còn trống: --";
            lblSelectedGiaVe.Text = "Đơn giá: 0 đ";

            lblSoLuong.Text = "1";
            lblTongTien.Text = "0 đ";
            lblGhiChuSoLuong.Text = "(Tối thiểu: 1 vé, tối đa: theo chỗ)";
        }
        #endregion

        #region 4. Bộ đếm số lượng & Tính tổng tiền thời gian thực
        private void btnGiam_Click(object sender, EventArgs e)
        {
            if (_selectedZoneDef == null)
            {
                MessageBox.Show("Vui lòng click chọn một khu vực trên sơ đồ trước!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_soLuong > 1)
            {
                _soLuong--;
                lblSoLuong.Text = _soLuong.ToString();
                RecalculateTotal();
            }
        }

        private void btnTang_Click(object sender, EventArgs e)
        {
            if (_selectedZoneDef == null)
            {
                MessageBox.Show("Vui lòng click chọn một khu vực trên sơ đồ trước!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_soLuong < _maxSoLuong)
            {
                _soLuong++;
                lblSoLuong.Text = _soLuong.ToString();
                RecalculateTotal();
            }
            else
            {
                MessageBox.Show($"Bạn đã đạt số lượng tối đa có thể đặt cho khu vực này ({_maxSoLuong} vé)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RecalculateTotal()
        {
            decimal total = _soLuong * _donGiaHienTai;
            lblTongTien.Text = $"{total:N0} đ";
        }
        #endregion

        #region 5. Tìm kiếm nhanh khu vực
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword)) return;

            // Tìm nút khu vực khớp và kích hoạt click
            foreach (var kvp in _zoneMetaMap)
            {
                string name = kvp.Value.Definition.DisplayName;
                string code = kvp.Value.Definition.Code;
                string cat = kvp.Value.Definition.Category;

                if (name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    cat.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ZoneButton_Click(kvp.Key, EventArgs.Empty);
                    break;
                }
            }
        }
        #endregion

        #region 6. Chú thích màu sắc & Bảng giá danh sách khu vực (2.1)
        /*
        private void InitLegendDots()
        {
            pnlLegendDots.Controls.Clear();

            AddDot("VIP", Color.FromArgb(147, 51, 234));
            AddDot("CAT (Thường)", Color.FromArgb(245, 158, 11));
            AddDot("GA (Phổ thông)", Color.FromArgb(56, 189, 248));
            AddDot("Đang chọn", Color.FromArgb(16, 185, 129));
        }

        private void AddDot(string title, Color color)
        {
            Panel pnl = new Panel { Width = 100, Height = 26, Margin = new Padding(2) };
            Panel dot = new Panel { Size = new Size(12, 12), Location = new Point(2, 6), BackColor = color };
            Label lbl = new Label { Text = title, Location = new Point(18, 4), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(51, 65, 85) };
            pnl.Controls.Add(dot);
            pnl.Controls.Add(lbl);
            pnlLegendDots.Controls.Add(pnl);
        }
        */

        private void InitLegendDots()
        {
            pnlLegendDots.Controls.Clear();
            pnlLegendDots.AutoSize = true;
            pnlLegendDots.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlLegendDots.WrapContents = true; // Tự động xuống dòng nếu khung hẹp

            AddDot("VIP", Color.FromArgb(147, 51, 234));
            AddDot("CAT (Thường)", Color.FromArgb(245, 158, 11));
            AddDot("GA (Phổ thông)", Color.FromArgb(56, 189, 248));
            AddDot("Đang chọn", Color.FromArgb(16, 185, 129));
        }

        private void AddDot(string title, Color color)
        {
            // Tạo Badge tinh gọn có viền bo nhẹ, tự dãn rộng theo độ dài chữ
            Panel badge = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 8, 6),
                Padding = new Padding(6, 3, 8, 3),
                BackColor = Color.FromArgb(248, 250, 252) // Màu nền ghi nhạt hiện đại
            };

            Panel dot = new Panel
            {
                Size = new Size(10, 10),
                Location = new Point(6, 6),
                BackColor = color
            };

            Label lbl = new Label
            {
                Text = title,
                Location = new Point(20, 2),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85)
            };

            badge.Controls.Add(dot);
            badge.Controls.Add(lbl);
            pnlLegendDots.Controls.Add(badge);
        }
        /*
        private void InitSummaryGrid()
        {
            dgvSummary.Columns.Clear();
            dgvSummary.Columns.Add("TenKV", "Khu vực");
            dgvSummary.Columns.Add("HangVe", "Hạng");
            dgvSummary.Columns.Add("GiaVe", "Đơn giá");
            dgvSummary.Columns.Add("ConLai", "Trống");

            dgvSummary.Columns["TenKV"].Width = 90;
            dgvSummary.Columns["HangVe"].Width = 80;
            dgvSummary.Columns["GiaVe"].Width = 90;
            dgvSummary.Columns["ConLai"].Width = 60;
        }
        */

        private void InitSummaryGrid()
        {
            dgvSummary.Columns.Clear();

            // Cấu hình UI phẳng, sạch sẽ cho DataGridView
            dgvSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSummary.AllowUserToResizeColumns = false;
            dgvSummary.AllowUserToResizeRows = false;
            dgvSummary.RowHeadersVisible = false; // Xóa cột thừa bên trái
            dgvSummary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSummary.MultiSelect = false;
            dgvSummary.BorderStyle = BorderStyle.None;
            dgvSummary.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSummary.BackgroundColor = Color.White;

            // Định dạng Font chữ không bị mất net / cắt chữ
            dgvSummary.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvSummary.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            dgvSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSummary.ColumnHeadersHeight = 32;
            dgvSummary.RowTemplate.Height = 28;

            // Thêm các cột với tỷ lệ Auto-Fill chuẩn
            var colTen = dgvSummary.Columns.Add("TenKV", "Khu vực");
            var colHang = dgvSummary.Columns.Add("HangVe", "Hạng");
            var colGia = dgvSummary.Columns.Add("GiaVe", "Đơn giá");
            var colConLai = dgvSummary.Columns.Add("ConLai", "Trống");

            // Phân bổ % độ rộng các cột tự dãn 100%
            dgvSummary.Columns["TenKV"].FillWeight = 30;
            dgvSummary.Columns["HangVe"].FillWeight = 25;
            dgvSummary.Columns["GiaVe"].FillWeight = 30;
            dgvSummary.Columns["ConLai"].FillWeight = 15;

            // Căn lề số liệu đẹp mắt (Đơn giá và Số lượng căn phải)
            dgvSummary.Columns["GiaVe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["ConLai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void BindSummaryGrid(DataTable dt)
        {
            dgvSummary.Rows.Clear();
            if (dt == null) return;

            foreach (DataRow r in dt.Rows)
            {
                string ten = r["TenKV"]?.ToString();
                string hang = r["HangVe"]?.ToString();
                decimal gia = Convert.ToDecimal(r["GiaVe"]);
                int conLai = r.Table.Columns.Contains("ConLai") ? Convert.ToInt32(r["ConLai"]) : 50;

                dgvSummary.Rows.Add(ten, hang, $"{gia:N0} đ", conLai);
            }
        }

        private void HighlightGridRow(string tenKV)
        {
            foreach (DataGridViewRow row in dgvSummary.Rows)
            {
                if (row.Cells["TenKV"].Value?.ToString().IndexOf(tenKV, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region 7. Nút Đặt vé -> Chuyển sang Trang Thanh toán
        private void btnDatVe_Click(object sender, EventArgs e)
        {
            // Kiểm tra ràng buộc
            if (_currentMaSuat <= 0)
            {
                MessageBox.Show("Vui lòng chọn suất diễn hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedZoneDef == null)
            {
                MessageBox.Show("Vui lòng click chọn một khu vực trên sơ đồ seatmap trước khi đặt vé!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenKV = _selectedZoneRow != null ? (_selectedZoneRow["TenKV"]?.ToString() ?? _selectedZoneDef.DisplayName) : _selectedZoneDef.DisplayName;
            string hangVe = _selectedZoneRow != null ? (_selectedZoneRow["HangVe"]?.ToString() ?? _selectedZoneDef.Category) : _selectedZoneDef.Category;
            int maKV = _selectedZoneRow != null ? Convert.ToInt32(_selectedZoneRow["MaKV"]) : (_currentMaSuat * 10 + 1);
            int tongSoGhe = _selectedZoneRow != null && _selectedZoneRow.Table.Columns.Contains("TongSoGhe") ? Convert.ToInt32(_selectedZoneRow["TongSoGhe"]) : 50;
            int daBan = _selectedZoneRow != null && _selectedZoneRow.Table.Columns.Contains("DaBan") ? Convert.ToInt32(_selectedZoneRow["DaBan"]) : 0;
            int conLai = _selectedZoneRow != null && _selectedZoneRow.Table.Columns.Contains("ConLai") ? Convert.ToInt32(_selectedZoneRow["ConLai"]) : 50;

            if (_soLuong > conLai)
            {
                MessageBox.Show($"Khu vực này hiện chỉ còn {conLai} ghế trống, không đủ cho {_soLuong} vé yêu cầu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Sinh vị trí ghế tự động theo logic đề bài:
            // Sắp xếp hàng ngang từ trái sang phải, ưu tiên vị trí gần sân khấu nhất (ghế nhỏ nhất còn trống)
            List<string> danhSachGhe = MockDataService.SinhViTriGheTuDong(maKV, tenKV, tongSoGhe, daBan, _soLuong);

            // Đóng gói dữ liệu sang DTO
            ThongTinDatVeDTO dto = new ThongTinDatVeDTO
            {
                MaSK = _currentMaSK,
                TenSK = cboSuKien.Text,
                MaSuat = _currentMaSuat,
                ThoiGianBatDau = DateTime.Now,
                DiaDiem = "Hội trường biểu diễn",
                LoaiSoDo = _currentLoaiSoDo,
                MaKV = maKV,
                MaVung = _selectedZoneDef.Code,
                TenKV = tenKV,
                HangVe = hangVe,
                GiaVe = _donGiaHienTai,
                TongSoGhe = tongSoGhe,
                DaBan = daBan,
                ConLai = conLai,
                SoLuong = _soLuong,
                DanhSachGhe = danhSachGhe
            };

            if (cboSuatDien.SelectedItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("ThoiGianBatDau") && drv["ThoiGianBatDau"] != DBNull.Value)
                    dto.ThoiGianBatDau = Convert.ToDateTime(drv["ThoiGianBatDau"]);
                if (drv.Row.Table.Columns.Contains("DiaDiem") && drv["DiaDiem"] != DBNull.Value)
                    dto.DiaDiem = drv["DiaDiem"].ToString();
            }

            // Chuyển hướng sang Trang Thanh toán (3.3)
            MoTrangThanhToan(dto);
        }

        private void MoTrangThanhToan(ThongTinDatVeDTO dto)
        {
            FrmThanhToan frmTT = new FrmThanhToan(dto);

            // Nếu Form đang nhúng trong FrmMain thì chuyển đổi trực tiếp trên FrmMain
            if (this.ParentForm is FrmMain mainForm)
            {
                mainForm.OpenChildForm(frmTT, null);
            }
            else
            {
                // Nếu chạy độc lập, hiển thị dạng Dialog
                frmTT.ShowDialog(this);
            }
        }
        #endregion

        private void lblLegendTitle_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using QuanLySuKienVaBanVe.GUI.Helpers;

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

        // Quản lý giữ ghế tạm thời
        private readonly List<int> _currentlyHeldSeatIds = new List<int>();

        // Lưu thông tin đơn vừa thanh toán để in vé
        private int _lastPaidMaDon = 0;
        private List<string> _lastPaidMaVeCodes = new List<string>();

        // Lưu ánh xạ button trên sơ đồ
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
            public string Category { get; set; }
            public Rectangle Bounds { get; set; }
        }

        public FrmBanVe()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Wire up event handlers
            this.Load += FrmBanVe_Load;
            this.SizeChanged += FrmBanVe_SizeChanged;
            this.FormClosing += FrmBanVe_FormClosing;
            pnlMainBody.SizeChanged += (s, e) => ResponsiveLayout();
            pnlSeatMapCanvas.Resize += (s, e) => CenterSeatMap();
            flpPriceList.ClientSizeChanged += (s, e) => AdjustPriceListRowWidths();

            cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;
            cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
        }

        private void FrmBanVe_Load(object sender, EventArgs e)
        {
            LoadIcons();
            ResponsiveLayout();
            LoadDanhSachSuKien();
        }

        private void LoadIcons()
        {
            try
            {
                if (pbEventTimeIcon != null) pbEventTimeIcon.Image = IconHelper.GetIcon("icon_calendar.png");
                if (pbEventLocIcon != null) pbEventLocIcon.Image = IconHelper.GetIcon("icon_location.png");
            }
            catch { }
        }

        private void FrmBanVe_SizeChanged(object sender, EventArgs e)
        {
            ResponsiveLayout();
        }

        private void FrmBanVe_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Tự động giải phóng ghế nếu form bị đóng
            if (_currentlyHeldSeatIds.Count > 0)
            {
                GheBLL.GiaiPhongDanhSachGhe(_currentlyHeldSeatIds);
                _currentlyHeldSeatIds.Clear();
            }
        }

        private void ResponsiveLayout()
        {
            if (pnlMainBody.ClientSize.Width <= 0) return;

            int padding = 16;
            int gap = 16;
            int totalW = pnlMainBody.ClientSize.Width - (padding * 2);
            int totalH = pnlMainBody.ClientSize.Height - (padding * 2);

            int rightW = 480;
            if (totalW < 1100) rightW = 450;
            if (totalW < 950) rightW = 400;
            int leftW = totalW - rightW - gap;

            if (leftW > 280)
            {
                pnlSeatMapContainer.SetBounds(padding, padding, leftW, totalH);
                pnlRightAction.SetBounds(padding + leftW + gap, padding, rightW, totalH);
            }

            CenterSeatMap();
            AdjustPriceListRowWidths();
        }

        #region 1. Cascade Loading: Sự kiện -> Suất diễn
        private void LoadDanhSachSuKien()
        {
            try
            {
                DataTable dtSK = SuKienBLL.LayDanhSach();

                cboSuKien.SelectedIndexChanged -= cboSuKien_SelectedIndexChanged;
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
                cboSuKien.DataSource = dtSK;
                cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;

                if (dtSK != null && dtSK.Rows.Count > 0)
                {
                    cboSuKien.SelectedIndex = 0;
                    cboSuKien_SelectedIndexChanged(cboSuKien, EventArgs.Empty);
                }
                else
                {
                    ClearSeatMap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Giải phóng ghế đang giữ nếu đổi sự kiện
            if (_currentlyHeldSeatIds.Count > 0)
            {
                GheBLL.GiaiPhongDanhSachGhe(_currentlyHeldSeatIds);
                _currentlyHeldSeatIds.Clear();
            }

            if (cboSuKien.SelectedValue != null && int.TryParse(cboSuKien.SelectedValue.ToString(), out int maSK))
            {
                _currentMaSK = maSK;
                lblEventTitle.Text = cboSuKien.Text;
                LoadSuatDienTheoSuKien(maSK);
            }
            else
            {
                _currentMaSK = 0;
                cboSuatDien.DataSource = null;
                ClearSeatMap();
            }
        }

        private void LoadSuatDienTheoSuKien(int maSK)
        {
            try
            {
                DataTable dtSuat = SuatDienBLL.LayDanhSachTheoSuKien(maSK);

                if (dtSuat != null && !dtSuat.Columns.Contains("MoTaHienThi"))
                {
                    dtSuat.Columns.Add("MoTaHienThi", typeof(string));
                    foreach (DataRow r in dtSuat.Rows)
                    {
                        DateTime tg = r["ThoiGianBatDau"] != DBNull.Value ? Convert.ToDateTime(r["ThoiGianBatDau"]) : DateTime.Now;
                        string diaDiem = r["DiaDiem"]?.ToString() ?? "";
                        string trangThai = r["TrangThai"]?.ToString() ?? "";
                        string statusLabel = trangThai == "MoBan" || trangThai == "MoCuaBanVe" ? "Mở bán" : "Đóng";
                        r["MoTaHienThi"] = $"{tg:dd/MM/yyyy - HH:mm} | {diaDiem} ({statusLabel})";
                    }
                }

                cboSuatDien.SelectedIndexChanged -= cboSuatDien_SelectedIndexChanged;
                cboSuatDien.DisplayMember = "MoTaHienThi";
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
            if (_currentlyHeldSeatIds.Count > 0)
            {
                GheBLL.GiaiPhongDanhSachGhe(_currentlyHeldSeatIds);
                _currentlyHeldSeatIds.Clear();
            }

            ReadCurrentSuatInfo();
        }

        private void ReadCurrentSuatInfo()
        {
            if (cboSuatDien.SelectedItem is DataRowView drv)
            {
                _currentMaSuat = Convert.ToInt32(drv["MaSuat"]);
                _currentLoaiSoDo = drv.Row.Table.Columns.Contains("LoaiSoDo") && drv["LoaiSoDo"] != DBNull.Value
                    ? Convert.ToInt32(drv["LoaiSoDo"]) : 1;

                DateTime tg = drv.Row.Table.Columns.Contains("ThoiGianBatDau") && drv["ThoiGianBatDau"] != DBNull.Value
                    ? Convert.ToDateTime(drv["ThoiGianBatDau"]) : DateTime.Now;
                string diaDiem = drv.Row.Table.Columns.Contains("DiaDiem") ? drv["DiaDiem"]?.ToString() : "";

                lblEventTime.Text = $"{tg:HH:mm} - {tg.AddHours(2.5):HH:mm}, {tg:dd} Tháng {tg:MM, yyyy}";
                lblEventLocation.Text = string.IsNullOrWhiteSpace(diaDiem) ? "Địa điểm sự kiện" : diaDiem;

                LoadKhuVucVaVe();
            }
        }
        #endregion

        #region 2. Bảng màu định danh theo MaVung (Chuẩn Ticketbox)
        public static Color GetZoneColor(string maVung)
        {
            if (string.IsNullOrWhiteSpace(maVung)) return Color.FromArgb(100, 116, 139);
            string code = maVung.Trim().ToUpperInvariant();

            switch (code)
            {
                case "VIP_A": return Color.FromArgb(217, 119, 6);   // Cam hổ phách
                case "VIP_B": return Color.FromArgb(202, 138, 4);   // Vàng đồng
                case "VIP_C": return Color.FromArgb(220, 38, 38);   // Đỏ san hô
                case "VIP_D": return Color.FromArgb(219, 39, 119);  // Hồng đậm
                case "GA_A":  return Color.FromArgb(37, 99, 235);   // Xanh dương
                case "GA_B":  return Color.FromArgb(13, 148, 136);  // Xanh ngọc bích
                case "GA_C":  return Color.FromArgb(79, 70, 229);   // Tím Indigo
                case "GA_D":  return Color.FromArgb(2, 132, 199);   // Xanh lam sáng
                case "GA_E":  return Color.FromArgb(5, 150, 105);   // Xanh lục bảo
                case "GA_F":  return Color.FromArgb(147, 51, 234);  // Tím hồng
                case "GA_G":  return Color.FromArgb(159, 18, 57);   // Đỏ rượu
                case "GA_H":  return Color.FromArgb(71, 85, 105);   // Xanh đá
                case "CAT":
                case "CAT_A": return Color.FromArgb(109, 40, 217);  // Tím đậm
                case "CAT_B": return Color.FromArgb(124, 45, 18);   // Nâu sẫm
                case "CAT_C": return Color.FromArgb(30, 58, 138);   // Xanh navy
                case "CAT_D": return Color.FromArgb(131, 24, 67);   // Hồng tím
                default:
                    if (code.StartsWith("VIP")) return Color.FromArgb(217, 119, 6);
                    if (code.StartsWith("GA")) return Color.FromArgb(37, 99, 235);
                    return Color.FromArgb(109, 40, 217);
            }
        }
        #endregion

        #region 3. Load Khu vực & Vẽ Sơ đồ Seatmap & Bảng giá chỉ đọc
        private void LoadKhuVucVaVe()
        {
            if (_currentMaSuat <= 0) return;

            // Load danh sách khu vực thực tế từ CSDL
            _dtKhuVucCurrent = KhuVucBLL.LayDanhSachTheoSuat(_currentMaSuat);

            // Nạp danh sách Giá vé chỉ đọc bên sidebar
            RenderPriceList(_dtKhuVucCurrent);

            // Vẽ sơ đồ ghế theo loại sơ đồ được chọn
            DrawSeatMap(_currentLoaiSoDo, _dtKhuVucCurrent);
        }

        private void RenderPriceList(DataTable dt)
        {
            flpPriceList.SuspendLayout();
            flpPriceList.Controls.Clear();
            flpPriceList.AutoScroll = false;
            flpPriceList.HorizontalScroll.Maximum = 0;
            flpPriceList.HorizontalScroll.Visible = false;
            flpPriceList.HorizontalScroll.Enabled = false;
            flpPriceList.AutoScroll = true;

            if (dt != null && dt.Rows.Count > 0)
            {
                int itemW = Math.Max(320, flpPriceList.ClientSize.Width - 6);

                foreach (DataRow r in dt.Rows)
                {
                    string maVung = r["MaVung"]?.ToString() ?? "";
                    string tenKV = r["TenKV"]?.ToString() ?? "";
                    decimal gia = Convert.ToDecimal(r["GiaVe"]);
                    int conLai = r.Table.Columns.Contains("ConLai") ? Convert.ToInt32(r["ConLai"]) : 0;
                    bool choPhep = !r.Table.Columns.Contains("ChoPhepBanVe") || Convert.ToBoolean(r["ChoPhepBanVe"]);

                    string hexColor = r.Table.Columns.Contains("MauSac") && r["MauSac"] != DBNull.Value ? r["MauSac"].ToString() : "";
                    Color zoneColor = !string.IsNullOrWhiteSpace(hexColor) ? ColorTranslator.FromHtml(hexColor) : GetZoneColor(maVung);

                    // Panel mỗi dòng khu vực
                    Panel pnlRow = new Panel
                    {
                        Width = itemW,
                        Height = 36,
                        Margin = new Padding(0, 0, 0, 6),
                        BackColor = Color.White,
                        Tag = "PriceRow"
                    };

                    // Ô màu bo góc bên trái (chuẩn Ticketbox)
                    Guna2Panel pnlSwatch = new Guna2Panel
                    {
                        Location = new Point(4, 8),
                        Size = new Size(24, 18),
                        BorderRadius = 4,
                        FillColor = zoneColor,
                        BorderThickness = 0
                    };

                    // Giá vé bên phải (Dock Right)
                    Label lblPrice = new Label
                    {
                        Dock = DockStyle.Right,
                        Width = 140,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleRight,
                        Padding = new Padding(0, 0, 8, 0)
                    };

                    if (!choPhep || conLai <= 0)
                    {
                        lblPrice.Text = "Hết vé";
                        lblPrice.ForeColor = Color.FromArgb(239, 68, 68);
                    }
                    else
                    {
                        lblPrice.Text = $"{gia:N0} đ";
                        lblPrice.ForeColor = Color.FromArgb(45, 194, 117);
                    }

                    // Tên khu vực
                    Label lblName = new Label
                    {
                        Location = new Point(34, 6),
                        Size = new Size(Math.Max(120, itemW - 34 - 140 - 12), 24),
                        Text = tenKV,
                        Font = new Font("Segoe UI Semibold", 9.5F),
                        ForeColor = Color.FromArgb(51, 65, 85),
                        TextAlign = ContentAlignment.MiddleLeft,
                        AutoEllipsis = true
                    };

                    pnlRow.Controls.Add(lblPrice);
                    pnlRow.Controls.Add(lblName);
                    pnlRow.Controls.Add(pnlSwatch);

                    flpPriceList.Controls.Add(pnlRow);
                }
            }

            flpPriceList.ResumeLayout();
        }

        private void AdjustPriceListRowWidths()
        {
            if (flpPriceList == null || flpPriceList.Controls.Count == 0) return;
            int newW = Math.Max(320, flpPriceList.ClientSize.Width - 6);
            foreach (Control c in flpPriceList.Controls)
            {
                if (c is Panel pnlRow && pnlRow.Tag?.ToString() == "PriceRow")
                {
                    pnlRow.Width = newW;
                    foreach (Control child in pnlRow.Controls)
                    {
                        if (child is Label lbl && child.Dock != DockStyle.Right && child.Location.X > 20)
                        {
                            lbl.Width = Math.Max(120, newW - 34 - 140 - 12);
                        }
                    }
                }
            }
        }

        private void ClearSeatMap()
        {
            if (seatMapWrapper != null)
                seatMapWrapper.Controls.Clear();
            _zoneMetaMap.Clear();
            _selectedZoneButton = null;
            flpPriceList.Controls.Clear();
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

            // 1. Vẽ các khối sân khấu & lối đi trang trí
            DrawStagesAndCatwalks(loaiSoDo);

            // 2. Thêm các Button khu vực có thể tương tác
            HashSet<int> matchedMaKVs = new HashSet<int>();
            foreach (var zone in definitions)
            {
                DataRow matchedRow = null;

                if (dtKhuVuc != null)
                {
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

                string hexColor = matchedRow != null && matchedRow.Table.Columns.Contains("MauSac") && matchedRow["MauSac"] != DBNull.Value ? matchedRow["MauSac"].ToString() : "";
                Color baseColor = !string.IsNullOrWhiteSpace(hexColor) ? ColorTranslator.FromHtml(hexColor) : GetZoneColor(zone.Code);

                if (matchedRow != null)
                {
                    decimal gia = Convert.ToDecimal(matchedRow["GiaVe"]);
                    int conLai = matchedRow.Table.Columns.Contains("ConLai") ? Convert.ToInt32(matchedRow["ConLai"]) : 0;
                    bool choPhep = !matchedRow.Table.Columns.Contains("ChoPhepBanVe") || Convert.ToBoolean(matchedRow["ChoPhepBanVe"]);

                    btn.FillColor = (!choPhep || conLai <= 0) ? Color.FromArgb(148, 163, 184) : baseColor;
                    btn.ForeColor = Color.White;
                    btn.BorderColor = Color.Transparent;
                    btn.BorderThickness = 0;

                    // Chỉ hiển thị tên vùng, nếu hết vé thì hiển thị HẾT VÉ (theo đúng chuẩn Ticketbox, không nhồi giá tiền)
                    btn.Text = (conLai > 0 && choPhep) ? zone.DisplayName : $"{zone.DisplayName}\nHẾT VÉ";

                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(btn, $"Khu vực: {zone.DisplayName}\nĐơn giá: {gia:N0} đ\nCòn trống: {conLai} vé\nBấm vào để chọn vé");

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
                    btn.Text = $"{zone.DisplayName}";
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
            if (loaiSoDo == 1) // Sơ đồ chữ T
            {
                list.Add(new ZoneDefinition { Code = "VIP_A", DisplayName = "VIP A", Category = "VIP", Bounds = new Rectangle(93, 160, 142, 129) });
                list.Add(new ZoneDefinition { Code = "VIP_B", DisplayName = "VIP B", Category = "VIP", Bounds = new Rectangle(362, 160, 132, 129) });
                list.Add(new ZoneDefinition { Code = "GA_A", DisplayName = "GA A", Category = "Phổ thông", Bounds = new Rectangle(17, 96, 70, 278) });
                list.Add(new ZoneDefinition { Code = "GA_B", DisplayName = "GA B", Category = "Phổ thông", Bounds = new Rectangle(514, 96, 70, 278) });
                list.Add(new ZoneDefinition { Code = "CAT", DisplayName = "CAT", Category = "Thường", Bounds = new Rectangle(69, 410, 464, 66) });
            }
            else if (loaiSoDo == 2) // Sơ đồ hình chữ nhật
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
            else // Sơ đồ lưới
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

                list.Add(new ZoneDefinition { Code = "CAT_A", DisplayName = "CAT A", Category = "Thường", Bounds = new Rectangle(150, 300, 190, 50) });
                list.Add(new ZoneDefinition { Code = "CAT_B", DisplayName = "CAT B", Category = "Thường", Bounds = new Rectangle(350, 300, 190, 50) });
            }
            return list;
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

        #region 4. Chọn Khu vực -> Popup Số lượng (FrmChonSoLuongVe) -> Thanh toán
        private void ZoneButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Guna2Button clickedBtn)) return;
            if (!_zoneMetaMap.ContainsKey(clickedBtn)) return;

            ZoneButtonMeta meta = _zoneMetaMap[clickedBtn];

            if (meta.DataRow == null)
            {
                MessageBox.Show("Khu vực này chưa được mở bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool choPhep = !meta.DataRow.Table.Columns.Contains("ChoPhepBanVe") || Convert.ToBoolean(meta.DataRow["ChoPhepBanVe"]);
            int conLai = meta.DataRow.Table.Columns.Contains("ConLai") ? Convert.ToInt32(meta.DataRow["ConLai"]) : 0;

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

            // Làm nổi bật viền xanh lá của nút được click
            if (_selectedZoneButton != null && _selectedZoneButton != clickedBtn)
            {
                _selectedZoneButton.BorderThickness = 0;
            }

            _selectedZoneButton = clickedBtn;
            clickedBtn.BorderColor = Color.FromArgb(16, 185, 129);
            clickedBtn.BorderThickness = 3;

            string tenKV = meta.DataRow["TenKV"]?.ToString() ?? meta.Definition.DisplayName;
            string hangVe = meta.DataRow["HangVe"]?.ToString() ?? meta.Definition.Category;
            decimal giaVe = Convert.ToDecimal(meta.DataRow["GiaVe"]);
            int maKV = Convert.ToInt32(meta.DataRow["MaKV"]);
            int tongSoGhe = meta.DataRow.Table.Columns.Contains("TongSoGhe") ? Convert.ToInt32(meta.DataRow["TongSoGhe"]) : 0;
            int daBan = meta.DataRow.Table.Columns.Contains("DaBan") ? Convert.ToInt32(meta.DataRow["DaBan"]) : 0;

            // Bước 2: Mở modal overlay mờ chỉ trong phần center khung trong (bỏ sidebar, header)
            Form topForm = this.FindForm() ?? this;
            using (Form overlay = new Form())
            {
                overlay.StartPosition = FormStartPosition.Manual;
                overlay.FormBorderStyle = FormBorderStyle.None;
                overlay.BackColor = Color.Black;
                overlay.Opacity = 0.50;
                overlay.ShowInTaskbar = false;
                // Chỉ làm mờ phần center (pnlMainBody), giữ nguyên header và sidebar
                Point centerScreenPt = pnlMainBody.PointToScreen(Point.Empty);
                overlay.Bounds = new Rectangle(centerScreenPt, pnlMainBody.ClientSize);
                overlay.Show(topForm);

                using (FrmChonSoLuongVe frmPopup = new FrmChonSoLuongVe(tenKV, giaVe, conLai, Math.Min(conLai, 10)))
                {
                    DialogResult drPopup = frmPopup.ShowDialog(overlay);
                    overlay.Close();

                    if (drPopup != DialogResult.OK)
                    {
                        // Người dùng bấm "Chọn khu vực khác" hoặc bấm X
                        clickedBtn.BorderThickness = 0;
                        _selectedZoneButton = null;
                        return;
                    }

                    int soLuongChon = frmPopup.SelectedQuantity;

                // Giải phóng các ghế cũ đang giữ nếu có
                if (_currentlyHeldSeatIds.Count > 0)
                {
                    GheBLL.GiaiPhongDanhSachGhe(_currentlyHeldSeatIds);
                    _currentlyHeldSeatIds.Clear();
                }

                // Thực thi phân bổ và giữ ghế tự động theo quy tắc:
                // "Thanh toán càng sớm, ghế càng gần sân khấu"
                List<GheDTO> dsGheGiu;
                string errGiuGhe;
                bool okGiuGhe = GheBLL.GiuGheTuDongTheoKhuVuc(maKV, soLuongChon, out dsGheGiu, out errGiuGhe);

                if (!okGiuGhe || dsGheGiu == null || dsGheGiu.Count < soLuongChon)
                {
                    MessageBox.Show("Không thể giữ vé: " + errGiuGhe, "Lỗi giữ vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    clickedBtn.BorderThickness = 0;
                    _selectedZoneButton = null;
                    LoadKhuVucVaVe();
                    return;
                }

                // Lưu danh sách ID ghế vừa giữ thành công
                _currentlyHeldSeatIds.Clear();
                _currentlyHeldSeatIds.AddRange(dsGheGiu.Select(g => g.MaGhe));

                // Bước 3: Đóng gói DTO và mở FrmThanhToan
                ThongTinDatVeDTO dto = new ThongTinDatVeDTO
                {
                    MaSK = _currentMaSK,
                    TenSK = cboSuKien.Text,
                    MaSuat = _currentMaSuat,
                    ThoiGianBatDau = DateTime.Now,
                    DiaDiem = lblEventLocation.Text,
                    LoaiSoDo = _currentLoaiSoDo,
                    MaKV = maKV,
                    MaVung = meta.Definition.Code,
                    TenKV = tenKV,
                    HangVe = hangVe,
                    GiaVe = giaVe,
                    TongSoGhe = tongSoGhe,
                    DaBan = daBan,
                    ConLai = conLai,
                    SoLuong = soLuongChon,
                    DanhSachMaGhe = _currentlyHeldSeatIds.ToList(),
                    DanhSachGhe = dsGheGiu.Select(g => g.MaGheHienThi).ToList(),
                    SoGiayConLai = 600
                };

                if (cboSuatDien.SelectedItem is DataRowView drv)
                {
                    if (drv.Row.Table.Columns.Contains("ThoiGianBatDau") && drv["ThoiGianBatDau"] != DBNull.Value)
                        dto.ThoiGianBatDau = Convert.ToDateTime(drv["ThoiGianBatDau"]);
                    if (drv.Row.Table.Columns.Contains("DiaDiem") && drv["DiaDiem"] != DBNull.Value)
                        dto.DiaDiem = drv["DiaDiem"].ToString();
                }

                using (FrmThanhToan frmTT = new FrmThanhToan(dto))
                {
                    DialogResult drTT = frmTT.ShowDialog(this);
                    if (drTT == DialogResult.OK)
                    {
                        // Thanh toán thành công!
                        _currentlyHeldSeatIds.Clear();
                        _lastPaidMaDon = dto.MaDonPhatHanh;
                        _lastPaidMaVeCodes = dto.DanhSachMaVeCode;

                        pnlBottomAction.Visible = true;
                        btnInVe.Visible = true;
                        btnInVe.Enabled = true;

                        LoadKhuVucVaVe();
                    }
                    else
                    {
                        // Người dùng bấm "Chọn lại vé" / Hết giờ / Hủy
                        if (_currentlyHeldSeatIds.Count > 0)
                        {
                            GheBLL.GiaiPhongDanhSachGhe(_currentlyHeldSeatIds);
                            _currentlyHeldSeatIds.Clear();
                        }

                        clickedBtn.BorderThickness = 0;
                        _selectedZoneButton = null;

                        LoadKhuVucVaVe();
                    }
                }
            }
            }
        }
        #endregion

        #region 5. Tìm kiếm nhanh khu vực
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword)) return;

            foreach (var kvp in _zoneMetaMap)
            {
                string name = kvp.Value.Definition.DisplayName;
                string code = kvp.Value.Definition.Code;

                if (name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ZoneButton_Click(kvp.Key, EventArgs.Empty);
                    break;
                }
            }
        }
        #endregion

        #region 6. In vé vừa đặt
        private void btnInVe_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng in vé đang được phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}

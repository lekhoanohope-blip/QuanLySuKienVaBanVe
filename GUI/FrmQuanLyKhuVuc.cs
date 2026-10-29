using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLyKhuVuc : Form
    {
        private int? _editingMaKV = null;
        private Panel seatMapWrapper;

        // [FIX-P2-3] Ánh xạ control vùng -> MaKV thực tế (chỉ dùng cho click)
        private readonly Dictionary<Guna.UI2.WinForms.Guna2Button, int> _zoneMaKVMap
            = new Dictionary<Guna.UI2.WinForms.Guna2Button, int>();

        public FrmQuanLyKhuVuc()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FrmQuanLyKhuVuc_Load(object sender, EventArgs e)
        {
            LoadComboBoxSuKien();

            // ============ PHÂN QUYỀN ADMIN / STAFF ============
            // Chỉ Admin được Thêm/Sửa/Xóa khu vực. Staff chỉ xem.
            if (!PhienDangNhap.LaQuanLy)
            {
                btnThemKhuVuc.Visible = false;          // Ẩn nút Thêm
                pnlRightInfo.Visible = false;          // Ẩn panel form nhập
                btnToggleInfo.Visible = false;          // Ẩn nút mở/đóng panel

                // Ẩn 2 cột nút Sửa / Xóa
                if (dgvKhuVuc.Columns["colSua"] != null)
                    dgvKhuVuc.Columns["colSua"].Visible = false;
                if (dgvKhuVuc.Columns["colXoa"] != null)
                    dgvKhuVuc.Columns["colXoa"].Visible = false;
            }
            // ====================================================
        }

        private void LoadComboBoxSuKien()
        {
            try
            {
                DataTable dt = SuKienBLL.LayDanhSach();

                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
                cboSuKien.DataSource = dt;

                cboSuKien.SelectedIndexChanged -= cboSuKien_SelectedIndexChanged;
                cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Sự kiện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue == null || cboSuKien.SelectedValue is DataRowView) return;
            try
            {
                int maSK = Convert.ToInt32(cboSuKien.SelectedValue);
                DataTable dt = SuatDienBLL.LayDanhSachTheoSuKien(maSK);

                cboSuatDien.DataSource = null;
                cboSuatDien.DisplayMember = "ThoiGianBatDau";
                cboSuatDien.ValueMember = "MaSuat";
                cboSuatDien.DataSource = dt;

                cboSuatDien.SelectedIndexChanged -= cboSuatDien_SelectedIndexChanged;
                cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Suất diễn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSuatDien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        // ============================================================
        // LOAD DATA
        // ============================================================
        private void LoadData()
        {
            if (cboSuatDien.SelectedValue == null || cboSuatDien.SelectedValue is DataRowView) return;

            try
            {
                int maSuat = Convert.ToInt32(cboSuatDien.SelectedValue);

                // 1. Lấy Loại Sơ Đồ qua BLL
                SuatDienDTO sd = SuatDienBLL.LayTheoMa(maSuat);
                int loaiSoDo = sd != null ? sd.LoaiSoDo : 1;

                // 2. Vẽ sơ đồ
                DrawSeatMap(loaiSoDo);

                // 3. Load bảng + legend qua BLL
                DataTable dt = KhuVucBLL.LayDanhSachTheoSuat(maSuat);

                dgvKhuVuc.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    int idx = dgvKhuVuc.Rows.Add();
                    dgvKhuVuc.Rows[idx].Cells["colMaKV"].Value = row["MaKV"];
                    dgvKhuVuc.Rows[idx].Cells["colTenKV"].Value = row["TenKV"];
                    dgvKhuVuc.Rows[idx].Cells["colLoaiKV"].Value = row["HangVe"];
                    dgvKhuVuc.Rows[idx].Cells["colSucChua"].Value = row["TongSoGhe"];
                    dgvKhuVuc.Rows[idx].Cells["colGiaVe"].Value = Convert.ToDecimal(row["GiaVe"]).ToString("N0") + " đ";
                    dgvKhuVuc.Rows[idx].Cells["colDaBan"].Value = row["DaBan"];
                    dgvKhuVuc.Rows[idx].Cells["colConLai"].Value = row["ConLai"];

                    bool choPhep = Convert.ToBoolean(row["ChoPhepBanVe"]);
                    dgvKhuVuc.Rows[idx].Cells["colTrangThai"].Value =
                        choPhep ? "Đang mở bán" : "Đang đóng";
                    dgvKhuVuc.Rows[idx].Tag = choPhep;
                }

                // Ánh xạ vùng -> MaKV (chỉ phục vụ click)
                BindZonesToMaKV(dt);

                // Vẽ lại legend (DÙNG LOGIC GỐC)
                UpdateCardsAndLegend(dt);

                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Khu vực: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // [FIX-P2-3] ÁNH XẠ VÙNG -> MaKV (chỉ cho click)
        // ============================================================
        private void BindZonesToMaKV(DataTable dt)
        {
            _zoneMaKVMap.Clear();
            if (seatMapWrapper == null) return;

            foreach (Control ctrl in seatMapWrapper.Controls)
            {
                if (!(ctrl is Guna.UI2.WinForms.Guna2Button btn)) continue;

                string category = btn.Tag?.ToString() ?? "";
                if (category == "STAGE") continue;

                string zoneText = btn.Text.Trim();
                int matchedMaKV = 0;

                // (1) Khớp chính xác
                foreach (DataRow row in dt.Rows)
                {
                    if (row["TenKV"].ToString().Trim()
                        .Equals(zoneText, StringComparison.OrdinalIgnoreCase))
                    {
                        matchedMaKV = Convert.ToInt32(row["MaKV"]);
                        break;
                    }
                }
                // (2) Khớp chứa
                if (matchedMaKV == 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["TenKV"].ToString().Trim()
                            .IndexOf(zoneText, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matchedMaKV = Convert.ToInt32(row["MaKV"]);
                            break;
                        }
                    }
                }
                // (3) Fallback theo category nếu duy nhất
                if (matchedMaKV == 0)
                {
                    string dbCategory = category == "GA" ? "PHO THONG"
                                      : category == "CAT" ? "THUONG"
                                      : category;
                    int count = 0, first = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string hangVe = RemoveSign(row["HangVe"].ToString().ToUpper());
                        if (hangVe.Contains(dbCategory))
                        {
                            count++;
                            if (first == 0) first = Convert.ToInt32(row["MaKV"]);
                        }
                    }
                    if (count == 1) matchedMaKV = first;
                }

                if (matchedMaKV > 0) _zoneMaKVMap[btn] = matchedMaKV;
            }
        }

        // ============================================================
        // VẼ SƠ ĐỒ
        // ============================================================
        private void DrawSeatMap(int loaiSoDo)
        {
            if (seatMapWrapper == null)
            {
                seatMapWrapper = new Panel { BackColor = Color.White };
                pnlSeatMap.Controls.Add(seatMapWrapper);
            }
            seatMapWrapper.Controls.Clear();
            _zoneMaKVMap.Clear();

            Color clrVip = Color.FromArgb(147, 51, 234);
            Color clrGA = Color.FromArgb(56, 189, 248);
            Color clrCAT = Color.FromArgb(245, 158, 11);
            Color clrStage = Color.FromArgb(220, 220, 230);

            if (loaiSoDo == 1)
            {
                AddZone(148, 73, 300, 60, "STAGE", "STAGE", clrStage, Color.Black);
                AddZone(93, 39, 413, 45, "STAGE", "STAGE", clrStage, Color.Black);
                AddZone(256, 112, 80, 230, "LỐI ĐI", "STAGE", clrStage, Color.Black);
                AddZone(128, 331, 329, 71, "STAGE", "STAGE", clrStage, Color.Black);
                AddZone(93, 160, 142, 129, "VIP A", "VIP", clrVip, Color.White);
                AddZone(362, 160, 132, 129, "VIP B", "VIP", clrVip, Color.White);
                AddZone(17, 96, 70, 278, "GA A", "GA", clrGA, Color.White);
                AddZone(514, 96, 70, 278, "GA B", "GA", clrGA, Color.White);
                AddZone(69, 420, 464, 66, "CAT", "CAT", clrCAT, Color.White);
            }
            else if (loaiSoDo == 2)
            {
                AddZone(130, 30, 400, 60, "SÂN KHẤU", "STAGE", clrStage, Color.Black);
                AddZone(40, 120, 80, 60, "GA A", "GA", clrGA, Color.White);
                AddZone(140, 120, 80, 60, "Vip B", "VIP", clrVip, Color.White);
                AddZone(240, 120, 180, 60, "Vip A", "VIP", clrVip, Color.White);
                AddZone(440, 120, 80, 60, "Vip C", "VIP", clrVip, Color.White);
                AddZone(540, 120, 80, 60, "GA B", "GA", clrGA, Color.White);
                AddZone(40, 200, 180, 70, "GA C", "GA", clrGA, Color.White);
                AddZone(240, 200, 180, 60, "Vip D", "VIP", clrVip, Color.White);
                AddZone(440, 200, 180, 70, "GA D", "GA", clrGA, Color.White);
                AddZone(40, 290, 170, 80, "CAT B", "CAT", clrCAT, Color.White);
                AddZone(240, 280, 180, 60, "GA E", "GA", clrGA, Color.White);
                AddZone(440, 290, 170, 80, "CAT C", "CAT", clrCAT, Color.White);
                AddZone(240, 360, 180, 40, "CAT A", "CAT", clrCAT, Color.White);
            }
            else if (loaiSoDo == 3)
            {
                AddZone(150, 10, 400, 60, "Sân khấu", "STAGE", clrStage, Color.Black);
                AddZone(150, 90, 90, 30, "Vip A", "VIP", clrVip, Color.White);
                AddZone(250, 90, 90, 30, "Vip B", "VIP", clrVip, Color.White);
                AddZone(350, 90, 90, 30, "Vip C", "VIP", clrVip, Color.White);
                AddZone(450, 90, 90, 30, "Vip D", "VIP", clrVip, Color.White);
                AddZone(150, 140, 90, 60, "GA A", "GA", clrGA, Color.White);
                AddZone(250, 140, 90, 60, "GA B", "GA", clrGA, Color.White);
                AddZone(350, 140, 90, 60, "GA C", "GA", clrGA, Color.White);
                AddZone(450, 140, 90, 60, "GA D", "GA", clrGA, Color.White);
                AddZone(150, 220, 90, 60, "GA E", "GA", clrGA, Color.White);
                AddZone(250, 220, 90, 60, "GA F", "GA", clrGA, Color.White);
                AddZone(350, 220, 90, 60, "GA G", "GA", clrGA, Color.White);
                AddZone(450, 220, 90, 60, "GA H", "GA", clrGA, Color.White);
                AddZone(20, 90, 100, 190, "CAT C", "CAT", clrCAT, Color.White);
                AddZone(580, 90, 100, 190, "CAT D", "CAT", clrCAT, Color.White);
                AddZone(150, 310, 190, 70, "CAT A", "CAT", clrCAT, Color.White);
                AddZone(360, 310, 190, 70, "CAT B", "CAT", clrCAT, Color.White);
            }

            seatMapWrapper.Controls.Add(pnlLegend);
            pnlLegend.Visible = true;
            PositionLegendAndResize();
            CenterSeatMap();
        }

        private void AddZone(int x, int y, int w, int h, string text, string tag, Color fillColor, Color textColor)
        {
            Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
            btn.BorderRadius = 8;
            btn.FillColor = fillColor;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.ForeColor = textColor;
            btn.Location = new Point(x, y);
            btn.Size = new Size(w, h);
            btn.Tag = tag;
            btn.Text = text;
            btn.Click += Zone_Click;
            seatMapWrapper.Controls.Add(btn);
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

            pnlLegend.Location = new Point(maxRight + 30, 10);
            pnlLegend.Size = new Size(220, 500);

            int wrapperWidth = maxRight + 30 + 220 + 20;
            int wrapperHeight = Math.Max(maxBottom + 20, 520);
            seatMapWrapper.Size = new Size(wrapperWidth, wrapperHeight);
        }

        // ============================================================
        // [FIX-P2-3] CLICK VÙNG
        // ============================================================
        private void Zone_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as Guna.UI2.WinForms.Guna2Button;
            if (clickedBtn == null) return;

            if (seatMapWrapper != null)
            {
                foreach (Control ctrl in seatMapWrapper.Controls)
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn) btn.BorderThickness = 0;
            }
            clickedBtn.BorderColor = Color.Black;
            clickedBtn.BorderThickness = 3;

            if (!_zoneMaKVMap.TryGetValue(clickedBtn, out int maKV)) return;

            foreach (DataGridViewRow row in dgvKhuVuc.Rows)
            {
                if (row.Cells["colMaKV"].Value != null &&
                    Convert.ToInt32(row.Cells["colMaKV"].Value) == maKV)
                {
                    row.Selected = true;
                    dgvKhuVuc.CurrentCell = row.Cells["colTenKV"];
                    PopulateInputsFromRow(row);
                    break;
                }
            }
        }

        private void PopulateInputsFromRow(DataGridViewRow row)
        {
            _editingMaKV = Convert.ToInt32(row.Cells["colMaKV"].Value);
            txtTenKV.Text = row.Cells["colTenKV"].Value?.ToString() ?? "";
            cboLoaiKV.Text = row.Cells["colLoaiKV"].Value?.ToString() ?? "";
            txtSucChua.Text = row.Cells["colSucChua"].Value?.ToString() ?? "";
            txtSucChua.Enabled = false;
            txtGiaVe.Text = (row.Cells["colGiaVe"].Value?.ToString() ?? "")
                                    .Replace(" đ", "").Replace(".", "");
            toggleBanVe.Checked = row.Tag is bool b ? b : true;
        }

        // ============================================================
        // LEGEND + CARDS — GIỮ NGUYÊN LOGIC GỐC
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

            lblCard1Value.Text = tongKV.ToString();
            lblCard2Value.Text = tongGhe.ToString("N0");
            lblCard3Value.Text = soKVBanVe.ToString();
            lblCard4Value.Text = minPrice.ToString("N0") + " đ";

            pnlLegend.Controls.Clear();
            pnlLegend.Controls.Add(lblLegendTitle);
            pnlLegend.Visible = true;

            var zoneInfos = new List<ZoneLegendInfo>();
            var processedTags = new HashSet<string>();

            if (seatMapWrapper != null)
            {
                foreach (Control ctrl in seatMapWrapper.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn && btn.Tag != null)
                    {
                        string tag = btn.Tag.ToString();
                        if (tag != "STAGE" && !processedTags.Contains(tag))
                        {
                            processedTags.Add(tag);
                            decimal matchedPrice = 0;

                            string searchKeyword = "";
                            if (tag == "VIP") searchKeyword = "VIP";
                            else if (tag == "GA") searchKeyword = "PHO THONG";
                            else if (tag == "CAT") searchKeyword = "THUONG";

                            if (!string.IsNullOrEmpty(searchKeyword))
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    string tenKV = RemoveSign(row["TenKV"].ToString().ToUpper().Trim());
                                    string hangVe = RemoveSign(row["HangVe"].ToString().ToUpper().Trim());

                                    if (tenKV.Contains(searchKeyword) || hangVe.Contains(searchKeyword))
                                    {
                                        matchedPrice = Convert.ToDecimal(row["GiaVe"]);
                                        break;
                                    }
                                }
                            }

                            zoneInfos.Add(new ZoneLegendInfo
                            {
                                Tag = tag,
                                Color = btn.FillColor,
                                Price = matchedPrice
                            });
                        }
                    }
                }
            }

            var sortedZones = zoneInfos.OrderByDescending(z => z.Price).ToList();
            int yOffset = 35;
            foreach (var zone in sortedZones)
            {
                Panel pnlItem = new Panel { Size = new Size(190, 45), Location = new Point(5, yOffset) };
                Guna.UI2.WinForms.Guna2CirclePictureBox dot = new Guna.UI2.WinForms.Guna2CirclePictureBox
                { FillColor = zone.Color, Size = new Size(14, 14), Location = new Point(0, 5) };
                Label lblName = new Label
                {
                    Text = zone.Tag,
                    AutoSize = false,
                    Width = 160,
                    Height = 18,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Location = new Point(22, 2)
                };
                Label lblPrice = new Label
                {
                    Text = zone.Price > 0 ? $"{zone.Price:N0} đ" : "Chưa có giá",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.Gray,
                    Location = new Point(22, 20)
                };
                pnlItem.Controls.Add(dot);
                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(lblPrice);
                pnlLegend.Controls.Add(pnlItem);
                yOffset += 50;
            }
        }

        private class ZoneLegendInfo
        {
            public string Tag { get; set; }
            public Color Color { get; set; }
            public decimal Price { get; set; }
        }

        private string RemoveSign(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            str = str.Replace("đ", "d").Replace("Đ", "D");
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ", "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ", "í", "ì", "ỉ", "ĩ", "ị", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ", "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự", "ý", "ỳ", "ỷ", "ỹ", "ỵ" };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "i", "i", "i", "i", "i", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "y", "y", "y", "y", "y" };
            for (int i = 0; i < arr1.Length; i++)
            {
                str = str.Replace(arr1[i], arr2[i]);
                str = str.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return str;
        }

        // ============================================================
        // NÚT BẤM
        // ============================================================
        private void btnThemKhuVuc_Click(object sender, EventArgs e)
        {
            _editingMaKV = null;
            ClearInput();
            txtTenKV.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboSuatDien.SelectedValue == null || cboSuatDien.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn Suất diễn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenKV.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên khu vực!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal giaVe = 0;
            if (!decimal.TryParse(
                    txtGiaVe.Text.Replace(".", "").Replace(",", "").Replace("đ", "").Trim(),
                    out giaVe))
            {
                MessageBox.Show("Giá vé không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (giaVe < 0)
            {
                MessageBox.Show("Giá vé phải >= 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int sucChua = 0;
            if (!int.TryParse(txtSucChua.Text.Trim(), out sucChua))
            {
                MessageBox.Show("Sức chứa không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (sucChua <= 0 || sucChua > 10000)
            {
                MessageBox.Show("Sức chứa phải là số nguyên dương (1..10000)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSuat = Convert.ToInt32(cboSuatDien.SelectedValue);
            bool choPhepBanVe = toggleBanVe.Checked;

            KhuVucDTO kv = new KhuVucDTO
            {
                MaKV = _editingMaKV ?? 0,
                MaSuat = maSuat,
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
                    ? "Thêm khu vực thành công!"
                    : "Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // [FIX-P2-7] CellContentClick + nút Sửa / Xóa
        private void dgvKhuVuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvKhuVuc.Columns[e.ColumnIndex].Name;
            if (colName != "colSua" && colName != "colXoa") return;

            int maKV = Convert.ToInt32(dgvKhuVuc.Rows[e.RowIndex].Cells["colMaKV"].Value);

            if (colName == "colSua")
            {
                PopulateInputsFromRow(dgvKhuVuc.Rows[e.RowIndex]);
            }
            else if (colName == "colXoa")
            {
                if (MessageBox.Show(
                    "Xóa khu vực này sẽ xóa toàn bộ ghế bên trong. Tiếp tục?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                string result = KhuVucBLL.Xoa(maKV);
                if (result == "Success")
                {
                    MessageBox.Show("Xóa khu vực thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ClearInput()
        {
            _editingMaKV = null;
            txtTenKV.Text = "";
            cboLoaiKV.SelectedIndex = 0;
            txtSucChua.Text = "";
            txtSucChua.Enabled = true;
            txtGiaVe.Text = "";
            toggleBanVe.Checked = true;

            if (seatMapWrapper != null)
            {
                foreach (Control ctrl in seatMapWrapper.Controls)
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn) btn.BorderThickness = 0;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e) { ClearInput(); }

        private void btnToggleInfo_Click(object sender, EventArgs e)
        {
            pnlRightInfo.Visible = !pnlRightInfo.Visible;
            btnToggleInfo.Text = pnlRightInfo.Visible ? "▶" : "◀";

            pnlLeftMap.PerformLayout();
            pnlSeatMap.PerformLayout();

            this.BeginInvoke(new Action(() => { CenterSeatMap(); }));
        }

        private void CenterSeatMap()
        {
            if (seatMapWrapper == null || pnlSeatMap == null) return;

            int offsetX = (pnlSeatMap.ClientSize.Width - seatMapWrapper.Width) / 2;
            int offsetY = (pnlSeatMap.ClientSize.Height - seatMapWrapper.Height) / 2;
            if (offsetX < 0) offsetX = 0;
            if (offsetY < 0) offsetY = 0;

            seatMapWrapper.Location = new Point(offsetX, offsetY);

            if (btnToggleInfo != null)
            {
                btnToggleInfo.Location = new Point(pnlLeftMap.Width - 40, 10);
                btnToggleInfo.BringToFront();
            }
        }

        private void pnlSeatMap_Resize(object sender, EventArgs e)
        {
            CenterSeatMap();
        }
    }
}

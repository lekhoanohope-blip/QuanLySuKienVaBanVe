using QuanLySuKienVaBanVe.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLyKhuVuc : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True";
        private int? _editingMaKV = null;
        private Panel seatMapWrapper;

        public FrmQuanLyKhuVuc()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FrmQuanLyKhuVuc_Load(object sender, EventArgs e)
        {
            LoadComboBoxSuKien();
            if (!PhienDangNhap.LaQuanLy)
            {
                btnThemKhuVuc.Visible = false;
                pnlRightInfo.Visible = false;
                btnToggleInfo.Visible = false;
                if (dgvKhuVuc.Columns["colThaoTac"] != null)
                    dgvKhuVuc.Columns["colThaoTac"].Visible = false;
            }
        }

        private void LoadComboBoxSuKien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaSK, TenSK FROM SuKien", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboSuKien.DisplayMember = "TenSK";
                    cboSuKien.ValueMember = "MaSK";
                    cboSuKien.DataSource = dt;

                    cboSuKien.SelectedIndexChanged -= cboSuKien_SelectedIndexChanged;
                    cboSuKien.SelectedIndexChanged += cboSuKien_SelectedIndexChanged;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải Sự kiện: " + ex.Message); }
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue == null || cboSuKien.SelectedValue is DataRowView) return;
            try
            {
                int maSK = Convert.ToInt32(cboSuKien.SelectedValue);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaSuat, ThoiGianBatDau FROM SuatDien WHERE MaSK = @MaSK", conn);
                    da.SelectCommand.Parameters.AddWithValue("@MaSK", maSK);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboSuatDien.DataSource = null;
                    cboSuatDien.DisplayMember = "ThoiGianBatDau";
                    cboSuatDien.ValueMember = "MaSuat";
                    cboSuatDien.DataSource = dt;

                    cboSuatDien.SelectedIndexChanged -= cboSuatDien_SelectedIndexChanged;
                    cboSuatDien.SelectedIndexChanged += cboSuatDien_SelectedIndexChanged;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải Suất diễn: " + ex.Message); }
        }

        private void cboSuatDien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (cboSuatDien.SelectedValue == null || cboSuatDien.SelectedValue is DataRowView) return;

            try
            {
                int maSuat = Convert.ToInt32(cboSuatDien.SelectedValue);

                // 1. Lấy Loại Sơ Đồ từ Database
                int loaiSoDo = 1;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "SELECT LoaiSoDo FROM SuatDien WHERE MaSuat = @MaSuat";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSuat", maSuat);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value) loaiSoDo = Convert.ToInt32(result);
                }

                // 2. Vẽ sơ đồ
                DrawSeatMap(loaiSoDo);

                // 3. Load bảng dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT kv.MaKV, kv.TenKV, kv.HangVe, kv.TongSoGhe, kv.GiaVe,
                               (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DaBan') as DaBan,
                               (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'Trong') as ConLai
                        FROM KhuVuc kv
                        WHERE kv.MaSuat = @MaSuat";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@MaSuat", maSuat);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

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
                        dgvKhuVuc.Rows[idx].Cells["colTrangThai"].Value = "Đang mở bán";
                    }
                    UpdateCardsAndLegend(dt);
                    ClearInput();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải Khu vực: " + ex.Message); }
        }

        // =================== HÀM VẼ SƠ ĐỒ TỰ ĐỘNG ===================
        private void DrawSeatMap(int loaiSoDo)
        {
            if (seatMapWrapper == null)
            {
                seatMapWrapper = new Panel { BackColor = Color.White };
                pnlSeatMap.Controls.Add(seatMapWrapper);
            }
            seatMapWrapper.Controls.Clear();

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

        // =================== XỬ LÝ CLICK ===================
        private void Zone_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button clickedBtn = sender as Guna.UI2.WinForms.Guna2Button;
            if (clickedBtn == null) return;

            string zoneTag = clickedBtn.Tag?.ToString() ?? "";

            if (seatMapWrapper != null)
            {
                foreach (Control ctrl in seatMapWrapper.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn) btn.BorderThickness = 0;
                }
            }

            clickedBtn.BorderColor = Color.Black;
            clickedBtn.BorderThickness = 3;

            if (zoneTag == "STAGE") return;

            foreach (DataGridViewRow row in dgvKhuVuc.Rows)
            {
                if (row.Cells["colTenKV"].Value != null &&
                    row.Cells["colTenKV"].Value.ToString().ToUpper().Contains(zoneTag.ToUpper()))
                {
                    row.Selected = true;
                    dgvKhuVuc.CurrentCell = row.Cells["colTenKV"];

                    _editingMaKV = Convert.ToInt32(row.Cells["colMaKV"].Value);

                    txtTenKV.Text = row.Cells["colTenKV"].Value.ToString();
                    cboLoaiKV.Text = row.Cells["colLoaiKV"].Value.ToString();
                    txtSucChua.Text = row.Cells["colSucChua"].Value.ToString();
                    txtSucChua.Enabled = false;
                    txtGiaVe.Text = row.Cells["colGiaVe"].Value.ToString().Replace(" đ", "").Replace(".", "");

                    break;
                }
            }
        }

        // =================== VẼ CHÚ THÍCH ===================
        private void UpdateCardsAndLegend(DataTable dt)
        {
            int tongKV = dt.Rows.Count; int tongGhe = 0; decimal minPrice = 0;
            foreach (DataRow row in dt.Rows)
            {
                tongGhe += Convert.ToInt32(row["TongSoGhe"]);
                decimal gia = Convert.ToDecimal(row["GiaVe"]);
                if (minPrice == 0 || gia < minPrice) minPrice = gia;
            }
            lblCard1Value.Text = tongKV.ToString(); lblCard2Value.Text = tongGhe.ToString("N0");
            lblCard3Value.Text = tongKV.ToString(); lblCard4Value.Text = minPrice.ToString("N0") + " đ";

            pnlLegend.Controls.Clear(); pnlLegend.Controls.Add(lblLegendTitle);

            var zoneInfos = new List<ZoneLegendInfo>();
            var processedTags = new HashSet<string>();

            // SỬA LỖI Ở ĐÂY: Duyệt qua seatMapWrapper thay vì pnlSeatMap
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

                            zoneInfos.Add(new ZoneLegendInfo { Tag = tag, Color = btn.FillColor, Price = matchedPrice });
                        }
                    }
                }
            }

            var sortedZones = zoneInfos.OrderByDescending(z => z.Price).ToList();
            int yOffset = 35;
            foreach (var zone in sortedZones)
            {
                Panel pnlItem = new Panel { Size = new Size(190, 45), Location = new Point(5, yOffset) };
                Guna.UI2.WinForms.Guna2CirclePictureBox dot = new Guna.UI2.WinForms.Guna2CirclePictureBox { FillColor = zone.Color, Size = new Size(14, 14), Location = new Point(0, 5) };
                Label lblName = new Label { Text = zone.Tag, AutoSize = false, Width = 160, Height = 18, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Location = new Point(22, 2) };
                Label lblPrice = new Label { Text = zone.Price > 0 ? $"{zone.Price:N0} đ" : "Chưa có giá", AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.Gray, Location = new Point(22, 20) };
                pnlItem.Controls.Add(dot); pnlItem.Controls.Add(lblName); pnlItem.Controls.Add(lblPrice);
                pnlLegend.Controls.Add(pnlItem);
                yOffset += 50;
            }
        }

        private class ZoneLegendInfo { public string Tag { get; set; } public Color Color { get; set; } public decimal Price { get; set; } }

        private string RemoveSign(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            str = str.Replace("đ", "d").Replace("Đ", "D");
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ", "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ", "í", "ì", "ỉ", "ĩ", "ị", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ", "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự", "ý", "ỳ", "ỷ", "ỹ", "ỵ" };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "i", "i", "i", "i", "i", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "y", "y", "y", "y", "y" };
            for (int i = 0; i < arr1.Length; i++) { str = str.Replace(arr1[i], arr2[i]); str = str.Replace(arr1[i].ToUpper(), arr2[i].ToUpper()); }
            return str;
        }

        // =================== CÁC HÀM XỬ LÝ NÚT BẤM KHÁC ===================
        private void btnThemKhuVuc_Click(object sender, EventArgs e) { _editingMaKV = null; ClearInput(); txtTenKV.Focus(); }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboSuatDien.SelectedValue == null) { MessageBox.Show("Vui lòng chọn Suất diễn!"); return; }
            if (string.IsNullOrWhiteSpace(txtTenKV.Text)) { MessageBox.Show("Vui lòng nhập Tên khu vực!"); return; }

            decimal giaVe = 0;
            if (!decimal.TryParse(txtGiaVe.Text.Replace(".", "").Replace(",", "").Replace("đ", "").Trim(), out giaVe)) { MessageBox.Show("Giá vé không hợp lệ!"); return; }
            int sucChua = 0;
            if (!int.TryParse(txtSucChua.Text.Trim(), out sucChua)) { MessageBox.Show("Sức chứa không hợp lệ!"); return; }

            try
            {
                int maSuat = Convert.ToInt32(cboSuatDien.SelectedValue);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (_editingMaKV == null)
                    {
                        string sql = "INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) OUTPUT INSERTED.MaKV VALUES (@MaSuat, @Ten, @Loai, @Gia, @Ghe)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@MaSuat", maSuat);
                        cmd.Parameters.AddWithValue("@Ten", txtTenKV.Text.Trim());
                        cmd.Parameters.AddWithValue("@Loai", cboLoaiKV.Text);
                        cmd.Parameters.AddWithValue("@Gia", giaVe);
                        cmd.Parameters.AddWithValue("@Ghe", sucChua);
                        int newMaKV = (int)cmd.ExecuteScalar();

                        string sqlGhe = "INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV, 'A', @Cot, N'Trong')";
                        for (int i = 1; i <= sucChua; i++)
                        {
                            SqlCommand cmdGhe = new SqlCommand(sqlGhe, conn);
                            cmdGhe.Parameters.AddWithValue("@MaKV", newMaKV);
                            cmdGhe.Parameters.AddWithValue("@Cot", i);
                            cmdGhe.ExecuteNonQuery();
                        }
                        MessageBox.Show("Thêm khu vực thành công!");
                    }
                    else
                    {
                        string sql = "UPDATE KhuVuc SET TenKV = @Ten, HangVe = @Loai, GiaVe = @Gia WHERE MaKV = @MaKV";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@Ten", txtTenKV.Text.Trim());
                        cmd.Parameters.AddWithValue("@Loai", cboLoaiKV.Text);
                        cmd.Parameters.AddWithValue("@Gia", giaVe);
                        cmd.Parameters.AddWithValue("@MaKV", _editingMaKV.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật thành công!");
                    }
                }
                LoadData();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lưu: " + ex.Message); }
        }

        private void dgvKhuVuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maKV = Convert.ToInt32(dgvKhuVuc.Rows[e.RowIndex].Cells["colMaKV"].Value);
            if (dgvKhuVuc.Columns[e.ColumnIndex].Name == "colThaoTac")
            {
                Point p = dgvKhuVuc.PointToClient(Cursor.Position);
                Rectangle rect = dgvKhuVuc.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int x = p.X - rect.X;
                if (x >= 10 && x <= 35)
                {
                    _editingMaKV = maKV;
                    txtTenKV.Text = dgvKhuVuc.Rows[e.RowIndex].Cells["colTenKV"].Value.ToString();
                    cboLoaiKV.Text = dgvKhuVuc.Rows[e.RowIndex].Cells["colLoaiKV"].Value.ToString();
                    txtSucChua.Text = dgvKhuVuc.Rows[e.RowIndex].Cells["colSucChua"].Value.ToString();
                    txtSucChua.Enabled = false;
                    txtGiaVe.Text = dgvKhuVuc.Rows[e.RowIndex].Cells["colGiaVe"].Value.ToString().Replace(" đ", "").Replace(".", "");
                }
                else if (x > 35 && x <= 75)
                {
                    if (MessageBox.Show("Xóa khu vực này sẽ xóa toàn bộ ghế bên trong. Tiếp tục?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();
                                SqlCommand cmdGhe = new SqlCommand("DELETE FROM Ghe WHERE MaKV = @MaKV", conn);
                                cmdGhe.Parameters.AddWithValue("@MaKV", maKV);
                                cmdGhe.ExecuteNonQuery();
                                SqlCommand cmdKV = new SqlCommand("DELETE FROM KhuVuc WHERE MaKV = @MaKV", conn);
                                cmdKV.Parameters.AddWithValue("@MaKV", maKV);
                                cmdKV.ExecuteNonQuery();
                            }
                            LoadData();
                        }
                        catch (Exception ex) { MessageBox.Show("Không thể xóa: " + ex.Message); }
                    }
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

            // SỬA LỖI Ở ĐÂY: Duyệt qua seatMapWrapper
            if (seatMapWrapper != null)
            {
                foreach (Control ctrl in seatMapWrapper.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn) btn.BorderThickness = 0;
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e) { ClearInput(); }

        // Xử lý ẩn/hiện Form thông tin
        private void btnToggleInfo_Click(object sender, EventArgs e)
        {
            pnlRightInfo.Visible = !pnlRightInfo.Visible;
            btnToggleInfo.Text = pnlRightInfo.Visible ? "▶" : "◀";

            // Ép layout tính toán lại kích thước
            pnlLeftMap.PerformLayout();
            pnlSeatMap.PerformLayout();

            // Đợi layout chạy xong mới căn giữa (tránh bị lệch do size cũ)
            this.BeginInvoke(new Action(() => {
                CenterSeatMap();
            }));
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

        // HÀM NÀY ĐƯỢC CHUYỂN TỪ DESIGNER SANG ĐÂY
        private void pnlSeatMap_Resize(object sender, EventArgs e)
        {
            CenterSeatMap();
        }
    }
}
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmDoiVe : Form
    {
        private readonly DataRow donVeRow;
        private readonly ChiTietVeDTO veCu;
        private DataTable dtGheTrongHienTai;
        private int? maGheMoiDuocChon;
        private decimal giaVeMoiDuocChon;
        private string maGheHienThiMoi;
        private bool dangDoiVe = false;

        public int MaDonMoiDaTao { get; private set; }

        public FrmDoiVe(DataRow donVeRow, ChiTietVeDTO veCu)
        {
            InitializeComponent();
            dgvGheTrong.AutoGenerateColumns = false;
            this.donVeRow = donVeRow;
            this.veCu = veCu;
        }

        private void FrmDoiVe_Load(object sender, EventArgs e)
        {
            if (donVeRow == null || veCu == null)
            {
                MessageBox.Show("Không có thông tin đơn vé hoặc chi tiết vé hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            HienThiThongTinVeCu();
            KhoiTaoPhuongThucThanhToan();
            LoadDanhSachSuatDien();
        }

        private void HienThiThongTinVeCu()
        {
            int maDon = Convert.ToInt32(donVeRow["MaDon"]);
            string tenKH = donVeRow["TenKH"]?.ToString() ?? "";
            string sdt = donVeRow["SDT"]?.ToString() ?? "";
            string tenSK = donVeRow["TenSK"]?.ToString() ?? "";
            DateTime tgBatDau = Convert.ToDateTime(donVeRow["ThoiGianBatDau"]);

            lblDonGoc.Text = $"Đơn gốc: #{maDon}";
            lblKhachHang.Text = $"Khách hàng: {tenKH} ({sdt})";
            lblSuKien.Text = $"Sự kiện: {tenSK}";
            lblSuatCu.Text = $"Suất diễn: {tgBatDau:dd/MM/yyyy HH:mm}";
            lblVeCu.Text = $"Ghế cũ: {veCu.MaGheHienThi} - {veCu.TenKV} ({veCu.HangVe})";
            lblGiaCu.Text = $"Giá vé cũ: {veCu.GiaVe:N0} VNĐ";
            lblGiaCuHienThi.Text = $"Giá vé cũ: {veCu.GiaVe:N0} VNĐ";
        }

        private void KhoiTaoPhuongThucThanhToan()
        {
            cboPhuongThuc.Items.Clear();
            cboPhuongThuc.Items.Add(new ComboBoxItem("Tiền mặt", PhuongThucThanhToan.TienMat));
            cboPhuongThuc.Items.Add(new ComboBoxItem("Chuyển khoản", PhuongThucThanhToan.ChuyenKhoan));
            cboPhuongThuc.Items.Add(new ComboBoxItem("Thẻ ngân hàng", PhuongThucThanhToan.TheNganHang));
            cboPhuongThuc.SelectedIndex = 0;
        }

        private void LoadDanhSachSuatDien()
        {
            int maSK = Convert.ToInt32(donVeRow["MaSK"]);
            int maSuatHienTai = Convert.ToInt32(donVeRow["MaSuat"]);

            DataTable dtSuat = DonVeBLL.LaySuatDienCungSuKien(maSK);
            cboSuatMoi.Items.Clear();

            int selectedIndex = 0;
            for (int i = 0; i < dtSuat.Rows.Count; i++)
            {
                DataRow r = dtSuat.Rows[i];
                int maSuat = Convert.ToInt32(r["MaSuat"]);
                DateTime tg = Convert.ToDateTime(r["ThoiGianBatDau"]);
                string diaDiem = r["DiaDiem"]?.ToString() ?? "";
                string display = string.Format("{0:dd/MM/yyyy HH:mm} - {1}", tg, diaDiem);

                cboSuatMoi.Items.Add(new ComboBoxItem(display, maSuat));
                if (maSuat == maSuatHienTai)
                {
                    selectedIndex = i;
                }
            }

            if (cboSuatMoi.Items.Count > 0)
            {
                cboSuatMoi.SelectedIndex = selectedIndex;
            }
            else
            {
                MessageBox.Show("Hiện không có suất diễn nào khác của sự kiện còn mở bán hoặc chưa diễn ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cboSuatMoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGheTrongTheoSuat();
        }

        /// <summary>
        /// Nạp danh sách ghế trống của suất diễn.
        /// TODO [Tích hợp TV1 - UcSoDoGhe]: Sau này khi ghép UcSoDoGhe, chỉ cần khởi tạo
        /// ucSoDoGhe.LoadSoDoGhe(maSuat) vào pnlSeatSelectorHost và bắt OnSeatSelectionChanged.
        /// </summary>
        private void LoadGheTrongTheoSuat()
        {
            if (cboSuatMoi.SelectedItem == null) return;
            int maSuat = (int)((ComboBoxItem)cboSuatMoi.SelectedItem).Value;

            dtGheTrongHienTai = DonVeBLL.LayDanhSachGheTrongTheoSuat(maSuat);

            // Nạp danh sách khu vực vào cboKhuVucMoi
            cboKhuVucMoi.Items.Clear();
            cboKhuVucMoi.Items.Add(new ComboBoxItem("— Tất cả khu vực —", 0));

            var danhSachKV = dtGheTrongHienTai.DefaultView.ToTable(true, "MaKV", "TenKV");
            foreach (DataRow r in danhSachKV.Rows)
            {
                int maKV = Convert.ToInt32(r["MaKV"]);
                string tenKV = r["TenKV"].ToString();
                cboKhuVucMoi.Items.Add(new ComboBoxItem(tenKV, maKV));
            }
            cboKhuVucMoi.SelectedIndex = 0;

            HienThiGheLenLuoi(dtGheTrongHienTai);
        }

        private void cboKhuVucMoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtGheTrongHienTai == null) return;
            if (cboKhuVucMoi.SelectedItem == null) return;

            int maKV = (int)((ComboBoxItem)cboKhuVucMoi.SelectedItem).Value;
            if (maKV == 0)
            {
                HienThiGheLenLuoi(dtGheTrongHienTai);
            }
            else
            {
                DataView dv = new DataView(dtGheTrongHienTai);
                dv.RowFilter = $"MaKV = {maKV}";
                HienThiGheLenLuoi(dv.ToTable());
            }
        }

        private void HienThiGheLenLuoi(DataTable dt)
        {
            dgvGheTrong.DataSource = dt;
            if (dgvGheTrong.Columns["colGiaVe"] != null)
            {
                dgvGheTrong.Columns["colGiaVe"].DefaultCellStyle.Format = "N0";
                dgvGheTrong.Columns["colGiaVe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvGheTrong.Columns["colGiaVe"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgvGheTrong.ClearSelection();
            dgvGheTrong.CurrentCell = null;
            CapNhatThongTinGheMoi(null, 0, string.Empty);
        }

        private DataGridViewRow LayDongGheDangChon()
        {
            if (dgvGheTrong.SelectedRows.Count > 0)
            {
                return dgvGheTrong.SelectedRows[0];
            }
            if (dgvGheTrong.CurrentRow != null && dgvGheTrong.CurrentRow.Selected)
            {
                return dgvGheTrong.CurrentRow;
            }
            return null;
        }

        private void DongBoGheDangChon()
        {
            var row = LayDongGheDangChon();
            if (row != null && row.Index >= 0 && row.Cells["colMaGhe"].Value != null && row.Cells["colMaGhe"].Value != DBNull.Value)
            {
                int maGhe = Convert.ToInt32(row.Cells["colMaGhe"].Value);
                decimal giaVe = Convert.ToDecimal(row.Cells["colGiaVe"].Value);
                string hienThi = row.Cells["colMaGheHienThi"].Value?.ToString() ?? "";
                string tenKV = row.Cells["colTenKV"].Value?.ToString() ?? "";

                CapNhatThongTinGheMoi(maGhe, giaVe, $"{hienThi} - {tenKV}");
            }
            else
            {
                CapNhatThongTinGheMoi(null, 0, string.Empty);
            }
        }

        private void dgvGheTrong_SelectionChanged(object sender, EventArgs e)
        {
            DongBoGheDangChon();
        }

        private void dgvGheTrong_CurrentCellChanged(object sender, EventArgs e)
        {
            DongBoGheDangChon();
        }

        private void dgvGheTrong_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dgvGheTrong.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.None)
            {
                dgvGheTrong.ClearSelection();
                dgvGheTrong.CurrentCell = null;
                DongBoGheDangChon();
            }
        }

        private void CapNhatThongTinGheMoi(int? maGhe, decimal giaVe, string tenHienThi)
        {
            maGheMoiDuocChon = maGhe;
            giaVeMoiDuocChon = giaVe;
            maGheHienThiMoi = tenHienThi;

            btnXacNhan.Enabled = !dangDoiVe && maGhe.HasValue;

            if (maGhe.HasValue)
            {
                lblGiaMoiHienThi.Text = string.Format("Giá vé mới: {0:N0} VNĐ ({1})", giaVe, tenHienThi);
                decimal giaVeCu = veCu != null ? veCu.GiaVe : 0;
                decimal delta = DonVeBLL.TinhChenhLech(giaVeCu, giaVe);
                cboPhuongThuc.Enabled = !dangDoiVe && delta != 0m;

                if (delta > 0)
                {
                    lblKetQuaChenhLech.ForeColor = Color.FromArgb(185, 28, 28);
                    lblKetQuaChenhLech.Text = string.Format("Chênh lệch: +{0:N0} VNĐ (Khách thanh toán thêm)", delta);
                }
                else if (delta == 0)
                {
                    lblKetQuaChenhLech.ForeColor = Color.FromArgb(22, 101, 52);
                    lblKetQuaChenhLech.Text = "Chênh lệch: 0 VNĐ (Đổi ngang giá, không thu thêm)";
                }
                else
                {
                    lblKetQuaChenhLech.ForeColor = Color.FromArgb(30, 64, 175);
                    lblKetQuaChenhLech.Text = string.Format("Chênh lệch: {0:N0} VNĐ (Hoàn lại tiền cho khách)", delta);
                }
            }
            else
            {
                lblGiaMoiHienThi.Text = "Giá vé mới: Chưa chọn ghế";
                lblKetQuaChenhLech.ForeColor = Color.FromArgb(71, 85, 105);
                lblKetQuaChenhLech.Text = "Chênh lệch: Vui lòng chọn ghế mới";
                cboPhuongThuc.Enabled = !dangDoiVe;
            }
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (dangDoiVe) return;

            DongBoGheDangChon();
            errorProvider.Clear();

            if (!maGheMoiDuocChon.HasValue || maGheMoiDuocChon.Value <= 0)
            {
                errorProvider.SetError(pnlSeatSelectorHost, "Vui lòng chọn 1 vị trí ghế mới từ danh sách ghế trống.");
                MessageBox.Show("Vui lòng chọn 1 vị trí ghế mới mong muốn đổi!", "Chưa chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDonCu = Convert.ToInt32(donVeRow["MaDon"]);
            int maGheMoi = maGheMoiDuocChon.Value;
            string phuongThuc = ((ComboBoxItem)cboPhuongThuc.SelectedItem)?.Value?.ToString() ?? PhuongThucThanhToan.TienMat;
            decimal giaVeCu = veCu != null ? veCu.GiaVe : 0;
            decimal delta = DonVeBLL.TinhChenhLech(giaVeCu, giaVeMoiDuocChon);
            int maCTCu = veCu != null ? veCu.MaCT : 0;
            string maGheCuHienThi = veCu != null ? veCu.MaGheHienThi : "";
            decimal giaCu = giaVeCu;
            string tenGheMoi = maGheHienThiMoi;
            decimal giaMoi = giaVeMoiDuocChon;

            string tenPhuongThuc = ((ComboBoxItem)cboPhuongThuc.SelectedItem)?.Text ?? "Tiền mặt";
            string thongBaoTaiChinh = delta > 0
                ? string.Format("Chênh lệch: +{0:N0} VNĐ\n👉 Nhân viên xác nhận ĐÃ THU THÊM số tiền này ({1}) từ khách hàng?", delta, tenPhuongThuc)
                : delta < 0
                ? string.Format("Chênh lệch: {0:N0} VNĐ\n👉 Nhân viên xác nhận ĐÃ HOÀN LẠI {1:N0} VNĐ ({2}) cho khách hàng?", delta, Math.Abs(delta), tenPhuongThuc)
                : "Chênh lệch: 0 VNĐ (Đổi vé ngang giá — không phát sinh thu/hoàn tiền).\n👉 Xác nhận hoàn tất đổi vé?";

            string loiNhan = string.Format(
                "Xác nhận đổi vé #{0} ({1} - {2:N0} VNĐ)\nsang ghế mới: {3} ({4:N0} VNĐ)?\n\n{5}",
                maCTCu, maGheCuHienThi, giaCu,
                tenGheMoi, giaMoi,

                thongBaoTaiChinh);

            if (MessageBox.Show(this, loiNhan, "Xác nhận đổi vé", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            {
                return;
            }

            dangDoiVe = true;
            string originalText = btnXacNhan.Text;
            try
            {
                UseWaitCursor = true;
                btnXacNhan.Enabled = false;
                btnXacNhan.Text = "Đang đổi vé...";
                btnDong.Enabled = false;
                dgvGheTrong.Enabled = false;
                cboSuatMoi.Enabled = false;
                cboKhuVucMoi.Enabled = false;
                cboPhuongThuc.Enabled = false;

                int maTK = PhienDangNhap.DaDangNhap ? PhienDangNhap.TaiKhoan.MaTK : 0;
                DonVeResult result = await Task.Run(() => DonVeBLL.DoiVe(maDonCu, maCTCu, maGheMoi, delta, maTK, phuongThuc));
                if (result.Success)
                {
                    MaDonMoiDaTao = result.MaDonMoi ?? 0;
                    MessageBox.Show(this, result.Message, "Đổi vé thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dangDoiVe = false;
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
                else
                {
                    MessageBox.Show(this, result.Message, "Không thể đổi vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadGheTrongTheoSuat();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Đã xảy ra lỗi trong quá trình thực hiện đổi vé: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dangDoiVe = false;
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                    btnXacNhan.Text = originalText;
                    btnXacNhan.Enabled = maGheMoiDuocChon.HasValue;
                    btnDong.Enabled = true;
                    dgvGheTrong.Enabled = true;
                    cboSuatMoi.Enabled = true;
                    cboKhuVucMoi.Enabled = true;
                    cboPhuongThuc.Enabled = !maGheMoiDuocChon.HasValue || (veCu != null && DonVeBLL.TinhChenhLech(veCu.GiaVe, giaVeMoiDuocChon) != 0m);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (dangDoiVe)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private class ComboBoxItem
        {
            public string Text { get; }
            public object Value { get; }

            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}

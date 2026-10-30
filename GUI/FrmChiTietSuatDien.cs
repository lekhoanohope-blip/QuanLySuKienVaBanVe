using System;
using System.Data;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietSuatDien : Form
    {
        private int? _maSuat = null;

        public FrmChiTietSuatDien(int? maSuat = null)
        {
            InitializeComponent();
            _maSuat = maSuat;
            this.Text = _maSuat.HasValue ? "Sửa Suất diễn" : "Thêm Suất diễn";
        }

        private void FrmChiTietSuatDien_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;

            cboLoaiSoDo.Items.Clear();
            cboLoaiSoDo.Items.AddRange(new object[] { "Sơ đồ 1 (Chữ T)", "Sơ đồ 2 (Hình chữ nhật)", "Sơ đồ 3 (Lưới)" });
            cboLoaiSoDo.SelectedIndex = 0;

            // Load danh sách sự kiện
            try
            {
                DataTable dt = SuKienBLL.LayDanhSach();
                cboSuKien.DataSource = dt;
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message);
            }

            if (_maSuat.HasValue)
            {
                SuatDienDTO sd = SuatDienBLL.LayTheoMa(_maSuat.Value);
                if (sd != null)
                {
                    cboSuKien.SelectedValue = sd.MaSK;
                    dtpBatDau.Value = sd.ThoiGianBatDau;
                    if (sd.ThoiGianKetThuc.HasValue)
                        dtpKetThuc.Value = sd.ThoiGianKetThuc.Value;
                    txtDiaDiem.Text = sd.DiaDiem;
                    cboTrangThai.Text = sd.TrangThai;

                    if (sd.LoaiSoDo >= 1 && sd.LoaiSoDo <= 3)
                        cboLoaiSoDo.SelectedIndex = sd.LoaiSoDo - 1;
                    else
                        cboLoaiSoDo.SelectedIndex = 0; 
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue == null) { MessageBox.Show("Vui lòng chọn sự kiện!"); return; }
            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text)) { MessageBox.Show("Vui lòng nhập địa điểm!"); return; }

            SuatDienDTO sd = new SuatDienDTO
            {
                MaSK = Convert.ToInt32(cboSuKien.SelectedValue),
                ThoiGianBatDau = dtpBatDau.Value,
                ThoiGianKetThuc = dtpKetThuc.Value,
                DiaDiem = txtDiaDiem.Text.Trim(),
                TrangThai = cboTrangThai.Text,
                LoaiSoDo = cboLoaiSoDo.SelectedIndex >= 0 ? cboLoaiSoDo.SelectedIndex + 1 : 1
            };

            string kq;
            if (_maSuat.HasValue)
            {
                sd.MaSuat = _maSuat.Value;
                kq = SuatDienBLL.Sua(sd);
            }
            else
            {
                kq = SuatDienBLL.Them(sd);
            }

            if (kq == "Success")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(kq, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e) { this.Close(); }
    }
}
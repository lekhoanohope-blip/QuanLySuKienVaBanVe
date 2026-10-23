using System;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLySuKien : Form
    {
        public FrmQuanLySuKien() { InitializeComponent(); }

        private void FrmQuanLySuKien_Load(object sender, EventArgs e)
        {
            LoadData();
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadData()
        {
            try
            {
                dgvSuKien.DataSource = SuKienBLL.LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSK.Text)) { MessageBox.Show("Vui lòng nhập tên sự kiện!"); txtTenSK.Focus(); return; }

            SuKienDTO sk = new SuKienDTO
            {
                TenSK = txtTenSK.Text.Trim(),
                LoaiSK = txtLoaiSK.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.Text
            };

            string ketQua = SuKienBLL.Them(sk);
            if (ketQua == "Success") { MessageBox.Show("Thêm thành công!"); LoadData(); ClearInput(); }
            else MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn sự kiện cần sửa!"); return; }

            SuKienDTO sk = new SuKienDTO
            {
                MaSK = Convert.ToInt32(dgvSuKien.CurrentRow.Cells["MaSK"].Value),
                TenSK = txtTenSK.Text.Trim(),
                LoaiSK = txtLoaiSK.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.Text
            };

            string ketQua = SuKienBLL.Sua(sk);
            if (ketQua == "Success") { MessageBox.Show("Cập nhật thành công!"); LoadData(); ClearInput(); }
            else MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn sự kiện cần xóa!"); return; }

            int maSK = Convert.ToInt32(dgvSuKien.CurrentRow.Cells["MaSK"].Value);
            if (MessageBox.Show("Bạn có chắc muốn xóa sự kiện này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string ketQua = SuKienBLL.Xoa(maSK);
                if (ketQua == "Success") { MessageBox.Show("Xóa thành công!"); LoadData(); ClearInput(); }
                else MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e) { ClearInput(); LoadData(); }

        private void dgvSuKien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSuKien.Rows[e.RowIndex];
                txtTenSK.Text = row.Cells["TenSK"].Value?.ToString();
                txtLoaiSK.Text = row.Cells["LoaiSK"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                cboTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();
            }
        }

        private void ClearInput()
        {
            txtTenSK.Text = ""; txtLoaiSK.Text = ""; txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = 0; txtTenSK.Focus();
        }

        private void btnChuyenTrang_Click(object sender, EventArgs e)
        {
            this.Hide();
            
            bool found = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FrmQuanLySuatDien)
                {
                    f.Show();
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                FrmQuanLySuatDien frm = new FrmQuanLySuatDien();
                frm.Show();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit(); // Tắt sập toàn bộ chương trình
            }
        }
    }
}
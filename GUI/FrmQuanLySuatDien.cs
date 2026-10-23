using System;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLySuatDien : Form
    {
        public FrmQuanLySuatDien() { InitializeComponent(); }

        private void FrmQuanLySuatDien_Load(object sender, EventArgs e)
        {
            LoadComboBoxSuKien();
            LoadData();
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadComboBoxSuKien()
        {
            try
            {
                cboSuKien.DataSource = SuKienBLL.LayDanhSach();
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sự kiện: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                dgvSuatDien.DataSource = SuatDienBLL.LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue == null) { MessageBox.Show("Vui lòng chọn sự kiện!"); return; }
            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text)) { MessageBox.Show("Vui lòng nhập địa điểm!"); txtDiaDiem.Focus(); return; }

            SuatDienDTO sd = new SuatDienDTO
            {
                MaSK = (int)cboSuKien.SelectedValue,
                ThoiGianBatDau = dtpBatDau.Value,
                ThoiGianKetThuc = dtpKetThuc.Value,
                DiaDiem = txtDiaDiem.Text.Trim(),
                TrangThai = cboTrangThai.Text
            };

            string ketQua = SuatDienBLL.Them(sd);
            if (ketQua == "Success") { MessageBox.Show("Thêm suất diễn thành công!"); LoadData(); ClearInput(); }
            else MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuatDien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn suất diễn!"); return; }

            int maSuat = Convert.ToInt32(dgvSuatDien.CurrentRow.Cells["MaSuat"].Value);
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string ketQua = SuatDienBLL.Xoa(maSuat);
                if (ketQua == "Success") { MessageBox.Show("Xóa thành công!"); LoadData(); ClearInput(); }
                else MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e) { ClearInput(); LoadData(); }

        private void ClearInput()
        {
            if (cboSuKien.Items.Count > 0) cboSuKien.SelectedIndex = 0;
            txtDiaDiem.Text = "";
            dtpBatDau.Value = DateTime.Now;
            dtpKetThuc.Value = DateTime.Now.AddHours(2);
            cboTrangThai.SelectedIndex = 0;
        }

        private void btnChuyenTrang_Click(object sender, EventArgs e)
        {
            this.Hide();
            bool found = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FrmQuanLySuKien)
                {
                    f.Show();
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                FrmQuanLySuKien frm = new FrmQuanLySuKien();
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
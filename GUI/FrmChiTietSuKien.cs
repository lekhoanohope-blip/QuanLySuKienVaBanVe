using System;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietSuKien : Form
    {
        private int? _maSK = null; 

        public FrmChiTietSuKien(int? maSK = null)
        {
            InitializeComponent();
            _maSK = maSK;
            this.Text = _maSK.HasValue ? "Sửa Sự kiện" : "Thêm Sự kiện";
        }

        private void FrmChiTietSuKien_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
            if (_maSK.HasValue)
            {
                SuKienDTO sk = SuKienBLL.LayTheoMa(_maSK.Value);
                if (sk != null)
                {
                    txtTen.Text = sk.TenSK;
                    txtLoai.Text = sk.LoaiSK;
                    txtMoTa.Text = sk.MoTa;
                    cboTrangThai.Text = sk.TrangThai;
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Vui lòng nhập tên sự kiện!"); return; }

            SuKienDTO sk = new SuKienDTO
            {
                TenSK = txtTen.Text.Trim(),
                LoaiSK = txtLoai.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.Text
            };

            string kq;
            if (_maSK.HasValue)
            {
                sk.MaSK = _maSK.Value;
                kq = SuKienBLL.Sua(sk);
            }
            else
            {
                kq = SuKienBLL.Them(sk);
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
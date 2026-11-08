using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietSuKien : Form
    {
        private int? _maSK = null;

        private sealed class TrangThaiOption
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public FrmChiTietSuKien(int? maSK = null)
        {
            InitializeComponent();
            HieuUng.MoDan(this);
            _maSK = maSK;
            this.Text = _maSK.HasValue ? "Sửa Sự kiện" : "Thêm Sự kiện";
        }

        private void FrmChiTietSuKien_Load(object sender, EventArgs e)
        {
            KhoiTaoTrangThai();

            if (_maSK.HasValue)
            {
                SuKienDTO sk = SuKienBLL.LayTheoMa(_maSK.Value);
                if (sk != null)
                {
                    txtTen.Text = sk.TenSK;
                    txtLoai.Text = sk.LoaiSK;
                    txtMoTa.Text = sk.MoTa;
                    ChonTrangThai(sk.TrangThai);
                }
            }
        }

        private void KhoiTaoTrangThai()
        {
            var dsTrangThai = new List<TrangThaiOption>
            {
                new TrangThaiOption { Value = "SapDien", Text = "Sắp diễn ra" },
                new TrangThaiOption { Value = "DangDien", Text = "Đang diễn ra" },
                new TrangThaiOption { Value = "DaKetThuc", Text = "Đã kết thúc" },
                new TrangThaiOption { Value = "DaHuy", Text = "Đã hủy" }
            };

            cboTrangThai.DisplayMember = "Text";
            cboTrangThai.ValueMember = "Value";
            cboTrangThai.DataSource = dsTrangThai;
            cboTrangThai.SelectedIndex = 0;
        }

        private void ChonTrangThai(string maTrangThai)
        {
            if (string.IsNullOrWhiteSpace(maTrangThai))
            {
                cboTrangThai.SelectedIndex = 0;
                return;
            }

            cboTrangThai.SelectedValue = maTrangThai.Trim();
            if (cboTrangThai.SelectedIndex < 0)
            {
                cboTrangThai.SelectedIndex = 0;
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
                TrangThai = cboTrangThai.SelectedValue?.ToString() ?? "SapDien"
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

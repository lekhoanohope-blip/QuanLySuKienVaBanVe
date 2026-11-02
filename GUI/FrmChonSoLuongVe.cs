using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChonSoLuongVe : Form
    {
        private readonly string _tenKV;
        private readonly decimal _donGia;
        private readonly int _conLai;
        private readonly int _maxQty;
        private int _soLuong = 1;

        public int SelectedQuantity => _soLuong;
        public decimal TongTien => _soLuong * _donGia;

        public FrmChonSoLuongVe(string tenKV, decimal donGia, int conLai, int maxChoPhep = 10)
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            _tenKV = string.IsNullOrWhiteSpace(tenKV) ? "Khu vực" : tenKV.Trim();
            _donGia = donGia;
            _conLai = Math.Max(0, conLai);
            _maxQty = Math.Max(1, Math.Min(_conLai > 0 ? _conLai : 1, maxChoPhep));

            this.Load += FrmChonSoLuongVe_Load;
        }

        private void FrmChonSoLuongVe_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Khu {_tenKV}";
            lblTenKV.Text = _tenKV;
            lblConLai.Text = $"Còn trống: {_conLai} vé";

            _soLuong = 1;
            UpdateUi();
        }

        private void UpdateUi()
        {
            lblSoLuong.Text = _soLuong.ToString();
            decimal total = _soLuong * _donGia;
            btnTiepTuc.Text = $"Tiếp tục - {total:N0} đ ≫";

            btnGiam.Enabled = _soLuong > 1;
            btnTang.Enabled = _soLuong < _maxQty;
        }

        private void btnGiam_Click(object sender, EventArgs e)
        {
            if (_soLuong > 1)
            {
                _soLuong--;
                UpdateUi();
            }
        }

        private void btnTang_Click(object sender, EventArgs e)
        {
            if (_soLuong < _maxQty)
            {
                _soLuong++;
                UpdateUi();
            }
        }

        private void lblChonKhuVucKhac_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnTiepTuc_Click(object sender, EventArgs e)
        {
            if (_conLai <= 0)
            {
                MessageBox.Show("Khu vực này hiện đã hết vé trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietKhachHang : Form
    {
        private readonly KhachHangDTO original;
        private bool saving;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MaKHDaLuu { get; private set; }

        public FrmChiTietKhachHang() : this(null) { }

        public FrmChiTietKhachHang(KhachHangDTO customer)
        {
            InitializeComponent();
            original = customer ?? new KhachHangDTO();
            bool editing = original.MaKH > 0;
            Text = editing ? "Sửa khách hàng" : "Thêm khách hàng";
            lblTitle.Text = Text;
            txtMaKH.Text = editing ? original.MaKH.ToString() : "Tự động tạo khi lưu";
            txtTenKH.Text = original.TenKH;
            txtSDT.Text = original.SDT;
            txtEmail.Text = original.Email;
            btnLuu.Text = editing ? "Lưu thay đổi" : "Thêm khách hàng";
            Shown += (sender, args) => txtTenKH.Focus();
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            if (saving) return;
            errorProvider.Clear();
            lblError.Text = string.Empty;
            var customer = new KhachHangDTO(original.MaKH, txtTenKH.Text.Trim(), txtSDT.Text.Trim(), txtEmail.Text.Trim());
            SetSaving(true);
            try
            {
                KhachHangResult result = await Task.Run(() => customer.MaKH > 0
                    ? KhachHangBLL.Sua(customer) : KhachHangBLL.Them(customer));
                if (!result.Success)
                {
                    SetSaving(false);
                    lblError.Text = result.Message;
                    Guna2TextBox field = result.FieldName == "SDT" ? txtSDT
                        : result.FieldName == "Email" ? txtEmail : txtTenKH;
                    if (!string.IsNullOrEmpty(result.FieldName))
                    {
                        errorProvider.SetError(field, result.Message);
                        field.Focus();
                    }
                    return;
                }
                MaKHDaLuu = result.MaKH;
                saving = false;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                lblError.Text = "Không thể lưu khách hàng. Kiểm tra kết nối cơ sở dữ liệu rồi thử lại.";
            }
            finally
            {
                if (!IsDisposed) SetSaving(false);
            }
        }

        private void SetSaving(bool value)
        {
            saving = value;
            txtTenKH.ReadOnly = value;
            txtSDT.ReadOnly = value;
            txtEmail.ReadOnly = value;
            btnLuu.Enabled = !value;
            btnHuy.Enabled = !value;
            btnLuu.Text = value ? "Đang lưu…" : original.MaKH > 0 ? "Lưu thay đổi" : "Thêm khách hàng";
            UseWaitCursor = value;
        }

        private void btnHuy_Click(object sender, EventArgs e) { Close(); }

        private void FrmChiTietKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) btnLuu.PerformClick();
            else if (e.KeyCode == Keys.Escape && !saving) Close();
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (saving) e.Cancel = true;
            else if (DialogResult != DialogResult.OK &&
                (txtTenKH.Text != original.TenKH || txtSDT.Text != original.SDT || txtEmail.Text != original.Email))
            {
                if (MessageBox.Show(this, "Bạn có thay đổi chưa lưu. Bỏ các thay đổi này?", "Hủy chỉnh sửa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}

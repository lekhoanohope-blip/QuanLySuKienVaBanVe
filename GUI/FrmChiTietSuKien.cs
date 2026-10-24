using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietSuKien : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True";
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT TenSK, LoaiSK, MoTa, TrangThai FROM SuKien WHERE MaSK = @MaSK";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSK", _maSK.Value);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtTen.Text = reader["TenSK"].ToString();
                        txtLoai.Text = reader["LoaiSK"].ToString();
                        txtMoTa.Text = reader["MoTa"].ToString();
                        cboTrangThai.Text = reader["TrangThai"].ToString();
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Vui lòng nhập tên sự kiện!"); return; }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd;
                    if (_maSK.HasValue)
                    {
                        cmd = new SqlCommand("UPDATE SuKien SET TenSK=@Ten, LoaiSK=@Loai, MoTa=@MoTa, TrangThai=@TT WHERE MaSK=@MaSK", conn);
                        cmd.Parameters.AddWithValue("@MaSK", _maSK.Value);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO SuKien (TenSK, LoaiSK, MoTa, TrangThai) VALUES (@Ten, @Loai, @MoTa, @TT)", conn);
                    }
                    cmd.Parameters.AddWithValue("@Ten", txtTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@Loai", txtLoai.Text.Trim());
                    cmd.Parameters.AddWithValue("@MoTa", txtMoTa.Text.Trim());
                    cmd.Parameters.AddWithValue("@TT", cboTrangThai.Text);

                    cmd.ExecuteNonQuery();
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnHuy_Click(object sender, EventArgs e) { this.Close(); }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietSuatDien : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True";
        private int? _maSuat = null;

        public FrmChiTietSuatDien(int? maSuat = null)
        {
            InitializeComponent();
            _maSuat = maSuat;
            if (_maSuat.HasValue) this.Text = "Sửa Suất diễn"; else this.Text = "Thêm Suất diễn";
        }

        private void FrmChiTietSuatDien_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;

            // Load danh sách sự kiện
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaSK, TenSK FROM SuKien", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboSuKien.DataSource = dt;
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
            }

            if (_maSuat.HasValue)
            {
                // Load dữ liệu cũ
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai FROM SuatDien WHERE MaSuat = @MaSuat";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSuat", _maSuat.Value);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cboSuKien.SelectedValue = reader["MaSK"];
                        dtpBatDau.Value = Convert.ToDateTime(reader["ThoiGianBatDau"]);
                        dtpKetThuc.Value = Convert.ToDateTime(reader["ThoiGianKetThuc"]);
                        txtDiaDiem.Text = reader["DiaDiem"].ToString();
                        cboTrangThai.Text = reader["TrangThai"].ToString();
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue == null) { MessageBox.Show("Vui lòng chọn sự kiện!"); return; }
            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text)) { MessageBox.Show("Vui lòng nhập địa điểm!"); return; }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd;
                    if (_maSuat.HasValue)
                    {
                        cmd = new SqlCommand("UPDATE SuatDien SET MaSK=@MaSK, ThoiGianBatDau=@BD, ThoiGianKetThuc=@KT, DiaDiem=@DD, TrangThai=@TT WHERE MaSuat=@MaSuat", conn);
                        cmd.Parameters.AddWithValue("@MaSK", Convert.ToInt32(cboSuKien.SelectedValue));
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai) VALUES (@MaSK, @BD, @KT, @DD, @TT)", conn);
                    }
                    cmd.Parameters.AddWithValue("@MaSK", (int)cboSuKien.SelectedValue);
                    cmd.Parameters.AddWithValue("@BD", dtpBatDau.Value);
                    cmd.Parameters.AddWithValue("@KT", dtpKetThuc.Value);
                    cmd.Parameters.AddWithValue("@DD", txtDiaDiem.Text.Trim());
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
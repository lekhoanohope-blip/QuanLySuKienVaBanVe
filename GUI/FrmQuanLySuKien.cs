using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLySuKien : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True";

        public FrmQuanLySuKien() { InitializeComponent(); this.DoubleBuffered = true; }

        private void FrmQuanLySuKien_Load(object sender, EventArgs e) { LoadData(); }

        private void LoadData()
        {
            try
            {
                string query = "SELECT MaSK, TenSK, LoaiSK, MoTa, TrangThai FROM SuKien";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvSuKien.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int idx = dgvSuKien.Rows.Add();
                        dgvSuKien.Rows[idx].Cells["colMaSK"].Value = row["MaSK"];
                        dgvSuKien.Rows[idx].Cells["colTenSK"].Value = row["TenSK"];
                        dgvSuKien.Rows[idx].Cells["colLoaiSK"].Value = row["LoaiSK"];
                        dgvSuKien.Rows[idx].Cells["colMoTa"].Value = row["MoTa"];
                        dgvSuKien.Rows[idx].Cells["colTrangThai"].Value = row["TrangThai"];
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi SQL: " + ex.Message); }
        }

        // =================== THÊM / SỬA / XÓA ===================
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (FrmChiTietSuKien frm = new FrmChiTietSuKien())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn sự kiện trên bảng!"); return; }
            int maSK = Convert.ToInt32(dgvSuKien.CurrentRow.Cells["colMaSK"].Value);
            using (FrmChiTietSuKien frm = new FrmChiTietSuKien(maSK))
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn sự kiện trên bảng!"); return; }
            int maSK = Convert.ToInt32(dgvSuKien.CurrentRow.Cells["colMaSK"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa sự kiện này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("DELETE FROM SuKien WHERE MaSK = @MaSK", conn);
                        cmd.Parameters.AddWithValue("@MaSK", maSK);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa (Sự kiện này đang có Suất diễn liên kết):\n" + ex.Message, "Lỗi");
                }
            }
        }

        // =================== XỬ LÝ CLICK ICON TRÊN BẢNG ===================
        private void dgvSuKien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvSuKien.Columns[e.ColumnIndex].Name == "colThaoTac")
            {
                // Đảm bảo dòng đó được chọn
                dgvSuKien.CurrentCell = dgvSuKien.Rows[e.RowIndex].Cells[e.ColumnIndex];

                Point p = dgvSuKien.PointToClient(Cursor.Position);
                Rectangle rect = dgvSuKien.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int x = p.X - rect.X;

                // Xác định click vào nút Sửa (bên trái) hay Xóa (bên phải)
                if (x >= 5 && x <= 40)
                {
                    btnSua_Click(null, null);
                }
                else if (x > 40 && x <= 80)
                {
                    btnXoa_Click(null, null);
                }
            }
        }

        // =================== VẼ GIAO DIỆN ===================
        private void dgvSuKien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvSuKien.Columns[e.ColumnIndex].Name == "colTrangThai")
            {
                e.PaintBackground(e.CellBounds, true);
                string status = e.Value?.ToString() ?? "";
                Color badgeColor = Color.Gray;
                Color textColor = Color.White;
                string displayStatus = status;

                if (status == "DangDien") { badgeColor = Color.FromArgb(209, 250, 229); textColor = Color.FromArgb(6, 95, 70); displayStatus = "Đang diễn ra"; }
                else if (status == "SapDien") { badgeColor = Color.FromArgb(219, 234, 254); textColor = Color.FromArgb(30, 64, 175); displayStatus = "Sắp diễn ra"; }
                else if (status == "DaKetThuc") { badgeColor = Color.FromArgb(254, 226, 226); textColor = Color.FromArgb(153, 27, 27); displayStatus = "Đã kết thúc"; }
                else if (status == "DaHuy") { badgeColor = Color.FromArgb(254, 226, 226); textColor = Color.FromArgb(153, 27, 27); displayStatus = "Đã hủy"; }

                Rectangle rect = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + 10, e.CellBounds.Width - 10, e.CellBounds.Height - 20);
                using (GraphicsPath path = GetRoundedRect(rect, 15))
                using (SolidBrush brush = new SolidBrush(badgeColor))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
                TextRenderer.DrawText(e.Graphics, displayStatus, e.CellStyle.Font, rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }

            if (dgvSuKien.Columns[e.ColumnIndex].Name == "colThaoTac")
            {
                e.PaintBackground(e.CellBounds, true);
                // Vẽ nút Sửa
                Rectangle btnSua = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + 10, 30, 30);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(219, 234, 254))) e.Graphics.FillRectangle(brush, btnSua);
                TextRenderer.DrawText(e.Graphics, "✏️", e.CellStyle.Font, btnSua, Color.FromArgb(30, 64, 175), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // Vẽ nút Xóa
                Rectangle btnXoa = new Rectangle(e.CellBounds.X + 45, e.CellBounds.Y + 10, 30, 30);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(254, 226, 226))) e.Graphics.FillRectangle(brush, btnXoa);
                TextRenderer.DrawText(e.Graphics, "🗑️", e.CellStyle.Font, btnXoa, Color.FromArgb(153, 27, 27), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
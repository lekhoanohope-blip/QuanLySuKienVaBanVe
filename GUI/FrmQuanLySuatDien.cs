using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLySuatDien : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? MaSKCanLoc { get; set; }

        public FrmQuanLySuatDien() { InitializeComponent(); this.DoubleBuffered = true; }

        private void FrmQuanLySuatDien_Load(object sender, EventArgs e) { LoadData(); }

        private void LoadData()
        {
            try
            {
                string query = @"
                    SELECT sd.MaSuat, sk.TenSK, sd.ThoiGianBatDau, sd.DiaDiem, sd.TrangThai 
                    FROM SuatDien sd
                    JOIN SuKien sk ON sd.MaSK = sk.MaSK
                    ORDER BY sd.ThoiGianBatDau DESC";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvSuatDien.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int idx = dgvSuatDien.Rows.Add();
                        dgvSuatDien.Rows[idx].Cells["colMaSuat"].Value = row["MaSuat"];
                        dgvSuatDien.Rows[idx].Cells["colTenSK"].Value = row["TenSK"];
                        dgvSuatDien.Rows[idx].Cells["colThoiGian"].Value = Convert.ToDateTime(row["ThoiGianBatDau"]).ToString("dd/MM/yyyy HH:mm");
                        dgvSuatDien.Rows[idx].Cells["colDiaDiem"].Value = row["DiaDiem"];
                        dgvSuatDien.Rows[idx].Cells["colTrangThai"].Value = row["TrangThai"];
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi SQL: " + ex.Message); }
        }

        // =================== THÊM / SỬA / XÓA ===================
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (FrmChiTietSuatDien frm = new FrmChiTietSuatDien())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSuatDien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn suất diễn trên bảng!"); return; }
            int maSuat = Convert.ToInt32(dgvSuatDien.CurrentRow.Cells["colMaSuat"].Value);
            using (FrmChiTietSuatDien frm = new FrmChiTietSuatDien(maSuat))
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuatDien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn suất diễn trên bảng!"); return; }
            int maSuat = Convert.ToInt32(dgvSuatDien.CurrentRow.Cells["colMaSuat"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa suất diễn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("DELETE FROM SuatDien WHERE MaSuat = @MaSuat", conn);
                        cmd.Parameters.AddWithValue("@MaSuat", maSuat);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa (Suất diễn này đã có Khu vực/Vé liên kết):\n" + ex.Message, "Lỗi");
                }
            }
        }

        // =================== XỬ LÝ CLICK ICON TRÊN BẢNG ===================
        private void dgvSuatDien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvSuatDien.Columns[e.ColumnIndex].Name == "colThaoTac")
            {
                dgvSuatDien.CurrentCell = dgvSuatDien.Rows[e.RowIndex].Cells[e.ColumnIndex];

                Point p = dgvSuatDien.PointToClient(Cursor.Position);
                Rectangle rect = dgvSuatDien.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int x = p.X - rect.X;

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
        private void dgvSuatDien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvSuatDien.Columns[e.ColumnIndex].Name == "colTrangThai")
            {
                e.PaintBackground(e.CellBounds, true);
                string status = e.Value?.ToString() ?? "";
                Color badgeColor = Color.Gray;
                Color textColor = Color.White;
                string displayStatus = status;

                if (status == "MoCuaBanVe") { badgeColor = Color.FromArgb(209, 250, 229); textColor = Color.FromArgb(6, 95, 70); displayStatus = "Mở bán vé"; }
                else if (status == "DongBanVe") { badgeColor = Color.FromArgb(254, 243, 199); textColor = Color.FromArgb(146, 64, 14); displayStatus = "Đóng bán vé"; }
                else if (status == "DangDien") { badgeColor = Color.FromArgb(219, 234, 254); textColor = Color.FromArgb(30, 64, 175); displayStatus = "Đang diễn ra"; }
                else if (status == "DaKetThuc") { badgeColor = Color.FromArgb(254, 226, 226); textColor = Color.FromArgb(153, 27, 27); displayStatus = "Đã kết thúc"; }

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

            if (dgvSuatDien.Columns[e.ColumnIndex].Name == "colThaoTac")
            {
                e.PaintBackground(e.CellBounds, true);
                Rectangle btnSua = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + 10, 30, 30);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(219, 234, 254))) e.Graphics.FillRectangle(brush, btnSua);
                TextRenderer.DrawText(e.Graphics, "✏️", e.CellStyle.Font, btnSua, Color.FromArgb(30, 64, 175), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

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
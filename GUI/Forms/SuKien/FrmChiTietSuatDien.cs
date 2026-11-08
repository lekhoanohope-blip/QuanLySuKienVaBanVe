using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmChiTietSuatDien : Form
    {
        private int? _maSuat = null;
        private bool _dangDongBoThoiGian;
        private readonly ToolTip _toolTip = new ToolTip();

        private sealed class TrangThaiOption
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public FrmChiTietSuatDien(int? maSuat = null)
        {
            InitializeComponent();
            HieuUng.MoDan(this);
            _maSuat = maSuat;
            this.Text = _maSuat.HasValue ? "Sửa Suất diễn" : "Thêm Suất diễn";

            dtpBatDau.ValueChanged += (s, e) => DongBoThoiGian();
            dtpKetThuc.ValueChanged += (s, e) => DongBoThoiGian();
            cboGioBatDau.SelectedIndexChanged += (s, e) => DongBoThoiGian();
            cboGioKetThuc.SelectedIndexChanged += (s, e) => DongBoThoiGian();
            cboSuKien.SelectedIndexChanged += (s, e) => CapNhatToolTipSuKien();
        }

        private void FrmChiTietSuatDien_Load(object sender, EventArgs e)
        {
            KhoiTaoTrangThai();
            KhoiTaoDanhSachGio(cboGioBatDau);
            KhoiTaoDanhSachGio(cboGioKetThuc);

            cboLoaiSoDo.Items.Clear();
            cboLoaiSoDo.Items.AddRange(new object[] { "Sơ đồ 1 (Chữ T)", "Sơ đồ 2 (Hình chữ nhật)", "Sơ đồ 3 (Lưới)" });
            cboLoaiSoDo.SelectedIndex = 0;

            try
            {
                DataTable dt = SuKienBLL.LayDanhSach();
                cboSuKien.DisplayMember = "TenSK";
                cboSuKien.ValueMember = "MaSK";
                cboSuKien.DataSource = dt;
                CapNhatDoRongDropDownSuKien(dt);
                CapNhatToolTipSuKien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message);
            }

            _dangDongBoThoiGian = true;
            try
            {
                dtpKetThuc.MinDate = new DateTime(1753, 1, 1);

                DateTime ngayMacDinh = DateTime.Today.AddDays(1);
                dtpBatDau.Value = ngayMacDinh;
                ChonGio(cboGioBatDau, new TimeSpan(19, 30, 0));
                dtpKetThuc.Value = ngayMacDinh;
                ChonGio(cboGioKetThuc, new TimeSpan(21, 30, 0));

                if (_maSuat.HasValue)
                {
                    SuatDienDTO sd = SuatDienBLL.LayTheoMa(_maSuat.Value);
                    if (sd != null)
                    {
                        cboSuKien.SelectedValue = sd.MaSK;
                        dtpBatDau.Value = sd.ThoiGianBatDau.Date;
                        ChonGio(cboGioBatDau, sd.ThoiGianBatDau.TimeOfDay);

                        DateTime thoiGianKetThuc = sd.ThoiGianKetThuc ?? sd.ThoiGianBatDau.AddHours(2);
                        dtpKetThuc.Value = thoiGianKetThuc.Date;
                        ChonGio(cboGioKetThuc, thoiGianKetThuc.TimeOfDay);

                        txtDiaDiem.Text = sd.DiaDiem;
                        ChonTrangThai(sd.TrangThai);

                        if (sd.LoaiSoDo >= 1 && sd.LoaiSoDo <= 3)
                            cboLoaiSoDo.SelectedIndex = sd.LoaiSoDo - 1;
                        else
                            cboLoaiSoDo.SelectedIndex = 0;
                    }
                }
            }
            finally
            {
                _dangDongBoThoiGian = false;
            }

            DongBoThoiGian();
        }

        private void KhoiTaoTrangThai()
        {
            var dsTrangThai = new List<TrangThaiOption>
            {
                new TrangThaiOption { Value = "MoCuaBanVe", Text = "Mở bán vé" },
                new TrangThaiOption { Value = "DongBanVe", Text = "Đóng bán vé" },
                new TrangThaiOption { Value = "DangDien", Text = "Đang diễn ra" },
                new TrangThaiOption { Value = "DaKetThuc", Text = "Đã kết thúc" }
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

        private static void KhoiTaoDanhSachGio(Guna2ComboBox cbo)
        {
            cbo.Items.Clear();
            for (int h = 0; h < 24; h++)
            {
                cbo.Items.Add(string.Format(CultureInfo.InvariantCulture, "{0:00}:00", h));
                cbo.Items.Add(string.Format(CultureInfo.InvariantCulture, "{0:00}:30", h));
            }
            cbo.SelectedIndex = 0;
        }

        private static void ChonGio(Guna2ComboBox cbo, TimeSpan gio)
        {
            string chuoiGio = string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", gio.Hours, gio.Minutes);
            int idx = cbo.Items.IndexOf(chuoiGio);
            if (idx < 0)
            {
                int insertIdx = cbo.Items.Count;
                for (int i = 0; i < cbo.Items.Count; i++)
                {
                    if (string.CompareOrdinal(cbo.Items[i].ToString(), chuoiGio) > 0)
                    {
                        insertIdx = i;
                        break;
                    }
                }
                cbo.Items.Insert(insertIdx, chuoiGio);
                idx = insertIdx;
            }
            cbo.SelectedIndex = idx;
        }

        private static TimeSpan ParseGio(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) &&
                TimeSpan.TryParseExact(text.Trim(), @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan ts))
            {
                return ts;
            }
            return TimeSpan.Zero;
        }

        private DateTime LayThoiGianBatDau() => dtpBatDau.Value.Date.Add(ParseGio(cboGioBatDau.Text));

        private DateTime LayThoiGianKetThuc() => dtpKetThuc.Value.Date.Add(ParseGio(cboGioKetThuc.Text));

        /// <summary>Ràng buộc ngày kết thúc không nhỏ hơn ngày bắt đầu và tự động đẩy giờ kết thúc nếu bị trùng/ngược.</summary>
        private void DongBoThoiGian()
        {
            if (_dangDongBoThoiGian || cboGioBatDau.Items.Count == 0 || cboGioKetThuc.Items.Count == 0)
                return;

            _dangDongBoThoiGian = true;
            try
            {
                DateTime ngayBatDau = dtpBatDau.Value.Date;
                if (dtpKetThuc.Value.Date < ngayBatDau)
                {
                    dtpKetThuc.Value = ngayBatDau;
                }
                dtpKetThuc.MinDate = ngayBatDau;

                DateTime batDau = LayThoiGianBatDau();
                DateTime ketThuc = LayThoiGianKetThuc();
                if (ketThuc <= batDau)
                {
                    DateTime ketThucMoi = batDau.AddHours(2);
                    dtpKetThuc.Value = ketThucMoi.Date;
                    ChonGio(cboGioKetThuc, ketThucMoi.TimeOfDay);
                }
            }
            finally
            {
                _dangDongBoThoiGian = false;
            }
        }

        private void CapNhatDoRongDropDownSuKien(DataTable dt)
        {
            int maxWidth = cboSuKien.Width;
            if (dt != null && dt.Columns.Contains("TenSK"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string ten = Convert.ToString(row["TenSK"]) ?? string.Empty;
                    int w = TextRenderer.MeasureText(ten, cboSuKien.Font).Width + 44;
                    if (w > maxWidth) maxWidth = w;
                }
            }
            cboSuKien.DropDownWidth = Math.Min(maxWidth, 760);
        }

        private void CapNhatToolTipSuKien()
        {
            _toolTip.SetToolTip(cboSuKien, cboSuKien.Text ?? string.Empty);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboSuKien.SelectedValue == null) { MessageBox.Show("Vui lòng chọn sự kiện!"); return; }
            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text)) { MessageBox.Show("Vui lòng nhập địa điểm!"); return; }

            DateTime batDau = LayThoiGianBatDau();
            DateTime ketThuc = LayThoiGianKetThuc();
            if (ketThuc <= batDau)
            {
                MessageBox.Show("Thời gian kết thúc phải lớn hơn thời gian bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SuatDienDTO sd = new SuatDienDTO
            {
                MaSK = Convert.ToInt32(cboSuKien.SelectedValue),
                ThoiGianBatDau = batDau,
                ThoiGianKetThuc = ketThuc,
                DiaDiem = txtDiaDiem.Text.Trim(),
                TrangThai = cboTrangThai.SelectedValue?.ToString() ?? "MoCuaBanVe",
                LoaiSoDo = cboLoaiSoDo.SelectedIndex >= 0 ? cboLoaiSoDo.SelectedIndex + 1 : 1
            };

            string kq;
            if (_maSuat.HasValue)
            {
                sd.MaSuat = _maSuat.Value;
                kq = SuatDienBLL.Sua(sd);
            }
            else
            {
                kq = SuatDienBLL.Them(sd);
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

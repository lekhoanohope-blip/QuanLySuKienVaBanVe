using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmDoiVe : Form
    {
        private readonly DataRow donVeRow;
        private readonly ChiTietVeDTO veCu;
        private DataTable shows, seats;
        private bool bindingShows, bindingSeats, loadingShows, loadingSeats, dangDoiVe;
        private int showRequest, seatRequest;
        private DataRow selectedSeat;
        private Func<int,DataTable> fetchShows = DonVeBLL.LaySuatDienCungSuKien;
        private Func<int,DataTable> fetchSeats = DonVeBLL.LayDanhSachGheTrongTheoSuat;
        public int MaDonMoiDaTao { get; private set; }
        private static readonly CultureInfo Vietnamese = CultureInfo.GetCultureInfo("vi-VN");
        private static string Money(decimal value) { return value.ToString("N0",Vietnamese) + " đ"; }
        private string ShowTime { get { return cboSuatMoi.SelectedItem is ShowItem item ? item.Start.ToString("dd/MM/yyyy · HH:mm") : ""; } }

        public FrmDoiVe(DataRow donVeRow, ChiTietVeDTO veCu)
        {
            this.donVeRow = donVeRow; this.veCu = veCu;
            InitializeComponent(); DoubleBuffered = true;
        }

        private async void FrmDoiVe_Load(object sender, EventArgs e)
        {
            if (donVeRow == null || veCu == null) { Close(); return; }
            customer.Text = "Đơn #" + donVeRow["MaDon"] + " · " + donVeRow["TenKH"];
            DateTime start = Convert.ToDateTime(donVeRow["ThoiGianBatDau"]);
            eventName.Text = Convert.ToString(donVeRow["TenSK"]) + " · " + start.ToString("dd/MM/yyyy · HH:mm");
            oldTicket.Text = veCu.MaGheHienThi + " · " + veCu.TenKV + " · " + veCu.HangVe +
                "\n" + Money(veCu.GiaVe) +
                "\n\nĐổi trước " + start.AddHours(-12).ToString("dd/MM/yyyy · HH:mm");
            cboPhuongThuc.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ ngân hàng" });
            cboPhuongThuc.SelectedIndex = 0;
            Rectangle work = Screen.FromControl(Owner ?? this).WorkingArea;
            Size limit = new Size(Math.Max(320,work.Width-32),Math.Max(320,work.Height-32));
            MaximumSize = limit;
            MinimumSize = new Size(Math.Min(Px(620),limit.Width),Math.Min(Px(520),limit.Height));
            Size = new Size(Math.Min(Width,limit.Width),Math.Min(Height,limit.Height));
            Location = new Point(work.Left+(work.Width-Width)/2,work.Top+(work.Height-Height)/2);
            UpdateSelection(); LayoutExchange();
            await LoadShowsAsync();
        }

        private async Task LoadShowsAsync()
        {
            int version = ++showRequest; ++seatRequest;
            loadingShows = true; loadingSeats = false; shows = null; seats = null;
            bindingShows = true; cboSuatMoi.Items.Clear(); bindingShows = false;
            ResetSeats("Đang tải suất diễn…"); RefreshEnabled();
            try {
                DataTable data = await Task.Run(() => fetchShows(Convert.ToInt32(donVeRow["MaSK"])));
                if (IsDisposed || version != showRequest) return;
                shows = data;
                bindingShows = true;
                foreach(DataRow row in data.Rows) cboSuatMoi.Items.Add(new ShowItem(row));
                int current = Convert.ToInt32(donVeRow["MaSuat"]);
                int index = 0;
                for(int i=0;i<cboSuatMoi.Items.Count;i++) if (((ShowItem)cboSuatMoi.Items[i]).Id == current) index=i;
                if(cboSuatMoi.Items.Count>0) cboSuatMoi.SelectedIndex=index;
                bindingShows = false;
                loadingShows = false;
                if(cboSuatMoi.Items.Count>0) await LoadSeatsAsync();
                else SetState("Không còn suất diễn mở bán cho sự kiện này.",false);
            } catch(Exception ex) {
                if(IsDisposed || version!=showRequest) return;
                System.Diagnostics.Trace.TraceError("Load exchange shows: {0}",ex);
                SetState("Không tải được suất diễn. Vui lòng thử lại.",true);
            } finally {
                if(!IsDisposed && version==showRequest) { bindingShows=false; loadingShows=false; RefreshEnabled(); }
            }
        }

        private async Task LoadSeatsAsync()
        {
            if (!(cboSuatMoi.SelectedItem is ShowItem item)) return;
            int version = ++seatRequest;
            loadingSeats=true; seats=null;
            location.Text=item.Location; LayoutExchange();
            ResetSeats("Đang tải ghế trống…"); RefreshEnabled();
            try {
                DataTable data=await Task.Run(() => fetchSeats(item.Id));
                if(IsDisposed || version!=seatRequest) return;
                seats=data;
                if(!seats.Columns.Contains("ZoneDisplay")) seats.Columns.Add("ZoneDisplay",typeof(string));
                foreach(DataRow row in seats.Rows) row["ZoneDisplay"]=row["TenKV"]+" · "+row["HangVe"];
                bindingSeats=true;
                cboKhuVucMoi.Items.Clear(); cboKhuVucMoi.Items.Add(new ZoneItem(0,"Tất cả khu vực"));
                foreach(DataRow row in seats.DefaultView.ToTable(true,"MaKV","TenKV").Rows)
                    cboKhuVucMoi.Items.Add(new ZoneItem(Convert.ToInt32(row["MaKV"]),Convert.ToString(row["TenKV"])));
                cboKhuVucMoi.SelectedIndex=0; txtTimGhe.Clear(); bindingSeats=false;
                loadingSeats=false; FilterSeats();
            } catch(Exception ex) {
                if(IsDisposed || version!=seatRequest) return;
                System.Diagnostics.Trace.TraceError("Load exchange seats: {0}",ex);
                SetState("Không tải được ghế. Vui lòng thử lại.",true);
            } finally {
                if(!IsDisposed && version==seatRequest) { bindingSeats=false; loadingSeats=false; RefreshEnabled(); }
            }
        }

        private void ResetSeats(string message)
        {
            bindingSeats=true; dgvGheTrong.DataSource=null; cboKhuVucMoi.Items.Clear(); txtTimGhe.Clear(); bindingSeats=false;
            selectedSeat=null; UpdateSelection(); count.Text=""; SetState(message,false);
        }
        private void SetState(string message,bool canRetry)
        {
            stateLabel.Text=message; stateLabel.Visible=true; retry.Visible=canRetry; dgvGheTrong.Visible=false;
            LayoutExchange();
        }
        private void FilterSeats()
        {
            if(seats==null || loadingSeats) return;
            int? previous=selectedSeat==null ? (int?)null : Convert.ToInt32(selectedSeat["MaGhe"]);
            int zone=(cboKhuVucMoi.SelectedItem as ZoneItem)?.Id ?? 0;
            string keyword=txtTimGhe.Text.Trim();
            DataTable filtered=seats.Clone();
            foreach(DataRow row in seats.Rows) {
                if(zone>0 && Convert.ToInt32(row["MaKV"])!=zone) continue;
                if(keyword.Length>0 && !new[] {"MaGheHienThi","TenKV","HangVe"}.Any(c =>
                    Vietnamese.CompareInfo.IndexOf(Convert.ToString(row[c]),keyword,CompareOptions.IgnoreCase)>=0)) continue;
                filtered.ImportRow(row);
            }
            bindingSeats=true; dgvGheTrong.DataSource=filtered; dgvGheTrong.ClearSelection(); dgvGheTrong.CurrentCell=null;
            if(previous.HasValue) foreach(DataGridViewRow row in dgvGheTrong.Rows) {
                if(Convert.ToInt32(((DataRowView)row.DataBoundItem)["MaGhe"])==previous.Value) {
                    dgvGheTrong.CurrentCell=row.Cells[0]; row.Selected=true; break;
                }
            }
            bindingSeats=false;
            dgvGheTrong.Visible=filtered.Rows.Count>0; stateLabel.Visible=filtered.Rows.Count==0; retry.Visible=false;
            stateLabel.Text=seats.Rows.Count==0 ? "Suất diễn này đã hết ghế trống." : "Không có ghế khớp bộ lọc. Thử từ khóa hoặc khu vực khác.";
            count.Text=string.Format("{0:N0} / {1:N0} ghế phù hợp",filtered.Rows.Count,seats.Rows.Count);
            UpdateSelection(); LayoutExchange();
        }
        private void UpdateSelection()
        {
            selectedSeat=(dgvGheTrong.CurrentRow?.DataBoundItem as DataRowView)?.Row;
            bool selected=selectedSeat!=null;
            decimal delta=selected ? DonVeBLL.TinhChenhLech(veCu.GiaVe,Convert.ToDecimal(selectedSeat["GiaVe"])) : 0;
            newTicket.Text=selected ? selectedSeat["MaGheHienThi"]+" · "+selectedSeat["ZoneDisplay"]+"\n"+
                ShowTime+"\n"+Money(Convert.ToDecimal(selectedSeat["GiaVe"])) : "Chọn một ghế bên trái";
            difference.Text=!selected ? "Hiện sau khi chọn ghế" : delta>0 ? "Thu thêm "+Money(delta) :
                delta<0 ? "Hoàn lại "+Money(-delta) : "Đổi ngang giá · Không thu thêm";
            difference.ForeColor=selected ? delta>0 ? Color.FromArgb(153,27,27) : delta<0 ? Color.FromArgb(30,64,175) :
                Color.FromArgb(6,95,70) : Color.FromArgb(71,85,105);
            methodLabel.Text=delta>0 ? "Phương thức thu tiền" : "Phương thức hoàn tiền";
            methodLabel.Visible=cboPhuongThuc.Visible=selected && delta!=0;
            RefreshEnabled(); LayoutSummary();
        }
        private void RefreshEnabled()
        {
            cboSuatMoi.Enabled=!dangDoiVe && !loadingShows && cboSuatMoi.Items.Count>0;
            cboKhuVucMoi.Enabled=txtTimGhe.Enabled=dgvGheTrong.Enabled=!dangDoiVe && !loadingSeats && !loadingShows && seats!=null;
            cboPhuongThuc.Enabled=!dangDoiVe;
            retry.Enabled=!dangDoiVe && !loadingSeats && !loadingShows;
            btnDong.Enabled=customerDetails.Enabled=!dangDoiVe;
            btnXacNhan.Enabled=!dangDoiVe && !loadingSeats && !loadingShows && selectedSeat!=null;
        }

        private async void ConfirmExchange(object sender,EventArgs e)
        {
            if(!btnXacNhan.Enabled || selectedSeat==null) return;
            int seatId=Convert.ToInt32(selectedSeat["MaGhe"]);
            decimal delta=DonVeBLL.TinhChenhLech(veCu.GiaVe,Convert.ToDecimal(selectedSeat["GiaVe"]));
            string[] methods={PhuongThucThanhToan.TienMat,PhuongThucThanhToan.ChuyenKhoan,PhuongThucThanhToan.TheNganHang};
            string method=methods[Math.Max(0,cboPhuongThuc.SelectedIndex)];
            string financial=delta==0 ? difference.Text : difference.Text+" · "+cboPhuongThuc.Text+
                "\nXác nhận đã "+(delta>0 ? "thu thêm" : "hoàn lại")+" khoản tiền này cho khách hàng.";
            string confirm="Đổi vé "+veCu.MaGheHienThi+" ("+Money(veCu.GiaVe)+") → "+selectedSeat["MaGheHienThi"]+
                " · "+selectedSeat["ZoneDisplay"]+" ("+Money(Convert.ToDecimal(selectedSeat["GiaVe"]))+")\n"+ShowTime+"\n\n"+financial;
            if(MessageBox.Show(this,confirm,"Xác nhận đổi vé",MessageBoxButtons.YesNo,MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)!=DialogResult.Yes) return;
            dangDoiVe=true; RefreshEnabled(); btnXacNhan.Text="Đang đổi vé…";
            try {
                int actor=PhienDangNhap.DaDangNhap ? PhienDangNhap.TaiKhoan.MaTK : 0;
                DonVeResult result=await Task.Run(() => DonVeBLL.DoiVe(Convert.ToInt32(donVeRow["MaDon"]),veCu.MaCT,seatId,delta,actor,method));
                if(IsDisposed) return;
                if(result.Success) {
                    MaDonMoiDaTao=result.MaDonMoi ?? 0;
                    MessageBox.Show(this,result.Message,"Đổi vé thành công",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    dangDoiVe=false; DialogResult=DialogResult.OK; Close(); return;
                }
                MessageBox.Show(this,result.Message,"Chưa thể đổi vé",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                await LoadSeatsAsync();
            } catch(Exception ex) {
                System.Diagnostics.Trace.TraceError("Exchange failed: {0}",ex);
                MessageBox.Show(this,"Không thể hoàn tất đổi vé. Vui lòng kiểm tra lại đơn trước khi thử lại.","Lỗi xử lý",MessageBoxButtons.OK,MessageBoxIcon.Error);
                await LoadSeatsAsync();
            } finally {
                dangDoiVe=false;
                if(!IsDisposed) { btnXacNhan.Text="Xác nhận đổi vé"; RefreshEnabled(); }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if(dangDoiVe) { e.Cancel=true; return; }
            ++showRequest; ++seatRequest; base.OnFormClosing(e);
        }
        private sealed class ShowItem {
            public int Id; public DateTime Start; public string Location;
            public ShowItem(DataRow row) { Id=Convert.ToInt32(row["MaSuat"]); Start=Convert.ToDateTime(row["ThoiGianBatDau"]); Location=Convert.ToString(row["DiaDiem"]); }
            public override string ToString() { return Start.ToString("dd/MM/yyyy · HH:mm"); }
        }
        private sealed class ZoneItem {
            public int Id; private readonly string text;
            public ZoneItem(int id,string text) { Id=id; this.text=text; }
            public override string ToString() { return text; }
        }
    }
}

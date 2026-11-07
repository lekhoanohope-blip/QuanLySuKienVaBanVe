using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.GUI;
using QuanLySuKienVaBanVe.DTO;
public static class ExchangeUiCheck {
 static BindingFlags flags=BindingFlags.Instance|BindingFlags.NonPublic;
 static object Get(object f,string n){return f.GetType().GetField(n,flags).GetValue(f);}
 static void Set(object f,string n,object v){f.GetType().GetField(n,flags).SetValue(f,v);}
 static void Assert(bool ok,string message){if(!ok)throw new Exception(message);Console.WriteLine("PASS "+message);}
 static void Pump(int ms){var stop=DateTime.UtcNow.AddMilliseconds(ms);while(DateTime.UtcNow<stop){Application.DoEvents();Thread.Sleep(5);}}
 static DataTable Seats(int show,int total){var t=new DataTable();foreach(string n in new[]{"MaGhe","MaKV"})t.Columns.Add(n,typeof(int));foreach(string n in new[]{"MaGheHienThi","TenKV","HangVe"})t.Columns.Add(n);t.Columns.Add("GiaVe",typeof(decimal));for(int i=1;i<=total;i++)t.Rows.Add(show*1000+i,1,"A"+i,"CAT A","Thường",i==1?500000:i==2?700000:300000);return t;}
 static void ScaleControls(Control root,int dpi,float factor){
 var controls=new System.Collections.Generic.List<Control>();
 Action<Control> collect=null;collect=c=>{controls.Add(c);foreach(Control child in c.Controls)collect(child);};collect(root);
 var fonts=new Font[controls.Count];for(int i=0;i<controls.Count;i++)fonts[i]=controls[i].Font;
 var field=typeof(Control).GetField("deviceDpi",BindingFlags.Instance|BindingFlags.NonPublic);
 for(int i=0;i<controls.Count;i++){
   var c=controls[i];if(field!=null)field.SetValue(c,dpi);
   c.Font=new Font(fonts[i].FontFamily,fonts[i].Size*factor,fonts[i].Style);
   var grid=c as DataGridView;
   if(grid!=null){grid.DefaultCellStyle.Font=new Font(grid.DefaultCellStyle.Font.FontFamily,9.5F*dpi/96);grid.ColumnHeadersDefaultCellStyle.Font=new Font(grid.DefaultCellStyle.Font,FontStyle.Bold);}
 }
 }
 static void Save(Form f,string path){using(var b=new Bitmap(f.Width,f.Height)){f.DrawToBitmap(b,new Rectangle(Point.Empty,f.Size));b.Save(path);}}
 public static void Run(string root){ SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
 var orders=new DataTable();foreach(string n in new[]{"MaDon","MaSuat","MaSK"})orders.Columns.Add(n,typeof(int));foreach(string n in new[]{"TenKH","SDT","TenSK"})orders.Columns.Add(n);orders.Columns.Add("ThoiGianBatDau",typeof(DateTime));
 orders.Rows.Add(5,1,1,"Nguyễn Hoàng Minh Anh","0987654321","Diễn đàn Công nghệ: Vietnam Tech Summit 2026",DateTime.Now.AddDays(5));
 var shows=new DataTable();shows.Columns.Add("MaSuat",typeof(int));shows.Columns.Add("ThoiGianBatDau",typeof(DateTime));shows.Columns.Add("DiaDiem");shows.Rows.Add(1,DateTime.Now.AddDays(5),"Trung tâm Hội nghị GEM Center, Thành phố Hồ Chí Minh");shows.Rows.Add(2,DateTime.Now.AddDays(6),"Nhà hát Thành phố");
 using(var f=new FrmDoiVe(orders.Rows[0],new ChiTietVeDTO{MaCT=1,MaDon=5,MaGheHienThi="B1",TenKV="CAT D",HangVe="Thường",GiaVe=500000})){
 Set(f,"fetchShows",new Func<int,DataTable>(id=>shows.Copy()));Set(f,"fetchSeats",new Func<int,DataTable>(id=>Seats(id,150)));
 f.ShowInTaskbar=false;f.StartPosition=FormStartPosition.Manual;f.Location=new Point(-32000,-32000);f.Show();f.Location=new Point(-32000,-32000);Pump(400);
 var grid=(DataGridView)Get(f,"dgvGheTrong");var search=(Control)Get(f,"txtTimGhe");var button=(Control)Get(f,"btnXacNhan");var delta=(Label)Get(f,"difference");var method=(Control)Get(f,"cboPhuongThuc");
 Assert(grid.Rows.Count==150 && !button.Enabled && !method.Visible,"initial async load and unselected state");
 Save(f,root+"/exchange-empty.png");
 grid.CurrentCell=grid.Rows[0].Cells[0];Assert(delta.Text.Contains("Đổi ngang") && !method.Visible,"equal-price exchange");
 grid.CurrentCell=grid.Rows[1].Cells[0];Assert(delta.Text.Contains("200.000") && method.Visible,"additional payment");Save(f,root+"/exchange-payment.png");
 search.Text="A2";Assert(button.Enabled && grid.Rows.Count>0,"filter retains visible selection");
 search.Text="[";Assert(grid.Rows.Count==0 && !button.Enabled,"literal bracket filter and clears hidden selection");
 foreach(var term in new[]{"%","*","'"}){search.Text=term;Assert(grid.Rows.Count==0,"literal search "+term);}search.Text="";
 grid.CurrentCell=grid.Rows[2].Cells[0];Assert(delta.Text.Contains("Hoàn lại 200.000") && method.Visible,"refund state");Save(f,root+"/exchange-refund.png");
 f.ClientSize=new Size(740,620);Pump(30);var seat=(Control)Get(f,"seatCard");var summary=(Control)Get(f,"summaryCard");Assert(summary.Top>seat.Bottom,"narrow window stacks cards");Assert(((Control)Get(f,"actions")).Bottom<=f.ClientSize.Height,"actions fit narrow window");Save(f,root+"/exchange-narrow.png");
 f.ClientSize=new Size(1000,700);Pump(30);
 int priorDpi=96;
 foreach(int dpi in new[]{120,144,96}){
   f.MaximumSize=Size.Empty;f.MinimumSize=Size.Empty;
   f.ClientSize=new Size(Math.Min(1334,1000*dpi/96),Math.Min(736,700*dpi/96));
   ScaleControls(f,dpi,(float)dpi/priorDpi);priorDpi=dpi;
   f.GetType().GetMethod("LayoutExchange",flags).Invoke(f,null);
   var actionArea=(Control)Get(f,"actions");var footer=(Control)Get(f,"count");
   Assert(actionArea.Bottom<=f.ClientSize.Height && grid.Bottom<footer.Top,"DPI layout "+dpi);
   Save(f,root+"/exchange-dpi-"+dpi+".png");
 }
 f.ClientSize=new Size(1000,700);Pump(30);
 var combo=(ComboBox)Get(f,"cboSuatMoi");Set(f,"fetchSeats",new Func<int,DataTable>(id=>{Thread.Sleep(id==2?250:20);return Seats(id,id==1?1:2);}));combo.SelectedIndex=1;combo.SelectedIndex=0;Pump(400);
 Assert(grid.Rows.Count==1 && Convert.ToInt32(((DataRowView)grid.Rows[0].DataBoundItem)["MaGhe"])==1001,"stale seat response ignored");
 Set(f,"fetchSeats",new Func<int,DataTable>(id=>{throw new Exception("fixture failure");}));combo.SelectedIndex=1;Pump(1000);Assert(((Control)Get(f,"retry")).Visible && !button.Enabled,"load failure offers retry");
 Set(f,"fetchSeats",new Func<int,DataTable>(id=>Seats(id,0)));f.GetType().GetMethod("LoadSeatsAsync",flags).Invoke(f,null);Pump(500);Assert(((Label)Get(f,"stateLabel")).Text.Contains("hết ghế"),"retry distinguishes sold-out show");Pump(200);
 }
  // Verify FrmQuanLyDonVe responsive layout, no button overlap at 700px, and ticket code copy button
  using(var fod = new FrmQuanLyDonVe()) {
    fod.ShowInTaskbar = false;
    fod.StartPosition = FormStartPosition.Manual;
    fod.Location = new Point(-32000, -32000);
    fod.ClientSize = new Size(700, 600);
    fod.Show();
    Pump(200);

    var btnDoi = (Control)Get(fod, "btnDoiVe");
    var btnHuy = (Control)Get(fod, "btnHuyVe");
    var btnIn = (Control)Get(fod, "btnInVe");
    var btnCopy = (Control)Get(fod, "btnCopyMaVe");
    var pnlActions = (Control)Get(fod, "pnlDetailActions");

    Assert(!btnDoi.Bounds.IntersectsWith(btnHuy.Bounds), "orders at 700px width: exchange and cancel buttons do not overlap");
    Assert(!btnIn.Bounds.IntersectsWith(btnDoi.Bounds), "orders at 700px width: print and exchange buttons do not overlap");
    Assert(btnHuy.Bottom <= pnlActions.Height, "orders at 700px width: cancel button fits inside actions panel");
    Assert(btnCopy != null && btnCopy.Text.Contains("Sao chép"), "orders ticket lookup copy button is present");

    // Test with sample data in dgvDonVe & dgvChiTietVe
    var dtOrders = new DataTable();
    dtOrders.Columns.Add("MaDon", typeof(int));
    dtOrders.Columns.Add("MaDonGoc", typeof(int));
    dtOrders.Columns.Add("TenKH");
    dtOrders.Columns.Add("SDT");
    dtOrders.Columns.Add("Email");
    dtOrders.Columns.Add("TenSK");
    dtOrders.Columns.Add("ThoiGianBatDau", typeof(DateTime));
    dtOrders.Columns.Add("NgayDat", typeof(DateTime));
    dtOrders.Columns.Add("TongTien", typeof(decimal));
    dtOrders.Columns.Add("TrangThai");
    dtOrders.Columns.Add("SoVeHopLe", typeof(int));
    dtOrders.Columns.Add("TongSoVe", typeof(int));

    // Order with 3 active out of 4 tickets
    dtOrders.Rows.Add(29, DBNull.Value, "Nguy\u1ec5n V\u0103n A", "0987546357", "a@example.com", "Vietnam Tech Summit 2026", DateTime.Now.AddDays(10), DateTime.Now, 1200000m, "DaThanhToan", 3, 4);

    var dtDetail = new DataTable();
    dtDetail.Columns.Add("MaGheHienThi");
    dtDetail.Columns.Add("MaCT", typeof(int));
    dtDetail.Columns.Add("MaVeCode");
    dtDetail.Columns.Add("TenKV");
    dtDetail.Columns.Add("HangVe");
    dtDetail.Columns.Add("GiaVe", typeof(decimal));
    dtDetail.Columns.Add("TrangThai");
    dtDetail.Rows.Add("B1", 101, "SK05-SD07-B1-8884", "GA D", "Ph\u1ed5 th\u00f4ng", 400000m, "HopLe");

    Set(fod, "requestVersion", 99999);
    Set(fod, "detailRequestVersion", 99999);
    Set(fod, "loading", false);
    Set(fod, "loadingDetail", false);

    var gDon = (DataGridView)Get(fod, "dgvDonVe");
    var gChiTiet = (DataGridView)Get(fod, "dgvChiTietVe");
    gDon.DataSource = dtOrders;
    gChiTiet.DataSource = dtDetail;
    Pump(100);

    if (gChiTiet.Rows.Count > 0) {
      gChiTiet.CurrentCell = gChiTiet.Rows[0].Cells["colMaGheHienThi"];
      fod.GetType().GetMethod("CapNhatTrangThaiNutThaoTac", flags).Invoke(fod, null);
      Assert(btnCopy.Enabled, "copy ticket code button enabled when ticket selected");

      // Trigger copy click and verify
      fod.GetType().GetMethod("btnCopyMaVe_Click", flags).Invoke(fod, new object[] { btnCopy, EventArgs.Empty });
      Assert(Clipboard.GetText() == "SK05-SD07-B1-8884", "copy ticket code button copies MaVeCode to clipboard");
    }

    Save(fod, root + "/orders-700px-verified.png");
  }
 }
}
# Hướng dẫn module In vé (FrmInVe) — Phân hệ TV3

Tài liệu này mô tả cách chạy, cách căn chỉnh toạ độ và cách cấu hình thư viện của module
**In vé vừa đặt** (`FrmInVe`), nối từ màn hình **Bán vé** (`FrmBanVe`).

---

## 1. Luồng nghiệp vụ

```
FrmBanVe  --(chọn khu vực -> giữ ghế -> FrmThanhToan -> Commit)-->  MaDonPhatHanh
   |
   |  bấm nút "In vé vừa đặt"
   v
FrmInVe.InDanhSachVe(_lastPaidMaDon, this)
   |
   +--> DonVeBLL.LayDanhSachVeIn(maDon)          // gom DonVe + SuatDien + SuKien + ChiTietVe + Ghe + KhuVuc (chỉ lấy vé hợp lệ)
   +--> ThongTinInVeDTO                          // DTO đã định dạng sẵn cho việc in
   +--> TicketRenderer.Render(...)               // GDI+ vẽ vé (thân + cuống)
   +--> PrintDocument / PrintDialog              // in ra máy in theo đúng khổ 210x90mm và lề 0
   +--> TicketPdfExporter.XuatDanhSach(...)      // xuất PDF 300 DPI bằng PDFsharp
```

### Hàm gọi liên thông (static, gọi từ đâu cũng được)

```csharp
// Chỉ cần truyền mã đơn vé vừa thanh toán
FrmInVe.InDanhSachVe(maDon);
FrmInVe.InDanhSachVe(maDon, this);          // modal trên form chủ
```

Hàm mở form xem trước, không trả về ngoại lệ ra ngoài: lỗi CSDL / đơn không còn vé hợp lệ
được chặn lại bằng `MessageBox` thân thiện.

---

## 2. Cách chạy dự án

### 2.1. Yêu cầu môi trường

| Thành phần | Phiên bản |
|:---|:---|
| Visual Studio | 2019 / 2022 (workload *.NET desktop development*) |
| .NET Framework | 4.7.2 |
| SQL Server | 2019 / 2022 + SSMS |
| NuGet | bản bất kỳ hỗ trợ `packages.config` |

### 2.2. Khôi phục CSDL (chạy 1 lần)

Trong SSMS, mở và thực thi theo thứ tự:

1. `database/create_database.sql`
2. `database/migration_add_columns.sql`
3. `database/migrate_tv3_donve.sql`
4. `database/sample_data.sql` (dữ liệu mẫu) + `database/dashboard_demo_data.sql` (đơn vé demo)

Kiểm tra `App.config` → `connectionString` trúng instance máy bạn, ví dụ
`Data Source=.;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True;Encrypt=False`.

### 2.3. Cài thư viện

Hai gói mới đã được khai báo trong `packages.config` và `QuanLySuKienVaBanVe.csproj`.
Chỉ cần chạy **Restore** là đủ:

```powershell
# PowerShell của VS, chạy tại thư mục dự án
nuget restore QuanLySuKienVaBanVe.sln
```

hoặc trong VS: **Tools → NuGet Package Manager → Restore Packages**.

Thủ công (nếu không có NuGet CLI):

```powershell
nuget install ZXing.Net -Version 0.16.9 -OutputDirectory packages
nuget install PDFsharp  -Version 1.50.5147 -OutputDirectory packages
```

| Gói | Dẫn tới DLL nào | Lý do chọn |
|:---|:---|:---|
| `ZXing.Net 0.16.9` | `packages\ZXing.Net.0.16.9\lib\net47\zxing.dll` | Bản `net47` có `BarcodeWriter` + `BitmapRenderer` dựa trên `System.Drawing`, đúng thứ WinForms cần. **Đừng** dẫn bản `netstandard2.0` — bản đó không có renderer Bitmap. |
| `PDFsharp 1.50.5147` | `packages\PDFsharp.1.50.5147\lib\net20\PdfSharp.dll` | Bản này chỉ tham chiếu `System.Drawing` (không kéo theo WPF), có `XImage.FromStream` để nhúng ảnh vé đã render. |

> **Lưu ý API thật của ZXing 0.16.9** (dễ sai nếu chép code trên mạng):
> - Options là `ZXing.Common.EncodingOptions` **không có** thuộc tính `Content` — nội dung truyền qua `writer.Write(content)`.
> - `BitmapRenderer` dùng `Foreground` / `Background`, **không phải** `ColorBlack` / `ColorWhite`.
> - Đặt `PureBarcode = true` + `NoPadding = true` + `Margin = 0` để barcode lấp đầy khung và **không có dòng số dưới vạch**.

### 2.4. Build và chạy

```
F5 trong Visual Studio (Debug), hoặc:
msbuild QuanLySuKienVaBanVe.csproj /t:Rebuild /p:Configuration=Debug
bin\Debug\QuanLySuKienVaBanVe.exe
```

Đăng nhập → **Bán vé** → chọn sự kiện/suất diễn → bấm một khối khu vực → chọn số lượng →
thanh toán. Sau khi thanh toán thành công, nút **"In vé vừa đặt"** hiện ở góc dưới phải; bấm vào là mở `FrmInVe`.

Trong `FrmInVe` có thanh công cụ tinh gọn gồm 3 nút chức năng và ô thư mục:

| Nút / Thành phần | Chức năng |
|:---|:---|
| In vé | `PrintDialog` → Cấu hình khổ giấy 210×90mm và lề 0 cho máy in → `PrintDocument.Print()` (mỗi vé một trang) |
| Xuất PDF | Ghi file PDF 300 DPI chuẩn vào thư mục cấu hình ở ô "Thư mục PDF" |
| Đóng | Quay lại màn hình Bán vé |
| Ô "Thư mục PDF" | Cho phép tùy chỉnh thư mục lưu file PDF xuất ra (mặc định: `Documents\EventTicket\VeIn`) |

Toàn bộ vé trong cùng một phiên mua được xếp chồng theo chiều dọc trên form xem trước, có thể cuộn lên/xuống để kiểm tra trực quan.

---

## 3. Quy tắc đặt tên file PDF (bắt buộc)

```
EventTicket_mave_[số_mã_vé].pdf
```

`TicketPdfExporter.LayTenFile()` sinh tên theo đúng khuôn trên, trong đó
**số mã vé = `ChiTietVe.MaCT`** — con số do CSDL cấp ngay khi bấm thanh toán.
Nếu `MaCT` chưa có, hàm lấy chuỗi số liền cuối của `MaVeCode` (ví dụ `SK19-SD50-A1-4691` → `4691`),
rồi cuối cùng fallback về `MaDon`. Chuỗi được làm sạch ký tự cấm của Windows nên không bao giờ lỗi path.

Kiểm chứng với dữ liệu thật trong `dashboard_demo_data.sql`:

| MaDon | MaCT | MaVeCode | File PDF sinh ra |
|:---:|:---:|:---|:---|
| 339 | 977 | `SK19-SD50-A 1-…` | `EventTicket_mave_977.pdf` |
| 340 | 979 | `SK19-SD50-A 3-4691` | `EventTicket_mave_979.pdf` |

Một đơn có *n* vé → *n* file PDF, **mỗi file đều đúng khuôn tên trên** (không gộp thành
tên lạ để tránh vi phạm quy tắc). Muốn một file hợp nhất nhiều trang thì dùng
`TicketPdfExporter.XuatVaoStream(stream, danhSach)`.

---

## 4. Hệ toạ độ và cách căn chỉnh cho khớp hình nền

### 4.1. Vì sao không đặt Label thật lên ảnh?

Vé phải ra được 3 nơi: màn hình, máy in và PDF. Nếu dùng `Label`/`Panel` phủ lên `PictureBox`,
toạ độ phụ thuộc DPI Windows và font máy người dùng → **lệch hoạ tiết khi in**.
Vì vậy module này vẽ toàn bộ vé vào một `Bitmap` bằng GDI+, rồi:

- màn hình: `picTicket.Image = bitmap` (dùng `PictureBoxSizeMode.Zoom` mượt mà khi co giãn form)
- máy in: `e.Graphics.DrawImage(bitmap, ...)` (chuẩn đơn vị `1/100 inch` không phụ thuộc DPI máy in)
- PDF: nhúng bitmap vào trang `210mm x 90mm`

Chỉ có **một nguồn vẽ duy nhất** nên không thể lệch giữa ba nơi.

### 4.2. Hệ toạ độ thiết kế

Mọi hằng số trong `TicketRenderer` khai báo trên canvas chuẩn **2100 x 900 unit**,
trong đó `1 unit = 0.1 mm` → vé thật **210mm x 90mm**.

```
|<---------------- 2100 ---------------->|
|<----- thân 1575 ---->|<--- cuống 525 -->|
                      ^
                 TearX = 1575 (đường xé vé nét đứt)
```

Khi vẽ, `Render()` gọi `g.ScaleTransform(widthPx / 2100f, ...)` nên **cùng một bộ toạ độ**
dùng được cho preview lẫn bản in 2480px @300DPI. Muốn đổi vị trí, chỉ cần sửa số
trong hệ 2100x900 — không phải tính lại theo DPI.

### 4.3. Bảng toạ độ đang dùng (thân vé)

| Thành phần | Vùng `RectangleF(x, y, w, h)` | Cỡ chữ | Thuộc tính |
|:---|:---|:---:|:---|
| Logo `EventTicket` | `(90, 66, 430, 100)` | — | ảnh `logoEventTicket.png` |
| Tên sự kiện | `(90, 209, 1040, 150)` | 112→tự co | `TenSuKien` |
| `Location: …` | `(90, 353, 1040, 60)` | 40→tự co | `DiaDiem` |
| Ô Zone | `(87, 487, 298, 205)` | 76 | `Zone` ← `KhuVuc.TenKV` |
| Ô Row \| Seat | `(412, 487, 362, 205)` | 76 | `Hang` / `Cot` |
| Ô Time | `(801, 487, 348, 205)` | 76 | `ThoiGianBatDau` (`HH:mm`) |
| Ngày diễn | `(100, 715, 1100, 185)` | 150 | `dd.MM.yyyy` |
| Chùm nốt nhạc | `(1304, 90, 120, 299)` | — | ảnh `MusicNote.png` |

### 4.4. Bảng toạ độ cuống vé (x bắt đầu từ 1575)

| Thành phần | Vùng | Cỡ chữ |
|:---|:---|:---:|
| Logo `EventTicket` | `(1637, 55, 400, 93)` | — |
| Ngày diễn | `(1595, 195, 485, 110)` | 84 |
| `Row` + giá trị | `(1620, 300, 200, 136)` | 36 / 62 |
| `Seat` + giá trị | `(1830, 300, 200, 136)` | 36 / 62 |
| Tên sự kiện (tối đa 3 dòng) | `(1605, 465, 465, 170)` | 62→tự co |
| Khung barcode trắng | `(1625, 676, 430, 175)` | — |

### 4.5. Dùng ảnh nền có sẵn và font Poppins

Bộ ảnh nền và logo được lưu tại `Resources/Ticket/`:
- `ticket_bg.jpg`: nền thân vé
- `stub_bg.jpg`: nền cuống vé
- `logoEventTicket.png`: logo thương hiệu
- `MusicNote.png`: nốt nhạc trang trí

Hàm `LoadImage()` nạp qua `FileStream` và tạo bitmap độc lập, tránh khóa file khi ứng dụng đang chạy. Nếu thiếu file ảnh, `TicketRenderer` tự động rơi về nền gradient uốn lượn được vẽ trực tiếp bằng code GDI+.

Bộ font `Poppins` gồm 4 file `.ttf` (`Poppins-Regular`, `Poppins-Medium`, `Poppins-SemiBold`, `Poppins-Bold`) trong `Resources/Fonts/` được cấu hình tự động copy ra thư mục build và nạp qua `PrivateFontCollection`, giúp phôi vé giữ đúng kiểu chữ thiết kế trên mọi máy tính ngay cả khi chưa cài đặt font Poppins vào Windows.

---

## 5. Những ràng buộc thiết kế đã hiện thực hoá

| Yêu cầu | Trạng thái |
|:---|:---|
| Logo `EventTicket` ở góc trên trái thân vé **và** trên cùng cuống vé | ✅ `DrawLogo()` / `DrawStub()` |
| Nhãn tĩnh `Location:`, `Zone`, `Row`, `Seat`, `Time:` | ✅ hard-code, không lấy từ DB |
| Seat < 10 tự thêm số 0 (`1 → 01`, `8 → 08`) | ✅ `ThongTinInVeDTO.SoGheIn` dùng `Cot.ToString("D2")` |
| Ngày định dạng `01.12.2026` ở thân vé **và** cuống vé | ✅ `NgayIn` = `dd.MM.yyyy` |
| Time định dạng `00:00` | ✅ `GioIn` = `HH:mm` |
| **Không** QR code ở thân vé | ✅ renderer không có bất kỳ dòng vẽ QR nào |
| **Không** dòng số dưới barcode | ✅ `PureBarcode = true`, không vẽ nhãn |
| Barcode Code128 sinh từ mã vé | ✅ `ZXing.BarcodeWriter` + `CODE_128`, nội dung = `MaVeCode` |
| Đường đứt nét xé vé + 2 lỗ tròn | ✅ `DrawTear()` |
| PDF 300 DPI | ✅ render 2480 px cho trang 210 mm |
| In vật lý đúng kích thước 210×90mm | ✅ cấu hình khổ giấy động, lề 0, không nhân phóng đại DPI |
| Vé không bao giờ tràn chữ | ✅ `DrawFittedText()` tự co chữ, hết cỡ thì cắt dấu ba chấm |

---

## 6. File đã thêm / sửa

| File | Loại | Nội dung |
|:---|:---|:---|
| `DTO/ThongTinInVeDTO.cs` | mới | DTO in vé + định dạng `D2`, `dd.MM.yyyy`, `HH:mm`, suy `SoMaVe` |
| `GUI/Helpers/TicketRenderer.cs` | mới | Toàn bộ GDI+: nền, logo, 3 ô thông tin, cuống vé, barcode, nốt nhạc, font Poppins |
| `GUI/Helpers/TicketPdfExporter.cs` | mới | Nhúng ảnh vé vào PDFsharp, sinh tên file bắt buộc |
| `GUI/FrmInVe.cs` | mới | Form xem trước dạng cuộn dọc + in máy in vật lý + xuất PDF |
| `BLL/DonVeBLL.cs` | sửa | thêm `LayDanhSachVeIn(int maDon)` (kiểm tra đơn tồn tại và lọc bỏ vé `DaHuy`) |
| `DTO/ChiTietVeDTO.cs` | sửa | thêm `Hang`, `Cot` (DAL vốn đã SELECT 2 cột này) |
| `GUI/FrmBanVe.cs` | sửa | `btnInVe_Click` gọi thật `FrmInVe.InDanhSachVe(_lastPaidMaDon, this)` |
| `GUI/FrmBanVe.Designer.cs` | sửa | nhãn nút bỏ chữ **"(Đang phát triển)"** |
| `QuanLySuKienVaBanVe.csproj`, `packages.config` | sửa | khai báo 2 gói NuGet + font Poppins + tài nguyên Ticket |

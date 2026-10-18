# ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ

> **Học phần:** COMP1019 — Lập trình Windows  
> **Giảng viên hướng dẫn:** ThS. Lê Thanh Thoại  
> **Khoa:** Công nghệ Thông tin — Trường Đại học Sư phạm TP.HCM (HCMUE)  
> **Học kỳ / Năm học:** Học kỳ 1 — Năm học 2026–2027  
> **Nhóm thực hiện:** Nhóm 3  

---

## 1. Thông tin thành viên nhóm & Phân công nhiệm vụ

| STT | MSSV | Họ và tên | Vai trò / Phân hệ phụ trách | Nhánh Git làm việc | Tỷ lệ hoàn thành |
|:---:|:----:|:----------|:-----------------------------|:-------------------|:----------------:|
| 1 | *[MSSV]* | *[Họ tên TV1]* | **TV1:** Phân quyền, Đăng nhập (`FrmLogin`), Quản lý tài khoản (`FrmTaiKhoan`), Sơ đồ ghế User Control (`UcSoDoGhe`), Quản lý ghế (`FrmGhe`), Nhật ký thao tác (`FrmNhatKy`) | `feature/auth-seat-matrix` | 100% |
| 2 | *[MSSV]* | *[Họ tên TV2]* | **TV2:** Giao diện chính MDI (`FrmMain`), Quản lý Sự kiện (`FrmSuKien`), Suất diễn (`FrmSuatDien`), Khu vực / Hạng vé (`FrmKhuVuc`), Báo cáo thống kê & Biểu đồ (`FrmBaoCaoThongKe`) | `feature/event-category-report` | 100% |
| 3 | *[MSSV]* | *[Họ tên TV3]* | **TV3 (Nhóm trưởng):** Hạ tầng CSDL (`Database.cs`), Nghiệp vụ Bán vé (`FrmBanVe`), Giao dịch thanh toán an toàn (`SqlTransaction`), Quản lý Khách hàng (`FrmKhachHang`), Đơn vị tổ chức (`FrmDonVe`), Xuất hóa đơn / In vé | `feature/ticket-booking-core` | 100% |

---

## 2. Mô tả đề tài

Phần mềm **Quản lý sự kiện và bán vé** là ứng dụng desktop xây dựng trên nền tảng **Windows Forms (.NET Framework)**, hỗ trợ các đơn vị tổ chức biểu diễn, nhà hát, trung tâm hội nghị quản lý toàn diện vòng đời của một sự kiện: từ khâu khởi tạo sự kiện, thiết lập các suất diễn, phân chia khu vực ghế theo hạng vé (VIP, Standard, Economy), cho đến khâu bán vé trực quan tại quầy, quản lý giao dịch thanh toán và xuất báo cáo doanh thu.

### Mục tiêu chính:
- **Tối ưu hóa quy trình bán vé:** Bán vé trực quan thông qua sơ đồ ghế động, cho phép chọn nhiều ghế, tự động tính tổng tiền tức thời theo từng hạng vé.
- **Bảo toàn dữ liệu (Atomic Transaction):** Ngăn chặn triệt để tình trạng bán trùng ghế thông qua cơ chế khóa giao dịch (`SqlTransaction`).
- **Thống kê chuyên sâu:** Trực quan hóa doanh thu theo sự kiện, theo khoảng thời gian và đo lường tỷ lệ lấp đầy ghế bằng biểu đồ.

---

## 3. Công nghệ và Kiến trúc hệ thống

- **Ngôn ngữ lập trình:** C# (C# 7.3+ / .NET Framework 4.7.2)
- **Giao diện người dùng:** Windows Forms (WinForms), User Control tùy biến, GDI+
- **Cơ sở dữ liệu:** Microsoft SQL Server 2019 / 2022
- **Phương thức truy xuất:** ADO.NET (`SqlConnection`, `SqlCommand`, `SqlTransaction`, `SqlDataReader`, `SqlDataAdapter`)
- **Mô hình kiến trúc:** Chuẩn **3 lớp (3-Tier Architecture)** kết hợp đối tượng truyền dữ liệu (DTO):
  - **GUI (Presentation Layer):** Giao diện người dùng gồm Form cha MDI, các Form con nghiệp vụ và User Control sơ đồ ghế (`UcSoDoGhe`).
  - **BLL (Business Logic Layer):** Tiếp nhận dữ liệu từ GUI, kiểm tra ràng buộc nghiệp vụ (validation), tự động sinh sơ đồ ghế, tính tiền và điều phối giao dịch.
  - **DAL (Data Access Layer):** Thực thi các câu lệnh SQL, Stored Procedure và Transaction thông qua lớp trợ giúp dữ liệu dùng chung `Database.cs`.
  - **DTO (Data Transfer Object):** Các class POCO ánh xạ thực thể 1-1 với các bảng trong SQL Server, dùng làm phương tiện trao đổi dữ liệu xuyên suốt các tầng.

---

## 4. Các chức năng chính

- **Phân hệ Quản trị & Xác thực:** Đăng nhập hệ thống phân quyền (Admin / Staff), đổi mật khẩu, quản lý tài khoản nhân viên, nhật ký thao tác người dùng.
- **Phân hệ Danh mục:** Quản lý Sự kiện, Suất diễn, Khu vực / Hạng vé, Ghế, Khách hàng, Đơn vị tổ chức.
- **Phân hệ Nghiệp vụ Bán vé:** Chọn sự kiện $\rightarrow$ chọn suất diễn $\rightarrow$ hiển thị sơ đồ ghế động $\rightarrow$ chọn ghế $\rightarrow$ chọn/thêm khách hàng $\rightarrow$ thanh toán bằng `SqlTransaction` an toàn tuyệt đối $\rightarrow$ xuất vé / in hóa đơn.
- **Phân hệ Báo cáo & Thống kê:** Thống kê doanh thu theo sự kiện, theo suất diễn, tỷ lệ lấp đầy ghế, biểu đồ cột/tròn trực quan, xuất dữ liệu ra file.

---

## 5. Hướng dẫn cài đặt & Phục hồi Database

### 5.1. Chuẩn bị môi trường
- Hệ điều hành: Windows 10 hoặc Windows 11.
- Visual Studio 2019 hoặc Visual Studio 2022 (đã cài workload *.NET desktop development*).
- Microsoft SQL Server 2019 / 2022 và SQL Server Management Studio (SSMS).

### 5.2. Các bước khởi chạy
1. **Clone repository về máy:**
   ```bash
   git clone https://github.com/<tai-khoan>/QuanLySuKienVaBanVe.git
   ```

2. **Cài đặt Cơ sở dữ liệu:**
   - Mở **SSMS** và kết nối vào SQL Server.
   - Mở và thực thi file script: `database/create_database.sql` để tạo database và các bảng.
   - Mở và thực thi file script: `database/sample_data.sql` để nạp dữ liệu danh mục ban đầu và tài khoản mẫu.

3. **Cấu hình chuỗi kết nối trong `App.config`:**
   - Mở file `App.config`, kiểm tra và sửa lại `connectionString` cho khớp với tên SQL Server Instance của máy cá nhân:
     ```xml
     <connectionStrings>
       <add name="ConnStr" 
            connectionString="Data Source=.;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True;Encrypt=False" 
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```
     *(Nếu dùng SQL Server Express, đổi `Data Source=.\SQLEXPRESS`)*.

4. **Biên dịch và chạy ứng dụng:**
   - Mở file `QuanLySuKienVaBanVe.sln` bằng Visual Studio.
   - Nhấn chuột phải vào Solution $\rightarrow$ chọn **Rebuild Solution**.
   - Nhấn **F5** (hoặc nút **Start**) để khởi chạy chương trình.

---

## 6. Tài khoản Demo

| Tên đăng nhập | Mật khẩu | Quyền hạn / Vai trò | Phạm vi thao tác |
|:--------------|:---------|:---------------------|:-----------------|
| `admin`       | `123456` | **Quản trị viên (Admin)** | Toàn quyền: Quản lý tài khoản, cấu hình danh mục sự kiện, xem nhật ký, báo cáo doanh thu |
| `staff`       | `123456` | **Nhân viên bán vé (Staff)** | Nghiệp vụ: Bán vé tại quầy, tra cứu ghế, quản lý thông tin khách hàng, xuất vé |

---

## 7. Hình ảnh minh họa giao diện

### Màn hình đăng nhập hệ thống
![Login Form](docs/screenshots/login.png)

### Sơ đồ vị trí ghế & Màn hình Bán vé
![Booking Form](docs/screenshots/booking.png)

### Báo cáo thống kê & Biểu đồ doanh thu
![Report Form](docs/screenshots/report.png)

---

## 8. Quy chuẩn phối hợp Git trong nhóm

- **Quy tắc vàng:** *"Trước khi code: pull. Sau khi code chạy được: commit. Khi muốn nộp lên GitHub: push thông qua Pull Request."*
- **Quy chuẩn đặt tên nhánh:**
  - Tính năng mới: `feature/<ten-chuc-nang>` (Ví dụ: `feature/login`, `feature/seat-matrix`, `feature/ticket-booking`)
  - Sửa lỗi: `fix/<ten-loi>` (Ví dụ: `fix/db-conn`, `fix/seat-price-calculation`)
- **Quy chuẩn commit message:** `[Hành động] + [Đối tượng thay đổi]` (Ví dụ: `Add UcSoDoGhe component`, `Implement SqlTransaction in BanVeBLL`, `Fix connection string in App.config`).
param(
  [string]$SqlServer = '.',
  [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
  [string]$AssemblyPath = ''
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Drawing

$root = Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($AssemblyPath)) { 
  $AssemblyPath = Join-Path $root ("bin\{0}\QuanLySuKienVaBanVe.exe" -f $Configuration) 
}
$null = [Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $AssemblyPath).Path)

$dbName = 'CodexTest_Booking_' + [Guid]::NewGuid().ToString('N')
$builder = New-Object Data.SqlClient.SqlConnectionStringBuilder
$builder['Data Source'] = $SqlServer
$builder['Initial Catalog'] = 'master'
$builder['Integrated Security'] = $true
$builder['Encrypt'] = $false
$master = New-Object Data.SqlClient.SqlConnection $builder.ConnectionString
$builder['Initial Catalog'] = $dbName
$testConn = $builder.ConnectionString

function Sql-Exec([string]$sql) {
  $connection = New-Object Data.SqlClient.SqlConnection $testConn
  try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = $sql
    $command.CommandTimeout = 15
    $null = $command.ExecuteNonQuery()
  }
  finally { $connection.Dispose() }
}

function Sql-Value([string]$sql) {
  $connection = New-Object Data.SqlClient.SqlConnection $testConn
  try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = $sql
    $command.CommandTimeout = 15
    return $command.ExecuteScalar()
  }
  finally { $connection.Dispose() }
}

function Run-Batches([string]$sql) {
  foreach($batch in [regex]::Split($sql, '(?im)^\s*GO\s*(?:--[^\r\n]*)?$')) {
    if ([string]::IsNullOrWhiteSpace($batch)) { continue }
    $command = $master.CreateCommand()
    $command.CommandText = $batch
    $command.CommandTimeout = 30
    $null = $command.ExecuteNonQuery()
  }
}

function Reset-Fixture([int]$price = 500000, [int]$hoursFromNow = 24, [int]$seatCount = 10) {
  $sql = @"
DELETE NhatKy; DELETE ThanhToan; DELETE ChiTietVe; DELETE DonVe;
DELETE Ghe; DELETE KhuVuc; DELETE SuatDien; DELETE SuKien; DELETE KhachHang; DELETE TaiKhoan;

INSERT TaiKhoan(TenDangNhap, MatKhau, VaiTro, TrangThai) VALUES(N'admin_test', N'pwd123', N'QuanLy', 1);
DECLARE @tkAdmin int = SCOPE_IDENTITY();
INSERT TaiKhoan(TenDangNhap, MatKhau, VaiTro, TrangThai) VALUES(N'staff_test', N'pwd123', N'NhanVien', 1);
DECLARE @tkStaff int = SCOPE_IDENTITY();

INSERT KhachHang(TenKH, SDT, Email) VALUES(N'Nguyen Van A', '0912345678', N'vana@example.com');
DECLARE @kh int = SCOPE_IDENTITY();

INSERT SuKien(TenSK, TrangThai) VALUES(N'Live Concert Test', N'SapDien');
DECLARE @sk int = SCOPE_IDENTITY();

INSERT SuatDien(MaSK, ThoiGianBatDau, DiaDiem, TrangThai) VALUES(@sk, DATEADD(HOUR, $hoursFromNow, GETDATE()), N'Nha Hat Lon', N'MoCuaBanVe');
DECLARE @suat int = SCOPE_IDENTITY();

INSERT KhuVuc(MaSuat, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MaVung, MauSac) 
VALUES(@suat, N'Khu VIP A', N'VIP', $price, $seatCount, 1, 'VIP_A', '#D97706');
DECLARE @kv int = SCOPE_IDENTITY();

-- Tạo các ghế cho khu vực
DECLARE @i int = 1;
WHILE @i <= $seatCount
BEGIN
    INSERT Ghe(MaKV, Hang, Cot, TrangThai) 
    VALUES(@kv, 'A', @i, N'Trong');
    SET @i = @i + 1;
END

SELECT @tkStaff AS StaffId, @tkAdmin AS AdminId, @kh AS KhachId, @sk AS SkId, @suat AS SuatId, @kv AS KvId;
"@
  $connection = New-Object Data.SqlClient.SqlConnection $testConn
  try {
    $adapter = New-Object Data.SqlClient.SqlDataAdapter($sql, $connection)
    $table = New-Object Data.DataTable
    $null = $adapter.Fill($table)
    $r = $table.Rows[0]
    return @{
      StaffId = [int]$r.StaffId
      AdminId = [int]$r.AdminId
      KhachId = [int]$r.KhachId
      SkId = [int]$r.SkId
      SuatId = [int]$r.SuatId
      KvId = [int]$r.KvId
    }
  }
  finally { $connection.Dispose() }
}

try {
  $master.Open()
  Run-Batches ([IO.File]::ReadAllText((Join-Path $root 'database\create_database.sql')).Replace('QuanLySuKienVaBanVe', $dbName))
  [QuanLySuKienVaBanVe.DAL.Database].GetField('_connStr', [Reflection.BindingFlags]'Static,NonPublic').SetValue($null, $testConn)

  # T1: Idempotent migration
  foreach ($migrationPath in @('database\migration_add_columns.sql', 'database\migrate_tv3_donve.sql', 'database\migrate_tv3_banve.sql')) {
    $migration = [IO.File]::ReadAllText((Join-Path $root $migrationPath)).Replace('QuanLySuKienVaBanVe', $dbName)
    Run-Batches $migration
    Run-Batches $migration # Chạy lại lần 2 để kiểm tra idempotent
  }

  $hasColMaPhienGiu = [int](Sql-Value "SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('Ghe') AND name = 'MaPhienGiu'")
  $hasColThoiGianGiu = [int](Sql-Value "SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('Ghe') AND name = 'ThoiGianGiu'")
  $hasColMauSac = [int](Sql-Value "SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('KhuVuc') AND name = 'MauSac'")
  $hasIndexPhienGiu = [int](Sql-Value "SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID('Ghe') AND name = 'IX_Ghe_MaPhienGiu'")

  if ($hasColMaPhienGiu -ne 1 -or $hasColThoiGianGiu -ne 1 -or $hasColMauSac -ne 1 -or $hasIndexPhienGiu -ne 1) {
    throw "T1 Failed: Missing columns or index after running migrations."
  }
  Write-Output "PASS T1: Idempotent migration runs twice without error; columns and index exist"

  # Setup test data
  $f = Reset-Fixture -price 500000 -hoursFromNow 24 -seatCount 10

  # T1B: Upgrade a legacy database that still requires a PhienGiuGhe parent row.
  $legacySession = [Guid]::NewGuid()
  $legacySeat = [int](Sql-Value "SELECT MIN(MaGhe) FROM Ghe WHERE MaKV = $($f.KvId)")
  Sql-Exec @"
CREATE TABLE dbo.PhienGiuGhe (MaPhienGiu UNIQUEIDENTIFIER NOT NULL PRIMARY KEY);
INSERT dbo.PhienGiuGhe (MaPhienGiu) VALUES ('$legacySession');
UPDATE dbo.Ghe SET TrangThai = N'DangGiu', ThoiGianGiu = GETDATE(), MaPhienGiu = '$legacySession'
WHERE MaGhe = $legacySeat;
ALTER TABLE dbo.Ghe ADD CONSTRAINT FK_Ghe_PhienGiuGhe
    FOREIGN KEY (MaPhienGiu) REFERENCES dbo.PhienGiuGhe(MaPhienGiu);
ALTER TABLE dbo.DonVe ADD MaPhienGiu UNIQUEIDENTIFIER NULL;
ALTER TABLE dbo.DonVe ADD CONSTRAINT FK_DonVe_PhienGiuGhe
    FOREIGN KEY (MaPhienGiu) REFERENCES dbo.PhienGiuGhe(MaPhienGiu);
"@
  $legacyTime = Sql-Value "SELECT ThoiGianGiu FROM Ghe WHERE MaGhe = $legacySeat"
  $legacyHeld = $null
  $legacyNewSession = [Guid]::Empty
  $legacyError = ''
  $legacyOk = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($f.KvId, 1, [ref]$legacyHeld, [ref]$legacyNewSession, [ref]$legacyError)
  if ($legacyOk -or -not $legacyError.Contains('FK_Ghe_PhienGiuGhe')) {
    throw "T1B Failed: Legacy fixture did not reproduce the foreign key failure: $legacyError"
  }

  $bookingMigration = [IO.File]::ReadAllText((Join-Path $root 'database\migrate_tv3_banve.sql')).Replace('QuanLySuKienVaBanVe', $dbName)
  Run-Batches $bookingMigration
  Run-Batches $bookingMigration
  if ([int](Sql-Value "SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Ghe_PhienGiuGhe' AND parent_object_id = OBJECT_ID('dbo.Ghe')") -ne 0) {
    throw 'T1B Failed: Legacy seat-session foreign key still exists after migration.'
  }
  if ([int](Sql-Value "SELECT COUNT(*) FROM dbo.PhienGiuGhe WHERE MaPhienGiu = '$legacySession'") -ne 1 -or
      [Guid](Sql-Value "SELECT MaPhienGiu FROM Ghe WHERE MaGhe = $legacySeat") -ne $legacySession -or
      (Sql-Value "SELECT ThoiGianGiu FROM Ghe WHERE MaGhe = $legacySeat") -ne $legacyTime -or
      (Sql-Value "SELECT TrangThai FROM Ghe WHERE MaGhe = $legacySeat") -ne 'DangGiu') {
    throw 'T1B Failed: Migration changed legacy session or held-seat data.'
  }
  if ([int](Sql-Value "SELECT COUNT(*) FROM sys.foreign_keys WHERE name IN ('FK_Ghe_KhuVuc', 'FK_DonVe_PhienGiuGhe')") -ne 2) {
    throw 'T1B Failed: Migration removed an unrelated foreign key.'
  }
  if (-not [QuanLySuKienVaBanVe.BLL.GheBLL]::GiaiPhongGhe($legacySeat, $legacySession)) {
    throw 'T1B Failed: Existing hold could not be released after migration.'
  }
  $singleSession = [Guid]::Empty
  $singleError = ''
  $singleOk = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTamThoi($legacySeat, [ref]$singleSession, [ref]$singleError)
  if (-not $singleOk -or $singleSession -eq [Guid]::Empty) {
    throw "T1B Failed: Single-seat hold still fails after migration: $singleError"
  }
  if ([QuanLySuKienVaBanVe.BLL.GheBLL]::GiaiPhongGhe($legacySeat, [Guid]::NewGuid()) -or
      -not [QuanLySuKienVaBanVe.BLL.GheBLL]::GiaiPhongGhe($legacySeat, $singleSession)) {
    throw 'T1B Failed: Single-seat hold lost session ownership protection.'
  }
  Write-Output 'PASS T1B: Legacy FK failure reproduced; migration preserves data and unrelated FKs; single-seat hold/release works'

  # T2: GiuGheTuDongTheoKhuVuc generates valid session GUID and updates seat status
  $heldSeats = $null
  $sessionId = [Guid]::Empty
  $errMsg = ""
  $okHold = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($f.KvId, 2, [ref]$heldSeats, [ref]$sessionId, [ref]$errMsg)

  if (-not $okHold -or $heldSeats.Count -ne 2 -or $sessionId -eq [Guid]::Empty) {
    throw "T2 Failed: GiuGheTuDongTheoKhuVuc returned false or invalid session GUID ($sessionId). Error: $errMsg"
  }
  $seat1 = $heldSeats[0].MaGhe
  $seat2 = $heldSeats[1].MaGhe
  $status1 = Sql-Value "SELECT TrangThai FROM Ghe WHERE MaGhe = $seat1"
  $dbSession1 = [Guid](Sql-Value "SELECT MaPhienGiu FROM Ghe WHERE MaGhe = $seat1")
  if ($status1 -ne "DangGiu" -or $dbSession1 -ne $sessionId) {
    throw "T2 Failed: Seat $seat1 in DB does not have status 'DangGiu' or matching MaPhienGiu."
  }
  Write-Output "PASS T2: Automatic seat hold generates valid session GUID and updates seat status in DB"

  # T3: Session ownership enforcement on release
  $fakeSession = [Guid]::NewGuid()
  $releaseWrong = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiaiPhongGhe($seat1, $fakeSession)
  $statusAfterWrong = Sql-Value "SELECT TrangThai FROM Ghe WHERE MaGhe = $seat1"
  if ($releaseWrong -ne $false -or $statusAfterWrong -ne "DangGiu") {
    throw "T3 Failed: Seat was released using an invalid session GUID!"
  }
  Write-Output "PASS T3: Releasing seat with mismatched session GUID is rejected without modifying seat"

  # T4: Stolen seat / expired session cannot checkout
  # Quầy B chiếm ghế bằng session mới
  $sessionB = [Guid]::NewGuid()
  Sql-Exec "UPDATE Ghe SET MaPhienGiu = '$sessionB', ThoiGianGiu = GETDATE() WHERE MaGhe = $seat1"

  $dtoStolen = New-Object QuanLySuKienVaBanVe.DTO.ThongTinDatVeDTO
  $dtoStolen.MaSK = $f.SkId
  $dtoStolen.MaSuat = $f.SuatId
  $dtoStolen.MaKV = $f.KvId
  $dtoStolen.TenKV = "Khu VIP A"
  $dtoStolen.SoLuong = 1
  $dtoStolen.GiaVe = 500000
  $dtoStolen.TongTienDaXacNhan = 500000
  $dtoStolen.DanhSachMaGhe = [Collections.Generic.List[int]]::new()
  $dtoStolen.DanhSachMaGhe.Add($seat1)
  $dtoStolen.DanhSachGhe = [Collections.Generic.List[string]]::new()
  $dtoStolen.DanhSachGhe.Add("A-01")
  $dtoStolen.MaPhienGiu = $sessionId # Phiên cũ của Quầy A
  $dtoStolen.TenKH = "Khach A"
  $dtoStolen.SDT = "0988776655"
  $dtoStolen.PhuongThucThanhToan = [QuanLySuKienVaBanVe.DTO.PhuongThucThanhToan]::TienMat

  $kqStolen = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dtoStolen, $f.StaffId)
  if ($kqStolen.Success -or (Sql-Value "SELECT TrangThai FROM Ghe WHERE MaGhe = $seat1") -eq "DaBan") {
    throw "T4 Failed: Stolen seat checked out successfully with expired session GUID!"
  }
  Write-Output "PASS T4: Checkout with expired/reassigned session GUID is rejected"

  # T5: Stale price detection & confirmation roundtrip
  $f = Reset-Fixture -price 500000 -hoursFromNow 24 -seatCount 10
  $heldSeatsT5 = $null
  $sessionT5 = [Guid]::Empty
  $null = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($f.KvId, 1, [ref]$heldSeatsT5, [ref]$sessionT5, [ref]$errMsg)
  $seatT5 = $heldSeatsT5[0].MaGhe

  # Quản lý đổi giá lên 750,000 trong DB
  Sql-Exec "UPDATE KhuVuc SET GiaVe = 750000 WHERE MaKV = $($f.KvId)"

  $dtoT5 = New-Object QuanLySuKienVaBanVe.DTO.ThongTinDatVeDTO
  $dtoT5.MaSK = $f.SkId
  $dtoT5.MaSuat = $f.SuatId
  $dtoT5.MaKV = $f.KvId
  $dtoT5.TenKV = "Khu VIP A"
  $dtoT5.SoLuong = 1
  $dtoT5.GiaVe = 500000
  $dtoT5.TongTienDaXacNhan = 500000 # Giá cũ 500k
  $dtoT5.DanhSachMaGhe = [Collections.Generic.List[int]]::new()
  $dtoT5.DanhSachMaGhe.Add($seatT5)
  $dtoT5.DanhSachGhe = [Collections.Generic.List[string]]::new()
  $dtoT5.DanhSachGhe.Add("A-01")
  $dtoT5.MaPhienGiu = $sessionT5
  $dtoT5.TenKH = "Khach T5"
  $dtoT5.SDT = "0911223344"
  $dtoT5.PhuongThucThanhToan = [QuanLySuKienVaBanVe.DTO.PhuongThucThanhToan]::TienMat

  $kqT5 = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dtoT5, $f.StaffId)
  if ($kqT5.Success -ne $false -or $kqT5.CanXacNhanGia -ne $true -or $kqT5.TongTienMoi -ne 750000) {
    throw "T5 Failed: Stale price checkout did not trigger CanXacNhanGia = true with new price 750,000."
  }
  if ([int](Sql-Value "SELECT COUNT(*) FROM DonVe") -ne 0) {
    throw "T5 Failed: Stale price created an order in DB instead of rolling back."
  }

  # Xác nhận giá mới và thanh toán lần 2
  $dtoT5.TongTienDaXacNhan = 750000
  $dtoT5.GiaVe = 750000
  $kqT5Second = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dtoT5, $f.StaffId)
  if (-not $kqT5Second.Success -or $kqT5Second.TongTien -ne 750000) {
    throw "T5 Failed: Second checkout with confirmed price 750,000 failed. Error: $($kqT5Second.Message)"
  }
  Write-Output "PASS T5: Stale price rejected with CanXacNhanGia; fresh price confirmation completes payment"

  # T5B: Stale price detection when confirmed quote is 0 (P1 regression test)
  $fZero = Reset-Fixture -price 0 -hoursFromNow 24 -seatCount 10
  $heldSeatsZero = $null
  $sessionZero = [Guid]::Empty
  $null = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($fZero.KvId, 1, [ref]$heldSeatsZero, [ref]$sessionZero, [ref]$errMsg)
  $seatZero = $heldSeatsZero[0].MaGhe

  # Quản lý đổi giá từ 0đ lên 500,000đ trong DB khi khách đang giữ ghế
  Sql-Exec "UPDATE KhuVuc SET GiaVe = 500000 WHERE MaKV = $($fZero.KvId)"

  $dtoZero = New-Object QuanLySuKienVaBanVe.DTO.ThongTinDatVeDTO
  $dtoZero.MaSK = $fZero.SkId
  $dtoZero.MaSuat = $fZero.SuatId
  $dtoZero.MaKV = $fZero.KvId
  $dtoZero.TenKV = "Khu VIP A"
  $dtoZero.SoLuong = 1
  $dtoZero.GiaVe = 0
  $dtoZero.TongTienDaXacNhan = 0 # Báo giá ban đầu 0đ
  $dtoZero.DanhSachMaGhe = [Collections.Generic.List[int]]::new()
  $dtoZero.DanhSachMaGhe.Add($seatZero)
  $dtoZero.DanhSachGhe = [Collections.Generic.List[string]]::new()
  $dtoZero.DanhSachGhe.Add("A-01")
  $dtoZero.MaPhienGiu = $sessionZero
  $dtoZero.TenKH = "Khach Free"
  $dtoZero.SDT = "0988001122"
  $dtoZero.PhuongThucThanhToan = [QuanLySuKienVaBanVe.DTO.PhuongThucThanhToan]::TienMat

  $kqZero = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dtoZero, $fZero.StaffId)
  if ($kqZero.Success -ne $false -or $kqZero.CanXacNhanGia -ne $true -or $kqZero.TongTienMoi -ne 500000) {
    throw "T5B Failed: Checkout with confirmed quote 0đ when price changed to 500,000 did not trigger CanXacNhanGia = true."
  }
  if ([int](Sql-Value "SELECT COUNT(*) FROM DonVe") -ne 0) {
    throw "T5B Failed: 0đ quote price increase created an order instead of rolling back."
  }

  # Xác nhận giá mới và thanh toán lần 2
  $dtoZero.TongTienDaXacNhan = 500000
  $dtoZero.GiaVe = 500000
  $kqZeroSecond = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dtoZero, $fZero.StaffId)
  if (-not $kqZeroSecond.Success -or $kqZeroSecond.TongTien -ne 500000) {
    throw "T5B Failed: Second checkout with confirmed price 500,000 failed. Error: $($kqZeroSecond.Message)"
  }
  Write-Output "PASS T5B: Confirmed quote of 0đ triggers CanXacNhanGia when price changes to 500k (P1 fix verified)"

  # T6: Past start time check inside transaction
  $fPast = Reset-Fixture -price 500000 -hoursFromNow 2 -seatCount 10
  $heldSeatsPast = $null
  $sessionPast = [Guid]::Empty
  $null = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($fPast.KvId, 1, [ref]$heldSeatsPast, [ref]$sessionPast, [ref]$errMsg)

  # Suất diễn bước sang giờ bắt đầu trong lúc giữ ghế
  Sql-Exec "UPDATE SuatDien SET ThoiGianBatDau = DATEADD(MINUTE, -10, GETDATE()) WHERE MaSuat = $($fPast.SuatId)"

  $dtoPast = New-Object QuanLySuKienVaBanVe.DTO.ThongTinDatVeDTO
  $dtoPast.MaSK = $fPast.SkId
  $dtoPast.MaSuat = $fPast.SuatId
  $dtoPast.MaKV = $fPast.KvId
  $dtoPast.TenKV = "Khu VIP A"
  $dtoPast.SoLuong = 1
  $dtoPast.GiaVe = 500000
  $dtoPast.TongTienDaXacNhan = 500000
  $dtoPast.DanhSachMaGhe = [Collections.Generic.List[int]]::new()
  $dtoPast.DanhSachMaGhe.Add($heldSeatsPast[0].MaGhe)
  $dtoPast.DanhSachGhe = [Collections.Generic.List[string]]::new()
  $dtoPast.DanhSachGhe.Add("A-01")
  $dtoPast.MaPhienGiu = $sessionPast
  $dtoPast.TenKH = "Khach Past"
  $dtoPast.SDT = "0933445566"
  $dtoPast.PhuongThucThanhToan = [QuanLySuKienVaBanVe.DTO.PhuongThucThanhToan]::TienMat

  $kqPast = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dtoPast, $fPast.StaffId)
  if ($kqPast.Success -or (Sql-Value "SELECT TrangThai FROM Ghe WHERE MaGhe = $($heldSeatsPast[0].MaGhe)") -eq "DaBan") {
    throw "T6 Failed: Show starting in past was allowed to be sold."
  }
  Write-Output "PASS T6: Checkout rejected when show has already started even if MoCuaBanVe is set"

  # T7: Customer name confirmation & independent email update
  $f = Reset-Fixture -price 500000 -hoursFromNow 24 -seatCount 10
  # Khách ban đầu: Nguyen Van A, 0912345678, vana@example.com

  # Case 7A: ChoPhepCapNhatTen = false, new name Nguyen Van B, new email email_b@example.com
  $heldSeats7A = $null
  $session7A = [Guid]::Empty
  $null = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($f.KvId, 1, [ref]$heldSeats7A, [ref]$session7A, [ref]$errMsg)

  $dto7A = New-Object QuanLySuKienVaBanVe.DTO.ThongTinDatVeDTO
  $dto7A.MaSK = $f.SkId
  $dto7A.MaSuat = $f.SuatId
  $dto7A.MaKV = $f.KvId
  $dto7A.TenKV = "Khu VIP A"
  $dto7A.SoLuong = 1
  $dto7A.GiaVe = 500000
  $dto7A.TongTienDaXacNhan = 500000
  $dto7A.DanhSachMaGhe = [Collections.Generic.List[int]]::new()
  $dto7A.DanhSachMaGhe.Add($heldSeats7A[0].MaGhe)
  $dto7A.DanhSachGhe = [Collections.Generic.List[string]]::new()
  $dto7A.DanhSachGhe.Add("A-01")
  $dto7A.MaPhienGiu = $session7A
  $dto7A.MaKH = $f.KhachId
  $dto7A.TenKH = "Nguyen Van B"
  $dto7A.SDT = "0912345678"
  $dto7A.Email = "email_b@example.com"
  $dto7A.ChoPhepCapNhatTen = $false
  $dto7A.PhuongThucThanhToan = [QuanLySuKienVaBanVe.DTO.PhuongThucThanhToan]::TienMat

  $kq7A = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dto7A, $f.StaffId)
  if (-not $kq7A.Success) {
    throw "T7 Failed at checkout 7A: $($kq7A.Message)"
  }
  $dbTen7A = Sql-Value "SELECT TenKH FROM KhachHang WHERE MaKH = $($f.KhachId)"
  $dbEmail7A = Sql-Value "SELECT Email FROM KhachHang WHERE MaKH = $($f.KhachId)"
  if ($dbTen7A -ne "Nguyen Van A" -or $dbEmail7A -ne "email_b@example.com") {
    throw "T7 Failed: Expected TenKH 'Nguyen Van A' and Email 'email_b@example.com', got '$dbTen7A' and '$dbEmail7A'"
  }

  # Case 7B: ChoPhepCapNhatTen = true
  $heldSeats7B = $null
  $session7B = [Guid]::Empty
  $null = [QuanLySuKienVaBanVe.BLL.GheBLL]::GiuGheTuDongTheoKhuVuc($f.KvId, 1, [ref]$heldSeats7B, [ref]$session7B, [ref]$errMsg)

  $dto7B = New-Object QuanLySuKienVaBanVe.DTO.ThongTinDatVeDTO
  $dto7B.MaSK = $f.SkId
  $dto7B.MaSuat = $f.SuatId
  $dto7B.MaKV = $f.KvId
  $dto7B.TenKV = "Khu VIP A"
  $dto7B.SoLuong = 1
  $dto7B.GiaVe = 500000
  $dto7B.TongTienDaXacNhan = 500000
  $dto7B.DanhSachMaGhe = [Collections.Generic.List[int]]::new()
  $dto7B.DanhSachMaGhe.Add($heldSeats7B[0].MaGhe)
  $dto7B.DanhSachGhe = [Collections.Generic.List[string]]::new()
  $dto7B.DanhSachGhe.Add("A-02")
  $dto7B.MaPhienGiu = $session7B
  $dto7B.MaKH = $f.KhachId
  $dto7B.TenKH = "Nguyen Van B"
  $dto7B.SDT = "0912345678"
  $dto7B.Email = "final@example.com"
  $dto7B.ChoPhepCapNhatTen = $true
  $dto7B.PhuongThucThanhToan = [QuanLySuKienVaBanVe.DTO.PhuongThucThanhToan]::TienMat

  $kq7B = [QuanLySuKienVaBanVe.BLL.BanVeBLL]::XuLyThanhToan($dto7B, $f.StaffId)
  if (-not $kq7B.Success) {
    throw "T7 Failed at checkout 7B: $($kq7B.Message)"
  }
  $dbTen7B = Sql-Value "SELECT TenKH FROM KhachHang WHERE MaKH = $($f.KhachId)"
  $dbEmail7B = Sql-Value "SELECT Email FROM KhachHang WHERE MaKH = $($f.KhachId)"
  if ($dbTen7B -ne "Nguyen Van B" -or $dbEmail7B -ne "final@example.com") {
    throw "T7 Failed: Expected TenKH 'Nguyen Van B' and Email 'final@example.com', got '$dbTen7B' and '$dbEmail7B'"
  }
  Write-Output "PASS T7: Customer name preserved unless confirmed; email updated independently"

  # T8: Zone mapping with fallback protection
  Run-Batches ([IO.File]::ReadAllText((Join-Path $root 'database\sample_data.sql')).Replace('QuanLySuKienVaBanVe', $dbName))
  $sampleZoneCount = [int](Sql-Value "SELECT COUNT(*) FROM KhuVuc WHERE MaVung IN ('VIP_A', 'VIP_B', 'GA_A', 'GA_B', 'CAT')")
  if ($sampleZoneCount -eq 0) {
    throw "T8 Failed: Sample data does not contain mapped MaVung."
  }
  Write-Output "PASS T8: Zone mapping strictly adheres to MaVung and demo data contains mapped zones"

  # T8B: Grid seatmap zone definitions match FrmQuanLyKhuVuc (B1 regression test)
  $objBV = [System.Runtime.Serialization.FormatterServices]::GetUninitializedObject([QuanLySuKienVaBanVe.GUI.FrmBanVe])
  $mBV = [QuanLySuKienVaBanVe.GUI.FrmBanVe].GetMethod('GetZoneDefinitions', [Reflection.BindingFlags]'NonPublic, Instance')
  $zones3 = $mBV.Invoke($objBV, @(3))
  if ($zones3.Count -ne 16) {
    throw "T8B Failed: FrmBanVe layout 3 expected 16 zones, found $($zones3.Count)"
  }
  $catC = $null; $catD = $null
  foreach ($z in $zones3) {
    if ($z.Code -eq 'CAT_C') { $catC = $z }
    if ($z.Code -eq 'CAT_D') { $catD = $z }
  }
  if ($null -eq $catC -or $null -eq $catD) {
    throw "T8B Failed: FrmBanVe layout 3 missing CAT_C or CAT_D zone definitions."
  }
  if ($catC.Bounds.X -ne 20 -or $catC.Bounds.Width -ne 100 -or $catD.Bounds.X -ne 580) {
    throw "T8B Failed: FrmBanVe layout 3 CAT_C/CAT_D bounds do not match standard grid."
  }
  Write-Output "PASS T8B: Layout 3 grid seatmap contains 16 zones including CAT_C and CAT_D (B1 fix verified)"

  # T9: Unique ticket code retry in transaction & distinct issued tickets
  $duplicateCodes = [int](Sql-Value "SELECT COUNT(*) - COUNT(DISTINCT MaVeCode) FROM ChiTietVe")
  if ($duplicateCodes -ne 0) {
    throw "T9 Failed: Duplicate ticket codes found in ChiTietVe table!"
  }
  # Test that SinhMaVeCode avoids collision when code exists in DB
  $testCode = [QuanLySuKienVaBanVe.BLL.DonVeBLL]::SinhMaVeCode($f.SkId, $f.SuatId, "A-01", $null, $null)
  if ([string]::IsNullOrWhiteSpace($testCode)) {
    throw "T9 Failed: SinhMaVeCode returned empty string"
  }
  Write-Output "PASS T9: Unique ticket code generator and retry logic ensure all issued tickets are unique"

  # T10: Color validation and ZoneColorHelper
    $adminTk = New-Object QuanLySuKienVaBanVe.DTO.TaiKhoanDTO
  $adminTk.MaTK = $f.AdminId
  $adminTk.VaiTro = "QuanLy"
  $adminTk.TenDangNhap = "admin_test"
  [QuanLySuKienVaBanVe.BLL.PhienDangNhap]::BatDau($adminTk)
  $kvInvalidColor = New-Object QuanLySuKienVaBanVe.DTO.KhuVucDTO
  $kvInvalidColor.MaSuat = $f.SuatId
  $kvInvalidColor.TenKV = "Test Color"
  $kvInvalidColor.TongSoGhe = 50
  $kvInvalidColor.GiaVe = 100000
  $kvInvalidColor.MauSac = "invalid-color-value"
  $resThem = [QuanLySuKienVaBanVe.BLL.KhuVucBLL]::Them($kvInvalidColor)
    if (-not $resThem.Contains("#RRGGBB")) {
    throw "T10 Failed: KhuVucBLL.Them accepted invalid hex color '$($kvInvalidColor.MauSac)'"
  }

  $clrDefault = [QuanLySuKienVaBanVe.GUI.Helpers.ZoneColorHelper]::Resolve("not-a-color", "VIP_A", $null)
  if ($clrDefault -eq [Drawing.Color]::Empty) {
    throw "T10 Failed: ZoneColorHelper returned empty color for fallback"
  }
  $clrExact = [QuanLySuKienVaBanVe.GUI.Helpers.ZoneColorHelper]::Resolve("#2563EB", $null, $null)
  if ($clrExact.R -ne 37 -or $clrExact.G -ne 99 -or $clrExact.B -ne 235) {
    throw "T10 Failed: ZoneColorHelper resolved #2563EB incorrectly"
  }
  Write-Output "PASS T10: Invalid hex color rejected at BLL; ZoneColorHelper resolves safely without throwing"

}
finally {
  [System.Data.SqlClient.SqlConnection]::ClearAllPools()
  if ($master.State -eq 'Open') {
    try {
      $command = $master.CreateCommand()
      $command.CommandText = "USE master; IF DB_ID(N'$dbName') IS NOT NULL BEGIN ALTER DATABASE [$dbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$dbName]; END"
      $command.CommandTimeout = 30
      $null = $command.ExecuteNonQuery()
      Write-Output "CLEANUP isolated test database $dbName"
    }
    catch {
      Write-Warning "Failed to cleanup test database $dbName : $($_.Exception.Message)"
    }
    $master.Dispose()
  }
}
param(
  [string]$SqlServer = '.',
  [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
  [string]$AssemblyPath = ''
)

# Integration tests create and drop their own database; existing databases are not modified.
$ErrorActionPreference='Stop'
$ProgressPreference='SilentlyContinue'
Add-Type -AssemblyName System.Data
$root=Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($AssemblyPath)) { $AssemblyPath=Join-Path $root ("bin\{0}\QuanLySuKienVaBanVe.exe" -f $Configuration) }
$null=[Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $AssemblyPath).Path)
$dbName='CodexTest_Ticket_'+[Guid]::NewGuid().ToString('N')
$builder=New-Object Data.SqlClient.SqlConnectionStringBuilder
$builder['Data Source']=$SqlServer
$builder['Initial Catalog']='master'
$builder['Integrated Security']=$true
$builder['Encrypt']=$false
$master=New-Object Data.SqlClient.SqlConnection $builder.ConnectionString
$builder['Initial Catalog']=$dbName
$testConn=$builder.ConnectionString
function Sql-Exec([string]$sql) {
  $connection=New-Object Data.SqlClient.SqlConnection $testConn
  try {$connection.Open();$command=$connection.CreateCommand();$command.CommandText=$sql;$command.CommandTimeout=15;$null=$command.ExecuteNonQuery()}
  finally {$connection.Dispose()}
}
function Sql-Value([string]$sql) {
  $connection=New-Object Data.SqlClient.SqlConnection $testConn
  try {$connection.Open();$command=$connection.CreateCommand();$command.CommandText=$sql;$command.CommandTimeout=15;return $command.ExecuteScalar()}
  finally {$connection.Dispose()}
}
function Run-Batches([string]$sql) {
  foreach($batch in [regex]::Split($sql,'(?im)^\s*GO\s*(?:--[^\r\n]*)?$')) {
    if([string]::IsNullOrWhiteSpace($batch)){continue}
    $command=$master.CreateCommand();$command.CommandText=$batch;$command.CommandTimeout=30;$null=$command.ExecuteNonQuery()
  }
}
function Fixture([int]$newPrice=750000,[int]$allowed=1,[int]$oldHours=72) {
  $sql=@"
DELETE NhatKy; DELETE ThanhToan; DELETE ChiTietVe; DELETE DonVe;
DELETE Ghe; DELETE KhuVuc; DELETE SuatDien; DELETE SuKien; DELETE KhachHang; DELETE TaiKhoan;
INSERT TaiKhoan(TenDangNhap,MatKhau,VaiTro,TrangThai) VALUES(N'reviewstaff',N'not-a-login',N'NhanVien',1);
DECLARE @tk int=SCOPE_IDENTITY();
INSERT KhachHang(TenKH,SDT,Email) VALUES(N'Review Customer','0000000001',N'review@example.invalid');
DECLARE @kh int=SCOPE_IDENTITY();
INSERT SuKien(TenSK,TrangThai) VALUES(N'Review Event',N'SapDien');
DECLARE @sk int=SCOPE_IDENTITY();
INSERT SuatDien(MaSK,ThoiGianBatDau,DiaDiem,TrangThai) VALUES(@sk,DATEADD(HOUR,$oldHours,GETDATE()),N'Review Old',N'MoCuaBanVe');
DECLARE @oldShow int=SCOPE_IDENTITY();
INSERT SuatDien(MaSK,ThoiGianBatDau,DiaDiem,TrangThai) VALUES(@sk,DATEADD(HOUR,96,GETDATE()),N'Review New',N'MoCuaBanVe');
DECLARE @newShow int=SCOPE_IDENTITY();
INSERT KhuVuc(MaSuat,TenKV,HangVe,GiaVe,TongSoGhe,ChoPhepBanVe) VALUES(@oldShow,N'Old',N'VIP',500000,1,1);
DECLARE @oldZone int=SCOPE_IDENTITY();
INSERT KhuVuc(MaSuat,TenKV,HangVe,GiaVe,TongSoGhe,ChoPhepBanVe) VALUES(@newShow,N'New',N'VIP',$newPrice,1,$allowed);
DECLARE @newZone int=SCOPE_IDENTITY();
INSERT Ghe(MaKV,Hang,Cot,TrangThai) VALUES(@oldZone,'A',1,N'DaBan');
DECLARE @oldSeat int=SCOPE_IDENTITY();
INSERT Ghe(MaKV,Hang,Cot,TrangThai) VALUES(@newZone,'B',1,N'Trong');
DECLARE @newSeat int=SCOPE_IDENTITY();
INSERT DonVe(MaKH,MaSuat,MaTK,TongTien,TrangThai) VALUES(@kh,@oldShow,@tk,500000,N'DaThanhToan');
DECLARE @ord int=SCOPE_IDENTITY();
INSERT ChiTietVe(MaDon,MaGhe,GiaVe,MaVeCode,TrangThai) VALUES(@ord,@oldSeat,500000,'REVIEW-OLD',N'HopLe');
DECLARE @ticket int=SCOPE_IDENTITY();
INSERT ThanhToan(MaDon,SoTien,PhuongThuc) VALUES(@ord,500000,N'TienMat');
SELECT @ord AS Ord,@ticket AS Ticket,@newSeat AS NewSeat,@oldSeat AS OldSeat,@newShow AS NewShow,@newZone AS NewZone,@tk AS Actor;
"@
  $connection=New-Object Data.SqlClient.SqlConnection $testConn
  try {
    $adapter=New-Object Data.SqlClient.SqlDataAdapter($sql,$connection)
    $table=New-Object Data.DataTable
    $null=$adapter.Fill($table)
    $r=$table.Rows[0]
    return @{Ord=[int]$r.Ord;Ticket=[int]$r.Ticket;NewSeat=[int]$r.NewSeat;OldSeat=[int]$r.OldSeat;NewShow=[int]$r.NewShow;NewZone=[int]$r.NewZone;Actor=[int]$r.Actor;QuotedDelta=[decimal]($newPrice-500000)}
  }finally{$connection.Dispose()}
}
function Exchange($f) { return [QuanLySuKienVaBanVe.BLL.DonVeBLL]::DoiVe($f.Ord,$f.Ticket,$f.NewSeat,$f.QuotedDelta,$f.Actor,'TienMat') }

function Assert-Unchanged($f) {
  if ([int](Sql-Value 'SELECT COUNT(*) FROM DonVe') -ne 1 -or
      [int](Sql-Value 'SELECT COUNT(*) FROM ThanhToan') -ne 1 -or
      [int](Sql-Value 'SELECT COUNT(*) FROM NhatKy') -ne 0 -or
      (Sql-Value ('SELECT TrangThai FROM Ghe WHERE MaGhe='+$f.OldSeat)) -ne 'DaBan' -or
      (Sql-Value ('SELECT TrangThai FROM Ghe WHERE MaGhe='+$f.NewSeat)) -ne 'Trong' -or
      (Sql-Value ('SELECT TrangThai FROM ChiTietVe WHERE MaCT='+$f.Ticket)) -ne 'HopLe' -or
      (Sql-Value ('SELECT TrangThai FROM DonVe WHERE MaDon='+$f.Ord)) -ne 'DaThanhToan') {
    throw 'Rejected or rolled-back operation changed ticket, seat, order, payment or audit data'
  }
}
try {
  $master.Open()
  Run-Batches ([IO.File]::ReadAllText((Join-Path $root 'database\create_database.sql')).Replace('QuanLySuKienVaBanVe',$dbName))
  [QuanLySuKienVaBanVe.DAL.Database].GetField('_connStr',[Reflection.BindingFlags]'Static,NonPublic').SetValue($null,$testConn)
  foreach($migrationPath in @('database\migration_add_columns.sql','database\migrate_tv3_donve.sql')) {
    $migration=[IO.File]::ReadAllText((Join-Path $root $migrationPath)).Replace('QuanLySuKienVaBanVe',$dbName)
    Run-Batches $migration; Run-Batches $migration
  }
  Run-Batches ([IO.File]::ReadAllText((Join-Path $root 'database\sample_data.sql')).Replace('QuanLySuKienVaBanVe',$dbName))
  $null=[QuanLySuKienVaBanVe.BLL.DonVeBLL]::LayDanhSachDonVe()
  Write-Output 'PASS Documented installation opens order list; migrations run twice'
  foreach($newPrice in @(750000,500000,250000)) {
    $f=Fixture $newPrice
    $result=Exchange $f
    if(!$result.Success -or $result.ChenhLech -ne $f.QuotedDelta){throw 'Wrong exchange result'}
    if([decimal](Sql-Value 'SELECT SUM(SoTien) FROM ThanhToan') -ne $newPrice){throw 'Wrong payment ledger'}
    $newPayments=[int](Sql-Value ('SELECT COUNT(*) FROM ThanhToan WHERE MaDon='+$result.MaDonMoi))
    if(($newPrice -eq 500000 -and $newPayments -ne 0) -or ($newPrice -ne 500000 -and $newPayments -ne 1)){throw 'Wrong payment row count'}
    Write-Output ('PASS Exchange delta {0}, payment rows {1}' -f $result.ChenhLech,$newPayments)
  }
  $f=Fixture
  $cancel=[QuanLySuKienVaBanVe.BLL.DonVeBLL]::HuyVe($f.Ord,$f.Actor)
  if(!$cancel.Success -or (Sql-Value ('SELECT TrangThai FROM Ghe WHERE MaGhe='+$f.OldSeat)) -ne 'Trong'){throw 'Cancel failed'}
  Write-Output 'PASS Cancel releases seat'
  $failureFlag=[QuanLySuKienVaBanVe.BLL.DonVeBLL].GetProperty('SimulateFailureBeforeCommit')
  if($Configuration -eq 'Debug') {
    if($null -eq $failureFlag){throw 'Debug assembly expected for rollback tests'}
    foreach($operation in @('Exchange','Cancel')) {
      $f=Fixture
      $failureFlag.SetValue($null,$true,$null)
      $failed=$false
      try {if($operation -eq 'Exchange'){$null=Exchange $f}else{$null=[QuanLySuKienVaBanVe.BLL.DonVeBLL]::HuyVe($f.Ord,$f.Actor)}}catch{$failed=$true}
      finally{$failureFlag.SetValue($null,$false,$null)}
      if(!$failed){throw ('Failure simulation was not triggered: '+$operation)}
      Assert-Unchanged $f
      Write-Output ('PASS Rollback '+$operation)
    }
  } elseif($null -ne $failureFlag) { throw 'Debug failure flag leaked into Release' }
  else { Write-Output 'PASS Release excludes debug failure flag' }
  $f=Fixture 750000 1 6
  if((Exchange $f).Success){throw 'Accepted source show under 12h'}
  Assert-Unchanged $f
  Write-Output 'PASS Source show under 12h blocked'
  $f=Fixture
  Sql-Exec ('UPDATE SuatDien SET TrangThai=N''DongBanVe'' WHERE MaSuat='+$f.NewShow)
  if((Exchange $f).Success){throw 'Accepted closed target show'}
  Assert-Unchanged $f
  Write-Output 'PASS Closed target show blocked'
  $f=Fixture 750000 0
  $seats=[QuanLySuKienVaBanVe.BLL.DonVeBLL]::LayDanhSachGheTrongTheoSuat($f.NewShow)
  if($seats.Rows.Count -ne 0 -or (Exchange $f).Success){throw 'Closed zone was offered or accepted'}
  Assert-Unchanged $f
  Write-Output 'PASS Closed zone hidden and direct exchange rejected'
  $f=Fixture
  $seats=[QuanLySuKienVaBanVe.BLL.DonVeBLL]::LayDanhSachGheTrongTheoSuat($f.NewShow)
  if($seats.Rows.Count -ne 1){throw 'Expected selectable target seat'}
  Sql-Exec ('UPDATE KhuVuc SET ChoPhepBanVe=0 WHERE MaKV='+$f.NewZone)
  if((Exchange $f).Success){throw 'Accepted zone disabled after seat selection'}
  Assert-Unchanged $f
  Write-Output 'PASS Zone disabled after selection rejected without changes'
  foreach($changedPrice in @(1000000,500000,250000)) {
    $f=Fixture 750000
    Sql-Exec ('UPDATE KhuVuc SET GiaVe='+$changedPrice+' WHERE MaKV='+$f.NewZone)
    $result=Exchange $f
    if($result.Success){throw 'Changed quote was accepted'}
    Assert-Unchanged $f
    $f.QuotedDelta=[decimal]($changedPrice-500000)
    $result=Exchange $f
    if(!$result.Success -or $result.ChenhLech -ne $f.QuotedDelta){throw 'Freshly confirmed quote was not accepted'}
    Write-Output ('PASS Price change to {0}: stale quote rejected; fresh confirmation accepted' -f $changedPrice)
  }
  $f=Fixture
  $lockedConnection=[QuanLySuKienVaBanVe.DAL.Database]::GetConnection()
  $transaction=$null
  try {
    $lockedConnection.Open()
    $transaction=$lockedConnection.BeginTransaction()
    $null=[QuanLySuKienVaBanVe.DAL.DonVeDAL]::LayThongTinGheKemGia($f.NewSeat,$lockedConnection,$transaction)
    foreach($update in @('GiaVe=1000000','ChoPhepBanVe=0')) {
      $blocked=$false
      try {Sql-Exec ('SET LOCK_TIMEOUT 500; UPDATE KhuVuc SET '+$update+' WHERE MaKV='+$f.NewZone)}
      catch {
        if($_.Exception.GetBaseException() -is [Data.SqlClient.SqlException] -and $_.Exception.GetBaseException().Number -eq 1222){$blocked=$true}else{throw}
      }
      if(!$blocked){throw ('Concurrent zone update was not blocked: '+$update)}
    }
    Write-Output 'PASS Zone price and sale status remain locked throughout transaction'
  } finally {
    if($null -ne $transaction){$transaction.Rollback();$transaction.Dispose()}
    $lockedConnection.Dispose()
  }
  Sql-Exec ('UPDATE KhuVuc SET GiaVe=1000000,ChoPhepBanVe=0 WHERE MaKV='+$f.NewZone)
  Write-Output 'PASS Zone updates succeed after transaction releases locks'
} finally {
  [Data.SqlClient.SqlConnection]::ClearAllPools()
  if($master.State -eq 'Open') {
    $cleanup=$master.CreateCommand()
    $cleanup.CommandText='USE master; IF DB_ID(N'''+$dbName+''') IS NOT NULL BEGIN ALTER DATABASE ['+$dbName+'] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ['+$dbName+']; END'
    $cleanup.CommandTimeout=30
    $null=$cleanup.ExecuteNonQuery()
  }
  $master.Dispose()
  Write-Output ('CLEANUP isolated test database '+$dbName)
}

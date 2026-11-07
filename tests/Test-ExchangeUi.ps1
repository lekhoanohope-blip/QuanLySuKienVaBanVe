param(
    [string]$AssemblyPath = (Join-Path $PSScriptRoot '..\bin\Debug\QuanLySuKienVaBanVe.exe'),
    [string]$OutputDirectory = (Join-Path ([IO.Path]::GetTempPath()) ('EventTicketUi-' + [Guid]::NewGuid().ToString('N')))
)
# Run with Windows PowerShell -STA. Uses in-memory fixtures; produces preview PNGs.
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms,System.Drawing,System.Data
$assemblyFile = (Resolve-Path -LiteralPath $AssemblyPath).Path
$assemblyDirectory = Split-Path -Parent $assemblyFile
$null = [Reflection.Assembly]::LoadFrom((Join-Path $assemblyDirectory 'Guna.UI2.dll'))
$null = [Reflection.Assembly]::LoadFrom($assemblyFile)
$null = New-Item -ItemType Directory -Path $OutputDirectory -Force
$sourceFile = Join-Path $PSScriptRoot 'ExchangeUiCheck.cs'
$compiler = Join-Path ([Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()) 'csc.exe'
# Relative paths avoid legacy compiler argument handling of Windows drive prefixes.
Push-Location -LiteralPath $OutputDirectory
try {
    Copy-Item -LiteralPath $sourceFile -Destination 'ExchangeUiCheck.cs'
    Copy-Item -LiteralPath $assemblyFile -Destination 'Application.exe'
    & $compiler /nologo /target:library /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll /r:System.Core.dll /r:Application.exe /out:ExchangeUiCheck.dll ExchangeUiCheck.cs
    if ($LASTEXITCODE -ne 0) { throw 'UI test harness compilation failed.' }
    $null = [Reflection.Assembly]::LoadFrom((Join-Path (Get-Location).Path 'ExchangeUiCheck.dll'))
    [ExchangeUiCheck]::Run((Get-Location).Path)
    Write-Output ('Previews: ' + (Get-Location).Path)
} finally { Pop-Location }

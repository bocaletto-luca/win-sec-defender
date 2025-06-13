# File: scripts/SignService.ps1
param(
  [string]$ExePath     = "bin\Release\net6.0\Defender.Service.exe",
  [string]$PfxPath     = "certs\code-sign.pfx",
  [string]$PfxPassword = "<YourPfxPassword>"
)
& signtool sign /fd SHA256 /a /f $PfxPath /p $PfxPassword `
    /tr http://timestamp.digicert.com /td SHA256 $ExePath

# Win-Sec-Defender

**Win-Sec-Defender** is an open-source Windows Security Defender Service:

- Runs as background **Windows Service**  
- Schedules security checks via **Quartz.NET** cron  
- Monitors Event Logs, processes, ports, file‐integrity…  
- Sends structured email alerts via SMTP  
- Logs to file and console with **Serilog**  
- Configurable via `appsettings.json`  

## ▶️ Install

1. Build: `dotnet publish -c Release -o C:\Defender`  
2. Register service (Admin PowerShell):  

##### New-Service -Name "WinSecDefender" `-BinaryPathName "C:\Defender\Defender.Service.exe" ` -StartupType Automatic
  
##### Set-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Services\WinSecDefender\ -Name FailureActions -Value 1

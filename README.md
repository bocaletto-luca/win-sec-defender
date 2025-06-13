# Defender-Service
#### Author: Bocaletto Luca

Defender-Service is a .NET 6 Windows security daemon that auto-installs as a Windows Service under LocalService. It schedules security checks (processes, Event Log), stores SMTP creds securely in Credential Manager, exposes `/healthz` and Prometheus `/metrics`, logs to file & Event Log, and sends alert emails with auto-restart on failure.

[![Build](https://github.com/yourorg/defender-service/actions/workflows/ci.yml/badge.svg)](https://github.com/yourorg/defender-service/actions) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Features

- Windows Service (auto-start, recovery policy)  
- Scheduled security checks (Quartz.NET cron)  
- Secure SMTP creds via Windows Credential Manager  
- Health endpoint (`/healthz`) & Prometheus metrics (`/metrics`)  
- Structured logging: file + Windows Event Log (Serilog)  
- Alert notifications via email  

## Prerequisites

- .NET 6 SDK  
- Windows 10+ / Server 2016+  
- Administrative rights for install  
- SMTP server  

## Installation

1. **Build & Publish**
   ```powershell
   dotnet publish -c Release -o C:\DefenderService
   ```
2. **Install MSI** (requires WiX-generated `DefenderService.msi`):
   ```powershell
   msiexec /i DefenderService.msi /qn
   ```

## Configuration

Edit `C:\Program Files\DefenderService\appsettings.json`:

```jsonc
{
  "Defender": {
    "CronSchedule": "0 0/30 * * * ?",
    "ProcessesToWatch": ["powershell", "wmic"],
    "Email": {
      "SmtpHost": "...",
      "SmtpPort": 587,
      "Username": "alert@domain.com",
      "CredentialTarget": "DefenderServiceMail",
      "From": "alert@domain.com",
      "To": "ops@domain.com"
    }
  }
}
```

1. Store SMTP password in Windows Credential Manager:
   ```powershell
   cmdkey /add:DefenderServiceMail /user:alert@domain.com /pass:SuperSecret
   ```

## Service Management

```powershell
# Start service
Start-Service DefenderService

# Stop service
Stop-Service DefenderService

# View logs
Get-Content "C:\ProgramData\DefenderService\logs\defender-*.log" -Tail 50
```

## Health & Metrics

- Health probe: `http://<host>:8000/healthz`  
- Prometheus metrics: `http://<host>:8000/metrics`  

## Contributing

Pull requests welcome! See [CONTRIBUTING.md](CONTRIBUTING.md).

## License
#### MIT

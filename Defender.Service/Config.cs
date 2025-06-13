// File: Defender.Service/Config.cs
using System;
using CredentialManagement;

namespace Defender.Service
{
    public class EmailConfig
    {
        public string SmtpHost         { get; set; } = "";
        public int    SmtpPort         { get; set; }
        public string Username         { get; set; } = "";
        public string CredentialTarget { get; set; } = "DefenderServiceMail"; 
        public string From             { get; set; } = "";
        public string To               { get; set; } = "";   // comma-separated

        public string Password
        {
            get
            {
                using var cred = new Credential { Target = CredentialTarget };
                if (!cred.Load())
                    throw new InvalidOperationException($"Credential '{CredentialTarget}' not found");
                return cred.Password;
            }
        }
    }

    public class DefenderConfig
    {
        public string   CronSchedule     { get; set; } = "0 0/30 * * * ?";
        public string[] ProcessesToWatch { get; set; } = Array.Empty<string>();
        public EmailConfig Email         { get; set; } = new();
    }
}

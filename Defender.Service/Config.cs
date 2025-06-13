// Defender.Service/Config.cs
public class EmailConfig
{
    public string SmtpHost { get; set; } = "";
    public int    SmtpPort { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string From     { get; set; } = "";
    public string To       { get; set; } = ""; // comma-sep
}

public class DefenderConfig
{
    public string          CronSchedule { get; set; } = "0 0/30 * * * ?"; 
    public EmailConfig     Email        { get; set; } = new();
    public List<string>    ProcessesToWatch { get; set; } = new();
    // … altro: paths, ports, event IDs…
}

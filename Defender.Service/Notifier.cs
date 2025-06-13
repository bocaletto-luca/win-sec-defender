// Defender.Service/Notifier.cs
using System.Net;
using System.Net.Mail;

public static class Notifier
{
    public static Task SendAsync(EmailConfig e, string body)
    {
        using var msg = new MailMessage();
        msg.From = new MailAddress(e.From);
        foreach (var addr in e.To.Split(',')) msg.To.Add(addr.Trim());
        msg.Subject = "[Defender] Security Report";
        msg.Body = body;

        using var cli = new SmtpClient(e.SmtpHost, e.SmtpPort)
        {
            Credentials = new NetworkCredential(e.Username, e.Password),
            EnableSsl = true
        };
        return cli.SendMailAsync(msg);
    }
}

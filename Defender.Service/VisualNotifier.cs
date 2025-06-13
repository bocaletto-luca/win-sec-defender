// VisualNotifier.cs
using Microsoft.Toolkit.Uwp.Notifications;  // NuGet: CommunityToolkit.WinUI.Notifications

public static class VisualNotifier
{
    public static void Toast(string title, string message)
    {
        new ToastContentBuilder()
          .AddText(title)
          .AddText(message)
          .Show();
    }

    public static void TrayBalloon(string title, string message)
    {
        var icon = new System.Windows.Forms.NotifyIcon {
            Icon = System.Drawing.SystemIcons.Information,
            Visible = true
        };
        icon.ShowBalloonTip(5000, title, message, System.Windows.Forms.ToolTipIcon.Info);
        // cleanup after a bit
        Task.Delay(6000).ContinueWith(_ => icon.Dispose());
    }
}

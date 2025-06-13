// File: Defender.Service/VisualNotifier.cs
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;  // ToastContentBuilder

namespace Defender.Service
{
    public static class VisualNotifier
    {
        public static void ShowToast(string title, string message)
        {
            // Windows 10+ Toast
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }

        public static void ShowTrayBalloon(string title, string message)
        {
            Task.Run(() =>
            {
                using var icon = new NotifyIcon
                {
                    Icon = System.Drawing.SystemIcons.Application,
                    Visible = true,
                    BalloonTipTitle = title,
                    BalloonTipText = message,
                    BalloonTipIcon = ToolTipIcon.Info
                };
                icon.ShowBalloonTip(5000);
                // keep alive briefly so the balloon is shown
                System.Threading.Thread.Sleep(6000);
            });
        }
    }
}

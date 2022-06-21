using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;

namespace Yeetnite_Launcher
{
    internal class Toast
    {
        public static void Show(string title, string[] lines)
        {
            if (lines.Length > 3) Debug.WriteLine("CustomToastClassError: Reached maximum 4 lines in toast notification");
            ToastContentBuilder toast = new ToastContentBuilder().AddText(title);

            for (int i = 0; i < lines.Length; i++)
                toast.AddText(lines[i]);

            toast.Show();
        }

        public static void ShowSuccess(string content)
        {
            Show("Success", new[] { content });
        }

        public static void ShowInfo(string content)
        {
            Show("Info", new[] { content });
        }

        public static void ShowWarning(string content)
        {
            Show("Warning", new[] { content });
        }

        public static void ShowError(string content)
        {
            Show("Error", new[] { content });
        }
    }
}

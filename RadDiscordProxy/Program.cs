#region

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DiscordRPC;
using RadLibrary;
using RadLibrary.Logging;
using static RadDiscordProxy.Storage;

#endregion

namespace RadDiscordProxy
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void Main(string[] args)
        {
            LoggerUtils.PrintSystemInformation();
            LoggerUtils.RegisterExceptionHandler();

            if (Config.GetBool("hideConsole") && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ShowWindow(GetConsoleWindow(), 0);

            var client = new DiscordRpcClient(Config["applicationId"]);

            client.Initialize();

            if (Config.GetBool("enableInviteFeature"))
            {
                client.RegisterUriScheme();

                client.Subscribe(EventType.JoinRequest);
                client.Subscribe(EventType.Join);

                client.OnJoinRequested += (o, message) =>
                {
                    NLogger.Warn("Join requested");

                    if (!Config.GetBool("enableJoinRequestSounds"))
                        return;

                    Console.Beep();
                    Thread.Sleep(500);
                    Console.Beep();
                    Thread.Sleep(500);
                    Console.Beep();
                };
                client.OnJoin += (o, message) =>
                {
                    var url = "https://music.youtube.com/watch?v=" + message.Secret;
                    NLogger.Warn($"Joined {url}");

                    if (Config.GetBool("enableAutoUrlOpen"))
                    {
                        // process.start won't work on .net core 3
                        var psi = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };
                        Process.Start(psi);

                        NLogger.Info($"Opened in default browser! ({url})");
                    }
                    else
                    {
                        Console.Beep();
                        NLogger.Info("Your link: {0}", url);
                    }
                };
            }

            Proxy.Client = client;
            Proxy.Start();

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                client.Dispose();
                Proxy.Stop();
            };

            Utilities.InfiniteWait();
        }
    }
}
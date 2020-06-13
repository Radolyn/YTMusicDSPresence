#region

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using DiscordRPC;
using RadLibrary;
using RadLibrary.Logging;
using RadLibrary.Logging.Loggers;
using static RadDiscordProxy.Storage;

#endregion

namespace RadDiscordProxy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogManager.AddExceptionsHandler();
            
            Environment.CurrentDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? throw new Exception("Failed to set environment directory.");
            
            Utilities.OnlyOneInstance("DiscordYTMusicPresence", () =>
            {
                var current = Process.GetCurrentProcess().ProcessName;
                var processes = Process.GetProcessesByName(current);

                int pid;

                if (processes.Length == 0)
                    pid = 0;
                else
                    pid = processes[0]?.Id ?? 0;

                LogManager.GetLogger<ConsoleLogger>("DiscordPresence").Fatal("The program is already running (PID: {0})", pid);
                Console.ReadLine();
            });

            if (Config.GetBool("hideConsole") && Utilities.IsWindows())
                Utilities.AllocateConsole();

            var client = new DiscordRpcClient(Config["applicationId"]);

            client.Initialize();

            if (Config.GetBool("enableInviteFeature"))
            {
                var inviteLogger = LogManager.GetLogger<ConsoleLogger>("InviteFeature");
                
                client.RegisterUriScheme();

                client.Subscribe(EventType.JoinRequest);
                client.Subscribe(EventType.Join);

                client.OnJoinRequested += (o, message) =>
                {
                    inviteLogger.Warn("Join requested");

                    if (!Config.GetBool("enableJoinRequestSounds"))
                        return;

                    Console.Beep(1000, 100);
                    Console.Beep(1200, 100);
                    Console.Beep(1400, 100);
                };
                client.OnJoin += (o, message) =>
                {
                    var url = "https://music.youtube.com/watch?v=" + message.Secret;
                    inviteLogger.Warn($"Joined {url}");

                    if (Config.GetBool("enableAutoUrlOpen"))
                    {
                        // process.start won't work on .net core 3
                        var psi = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };
                        Process.Start(psi);

                        inviteLogger.Info($"Opened in default browser! ({url})");
                    }
                    else
                    {
                        Console.Beep();
                        inviteLogger.Info("Your link: {0}", url);
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
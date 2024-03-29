﻿#region

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPC;
using RadLibrary.Logging;
using RadLibrary.Logging.Loggers;
using static RadDiscordProxy.Storage;

#endregion

#pragma warning disable 4014

namespace RadDiscordProxy
{
    public static class Proxy
    {
        private static HttpListener _server;
        public static DiscordRpcClient Client;
        private static bool _stop;

        public static void Start()
        {
            var logger = LogManager.GetClassLogger();

            logger.Info("Starting HTTP listener...");

            _server = new HttpListener();

            _server.Prefixes.Add($"http://localhost:{Config["port"]}/presence/");

            _server.Start();

            Task.Run(Work);

            logger.Info("Ok");
        }

        private static async Task Work()
        {
            var classLogger = LogManager.GetClassLogger();

            classLogger.Info("Starting worker...");

            var logger = LogManager.GetLogger<ConsoleLogger>("Worker");

            while (!_stop)
                try
                {
                    var token = new CancellationTokenSource();

                    Task.Run(() =>
                    {
                        Thread.Sleep(Config["timeout"].ValueAs<int>());
                        if (!token.IsCancellationRequested)
                            Client.ClearPresence();
                    }, token.Token);

                    var context = await _server.GetContextAsync();

                    token.Cancel();

                    var request = context.Request;

                    var raw = await new StreamReader(request.InputStream).ReadToEndAsync();

                    var data = Data.FromJson(raw);

                    data.Artist = data.Artist.LimitLength(90);
                    data.Song = data.Song.LimitLength(90);

                    logger.Info(raw);

                    SetPresence(data);

                    var buff = Encoding.UTF8.GetBytes("OK (RadDiscordProxy)");
                    // ReSharper disable once MethodSupportsCancellation
                    await context.Response.OutputStream.WriteAsync(buff, 0, buff.Length);
                    context.Response.OutputStream.Close();
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
        }

        public static void Stop()
        {
            _stop = true;
            _server.Abort();
        }

        private static void SetPresence(Data data)
        {
            var activity = new RichPresence
            {
                State = data.Paused ? null : data.Song + " by " + data.Artist,
                Details = data.Paused ? Config["pausedText"].Value : Config["playingText"].Value,
                Assets = new Assets
                {
                    LargeImageKey = Config["largeImageKey"].Value,
                    LargeImageText = Config["largeImageText"].Value,
                    SmallImageKey = data.Paused ? Config["paused"].Value : Config["playing"].Value,
                    SmallImageText = data.Paused ? Config["pausedText"].Value : Config["playingText"].Value
                },
                Party = new Party
                {
                    ID = Convert.ToBase64String(Encoding.UTF8.GetBytes(data.Id)),
                    Max = 1338,
                    Size = 1
                }
            };

            if (!data.Paused)
                activity.Timestamps = new Timestamps
                {
                    StartUnixMilliseconds = data.Start,
                    EndUnixMilliseconds = data.End
                };

            if (!data.Paused && Config["enableInviteFeature"].ValueAs<bool>())
                activity.Secrets = new Secrets
                {
                    JoinSecret = data.Id
                };

            Client.SetPresence(activity);
        }

        private static string LimitLength(this string source, int maxLength)
        {
            if (source.Length <= maxLength) return source;

            return source.Substring(0, maxLength);
        }
    }
}

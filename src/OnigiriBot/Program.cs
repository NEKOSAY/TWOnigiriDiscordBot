using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Realms;
using Realms.Sync;
using System.Text.Json;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using OnigiriBot.Services;
using HtmlAgilityPack;
using System.Net;

namespace OnigiriBot
{
    class Program
    {
        public const string AppName = "BOT";
        public const string AppVersion = "1.0.0";
        public const string AppPrefix = "o!";

        private DiscordSocketClient _discordClient;
        private FirebaseClient _firebaseClient;
        private Connection _currentConnection;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        async Task MainAsync()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            using var services = ConfigureServices();
            _currentConnection = services.GetRequiredService<Connection>();
            _firebaseClient = services.GetRequiredService<FirebaseClient>();
            _discordClient = services.GetRequiredService<DiscordSocketClient>();
            services.GetRequiredService<CommandService>().Log += log => Task.Factory.StartNew(() => {
                Console.WriteLine(log.ToString(null, true, true, DateTimeKind.Utc));
            });
            _discordClient.Log += log => Task.Factory.StartNew(() => { 
                Console.WriteLine(log.ToString(null, true, true, DateTimeKind.Utc));
            });
            _discordClient.Ready += () => Task.Factory.StartNew(async () => { 
                Console.WriteLine($"{_discordClient.CurrentUser} is connected!");
                await _firebaseClient.Child("connectionHistory").PostAsync(_currentConnection);
                await _firebaseClient.Child("connection").PutAsync(_currentConnection);
                _firebaseClient.Child("connection").AsObservable<dynamic>().Subscribe(async (o) =>
                {
                    if (o.Key == nameof(Connection.LastUpdatedTime))
                    {
                        if ((DateTime)o.Object > _currentConnection.LastUpdatedTime)
                        {
                            Console.WriteLine("new client detected,ready to close.");
                            await _discordClient.LogoutAsync();
                            _firebaseClient.Dispose();
                            Environment.Exit(0);
                        }
                    }
                });
            });
            await _discordClient.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("OnigiriBotToken"));
            await _discordClient.StartAsync();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<FirebaseClient>(_ => 
                    new FirebaseClient(Environment.GetEnvironmentVariable("OnigiriBotFirebaseUrl"), 
                    new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(Environment.GetEnvironmentVariable("OnigiriBotFirebaseSecret")) }))
                .AddSingleton<App>(_ => App.Create(
                    new AppConfiguration(Environment.GetEnvironmentVariable("OnigiriBotRealmsAppId")) { LocalAppName = AppName, LocalAppVersion = AppVersion }))
                .AddSingleton<Realms.Sync.User>(_ => _.GetRequiredService<App>().LogInAsync(Credentials.ApiKey(Environment.GetEnvironmentVariable("OnigiriBotRealmsAppKey"))).Result)
                .AddSingleton<MongoClient>(_ => _.GetRequiredService<Realms.Sync.User>().GetMongoClient(Environment.GetEnvironmentVariable("OnigiriBotMongoClientServiceName")))
                .AddSingleton<MongoClient.Database>(_ => _.GetRequiredService<MongoClient>().GetDatabase("onigiri"))
                .AddSingleton<MongoClient.Collection<User>>(_ => _.GetRequiredService<MongoClient.Database>().GetCollection<User>("user"))
                .AddSingleton<MongoClient.Collection<Item>>(_ => _.GetRequiredService<MongoClient.Database>().GetCollection<Item>("item"))
                .AddSingleton<MongoClient.Collection<Trace>>(_ => _.GetRequiredService<MongoClient.Database>().GetCollection<Trace>("trace"))
                .AddSingleton<Connection>(_ => new Connection() { LastUpdatedTime = DateTime.Now, IPAddress = Helper.GetLocalIPAddress()})
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton <HtmlWeb>()
                .AddSingleton<OnigiriService>()
                .BuildServiceProvider();
        }
    }
}

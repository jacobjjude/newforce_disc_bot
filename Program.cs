using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using main_bot.Commands;

namespace main_bot
{
    class Program
    {
        private static DiscordSocketClient client; // Class-level declaration

        public static Task Main(string[] args)
            => new Program().MainAsync();
        
        public async Task MainAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages
            }); // Updated 'client' initialization with 'GatewayIntents'
            var commandHandler = new CommandHandler(client); // Create an instance of CommandHandler with the 'client' argument


            client.Log += LogAsync;
            client.Ready += ReadyAsync;

            await commandHandler.InstallCommandsAsync(); // Call InstallCommandsAsync() on the instance

            Console.WriteLine(Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN")); // For debugging purposes only, remove after verification
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"));
            await client.StartAsync();

            var channel = client.GetChannel(1087402184859197613) as SocketGuildChannel; // Cast to SocketGuildChannel to access more properties
            if(channel != null)
            {
                Console.WriteLine($"Channel found: {channel.Name}");
            }
            else
            {
                Console.WriteLine("Channel not found.");
            }

            // Block this task until the program is closed
            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> [{client.CurrentUser}]");
            return Task.CompletedTask;
        }
    }
}

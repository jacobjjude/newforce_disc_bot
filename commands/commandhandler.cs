using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace main_bot.Commands
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _commands = new CommandService();

            _client.MessageReceived += HandleCommandAsync;
        }

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);

            // Since we cannot get a count directly, just log a confirmation for now.
            Console.WriteLine("Command modules have been loaded.");
        }



        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            Console.WriteLine($"Message received: {messageParam.Content}"); // Add this line for debugging
            

            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            //Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('.', ref argPos) || 
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Execute the command if it was triggered
            var context = new SocketCommandContext(_client, message);
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);

            var result = await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error executing command: {result.ErrorReason}");
            }
        }
    }
}
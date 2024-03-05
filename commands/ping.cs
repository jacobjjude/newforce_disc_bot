using Discord.Commands;
using System.Threading.Tasks;

namespace main_bot.Commands
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong!");
        }
    }
}
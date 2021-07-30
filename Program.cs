using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DISCORD_BOT;

namespace FNAccountlvlBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            new CommandHandler(_client);

            var token = File.ReadAllText(Directory.GetCurrentDirectory().ToString() + "/token.txt");

            await _client.LoginAsync(TokenType.Bot, token); //Token goes here!

            await _client.StartAsync();

            _handler = new CommandHandler(_client);

            await Task.Delay(-1);
        }
    }
}

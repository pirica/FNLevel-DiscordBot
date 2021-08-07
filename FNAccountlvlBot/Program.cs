using Discord;
using Discord.WebSocket;
using DISCORD_BOT;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FNAccountlvlBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - Initialize application...");
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            new CommandHandler(_client);

            if (File.Exists(Directory.GetCurrentDirectory() + "/linked.txt") == false)
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + "/linked.txt", "{");
            }

            if (File.Exists(Directory.GetCurrentDirectory().ToString() + "/token.txt"))
            {
                var token = File.ReadAllText(Directory.GetCurrentDirectory().ToString() + "/token.txt");

                await _client.LoginAsync(TokenType.Bot, token); //Token goes here!
            }
            else
            {
                throw new System.Exception("Failed to find a token in " + Directory.GetCurrentDirectory().ToString() + @"\Bot" + ". Please create token.txt in this directory.");
            }

            await _client.SetActivityAsync(new Game("with levels", ActivityType.Playing));

            await _client.StartAsync();

            _handler = new CommandHandler(_client);

            await Task.Delay(-1);
        }
    }
}

using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Fortnite_API;
using Fortnite_API.Objects.V1;

namespace DISCORD_BOT
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;

        private CommandService _service;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _service = new CommandService();

            _client.MessageReceived += HandleCommandAsynce;
        }

        private async Task HandleCommandAsynce(SocketMessage s)
        {
            var msg = s as SocketUserMessage;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;

            var isbot = msg.Author.IsBot.ToString();

            var api = new FortniteApiClient();

            if (msg.HasStringPrefix("!lvl", ref argPos) && isbot == "False")
            {
                var account = msg.ToString().Remove(0, 5);

                var level = await api.V1.Stats.GetBrV2Async(x =>
                {
                    x.Name = account;
                    x.ImagePlatform = BrStatsV2V1ImagePlatform.All;
                });

                if (level.IsSuccess)
                {
                    var sendmsg = await context.Channel.SendMessageAsync(level.Data.BattlePass.Level.ToString());
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
                else
                {
                    var sendmsg = await context.Channel.SendMessageAsync("Account '" + account + "' is invalid. Please check your spelling");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
            }
        }
    }
}

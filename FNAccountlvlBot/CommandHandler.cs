using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Fortnite_API;
using Fortnite_API.Objects.V1;
using System.Reflection;
using Discord;
using System.IO;
using System;

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

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;

            var isbot = msg.Author.IsBot.ToString();

            var api = new FortniteApiClient();

            if (msg.HasStringPrefix("!lvl", ref argPos) && isbot == "False" && msg.ToString().Contains("-byID") == false)
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !lvl command... responding:");

                var account = msg.ToString().Remove(0, 5);

                var level = await api.V1.Stats.GetBrV2Async(x =>
                {
                    x.Name = account;
                    x.ImagePlatform = BrStatsV2V1ImagePlatform.All;
                });

                if (level.IsSuccess)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - the given account existed.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", Account '**" + account + "**' is level **" + level.Data.BattlePass.Level.ToString() + "**. " +
                        "Account ID: **" + level.Data.Account.Id.ToString() + "**.");

                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - the given account did not exist.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", account '**" + account + "**' is invalid. Please check your spelling. If you are using an ID, please type !lvl-byID + {ACCOUNT ID}");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
            }
            else if (msg.HasStringPrefix("!lvl-byID", ref argPos) && isbot == "False")
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !lvl-byID command... responding:");

                var ID = msg.ToString().Remove(0, 10);

                var level = await api.V1.Stats.GetBrV2Async(x =>
                {
                    x.AccountId = ID;
                    x.ImagePlatform = BrStatsV2V1ImagePlatform.All;
                });

                if (level.IsSuccess)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - the given account existed.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", Account '**" + ID + "**' is level **" + level.Data.BattlePass.Level.ToString() + "**. " +
                        "Account name: **" + level.Data.Account.Name.ToString() + "**.");

                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - the given account did not exist.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", account '**" + ID + "**' is invalid. Please check your spelling. If you are using a name, please type !lvl + {ACCOUNT NAME}");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
            }
            else if (msg.HasStringPrefix("!bot-ver", ref argPos) && isbot == "False")
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !bot-ver command... responding");

                var AppVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", this is version **" + AppVer + "** of FNAccountlvlBot.exe");
                var etc = await context.Channel.SendMessageAsync("TEMP");
                await context.Channel.DeleteMessageAsync(etc);
                await context.Channel.DeleteMessageAsync(etc);
                await context.Channel.DeleteMessageAsync(sendmsg);
            }
            else if (msg.HasStringPrefix("!link", ref argPos) && isbot == "False")
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !link command... responding:");

                var user = context.Message.Author.ToString();

                var searched = "Not linked with discord!!";

                searched = msg.ToString().Remove(0, 6);

                var level = await api.V1.Stats.GetBrV2Async(x =>
                {
                    x.Name = searched;
                    x.ImagePlatform = BrStatsV2V1ImagePlatform.All;
                });

                if (level.IsSuccess)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - account exists writing to " + Directory.GetCurrentDirectory() + @"\linked.txt...");

                    File.WriteAllText(Directory.GetCurrentDirectory() + "/linked.txt", File.ReadAllText(Directory.GetCurrentDirectory() + "/linked.txt").Replace("{", user + "-" + msg.ToString().Remove(0, 6) + "\n{"));

                    Console.WriteLine(DateTime.Now.ToString() + " - wrote to " + Directory.GetCurrentDirectory() + @"\linked.txt...");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", account linked. Use !me to show your level to others!");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - account did not exist.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", account not successfully linked error. The epic account does not exist.");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
            }
            else if (msg.HasStringPrefix("!me", ref argPos) && isbot == "False")
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !me command... responding:");

                var user = context.Message.Author.ToString();

                var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/linked.txt");

                var searched = "Not linked with discord!!";

                foreach (var line in lines)
                {
                    if (line.Contains(user))
                    {
                        searched = line.Remove(0, user.Length + 1);
                    }
                }

                if (searched != "Not linked with discord!!")
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - user has linked an account.");
                    var level = await api.V1.Stats.GetBrV2Async(x =>
                    {
                        x.Name = searched;
                        x.ImagePlatform = BrStatsV2V1ImagePlatform.All;
                    });

                    if (level.IsSuccess)
                    {
                        Console.WriteLine(DateTime.Now.ToString() + " - user has linked a valid account. Creating embed...");
                        var embed = new EmbedBuilder()
                            .WithThumbnailUrl(context.User.GetAvatarUrl() ?? context.User.GetDefaultAvatarUrl())
                            .WithTitle("Here is your Fortnite level!")
                            .WithDescription("Found: **" + searched + "** is linked with **" + context.Message.Author.ToString() + "**")
                            .WithColor(new Color(74, 58, 183))
                            .AddField("Level: ", level.Data.BattlePass.Level.ToString())
                            .WithCurrentTimestamp()
                            .Build();

                        var sendmsg = await context.Channel.SendMessageAsync(embed: embed);
                        var etc = await context.Channel.SendMessageAsync("TEMP");
                        await context.Channel.DeleteMessageAsync(etc);
                        await context.Channel.DeleteMessageAsync(etc);
                        await context.Channel.DeleteMessageAsync(sendmsg);
                        Console.WriteLine(DateTime.Now.ToString() + " - embed sent.");
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now.ToString() + " - user has linked an invalid account. Finishing response.");
                        var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", account " + searched + "does not exist.");
                        var etc = await context.Channel.SendMessageAsync("TEMP");
                        await context.Channel.DeleteMessageAsync(etc);
                        await context.Channel.DeleteMessageAsync(etc);
                        await context.Channel.DeleteMessageAsync(sendmsg);
                    }
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - user has not linked an account.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", account not linked error. Have you changed your username or #?");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
            }
            else if (msg.HasStringPrefix("!help", ref argPos) && isbot == "False") //Add new commands
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !help command... responding");

                var sendmsg = await context.Channel.SendMessageAsync("Here are all commands:\n" +
                    "**!lvl {EPIC NAME}** - shows the BP level for a specified Epic account (via name)\n" +
                    "**!lvl-byID {EPIC ID}** - shows the BP level for a specified Epic account (via ID)\n" +
                    "**!help** - provides help\n" +
                    "**!me** - shows your BP level (Note - requires you to have linked your Epic with this server using !link)\n" +
                    "**!link {EPIC NAME}** - links your Epic with this server\n" +
                    "**!bot-ver** - shows the version of the bot that is currently running\n" +
                    "**!unlink** - unlinks any epic accounts you have linked to this server");
                var etc = await context.Channel.SendMessageAsync("TEMP");
                await context.Channel.DeleteMessageAsync(etc);
                await context.Channel.DeleteMessageAsync(etc);
                await context.Channel.DeleteMessageAsync(sendmsg);
            }
            else if (msg.HasStringPrefix("!unlink", ref argPos) && isbot == "False")
            {
                Console.WriteLine(DateTime.Now.ToString() + " - recieved !unlink command... responding:");

                var user = context.Message.Author.ToString();

                var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/linked.txt");

                var searched = "Not linked with discord!!";

                int i = 0;

                int Tempi = 0;

                var result = File.ReadAllText(Directory.GetCurrentDirectory() + "/linked.txt");

                foreach (var line in lines)
                {
                    if (line.Contains(user))
                    {
                        result = File.ReadAllText(Directory.GetCurrentDirectory() + "/linked.txt");

                        searched = line;
                        Tempi = i;
                        i = Tempi + 1;
                        result = result.Replace(line, "");

                        File.WriteAllText(Directory.GetCurrentDirectory() + "/linked.txt", result);
                    }
                }

                Console.WriteLine(DateTime.Now.ToString() + " - found " + i + " account(s) are linked with command user, finishing...");

                var sendmsg = await context.Channel.SendMessageAsync("Unlinked " + i + " account(s) associated with " + context.Message.Author.Mention + ".");
                var etc = await context.Channel.SendMessageAsync("TEMP");
                await context.Channel.DeleteMessageAsync(etc);
                await context.Channel.DeleteMessageAsync(etc);
                await context.Channel.DeleteMessageAsync(sendmsg);
            }
            else
            {
                if (msg.HasStringPrefix("!", ref argPos))
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - recieved invalid command.");

                    var sendmsg = await context.Channel.SendMessageAsync(context.Message.Author.Mention + ", invalid command. Use !help for all commands.");
                    var etc = await context.Channel.SendMessageAsync("TEMP");
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(etc);
                    await context.Channel.DeleteMessageAsync(sendmsg);
                }
            }
        }
    }
}

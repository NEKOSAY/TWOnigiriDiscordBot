using Discord;
using Discord.Commands;
using OnigiriBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public OnigiriService OnigiriService { get; set; }
        public Connection Connection { get; set; }

        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync("pong!");

        [Command("help")]
        [Alias("list")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder();
            builder.WithTitle("鬼斬BOT 指令列表");
            builder.AddField("道具管理", "o!item help", true);
            builder.AddField("團隊管理", "o!group help", true);
            builder.AddField("鬼斬輔助", "o!onigiri help", true);
            builder.WithFooter(new EmbedFooterBuilder() { Text = "測試版本並不代表最終上線品質" });
            builder.WithColor(Color.Orange);
            builder.WithThumbnailUrl("https://cdn.discordapp.com/avatars/788962824868986911/3dfa41576f6dc039d2752f0349a13782.png?size=32");
            await ReplyAsync("",false,builder.Build());
        }
    }
}

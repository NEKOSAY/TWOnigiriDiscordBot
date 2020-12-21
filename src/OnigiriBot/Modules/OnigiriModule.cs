using Discord;
using Discord.Commands;
using OnigiriBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot.Modules
{
    [Group("onigiri")]
    public class OnigiriModule : ModuleBase<SocketCommandContext>
    {
        public OnigiriService OnigiriService { get; set; }

        private const string Preparing = "凖備中";

        [Group("info")]
        public class OnigiriInfoModule : ModuleBase<SocketCommandContext>
        {
            public OnigiriService OnigiriService { get; set; }

            [Command("update")]
            [Alias("")]
            public async Task UpdateInfoAsync()
            {
                var builder = await OnigiriService.GetOnigiriInfoAsync("update");
                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
        }

        [Command("broadcast")]
        public async Task BroadcastAsync(string OnOff = "ON")
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("notic")]
        public async Task NoticAsync(string OnOff = "ON")
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("help")]
        [Alias("list")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder();
            builder.WithTitle("鬼斬BOT-鬼斬輔助 指令列表");
            builder.AddField("最新更新公告", "o!onigiri info", false);
            builder.AddField("鬼之島頻道一廣播", "o!onigiri broadcast", false);
            builder.AddField("維護通知", "o!onigiri notic", false);
            builder.WithFooter(new EmbedFooterBuilder() { Text = "測試版本並不代表最終上線品質" });
            builder.WithColor(Color.Orange);
            builder.WithThumbnailUrl("https://cdn.discordapp.com/avatars/788962824868986911/3dfa41576f6dc039d2752f0349a13782.png?size=32");
            await ReplyAsync("", false, builder.Build());
        }
    }
}

using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot.Modules
{
    [Group("item")]
    public class ItemModule : ModuleBase<SocketCommandContext>
    {
        private const string Preparing = "凖備中";


        [RequireUserPermission(GuildPermission.ViewChannel)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [Command("info")]
        public async Task ItemRemoveAsync([Remainder] string itemName)
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("buy")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ItemInfoAsync(string itemName, int Coin = -1, IUser user = null)
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("sub")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ItemSubscriberAsync(string itemName, bool OnOff = false)
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("help")]
        [Alias("list")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder();
            builder.WithTitle("鬼斬BOT-道具管理 指令列表");
            builder.AddField("道具信息", "o!item info", false);
            builder.AddField("求購", "o!item buy", false);
            builder.AddField("販賣通知", "o!item sub", false);
            builder.WithFooter(new EmbedFooterBuilder() { Text = "測試版本並不代表最終上線品質" });
            builder.WithColor(Color.Orange);
            builder.WithThumbnailUrl("https://cdn.discordapp.com/avatars/788962824868986911/3dfa41576f6dc039d2752f0349a13782.png?size=32");
            await ReplyAsync("", false, builder.Build());
        }
    }
}

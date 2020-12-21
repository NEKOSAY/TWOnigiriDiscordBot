using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot.Modules
{
    [Group("group")]
    public class GroupModule : ModuleBase<SocketCommandContext>
    {
        private const string Preparing = "凖備中";

        [Command("create")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GroupCreateAsync(IUser user = null, params string[] args)
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("update")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GroupUpdateAsync([Remainder] string itemName)
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("delete")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GroupDeleteAsync([Remainder] string itemName)
        {
            await Context.Channel.SendMessageAsync(Preparing);
        }

        [Command("help")]
        [Alias("list")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder();
            builder.WithTitle("鬼斬BOT-團隊管理 指令列表");
            builder.AddField("建立團隊", "o!group add", false);
            builder.AddField("移除團隊", "o!group remove", false);
            builder.AddField("更新團隊狀態", "o!group update", false);
            builder.WithFooter(new EmbedFooterBuilder() { Text = "測試版本並不代表最終上線品質" });
            builder.WithColor(Color.Orange);
            builder.WithThumbnailUrl("https://cdn.discordapp.com/avatars/788962824868986911/3dfa41576f6dc039d2752f0349a13782.png?size=32");
            await ReplyAsync("", false, builder.Build());
        }
    }
}

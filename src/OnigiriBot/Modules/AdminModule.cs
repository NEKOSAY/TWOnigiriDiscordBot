using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot.Modules
{
    [Group("admin")]
    [RequireContext(ContextType.DM, ErrorMessage = "該指令僅私聊可用")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Group("item")]
        public class AdminItemModule : ModuleBase<SocketCommandContext>
        {
            [Command("add")]
            public async Task AdminItemAddAsync(params string[] args)
            {
                await Context.Channel.SendMessageAsync(args.Aggregate((a,b)=> {
                    {
                        return a += $",{b}";
                    }
                }));
            }
        }
    }
}

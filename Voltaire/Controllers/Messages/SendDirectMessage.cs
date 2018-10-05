﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class SendDirectMessage
    {
        public static async Task PerformAsync(SocketCommandContext currentContext, string userName, string message, bool replyable, DataBase db)
        {
            userName = userName.StartsWith('@') ? userName.Substring(1) : userName;
            try
            {
                var guildList = Send.GuildList(currentContext);
                List<SocketGuildUser> allUsersList = ToUserList(guildList);

                var userList = allUsersList.Where(x => x.Username != null && (x.Username.ToLower() == userName.ToLower() || x.Id.ToString() == userName) && !x.IsBot);

                var allowDmList = userList.Where(x => FilterGuildByDirectMessageSetting(x, db));

                if (!allowDmList.Any() && userList.Any())
                {
                    await currentContext.Channel.SendMessageAsync("user found, but channel permissions do not allow annonymous direct messaging");
                    return;
                }

                var user = allowDmList.Where(x => FilterGuildByRole(x,currentContext.User, db)).FirstOrDefault();

                if (user == null && allowDmList.Any())
                {
                    await currentContext.Channel.SendMessageAsync("user found, but you do not have the role required to DM them");
                    return;
                }
                else if(user == null)
                {
                    await currentContext.Channel.SendMessageAsync("user not found");
                    return;
                }

                var userChannel = await user.GetOrCreateDMChannelAsync();
                var prefix = PrefixHelper.ComputePrefix(currentContext, user.Guild, db, "anonymous user");
                var messageFunction = Send.SendMessageToChannel(userChannel, replyable, currentContext.User);
                await messageFunction(prefix, message);
                await Send.SendSentEmote(currentContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public static List<SocketGuildUser> ToUserList(IEnumerable<SocketGuild> guildList)
        {
            return guildList.Aggregate(new List<SocketGuildUser>(), (acc, item) => acc.Concat(item.Users).ToList());
        }

        private static bool FilterGuildByDirectMessageSetting(SocketGuildUser user, DataBase db)
        {
            return !db.Guilds.Any(x => x.DiscordId == user.Guild.Id.ToString() && !x.AllowDirectMessage);
        }

        private static bool FilterGuildByRole(SocketGuildUser reciver, IUser sender, DataBase db)
        {
            var guild = db.Guilds.FirstOrDefault(x => x.DiscordId == reciver.Guild.Id.ToString());
            if (guild == null || guild.AllowedRole == null)
            {
                return true;
            }
            var role = reciver.Guild.Roles.FirstOrDefault(x => x.Id.ToString() == guild.AllowedRole);
            if (role == null)
            {
                return false;
            }

            return role.Members.Contains(sender);
        }
    }
}

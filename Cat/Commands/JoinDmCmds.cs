using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Cat.Commands
{
    public class JoinDmCmds : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;
        /********************************************Misc********************************************/

        [Command("JoinDmTest")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmTestCmd()
        {
            JoinDmTest(Context.User, Context.Guild);
        }

        /********************************************Join dm on/of settings********************************************/

        //General settings
        [Command("JoinDm")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDm(bool setting)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmBool = setting;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            embed.WithDescription($"Done!  :thumbsup: ");

            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        /********************************************Join dm text********************************************/

        [Command("JoinDmDescription")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmDesc(string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmDescriptionString = msg;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm description added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmTitle")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmTitle(string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmTitleString = msg;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm title added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmImage")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmImage(string imageUrl)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmImageUrlString = imageUrl;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm image added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmTumbnailImage")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmTumbnailImage(string imageUrl)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmTumbnailImageUrlString = imageUrl;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm image added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmFooter")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmFooter(string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmFooterString = msg;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm footer added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmInlineField1")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmInlineField1(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmInlineField1String = msg;
            server.JoinDmInlineField1TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm inline field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmInlineField2")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmInlineField2(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmInlineField2String = msg;
            server.JoinDmInlineField2TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm inline field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmInlineField3")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmInlineField3(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmInlineField3String = msg;
            server.JoinDmInlineField3TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm inline field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmInlineField4")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmInlineField4(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmInlineField4String = msg;
            server.JoinDmInlineField4TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm inline field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmField1")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmField1(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmField1String = msg;
            server.JoinDmField1TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmField2")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmField2(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmField2String = msg;
            server.JoinDmField2TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmField3")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmField3(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmField3String = msg;
            server.JoinDmField3TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmField4")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmField4(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmField4String = msg;
            server.JoinDmField4TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmField5")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmField5(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmField5String = msg;
            server.JoinDmField5TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        [Command("JoinDmField6")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDmField6(string title, string msg)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.JoinDmField6String = msg;
            server.JoinDmField6TitleString = title;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            await Context.Channel.SendMessageAsync("New Dm field added :thumbsup:\nExample: ");
            JoinDmTest(Context.User, Context.Guild);
        }

        private async void JoinDmTest(SocketUser user, SocketGuild guild)
        {
            var s = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);
            if (s.JoinDmBool)
            {
                var embed = new EmbedBuilder();
                rnd = new Random();

                if(s.JoinDmTitleString != "") embed.WithTitle(s.JoinDmTitleString);
                if (s.JoinDmImageUrlString != "") embed.WithImageUrl(s.JoinDmImageUrlString);
                if (s.JoinDmTumbnailImageUrlString != "") embed.WithThumbnailUrl(s.JoinDmTumbnailImageUrlString);
                if (s.JoinDmDescriptionString != "") embed.WithDescription(s.JoinDmDescriptionString);
                if (s.JoinDmField1TitleString != "" && s.JoinDmTitleString != "") embed.AddField(s.JoinDmField1TitleString, s.JoinDmField1String);
                if (s.JoinDmField2TitleString != "" && s.JoinDmField2String != "") embed.AddField(s.JoinDmField2TitleString, s.JoinDmField2String);
                if (s.JoinDmField3TitleString != "" && s.JoinDmField3String != "") embed.AddField(s.JoinDmField3TitleString, s.JoinDmField3String);
                if (s.JoinDmField4TitleString != "" && s.JoinDmField4String != "") embed.AddField(s.JoinDmField4TitleString, s.JoinDmField4String);
                if (s.JoinDmField5TitleString != "" && s.JoinDmField5String != "") embed.AddField(s.JoinDmField5TitleString, s.JoinDmField5String);
                if (s.JoinDmField6TitleString != "" && s.JoinDmField6String != "") embed.AddField(s.JoinDmField6TitleString, s.JoinDmField6String);
                if (s.JoinDmInlineField1TitleString != "" && s.JoinDmInlineField1String != "") embed.AddInlineField(s.JoinDmInlineField1TitleString, s.JoinDmInlineField1String);
                if (s.JoinDmInlineField2TitleString != "" && s.JoinDmInlineField2String != "") embed.AddInlineField(s.JoinDmInlineField2TitleString, s.JoinDmInlineField2String);
                if (s.JoinDmInlineField3TitleString != "" && s.JoinDmInlineField3String != "") embed.AddInlineField(s.JoinDmInlineField3TitleString, s.JoinDmInlineField3String);
                if (s.JoinDmInlineField4TitleString != "" && s.JoinDmInlineField4String != "") embed.AddInlineField(s.JoinDmInlineField4TitleString, s.JoinDmInlineField4String);
                if (s.JoinDmFooterString != "") embed.WithFooter(s.JoinDmFooterString);

                embed.WithCurrentTimestamp();
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                rnd = new Random();

                embed.WithDescription("You need to turn the join dm on first.\nYou can do that with the commands: `JoinDm true`");

                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }
    }
}
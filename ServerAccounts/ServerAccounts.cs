using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Cat.ServerAccounts
{
    public class ServerAccounts
    {
        private static List<ServerAccount> Servers;

        //private static string accountsFile = "Data/403577303784882186.json";

        public static void SaveAccounts(SocketGuild guild)
        {
            Datastorage.SaveServerAccounts(Servers, $"Data/ServerData/{guild.Id}/{guild.Id}.json");
        }

        public static ServerAccount GetAccount(SocketGuild guild)
        {
            string accountsFile = $"Data/ServerData/{guild.Id}/{guild.Id}.json";
            if (Datastorage.SaveExists(accountsFile))
            {
                Servers = Datastorage.LoadServerAccounts(accountsFile).ToList();
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory($"Data/ServerData/{guild.Id}");
                Servers = new List<ServerAccount>();
                SaveAccounts(guild);
            }
            return GetOrCreateAccount(guild.Id, guild);
        }

        private static ServerAccount GetOrCreateAccount(ulong id, SocketGuild guild)
        {
            var result = from a in Servers
                         where a.ID == id
                select a;

            var account = result.FirstOrDefault();
            if (account == null) account = CreateServerAccount(id, guild);
            return account;
        }

        private static ServerAccount CreateServerAccount(ulong id, SocketGuild guild)
        {
            var newAccount = new ServerAccount()
            {
                ID = id,
                ServerName = guild.Name,
                Prefix = null,
                //join Dm Stuff
                JoinDmBool = false,
                JoinDmDescriptionString = $"This is the default join DM. If this is not a test pls let the owner of the server know that the JoinDm setting are wrong",
                JoinDmField1String = $"",
                JoinDmField2String = $"",
                JoinDmField3String = $"",
                JoinDmField4String = $"",
                JoinDmField5String = $"",
                JoinDmField6String = $"",
                JoinDmInlineField1String = $"",
                JoinDmInlineField2String = $"",
                JoinDmInlineField3String = $"",
                JoinDmInlineField4String = $"",
                JoinDmField1TitleString = $"",
                JoinDmField2TitleString = $"",
                JoinDmField3TitleString = $"",
                JoinDmField4TitleString = $"",
                JoinDmField5TitleString = $"",
                JoinDmField6TitleString = $"",
                JoinDmInlineField1TitleString = $"",
                JoinDmInlineField2TitleString = $"",
                JoinDmInlineField3TitleString = $"",
                JoinDmInlineField4TitleString = $"",
                JoinDmFooterString = $"",
                JoinDmImageUrlString = $"",
                JoinDmTumbnailImageUrlString = $"",
                JoinDmTitleString = $"",
                //welcome Msg Stuff
                WelcomeMsgDescriptionString = $"This is the default welcome message. If this is not a test pls let the owner of the server know that the WelcomeMsg setting are wrong",
                WelcomeMsgField1String = $"",
                WelcomeMsgField2String = $"",
                WelcomeMsgField3String = $"",
                WelcomeMsgField4String = $"",
                WelcomeMsgField5String = $"",
                WelcomeMsgField6String = $"",
                WelcomeMsgInlineField1String = $"",
                WelcomeMsgInlineField2String = $"",
                WelcomeMsgInlineField3String = $"",
                WelcomeMsgInlineField4String = $"",
                WelcomeMsgField1TitleString = $"",
                WelcomeMsgField2TitleString = $"",
                WelcomeMsgField3TitleString = $"",
                WelcomeMsgField4TitleString = $"",
                WelcomeMsgField5TitleString = $"",
                WelcomeMsgField6TitleString = $"",
                WelcomeMsgInlineField1TitleString = $"",
                WelcomeMsgInlineField2TitleString = $"",
                WelcomeMsgInlineField3TitleString = $"",
                WelcomeMsgInlineField4TitleString = $"",
                WelcomeMsgFooterString = $"",
                WelcomeMsgImageUrlString = $"",
                WelcomeMsgTumbnailImageUrlString = $"",
                WelcomeMsgTitleString = $"",
                //Role assignment Stuff
                NewUserJoinedRoleBool = false,
                NewUserJoinedRoleString = $"",
                WelcomeMsg = false,
                //lvl stuff
                LevelupMsgBool = true,
                LevelupMsgChannel = 0,
                WeeklyTopBool = false,
                MonthlyTopBool = false,
                YearlyTopBool = false
            };

            Servers.Add(newAccount);
            SaveAccounts(guild);
            return newAccount;
        }
    }
}
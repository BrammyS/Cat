using System;

namespace Cat.Persistence.JsonStorage.UserAccounts
{
    public class UserAccount
    {
        public ulong Id { get; set; }

        public string Username { get; set; }

        public ulong Level { get; set; }

        public ulong Xp { get; set; }

        public DateTime TimeConnected { get; set; }

        public ulong TotalTimeConnected { get; set; }

        public DateTime LastEmote { get; set; }

        public ulong TimeConnectedWeek { get; set; }

        public ulong TimeConnectedMonth { get; set; }

        public ulong TimeConnectedYear { get; set; }

        public DateTime LastCommandUsed { get; set; }
        public DateTime LastMessageSend { get; set; }

        public ulong TimesTimedOut { get; set; }
    }
}

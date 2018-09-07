using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public string Username { get; set; }

        public ulong Level { get; set; }

        public ulong XP { get; set; }

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


using System;
using System.Collections.Generic;

namespace Cat.Persistence.Domain.Tables
{
    public class User
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal TotalTimesTimedOut { get; set; }
        public DateTime CommandUsed { get; set; }
        public decimal SpamWarning { get; set; }

        public List<UserInfo> UserInfos { get; set; }

    }
}

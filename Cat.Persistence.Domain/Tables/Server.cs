using System;
using System.Collections.Generic;

namespace Cat.Persistence.Domain.Tables
{
    public class Server
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public decimal TotalMembers { get; set; }
        public DateTime JoinDate { get; set; }
        public bool Active { get; set; }

        public List<UserInfo> UserInfos { get; set; }
    }
}

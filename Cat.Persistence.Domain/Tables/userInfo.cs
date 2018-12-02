using System;

namespace Cat.Persistence.Domain.Tables
{
    public class UserInfo
    {
        public decimal Id { get; set; }
        public decimal ServerId { get; set; }
        public decimal UserId { get; set; }
        public decimal Xp { get; set; }
        public decimal Level { get; set; }
        public decimal TimeConnected { get; set; }
        public decimal MessagesSend { get; set; }
        public DateTime LastMessageSend { get; set; }
        public DateTime LastVoiceStateUpdateReceived { get; set; }

        public Server Server { get; set; }
        public User User { get; set; }
    }
}
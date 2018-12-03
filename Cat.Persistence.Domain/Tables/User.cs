using System;

namespace Cat.Persistence.Domain.Tables
{
    public class User
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal TotalTimesTimedOut { get; set; }
        public DateTime CommandUsed { get; set; }
        public decimal SpamWarning { get; set; }
        public decimal ServerId { get; set; }
        public decimal UserId { get; set; }
        public decimal Xp { get; set; }
        public decimal Level { get; set; }
        public decimal TimeConnected { get; set; }
        public decimal MessagesSend { get; set; }
        public DateTime LastMessageSend { get; set; }
        public DateTime LastVoiceStateUpdateReceived { get; set; }

        public Server Server { get; set; }
    }
}

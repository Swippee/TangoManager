using System;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Events.PacketLockEntityEvents;

namespace TangoManagerAPI.Entities.Models
{
    public class PacketLockEntity
    {
        public static TimeSpan CacheExpiration => TimeSpan.FromMinutes(10);
        private static TimeSpan AccessTimeout => TimeSpan.FromMinutes(20);
 
        public PacketLockEntity(string packetName, string lockToken)
        {
            PacketName = packetName;
            LockToken = lockToken;
            CreationDateTime = DateTime.UtcNow;
            LastAccessedDateTime = DateTime.UtcNow;
        }

        public string LockToken { get; set; }
        public string PacketName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastAccessedDateTime { get; set; }
        public bool IsExpired => TimeSpan.FromMinutes((DateTime.UtcNow - LastAccessedDateTime).TotalMinutes) >= AccessTimeout;

        public AEvent UpdateLastAccessedDateTime()
        {
            LastAccessedDateTime = DateTime.UtcNow;
            return new PacketLockAccessedEvent(this);
        }
    }
}

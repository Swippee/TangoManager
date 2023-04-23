using System;

namespace TangoManagerAPI.Entities.Models
{
    public class PacketLockEntity
    {
        public static TimeSpan CacheExpiration => TimeSpan.FromMinutes(10);
        public static TimeSpan AccessTimeout => TimeSpan.FromMinutes(20);
 
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
        public bool IsExpired => LastAccessedDateTime >= CreationDateTime;

        public void UpdateLastAccessedDateTime()
        {
            LastAccessedDateTime = DateTime.UtcNow;
        }
    }
}

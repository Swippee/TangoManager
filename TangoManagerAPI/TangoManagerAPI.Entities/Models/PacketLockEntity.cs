using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TangoManagerAPI.Entities.Ports;

namespace TangoManagerAPI.Entities.Models
{
    [Serializable]
    public class PacketLockEntity : ICloneable<PacketLockEntity>
    {
        public PacketLockEntity()
        {

        }

        public PacketLockEntity(string packetName, string token)
        {
            if (string.IsNullOrEmpty(packetName))
                throw new ArgumentNullException(nameof(packetName), "Packet name cannot be null or empty!");

            DateLock = DateTime.UtcNow;
            PacketName = packetName;
        }

        public string PacketName { get; set; }
        public string token { get; set; }
        public DateTime DateLock { get; set; }


     
        public PacketLockEntity Clone()
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            formatter.Serialize(ms, this);
#pragma warning restore SYSLIB0011
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011
            return (PacketLockEntity)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(PacketName);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Ports.Repository
{
    public interface IWriteRepository
    {
        void AddPaquet(PaquetEntity paquet);
        void RemovePaquet(string name);
    }
}

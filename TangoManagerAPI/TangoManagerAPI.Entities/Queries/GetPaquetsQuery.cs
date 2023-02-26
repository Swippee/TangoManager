﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Queries
{
    public sealed class GetPaquetsQuery : AQuery<IEnumerable<PaquetEntity>, GetPaquetsQuery>
    {
        public override async Task<IEnumerable<PaquetEntity>> QueryAsync(IQueryRouter queryRouter)
        {
            return await queryRouter.RouteAsync(this);
        }
    }
}
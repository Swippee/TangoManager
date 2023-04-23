﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using TangoManagerAPI.ActionFilters;
using TangoManagerAPI.Application.Commands.CommandsAuth;
using TangoManagerAPI.DTO;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetsController : ControllerBase
    {
        private readonly IQueryRouter _queryRouter;
        private readonly ICommandRouter _commandRouter;

        public PaquetsController(IQueryRouter queryRouter, ICommandRouter commandRouter)
        {
            _queryRouter = queryRouter;
            _commandRouter = commandRouter;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var res = await new GetAllPaquetsQuery().QueryAsync(_queryRouter);
            List<PaquetEntity> result = new List<PaquetEntity>();
            res.ToList().ForEach(i => result.Add(i.RootEntity));

            if (!result.Any())
                return StatusCode((int)HttpStatusCode.NoContent);

            return StatusCode((int)HttpStatusCode.OK, res);
        }

        [HttpPost]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> CreateAsync([FromBody] CreatePacketRequest createPacketRequest)
        {
            var cmd = new CreatePaquetCommand(createPacketRequest.Name, createPacketRequest.Description);
            var packetAggregate = await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.Created, packetAggregate.RootEntity);
        }

        [HttpDelete]
        [Route("{name}")]
        [PacketActionFilter("name")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string name)
        {
            var cmd = new DeletePaquetCommand(name);
            await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPut]
        [Route("{packetName}")]
        [PacketActionFilter("packetName")]
        public async Task<ActionResult> AddCardAsync([FromRoute] string packetName, [FromBody] AddCardToPacketRequest addCardToPacketRequest)
        {
            var cmd = new AddCardToPacketCommand(packetName, addCardToPacketRequest.Question, addCardToPacketRequest.Answer, addCardToPacketRequest.Score);
            var packetAggregate = await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.OK, packetAggregate.RootEntity);
        }


        [HttpPost]
        [Route("packetLock/{packetName}")]
        public async Task<ActionResult> CreatePacketLockAsync([FromRoute] string packetName)
        {
            var cmd = new LockPacketCommand(packetName);
            var lockPacketEntity = await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.Created, lockPacketEntity);
        }

        [HttpDelete]
        [Route("packetLock/{packetName}")]
        public async Task<ActionResult> DeletePacketLockAsync([FromRoute] string packetName, [FromBody] DeletePacketLockRequest deletePacketLockRequest)
        {
            var cmd = new UnlockPacketCommand(packetName, deletePacketLockRequest.PacketToken);
            await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}

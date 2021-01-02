using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Internal.Messages.Core.Abstractions.Services;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.ResourceParameters;
using Internal.Messages.WebApi.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Internal.Messages.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : BaseController<UsersController>
    {
        private readonly IUsersService usersService;

        public UsersController(
            IUsersService usersService,
            ILogger<UsersController> logger,
            IMapper mapper)
            : base(logger, mapper)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<ReadUser>>> GetUsers([FromQuery] UsersResourceParameters parameters)
        {
            var users = await usersService.UsersRepository.GetUsersAsync(parameters);

            return Ok(mapper.Map<IEnumerable<ReadUser>>(users));
        }

        [HttpGet("{userId:int}", Name = "GetUserById")]
        public async Task<ActionResult<ReadUser>> GetUser(int userId)
        {
            var user = await usersService.UsersRepository.GetByIdAsync(userId);

            return user == null ? NotFound() : (ActionResult)Ok(mapper.Map<ReadUser>(user));
        }

        [HttpGet("{userGuid:guid}", Name = "GetUserByGuid")]
        public async Task<ActionResult<ReadUser>> GetUser(Guid userGuid)
        {
            var user = await usersService.UsersRepository.GetByGuidAsync(userGuid);

            return user == null ? NotFound() : (ActionResult)Ok(mapper.Map<ReadUser>(user));
        }

        [HttpPost]
        public async Task<ActionResult<ReadUser>> PostUser(CreateUser createUser)
        {
            var user = mapper.Map<User>(createUser);

            await usersService.UsersRepository.CreateAsync(user);

            var result = mapper.Map<ReadUser>(user);

            return CreatedAtRoute("GetUserByGuid", new { userId = result.Guid }, result);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var user = await usersService.UsersRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            usersService.UsersRepository.DeleteAsync(user);
            await usersService.UsersRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "DELETE, GET, HEAD, OPTIONS, POST");

            return Ok();
        }
    }
}

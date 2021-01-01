using System;
using System.Threading.Tasks;
using AutoMapper;
using Internal.Messages.WebApi.Filters;
using Internal.Messages.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Internal.Messages.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> logger;
        protected readonly IMapper mapper;

        public BaseController(ILogger<T> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }
    }
}

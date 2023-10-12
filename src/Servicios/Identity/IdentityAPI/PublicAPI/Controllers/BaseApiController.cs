using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace PublicAPI.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly ILoggerManager _logger;

    public BaseApiController(IMediator mediator, ILoggerManager logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}
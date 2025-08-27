using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Application.Auth.Login;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(req.Username, req.Password), ct);
        return Ok(new { token = result.Token });
    }
}

using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Auth.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _users;
        private readonly IJwtTokenGenerator _jwt;

        public LoginCommandHandler(IUserRepository users, IJwtTokenGenerator jwt)
        {
            _users = users;
            _jwt = jwt;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _users.GetByUsernameAsync(request.Username, ct);
            if (user is null) throw new UnauthorizedAccessException("Invalid credentials.");

            if (user.Password != request.Password) 
                throw new UnauthorizedAccessException("Invalid credentials.");

            var roles = new List<string> { user.Role.ToString() };
            var token = _jwt.GenerateToken(user.Id, user.Username, roles);

            return new LoginResponse { Token = token };
        }
    }
}

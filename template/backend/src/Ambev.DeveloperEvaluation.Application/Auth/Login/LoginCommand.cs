using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Auth.Login
{
    public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
}

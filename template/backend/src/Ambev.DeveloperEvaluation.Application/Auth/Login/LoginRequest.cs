using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Auth.Login
{
    public sealed class LoginRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Models.Response
{
    public class AuthenticationResponse
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
}

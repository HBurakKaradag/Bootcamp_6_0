using System;

namespace Hotels.API.Models
{
    public class TokenRequest
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
    }
}
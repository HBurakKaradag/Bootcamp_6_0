using System;

namespace Hotels.API.Models
{
    public class RabbitConfiguration
    {
        public string Host { get; set; }
        public string Exchange { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
    }
}

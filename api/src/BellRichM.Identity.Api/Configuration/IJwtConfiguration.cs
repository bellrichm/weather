using System;

namespace BellRichM.Identity.Api.Configuration
{
    public interface IJwtConfiguration
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        TimeSpan ValidFor { get; set; }
        string SecretKey {get; set;}
        void Validate();        
    }
}
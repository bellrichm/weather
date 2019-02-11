using FluentAssertions;
using Machine.Specifications;
using System;
using System.Net.Http;

namespace BellRichM.Identity.Api.Smoke.Controllers
{
    public class UserControllerSetupTearDown : IAssemblyContext
    {
    public void OnAssemblyStart()
    {
        var baseURL = Environment.GetEnvironmentVariable("SMOKE_BASEURL");
        if (baseURL != null)
        {
            var spHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };
            UserControllerSmoke.Client = new HttpClient(spHandler);
        }
        else
        {
            baseURL = "http://bellrichm-weather.azurewebsites.net";
            UserControllerSmoke.Client = new HttpClient();
        }

        #pragma warning disable S1075
        UserControllerSmoke.Client.BaseAddress = new Uri(baseURL);
        #pragma warning disable S1075
    }

    public void OnAssemblyComplete()
    {
        UserControllerSmoke.Client.Dispose();
    }
    }
}
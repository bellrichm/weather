using Machine.Specifications;
using System;
using System.Net.Http;

namespace BellRichM.Identity.Api.Smoke.Controllers
{
    #pragma warning disable CA1001
    public class UserControllerSetupTearDown : IAssemblyContext
    {
    private HttpClientHandler _spHandler;

    public void OnAssemblyStart()
    {
        var baseURL = Environment.GetEnvironmentVariable("SMOKE_BASEURL");
        if (baseURL != null)
        {
            _spHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };
            UserControllerSmoke.Client = new HttpClient(_spHandler);
        }
        else
        {
            baseURL = "http://bellrichm-weather.azurewebsites.net";
            UserControllerSmoke.Client = new HttpClient();
        }

        UserControllerSmoke.Client.BaseAddress = new Uri(baseURL);
    }

    public void OnAssemblyComplete()
    {
        UserControllerSmoke.Client.Dispose();
    }
    }
    #pragma warning restore CA1001
}
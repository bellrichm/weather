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
        UserControllerSmoke.Client = new HttpClient();
        #pragma warning disable S1075
        UserControllerSmoke.Client.BaseAddress = new Uri("http://bellrichm-weather.azurewebsites.net");
        #pragma warning disable S1075
    }

    public void OnAssemblyComplete()
    {
        UserControllerSmoke.Client.Dispose();
    }
    }
}
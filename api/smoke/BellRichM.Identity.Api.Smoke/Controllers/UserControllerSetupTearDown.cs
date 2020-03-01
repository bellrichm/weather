using Machine.Specifications;
using System;
using System.Net.Http;

namespace BellRichM.Identity.Api.Smoke.Controllers
{
    public class UserControllerSetupTearDown : IAssemblyContext, IDisposable
    {
        private HttpClientHandler _spHandler;
        private bool disposed = false;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
         {
            if (!disposed)
            {
                if (disposing)
                {
                    _spHandler.Dispose();
                }

                disposed = true;
            }
         }
    }
}
using BellRichM.Identity.Api.Smoke.Models;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Smoke
{
  internal class UserControllerSmoke
  {
    protected const string GetByIdRoute = "/api/user/";
    protected static HttpClient client;

    Establish context = () =>
    {
      client = new HttpClient();
      client.BaseAddress = new Uri("http://bellrichm-weather.azurewebsites.net");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));
    };

    Cleanup after = () =>
      client.Dispose();
  }

  internal class When_not_authorized_to_get_user : UserControllerSmoke
  {
    private static HttpResponseMessage response;

    Establish context = () =>
    {
    };

    Cleanup after = () =>
      response.Dispose();

    Because of = () =>
      response = client.GetAsync(string.Empty).Await();

    It should_return_unauthorized_response_code = () =>
      response.StatusCode.Should().Equals(HttpStatusCode.Forbidden);

    It should_return_no_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldBeEmpty();
    };
  }

  internal class When_invalid_user_password : UserControllerSmoke
  {
    private static UserLoginModel userLogin;
    private static HttpResponseMessage response;

    Establish context = () =>
    {
      userLogin = new UserLoginModel
      {
        UserName = "InvalidUser",
        Password = "InvalidPassword"
      };
    };

    Cleanup after = () =>
      response.Dispose();

    Because of = () =>
    {
      var jsonObject = JsonConvert.SerializeObject(userLogin);
      var postContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
      response = client.PostAsync("user/login", postContent).Await();
    };

    It should_return_unauthorized_response_code = () =>
      response.StatusCode.Should().Equals(HttpStatusCode.Forbidden);

    It should_return_no_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldBeEmpty();
    };
  }
}
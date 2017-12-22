using BellRichM.Identity.Api.Smoke.Models;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Smoke
{
  public class UserControllerSmoke
  {
    public static HttpClient Client {get; set;}

    Establish context = () =>
    {
      Client.DefaultRequestHeaders.Accept.Clear();
      Client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));
    };
  }

  internal class When_retrieving_default_page : UserControllerSmoke
  {
    private static HttpResponseMessage response;

    Cleanup after = () =>
      response.Dispose();

    Because of = () =>
      response = Client.GetAsync(string.Empty).Await();

    It should_return_success_code = () =>
      response.StatusCode.ShouldEqual(HttpStatusCode.OK);

    It should_return_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldNotBeEmpty();
    };
  }

  internal class When_invalid_user_password : UserControllerSmoke
  {
    private static HttpResponseMessage response;
    private static StringContent postContent;

    Establish context = () =>
    {
      var userLogin = new UserLoginModel
      {
        UserName = "InvalidUser",
        #pragma warning disable S2068
        Password = "InvalidPassword"
        #pragma warning restore S2068
      };

      var jsonObject = JsonConvert.SerializeObject(userLogin);
      postContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
    };

    Cleanup after = () =>
    {
      postContent.Dispose();
      response.Dispose();
    };

    Because of = () =>
      response = Client.PostAsync("api/user/login", postContent).Await();

    It should_return_bad_request_response_code = () =>
      response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

    It should_return_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldNotBeEmpty();
    };
  }
}
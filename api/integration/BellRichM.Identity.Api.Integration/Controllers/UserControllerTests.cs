using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Integration
{
  internal class UserControllerTests
  {
    protected const string GetByIdRoute = "/api/user/";

    Establish context = () =>
    {
      Client.DefaultRequestHeaders.Clear();
    };

    public static Microsoft.Extensions.Logging.ILogger Logger { get; set; }

    public static HttpClient Client { get; set; }

    public static string UserAdminJwt { get; set; }

    public static string UserTestJwt { get; set; }

    public static User TestUser { get; set; }
  }

  internal class When_request_has_no_authorization_header : UserControllerTests
  {
    private static HttpResponseMessage response;

    Cleanup after = () =>
      response.Dispose();

    Because of = () =>
      response = Client.GetAsync(GetByIdRoute + TestUser.Id).Await();

    It should_return_unauthorized_response_code = () =>
      response.StatusCode.Should().Equals(HttpStatusCode.Unauthorized);

    It should_return_no_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldBeEmpty();
    };
  }

  internal class When_not_authorized_to_get_user : UserControllerTests
  {
    private static HttpResponseMessage response;

    Establish context = () =>
      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserTestJwt);

    Cleanup after = () =>
      response.Dispose();

    Because of = () =>
      response = Client.GetAsync(GetByIdRoute + TestUser.Id).Await();

    It should_return_unauthorized_response_code = () =>
      response.StatusCode.Should().Equals(HttpStatusCode.Forbidden);

    It should_return_no_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldBeEmpty();
    };
  }

  internal class When_authorized_to_get_an_user : UserControllerTests
  {
    private static HttpResponseMessage response;

    Establish context = () =>
      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserAdminJwt);

    Cleanup after = () =>
      response.Dispose();

    Because of = () =>
      response = Client.GetAsync(GetByIdRoute + TestUser.Id).Await();

    It should_return_unauthorized_response_code = () =>
      response.StatusCode.Should().Equals(HttpStatusCode.OK);

    It should_have_content = () =>
    {
      var responseString = (string)response.Content.ReadAsStringAsync().Await();
      responseString.ShouldNotBeEmpty();
    };

    It should_return_a_UserModel = () =>
    {
      var responseString = response.Content.ReadAsStringAsync().Await();
      var user = JsonConvert.DeserializeObject<UserModel>(responseString);
      user.ShouldNotBeNull();
    };
  }
}
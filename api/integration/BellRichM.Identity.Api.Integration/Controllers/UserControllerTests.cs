using FluentAssertions;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Integration
{
    internal class when_request_has_no_authorization_header : UserControllerTests
    {
        private static HttpResponseMessage response;
        
        Because of = () => 
            response = Client.GetAsync(getByIdRoute + TestUser.Id).Await();            

        It should_return_unauthorized_response_code = () => 
            response.StatusCode.Should().Equals(HttpStatusCode.Unauthorized);

        It should_return_no_content = () =>
        {
            var responseString = (string)response.Content.ReadAsStringAsync().Await();           
            responseString.ShouldBeEmpty();
        };
    }

    internal class when_not_authorized_to_get_user : UserControllerTests
    {
        private static HttpResponseMessage response;


        Establish context = () => 
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserTestJwt);            

        Because of = () => 
            response = Client.GetAsync(getByIdRoute + TestUser.Id).Await();            

        It should_return_unauthorized_response_code = () => 
            response.StatusCode.Should().Equals(HttpStatusCode.Forbidden);

        It should_return_no_content = () =>
        {
            var responseString = (string)response.Content.ReadAsStringAsync().Await();          
            responseString.ShouldBeEmpty();
        };        
    }

    internal class when_authorized_to_get_an_user : UserControllerTests
    {
        private static HttpResponseMessage response;

        Establish context = () => 
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserAdminJwt);            

        Because of = () => 
            response = Client.GetAsync(getByIdRoute + TestUser.Id).Await();            

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

    public class UserControllerTests
    {
        public static HttpClient Client {get; set;}
        public static string UserAdminJwt {get; set;}
        public static string UserTestJwt {get; set;}
        public static User TestUser{get; set;}
        public static Microsoft.Extensions.Logging.ILogger Logger;	

        protected const string getByIdRoute = "/api/user/";

        Establish context = () =>
        {
           Client.DefaultRequestHeaders.Clear();
        };
    }    
}
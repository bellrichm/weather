using BellRichM.Attribute.Selector;
using BellRichM.TestRunner;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

using It = Machine.Specifications.It;

namespace BellRichM.Attribute.Test.Selector
{
    public class AcceptTypeAttributeSpecs
    {
    }

    internal class When_request_has_no_accept_header : AcceptTypeAttributeSpecs
    {
        protected static DefaultHttpContext httpContext;
        protected static RouteContext routeContext;

        protected static AcceptTypeAttribute acceptTypeAttribute;

        protected static bool result;

        protected Establish context = () =>
        {
            httpContext = new DefaultHttpContext();
            routeContext = new RouteContext(httpContext);

            acceptTypeAttribute = new AcceptTypeAttribute("foobarx");
        };

        protected Because of = () => result = acceptTypeAttribute.IsValidForRequest(routeContext, null);

        protected It should_return_false = () => result.ShouldBeFalse();
    }

    internal class When_request_has_accept_header_with_incorrect_value : AcceptTypeAttributeSpecs
    {
        protected static DefaultHttpContext httpContext;
        protected static RouteContext routeContext;

        protected static AcceptTypeAttribute acceptTypeAttribute;

        protected static bool result;

        protected Establish context = () =>
        {
            httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept"] = "foobar";
            routeContext = new RouteContext(httpContext);

            acceptTypeAttribute = new AcceptTypeAttribute("foobarx");
        };

        protected Because of = () => result = acceptTypeAttribute.IsValidForRequest(routeContext, null);

        protected It should_return_false = () => result.ShouldBeFalse();
    }

    internal class When_request_has_accept_header_with_correct_value : AcceptTypeAttributeSpecs
    {
        protected static DefaultHttpContext httpContext;
        protected static RouteContext routeContext;

        protected static AcceptTypeAttribute acceptTypeAttribute;

        protected static bool result;

        protected Establish context = () =>
        {
            httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept"] = "foobar";
            routeContext = new RouteContext(httpContext);

            acceptTypeAttribute = new AcceptTypeAttribute("foobar");
        };

        protected Because of = () => result = acceptTypeAttribute.IsValidForRequest(routeContext, null);

        protected It should_return_true = () => result.ShouldBeTrue();
    }

    internal class When_request_has_accept_header_with_format_information : AcceptTypeAttributeSpecs
    {
        protected static DefaultHttpContext httpContext;
        protected static RouteContext routeContext;

        protected static AcceptTypeAttribute acceptTypeAttribute;

        protected static bool result;

        protected Establish context = () =>
        {
            httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept"] = "foobar+json";
            routeContext = new RouteContext(httpContext);

            acceptTypeAttribute = new AcceptTypeAttribute("foobar");
        };

        protected Because of = () => result = acceptTypeAttribute.IsValidForRequest(routeContext, null);

        protected It should_return_true = () => result.ShouldBeTrue();
    }

    class Program
    {
        static void Main()
        {
            var embeddedRunner = new EmbeddedRunner(typeof(AcceptTypeAttributeSpecs));
            embeddedRunner.OnAssemblyStart();
            embeddedRunner.RunTests();
            embeddedRunner.OnAssemblyComplete();
        }
    }
}
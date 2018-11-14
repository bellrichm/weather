using BellRichM.Api.Controllers;
using BellRichM.Api.Models;
using BellRichM.Exceptions;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Api.Test
{
    internal class ApiControllerSpecs
    {
        protected static string traceIdentifier = "foobar";
        protected static ErrorResponseModel errorResponseModel;
        protected static TestController testController;
        Establish context = () =>
        {
            testController = new TestController();
            testController.ControllerContext.HttpContext = new DefaultHttpContext();
            testController.ControllerContext.HttpContext.TraceIdentifier = traceIdentifier;
        };

        Cleanup after = () =>
            testController.Dispose();
    }

    internal class When_invalid_model_state : ApiControllerSpecs
    {
        protected static string errorResponseCode = "InvalidInput";
        protected static string errorResponseText = "Invalid input";

        protected static string firstKey = "key1";
        protected static string firstKeyMessageOne = "errorMessage1a";
        protected static string firstKeyMessageTwo = "errorMessage1b";
        protected static string secondKey = "key2";
        protected static string secondKeyMessage = "errorMessage2";
        protected static string codeSuffix = "Invalid";

        Establish context = () =>
        {
            testController.ModelState.AddModelError(firstKey, firstKeyMessageOne);
            testController.ModelState.AddModelError(firstKey, firstKeyMessageTwo);
            testController.ModelState.AddModelError(secondKey, secondKeyMessage);
        };

        Because of = () =>
        {
            errorResponseModel = testController.CreateModel();
        };

        It should_have_correct_correlation_id = () =>
            errorResponseModel.CorrelationId.ShouldEqual(traceIdentifier);

        It should_have_correct_code = () =>
            errorResponseModel.Code.ShouldEqual(errorResponseCode);

        It should_have_correct_text = () =>
            errorResponseModel.Text.ShouldEqual(errorResponseText);

        It should_have_correct_number_of_error_details = () =>
            errorResponseModel.ErrorDetails.Count().ShouldEqual(3);

        It should_have_correct_error_detail_content = () =>
        {
            errorResponseModel.ErrorDetails.Where(errorDetail =>
                errorDetail.Code == firstKey + codeSuffix &&
                errorDetail.Text == firstKeyMessageOne).Count().ShouldEqual(1);

            errorResponseModel.ErrorDetails.Where(errorDetail =>
                errorDetail.Code == firstKey + codeSuffix &&
                errorDetail.Text == firstKeyMessageTwo).Count().ShouldEqual(1);

            errorResponseModel.ErrorDetails.Where(errorDetail =>
                errorDetail.Code == secondKey + codeSuffix &&
                errorDetail.Text == secondKeyMessage).Count().ShouldEqual(1);
        };
    }

    internal class When_processing_an_exceptio : ApiControllerSpecs
    {
        protected static string exceptionCode = "exceptionCode";
        protected static string exceptionMessage = "exceptionMessage";
        protected static string firstCode = "code 1";
        protected static string firstText = "text 1";
        protected static string secondCode = "code 2";
        protected static string secondText = "text 2";

        protected static ExceptionDetail firstExceptionDetail;
        protected static ExceptionDetail secondExceptionDetail;
        protected static TestException testException;

        Establish context = () =>
        {
            firstExceptionDetail = new ExceptionDetail
            {
                Code = firstCode,
                Text = firstText
            };

            secondExceptionDetail = new ExceptionDetail
            {
                Code = secondCode,
                Text = secondText
            };

            var exceptionDetails = new List<ExceptionDetail>()
            {
                firstExceptionDetail,
                secondExceptionDetail
            };

            testException = new TestException(exceptionCode, exceptionDetails, exceptionMessage);
        };

        Because of = () =>
        {
            errorResponseModel = testController.CreateModel(testException);
        };

        It should_have_correct_correlation_id = () =>
            errorResponseModel.CorrelationId.ShouldEqual(traceIdentifier);

        It should_have_correct_code = () =>
            errorResponseModel.Code.ShouldEqual(exceptionCode);

        It should_have_correct_text = () =>
            errorResponseModel.Text.ShouldEqual(exceptionMessage);

        It should_have_correct_number_of_error_details = () =>
            errorResponseModel.ErrorDetails.Count().ShouldEqual(testException.ErrorDetails.Count());

        It should_have_correct_error_detail_content = () =>
        {
            errorResponseModel.ErrorDetails.Where(errorDetail =>
                errorDetail.Code == firstExceptionDetail.Code &&
                errorDetail.Text == firstExceptionDetail.Text).Count().ShouldEqual(1);

            errorResponseModel.ErrorDetails.Where(errorDetail =>
                errorDetail.Code == secondExceptionDetail.Code &&
                errorDetail.Text == secondExceptionDetail.Text).Count().ShouldEqual(1);
        };
    }

    internal class TestController : ApiController
    {
        internal new ErrorResponseModel CreateModel()
        {
            return base.CreateModel();
        }

        internal new ErrorResponseModel CreateModel(BusinessException businessException)
        {
            return base.CreateModel(businessException);
        }
    }

#pragma warning disable CA1032
    internal class TestException : BusinessException
    {
         public TestException(string code, IEnumerable<ExceptionDetail> exceptionDetails, string message)
         : base(code, exceptionDetails, message)
         {
         }
    }
#pragma warning restore CA1032
}
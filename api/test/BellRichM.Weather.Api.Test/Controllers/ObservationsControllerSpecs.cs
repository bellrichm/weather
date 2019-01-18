using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Controllers;
using BellRichM.Weather.Api.Models;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Test
{
    public class DummySpecs
    {
        protected static int dateTime;
        protected static LoggingData loggingData;

        protected static ObservationModel observationModel;

        protected static ObservationsController observationsController;
        protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            dateTime = 999306300;

            observationModel = new ObservationModel
            {
                DateTime = 999306300,
                USUnits = 1,
                Interval = 5,
                Barometer = 29.688,
                Pressure = 29.172642123245641,
                Altimeter = 29.686688005475741,
                OutsideTemperature = 67.0,
                OutsideHumidity = 80.0,
                WindSpeed = 1.0000024854909466,
                WindDirection = 135.0,
                WindGust = 2.0000049709818932,
                WindGustDirection = 135.0,
                RainRate = 0.0,
                Rain = 0.0,
                DewPoint = 60.619405344958267,
                Windchill = 67.0,
                HeatIndex = 67.0,
                Evapotranspiration = 0.0,
                Radiation = 0.0,
                Ultraviolet = 0.0
            };

            loggerMock = new Mock<ILoggerAdapter<ObservationsController>>();

            observationsController = new ObservationsController(loggerMock.Object);
        };
    }

    internal class When_getting_an_observation : DummySpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ObservationsController_Get,
                        "{@dateTime}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => observationsController.GetObservation(dateTime).Await());
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging;
#pragma warning restore 169

        It should_throw_not_implemented = () =>
        {
            exception.ShouldBeOfExactType<NotImplementedException>();
        };
    }

    internal class When_decorating_Observation_GetObservation_method : DummySpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
        {
            methodInfo = typeof(ObservationsController).GetMethod("GetObservation");
        };

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewObservations");
    }

    internal class When_creating_an_observation : DummySpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ObservationsController_Create,
                        "{@observationCreate}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => observationsController.Create(observationModel).Await());
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging;
#pragma warning restore 169

        It should_throw_not_implemented = () =>
        {
            exception.ShouldBeOfExactType<NotImplementedException>();
        };
    }

    internal class When_decorating_Observation_Create_method : DummySpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
        {
            methodInfo = typeof(ObservationsController).GetMethod("Create");
        };

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanCreateObservations");
    }

    internal class When_updating_an_observation : DummySpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ObservationsController_Update,
                        "{@observationUpdate}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => observationsController.Update(observationModel).Await());
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging;
#pragma warning restore 169

        It should_throw_not_implemented = () =>
        {
            exception.ShouldBeOfExactType<NotImplementedException>();
        };
    }

    internal class When_decorating_Observation_Update_method : DummySpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
        {
            methodInfo = typeof(ObservationsController).GetMethod("Update");
        };

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanUpdateObservations");
    }

    internal class When_deleting_an_observation : DummySpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ObservationsController_Delete,
                        "{@dateTime}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => observationsController.Delete(dateTime).Await());
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging;
#pragma warning restore 169

        It should_throw_not_implemented = () =>
        {
            exception.ShouldBeOfExactType<NotImplementedException>();
        };
    }

    internal class When_decorating_Observation_Delete_method : DummySpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
        {
            methodInfo = typeof(ObservationsController).GetMethod("Delete");
        };

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanDeleteObservations");
    }
}

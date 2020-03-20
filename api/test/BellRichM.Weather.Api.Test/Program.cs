using BellRichM.TestRunner;
using System;
using System.Reflection;

namespace BellRichM.Weather.Api.Test
{
    class Program
    {
        static void Main()
        {
            Type type;
            EmbeddedRunner embeddedRunner;

            // Run all the tests
            /*
            Assembly assembly = Assembly.GetExecutingAssembly();
            embeddedRunner = new EmbeddedRunner(assembly);
            */

            // Run the collection of tests
            /*
            type = Type.GetType("BellRichM.Weather.Api.TestControllers.Test.ObservationsControllerSpecs");
            embeddedRunner = new EmbeddedRunner(type);
            */

            // Run the test
            // note, fullname is "BellRichM.Weather.Api.TestControllers.Test.ObservationsControllerSpecs+When_deleting_an_observation_with_an_invalid_modelState"
            type = Type.GetType("BellRichM.Weather.Api.TestControllers.Test.ObservationsControllerSpecs");
            embeddedRunner = new EmbeddedRunner(type, "When_deleting_an_observation_with_an_invalid_modelState");

            embeddedRunner.RunTests();
        }
    }
}
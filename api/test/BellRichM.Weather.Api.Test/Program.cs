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

            type = Type.GetType("BellRichM.Weather.Api.TestControllers.Test.ObservationsControllerSpecs");
            embeddedRunner = new EmbeddedRunner(type);
            // embeddedRunner.RunTests();

            type = Type.GetType("BellRichM.Weather.Api.Test.ObservationSqliteServiceSpecs");
            embeddedRunner = new EmbeddedRunner(type);
            // embeddedRunner.RunTests();

            Assembly assembly = Assembly.GetExecutingAssembly();
            embeddedRunner = new EmbeddedRunner(assembly);
            embeddedRunner.RunTests();
        }
    }
}
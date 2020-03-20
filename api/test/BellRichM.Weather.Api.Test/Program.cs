using BellRichM.TestRunner;
using System;

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
            embeddedRunner.OnAssemblyStart();
            embeddedRunner.RunTests();
            embeddedRunner.OnAssemblyComplete();

            type = Type.GetType("BellRichM.Weather.Api.Test.ObservationSqliteServiceSpecs");
            embeddedRunner = new EmbeddedRunner(type);
            embeddedRunner.OnAssemblyStart();
            embeddedRunner.RunTests();
            embeddedRunner.OnAssemblyComplete();
        }
    }
}
using BellRichM.TestRunner;
using System;

namespace BellRichM.Weather.Api.Test
{
    class Program
    {
        static void Main()
        {
            var type = Type.GetType("BellRichM.Weather.Api.TestControllers.Test.ObservationsControllerSpecs");
            var embeddedRunner = new EmbeddedRunner(type);
            embeddedRunner.OnAssemblyStart();
            embeddedRunner.RunTests();
            embeddedRunner.OnAssemblyComplete();
        }
    }
}
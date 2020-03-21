using BellRichM.TestRunner;
using System;
using System.Linq;
using System.Reflection;

#pragma warning disable CA1303
namespace BellRichM.Weather.Api.Test
{
    class Program
    {
        static void Main(string testclass = null, string test = null)
        {
            if (test != null && testclass == null)
            {
                Console.WriteLine("When specifying a test, --test, a testclass, --testclass os required.");
            }

            EmbeddedRunner embeddedRunner;
            var assembly = Assembly.GetExecutingAssembly();

            if (test != null)
            {
                var type = assembly.GetTypes().SingleOrDefault(t => t.Name == testclass);
                embeddedRunner = new EmbeddedRunner(type, test);
            }
            else if (testclass != null)
            {
                var type = assembly.GetTypes().SingleOrDefault(t => t.Name == testclass);
                embeddedRunner = new EmbeddedRunner(type);
            }
            else
            {
                embeddedRunner = new EmbeddedRunner(assembly);
            }

            embeddedRunner.RunTests();
        }
    }
}
#pragma warning restore CA1303
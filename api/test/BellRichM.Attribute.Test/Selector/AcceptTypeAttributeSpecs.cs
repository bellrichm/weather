using BellRichM.TestRunner;
using Machine.Specifications;

namespace BellRichM.Attribute.Test.Selector
{
    public class AcceptTypeAttributeSpecs
    {
        protected Establish context = () =>
        { };

        protected Because of = () =>
        { };

        protected It should = () =>
        { };
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
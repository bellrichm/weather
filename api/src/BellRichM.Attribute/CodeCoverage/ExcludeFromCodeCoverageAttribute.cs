using System;

namespace BellRichM.Attribute.CodeCoverage
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Interface)]
    public class ExcludeFromCodeCoverageAttribute : System.Attribute
    {
    }
}

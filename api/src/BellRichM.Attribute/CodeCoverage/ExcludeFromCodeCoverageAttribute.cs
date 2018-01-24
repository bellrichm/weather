using System;

namespace BellRichM.Attribute.CodeCoverage
{
    /// <summary>
    /// Use this attribute to exclude code from coverage analysis.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Interface)]
    public class ExcludeFromCodeCoverageAttribute : System.Attribute
    {
    }
}

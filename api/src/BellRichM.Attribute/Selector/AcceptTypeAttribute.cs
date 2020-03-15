using Microsoft.AspNetCore.Mvc.Abstractions;

using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using System;

namespace BellRichM.Attribute.Selector
{
    /// <summary>
    /// The accept header type selector attribute/\.
    /// </summary>
    public class AcceptTypeAttribute : ActionMethodSelectorAttribute
    {
        private readonly string _acceptType;

        /// <summary>
        /// Initializes a new instance of AcceptTypeAttribute.
        /// </summary>
        /// <param name="acceptType">The aceept header media type.</param>
        public AcceptTypeAttribute(string acceptType)
        {
            this._acceptType = acceptType;
        }

        /// <inheritdoc/>
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (routeContext == null)
            {
                throw new ArgumentNullException(nameof(routeContext));
            }

            var accept = routeContext.HttpContext.Request.Headers.ContainsKey("Accept") ? routeContext.HttpContext.Request.Headers["Accept"].ToString() : null;
            if (accept == null)
            {
                return false;
            }

            var acceptWithoutFormat = accept;
            if (accept.Contains("+", StringComparison.OrdinalIgnoreCase))
            {
                acceptWithoutFormat = accept.Substring(0, accept.IndexOf("+", StringComparison.OrdinalIgnoreCase));
            }

            return acceptWithoutFormat == _acceptType;
        }
    }
}
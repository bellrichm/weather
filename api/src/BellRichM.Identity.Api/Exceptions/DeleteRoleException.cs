using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
	[Serializable]
    [ExcludeFromCodeCoverage]
	public class DeleteRoleException : RoleException
	{
		[ExcludeFromCodeCoverage]   
		public DeleteRoleException(string code) : base(code) {}

		[ExcludeFromCodeCoverage]   
		public DeleteRoleException(string code, string message) : base(code, message) {}

		[ExcludeFromCodeCoverage]   
		public DeleteRoleException(string code, string message, Exception innerException) : base(code, message, innerException) {}

		protected DeleteRoleException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}    
}
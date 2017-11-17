using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
	[Serializable]
	[ExcludeFromCodeCoverage]
	public class CreateRoleException : RoleException
	{
		public CreateRoleException(string code) : base(code) {}

		public CreateRoleException(string code, string message) : base(code, message) {}

		public CreateRoleException(string code, string message, Exception innerException) : base(code, message, innerException) {}

		protected CreateRoleException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}    
}
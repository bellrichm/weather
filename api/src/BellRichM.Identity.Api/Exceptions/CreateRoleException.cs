using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
	[Serializable]
	public class CreateRoleException : Exception
	{
   		public string Code {get;}

		[ExcludeFromCodeCoverage]   
		public CreateRoleException(string code) : base()
		{
            	Code = code;
		}

		[ExcludeFromCodeCoverage]   
		public CreateRoleException(string code, string message) : base(message)
		{
            	Code = code;
		}

		[ExcludeFromCodeCoverage]   
		public CreateRoleException(string code, string message, Exception innerException) : base(message, innerException)
		{
            	Code = code;
		}

		protected CreateRoleException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
            	Code = info.GetString("Code");
		}


		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
            	if (info == null)
                		throw new ArgumentNullException("info");
			            
            	base.GetObjectData(info, context);
            	info.AddValue("Code", Code);
		}
	}    
}
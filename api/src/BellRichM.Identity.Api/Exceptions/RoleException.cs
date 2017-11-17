using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
	[Serializable]
	public abstract class RoleException : Exception
	{
   		public string Code {get;}

		[ExcludeFromCodeCoverage]   
		public RoleException(string code) : base()
		{
            	Code = code;
		}

		[ExcludeFromCodeCoverage]   
		public RoleException(string code, string message) : base(message)
		{
            	Code = code;
		}

		[ExcludeFromCodeCoverage]   
		public RoleException(string code, string message, Exception innerException) : base(message, innerException)
		{
            	Code = code;
		}

		protected RoleException(SerializationInfo info, StreamingContext context) : base(info, context)
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
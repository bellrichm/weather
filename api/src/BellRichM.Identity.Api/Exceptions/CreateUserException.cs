using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
  [Serializable]
  [ExcludeFromCodeCoverage]
  public class CreateUserException : RoleException
  {
    public CreateUserException(string code)
      : base(code)
      {
      }

    public CreateUserException(string code, string message)
      : base(code, message)
      {
      }

    public CreateUserException(string code, string message, Exception innerException)
      : base(code, message, innerException)
      {
      }

    protected CreateUserException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      {
      }
  }
}
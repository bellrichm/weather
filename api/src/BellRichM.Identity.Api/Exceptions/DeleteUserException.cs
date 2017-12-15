using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
  [Serializable]
  [ExcludeFromCodeCoverage]
  public class DeleteUserException : RoleException
  {
    public DeleteUserException(string code)
      : base(code)
      {
      }

    public DeleteUserException(string code, string message)
      : base(code, message)
      {
      }

    public DeleteUserException(string code, string message, Exception innerException)
      : base(code, message, innerException)
      {
      }

    protected DeleteUserException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      {
      }
  }
}
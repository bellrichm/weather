using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// The exception thrown when unable to delete a user.
  /// </summary>
  /// <seealso cref="RoleException" />
  [Serializable]
  [ExcludeFromCodeCoverage]
  public class DeleteUserException : RoleException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    public DeleteUserException(string code)
      : base(code)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    public DeleteUserException(string code, string message)
      : base(code, message)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public DeleteUserException(string code, string message, Exception innerException)
      : base(code, message, innerException)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    protected DeleteUserException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      {
      }
  }
}
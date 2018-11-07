using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;
using BellRichM.Exceptions;

namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// The exception thrown when unable to create a role.
  /// </summary>
  /// <seealso cref="BusinessException" />
  [Serializable]
  [ExcludeFromCodeCoverage]
  public class CreateRoleException : BusinessException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    public CreateRoleException(string code)
      : base(code)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    public CreateRoleException(string code, string message)
      : base(code, message)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public CreateRoleException(string code, string message, Exception innerException)
      : base(code, message, innerException)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="exceptionDetails">Additional details about the exception.</param>
    public CreateRoleException(string code, IEnumerable<ExceptionDetail> exceptionDetails)
      : base(code, exceptionDetails)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="exceptionDetails">Additional details about the exception.</param>
    /// <param name="message">The message describing the exception.</param>
    public CreateRoleException(string code, IEnumerable<ExceptionDetail> exceptionDetails, string message)
      : base(code, exceptionDetails, message)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="exceptionDetails">Additional details about the exception.</param>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public CreateRoleException(string code, IEnumerable<ExceptionDetail> exceptionDetails, string message, Exception innerException)
      : base(code, exceptionDetails, message, innerException)
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    protected CreateRoleException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      {
      }
  }
}
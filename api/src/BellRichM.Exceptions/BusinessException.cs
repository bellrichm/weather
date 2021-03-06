using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Exceptions
{
  /// <summary>
  /// The base exception class for the weather APIs.
  /// </summary>
  /// <seealso cref="System.Exception" />
  [Serializable]

  public abstract class BusinessException : Exception
  {
    [NonSerialized]
    private string _code;
    [NonSerialized]
    private IEnumerable<ExceptionDetail> _errorDetails;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    protected BusinessException()
            : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string code)
            : base()
    {
      _code = code;
      _errorDetails = new List<ExceptionDetail>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string code, string message)
            : base(message)
    {
      _code = code;
      _errorDetails = new List<ExceptionDetail>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string code, string message, Exception innerException)
            : base(message, innerException)
    {
      _code = code;
      _errorDetails = new List<ExceptionDetail>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="errorDetails">Additional details about the exception.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string code, IEnumerable<ExceptionDetail> errorDetails)
            : base()
    {
      _code = code;
      _errorDetails = errorDetails;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="errorDetails">Additional details about the exception.</param>
    /// <param name="message">The message describing the exception.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string code, IEnumerable<ExceptionDetail> errorDetails, string message)
            : base(message)
    {
      _code = code;
      _errorDetails = errorDetails;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string message, Exception innerException)
            : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="errorDetails">Additional details about the exception.</param>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    [ExcludeFromCodeCoverage]
    protected BusinessException(string code, IEnumerable<ExceptionDetail> errorDetails, string message, Exception innerException)
            : base(message, innerException)
    {
      _code = code;
      _errorDetails = errorDetails;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    protected BusinessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
      _code = info.GetString("Code");
      _errorDetails = new List<ExceptionDetail>();
      _errorDetails = (IEnumerable<ExceptionDetail>)info.GetValue("ErrorDetails", _errorDetails.GetType());
    }

    /// <summary>
    /// Gets the code.
    /// </summary>
    /// <value>
    /// The code.
    /// </value>
    public string Code
    {
        get { return _code; }
    }

    /// <summary>
    /// Gets the error details.
    /// </summary>
    /// <value>
    /// The errors.
    /// </value>
    public IEnumerable<ExceptionDetail> ErrorDetails
    {
        get { return _errorDetails; }
    }

    /// <summary>
    /// Sets the <see cref="SerializationInfo"></see> with information about the exception.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    /// <exception cref="ArgumentNullException">Thrown when ???.</exception>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException(nameof(info));
      }

      base.GetObjectData(info, context);
      info.AddValue("Code", Code);
      info.AddValue("ErrorDetails", ErrorDetails);
    }
  }
}

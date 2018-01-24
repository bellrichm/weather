using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// The base exception class for the weather APIs
  /// </summary>
  /// <seealso cref="System.Exception" />
  [Serializable]
  public abstract class RoleException : Exception
  {
    #pragma warning disable CA2235 // TODO: Investigate, seems to be a false positive
    [NonSerialized]
    private string _code;
    #pragma warning restore CA2235

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    [ExcludeFromCodeCoverage]
    public RoleException(string code)
            : base()
    {
      _code = code;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    [ExcludeFromCodeCoverage]
    public RoleException(string code, string message)
            : base(message)
    {
      _code = code;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleException"/> class.
    /// </summary>
    /// <param name="code">The code that provides additional detail.</param>
    /// <param name="message">The message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    [ExcludeFromCodeCoverage]
    public RoleException(string code, string message, Exception innerException)
            : base(message, innerException)
    {
      _code = code;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    protected RoleException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
      _code = info.GetString("Code");
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
    /// Sets the <see cref="SerializationInfo"></see> with information about the exception.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    /// <exception cref="ArgumentNullException">Thrown when ???</exception>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }

      base.GetObjectData(info, context);
      info.AddValue("Code", Code);
    }
  }
}
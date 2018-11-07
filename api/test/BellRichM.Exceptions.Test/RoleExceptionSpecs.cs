using BellRichM.Exceptions;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using It = Machine.Specifications.It;

#pragma warning disable SA1649 // File name should match first type name
namespace BellRichM.Api.Exceptions.Test
{
  internal class When_info_argument_is_null
  {
    protected static RoleExceptionTestClass businessException;
    protected static Exception thrownException;
    Establish context = () =>
      businessException = new RoleExceptionTestClass("code");

    Because of = () =>
      thrownException = Catch.Exception(() => businessException.GetObjectData(null, default(StreamingContext)));

    It should_throw_expected_exception = () =>
      thrownException.ShouldBeOfExactType<ArgumentNullException>();
  }

  internal class When_serializing_deserializing_RoleException
  {
    protected static BusinessException originalException;
    protected static BusinessException deserializedException;
    protected static MemoryStream serializedStream;
    protected static BinaryFormatter formatter;
    Establish context = () =>
    {
      var innerEx = new Exception("foo");
      originalException = new RoleExceptionTestClass("code", "message", innerEx);
      serializedStream = new MemoryStream(new byte[4096]);
      formatter = new BinaryFormatter();
    };

    Cleanup after = () =>
      serializedStream.Dispose();

    Because of = () =>
    {
      formatter.Serialize(serializedStream, originalException);
      serializedStream.Position = 0;
      deserializedException = (RoleExceptionTestClass)formatter.Deserialize(serializedStream);
    };

    It should_have_correct_Code = () =>
      originalException.Code.ShouldEqual(deserializedException.Code);

    It should_have_correct_Message = () =>
      originalException.Message.ShouldEqual(deserializedException.Message);

    It should_have_correct_innerException_Message = () =>
      originalException.InnerException.Message.ShouldEqual(deserializedException.InnerException.Message);
  }

  [Serializable]
  #pragma warning disable S3376 // Class name should end with exception

  internal class RoleExceptionTestClass : BusinessException
  #pragma warning restore S3376 // Class name should end with exception
  {
    public RoleExceptionTestClass(string code)
      : base(code)
      {
      }

    public RoleExceptionTestClass(string code, string message)
      : base(code, message)
      {
      }

    public RoleExceptionTestClass(string code, string message, Exception innerException)
      : base(code, message, innerException)
      {
      }

    protected RoleExceptionTestClass(SerializationInfo info, StreamingContext context)
      : base(info, context)
      {
      }
  }
}
#pragma warning restore SA1649 // File name should match first type name

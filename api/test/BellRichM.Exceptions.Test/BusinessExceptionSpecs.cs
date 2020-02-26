using Machine.Specifications;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using It = Machine.Specifications.It;

namespace BellRichM.Exceptions.Test
{
  internal class BusinessExceptionSpecs
  {
  }

  internal class When_info_argument_is_null : BusinessExceptionSpecs
  {
    protected static TestRoleException businessException;
    protected static Exception thrownException;
    Establish context = () =>
      businessException = new TestRoleException("code");

    Because of = () =>
      thrownException = Catch.Exception(() => businessException.GetObjectData(null, default(StreamingContext)));

    It should_throw_expected_exception = () =>
      thrownException.ShouldBeOfExactType<ArgumentNullException>();
  }

  internal class When_serializing_deserializing_RoleException : BusinessExceptionSpecs
  {
    protected static BusinessException originalException;
    protected static BusinessException deserializedException;
    protected static MemoryStream serializedStream;
    protected static BinaryFormatter formatter;
    Establish context = () =>
    {
      var innerEx = new Exception("foo");
      originalException = new TestRoleException("code", "message", innerEx);
      serializedStream = new MemoryStream(new byte[4096]);
      formatter = new BinaryFormatter();
    };

    Cleanup after = () =>
      serializedStream.Dispose();

    Because of = () =>
    {
      formatter.Serialize(serializedStream, originalException);
      serializedStream.Position = 0;
      deserializedException = (TestRoleException)formatter.Deserialize(serializedStream);
    };

    It should_have_correct_Code = () =>
      originalException.Code.ShouldEqual(deserializedException.Code);

    It should_have_correct_Message = () =>
      originalException.Message.ShouldEqual(deserializedException.Message);

    It should_have_correct_innerException_Message = () =>
      originalException.InnerException.Message.ShouldEqual(deserializedException.InnerException.Message);
  }

  [Serializable]
  internal class TestRoleException : BusinessException
  {
    public TestRoleException()
      : base()
      {
      }

    public TestRoleException(string code)
      : base(code)
      {
      }

    public TestRoleException(string code, string message)
      : base(code, message)
      {
      }

    public TestRoleException(string message, Exception innerException)
      : base(message, innerException)
      {
      }

    public TestRoleException(string code, string message, Exception innerException)
      : base(code, message, innerException)
      {
      }

    protected TestRoleException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      {
      }
  }
}
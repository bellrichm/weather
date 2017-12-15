using FluentAssertions;
using Machine.Specifications;
using Moq;

using It = Machine.Specifications.It;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using BellRichM.Identity.Api.Exceptions;

namespace BellRichM.Identity.Api.Test.Exceptions
{
    internal class when_info_argument_is_null
    {
        protected static RoleExceptionTestClass roleException;
        protected static Exception thrownException;
        Establish context = () =>
            roleException = new RoleExceptionTestClass("code");

        Because of = () =>
              thrownException = Catch.Exception(() => roleException.GetObjectData(null, new StreamingContext()));

    	It should_throw_expected_exception = () =>
			thrownException.ShouldBeOfExactType<ArgumentNullException>();
    }

    internal class when_serializing_deserializing_RoleException
    {
        protected static RoleException originalException;
        protected static RoleException deserializedException;
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
	public class RoleExceptionTestClass : RoleException
	{
		public RoleExceptionTestClass(string code) : base(code) {}

		public RoleExceptionTestClass(string code, string message) : base(code, message) {}

		public RoleExceptionTestClass(string code, string message, Exception innerException) : base(code, message, innerException) {}

		protected RoleExceptionTestClass(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}
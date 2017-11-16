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
        protected static CreateRoleException createRoleException;
        protected static Exception thrownException;
        Establish context = () =>
            createRoleException = new CreateRoleException("code");

        Because of = () =>
              thrownException = Catch.Exception(() => createRoleException.GetObjectData(null, new StreamingContext()));

    	It should_throw_expected_exception = () =>    	
			thrownException.ShouldBeOfExactType<ArgumentNullException>();	
    }

    internal class when_serializing_deserializing_CreateRoleException
    {
        protected static CreateRoleException originalException;
        protected static CreateRoleException deserializedException;
        protected static MemoryStream serializedStream;
        protected static BinaryFormatter formatter;
        Establish context = () =>
        {
            var innerEx = new Exception("foo");
            originalException = new CreateRoleException("code", "message", innerEx);
            serializedStream = new MemoryStream(new byte[4096]);
            formatter = new BinaryFormatter();
        };

        Because of = () =>
        {
            formatter.Serialize(serializedStream, originalException);
            serializedStream.Position = 0;
            deserializedException = (CreateRoleException)formatter.Deserialize(serializedStream); 
        };

        It should_have_correct_Code = () =>
            originalException.Code.ShouldEqual(deserializedException.Code);

        It should_have_correct_Message = () =>
            originalException.Message.ShouldEqual(deserializedException.Message);
        

        It should_have_correct_innerException_Message = () =>
            originalException.InnerException.Message.ShouldEqual(deserializedException.InnerException.Message);
    }
}

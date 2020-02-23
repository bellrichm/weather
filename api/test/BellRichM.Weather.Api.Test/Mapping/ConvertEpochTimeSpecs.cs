using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Mapping;
using BellRichM.Weather.Api.Models;
using FluentAssertions;
using Machine.Specifications;

using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Mapping.Test
{
    public class ConvertEpochTimeSpecs
    {
        protected static readonly int Year = 2001;
        protected static readonly int Month = 9;
        protected static readonly int Day = 1;
        protected static readonly int Hour = 1;
        protected static readonly int Minute = 5;
        protected static readonly int DateTime = 999306300;

        protected static ObservationModel observationModel;
        protected static Observation observation;

        protected static ConvertEpochTime convertEpochTime;

        Establish context = () =>
        {
            observationModel = new ObservationModel
            {
                DateTime = DateTime
            };

            observation = new Observation
            {
                Year = -1,
                Month = -1,
                Day = -1,
                Hour = -1,
                Minute = -1
            };

            convertEpochTime = new ConvertEpochTime();
        };
    }

    internal class When_converting_epoch_fime : ConvertEpochTimeSpecs
    {
        Because of = () =>
            convertEpochTime.Process(observationModel, observation, null);

        It should_have_correct_year = () =>
            observation.Year.Should().Equals(Year);

        It should_have_correct_month = () =>
            observation.Year.Should().Equals(Month);

        It should_have_correct_day = () =>
            observation.Year.Should().Equals(Day);

        It should_have_correct_hour = () =>
            observation.Year.Should().Equals(Hour);

        It should_have_correct_minute = () =>
            observation.Year.Should().Equals(Minute);
    }
}

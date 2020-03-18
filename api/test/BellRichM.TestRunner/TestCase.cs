using System;
using System.Collections.Generic;

namespace BellRichM.TestRunner
{
    public class TestCase
    {
        public TestCase()
        {
            ItDelegates = new List<Delegate>();
            LoggingBehaviors = new List<object>();
        }

        public Delegate EstablishDelegate { get; set; }

        public Delegate BecauseDelegate { get; set; }

        public List<Delegate> ItDelegates { get; }

        public List<object> LoggingBehaviors { get; }
    }
}
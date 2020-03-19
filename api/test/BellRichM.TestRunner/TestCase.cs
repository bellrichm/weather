using System;
using System.Collections.Generic;

namespace BellRichM.TestRunner
{
    public class TestCase
    {
        public TestCase()
        {
            EstablishDelegates = new List<Delegate>();
            ItDelegates = new List<Delegate>();
            LoggingBehaviors = new List<object>();
        }

        public List<Delegate> EstablishDelegates { get; }

        public Delegate BecauseDelegate { get; set; }

        public List<Delegate> ItDelegates { get; }

        public List<object> LoggingBehaviors { get; }
    }
}
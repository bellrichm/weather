using System;
using System.Collections.Generic;

namespace BellRichM.TestRunner
{
    public class TestCase
    {
        public TestCase()
        {
            EstablishDelegates = new List<Delegate>();
            ItDelegatesDetail = new List<DelegateDetail>();
            LoggingBehaviors = new List<object>();
        }

        public List<Delegate> EstablishDelegates { get; }

        public Delegate BecauseDelegate { get; set; }

        public List<DelegateDetail> ItDelegatesDetail { get; }

        public List<object> LoggingBehaviors { get; }
    }
}
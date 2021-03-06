﻿using DayZServerMonitorCore;
using System;
using System.Collections.Generic;

namespace TestDayZServerMonitorCore
{
    internal class MockLogger : ILogger
    {
        public List<string> StatusTexts { get; }
        public List<string> ErrorTexts { get; }
        public List<string> DebugTexts { get; }
        public List<Exception> ErrorExceptions { get; }

        public MockLogger()
        {
            StatusTexts = new List<string>();
            ErrorTexts = new List<string>();
            DebugTexts = new List<string>();
            ErrorExceptions = new List<Exception>();
        }

        public void Status(string text)
        {
            StatusTexts.Add(text);
        }

        public void Error(string text, Exception exception)
        {
            ErrorTexts.Add(text);
            ErrorExceptions.Add(exception);
        }

        public void Debug(string text)
        {
            DebugTexts.Add(text);
        }
    }
}

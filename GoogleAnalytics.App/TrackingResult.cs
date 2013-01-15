using System;
using System.Collections.Generic;

namespace GoogleAnalytics.App
{
    public class TrackingResult
    {
        public string Url { get; set; }
        public Dictionary<string,string> Parameters { get; set; }
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}
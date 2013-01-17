using System;
using System.Collections.Generic;
using System.Globalization;

namespace GoogleAnalytics.App
{
    public partial class Tracker
    {
        const string EndpointUrl = "http://www.google-analytics.com/collect";
        const string EndpointUrlSsl = "https://ssl.google-analytics.com/collect";
        const string ProtocolVersion = "1";

        public string TrackingId { get; set; } // utmac
        public string ClientId { get; set; }
        public string AppScreen { get; set; }
        public bool SessionStart { get; set; }
        public IAnalyticsSession AnalyticsSession { get; set; }

        public string AppName { get; set; }
        public string AppId { get; set; }
        public string AppVersion { get; set; }

        public string Language { get; set; }
        public string UserAgent { get; set; }

        public string OsPlatform { get; set; }
        public string OsVersion { get; set; }
        public string DeviceName { get; set; }
        public string ScreenResolution { get; set; }
        public string ScreenColors { get; set; }

        public List<string> CustomDimensions { get; set; }
        public List<Int64?> CustomMetrics { get; set; } 

        public bool ThrowOnErrors { get; set; }

        public bool UseHttps { get; set; }

        public bool Anonymize { get; set; }

        public Tracker(string trackingId, IAnalyticsSession analyticsSession)
        {
            TrackingId = trackingId;
            SessionStart = true;
            AnalyticsSession = analyticsSession;

            ClientId = AnalyticsSession.GetClientId();
            AppName = AnalyticsSession.GetAppName();
            AppVersion = AnalyticsSession.GetAppVersion();
            ScreenResolution = AnalyticsSession.GetScreenResolution();
            ScreenColors = AnalyticsSession.GetScreenColors();
            OsPlatform = AnalyticsSession.GetOsPlatform();
            OsVersion = AnalyticsSession.GetOsVersion();
            DeviceName = AnalyticsSession.GetDeviceName();

            Language = CultureInfo.CurrentUICulture.Name;
            UserAgent = string.Format("GoogleAnalytics/2.0 ({0}; U; {1}; {2}; {3})", OsPlatform, OsVersion, Language, DeviceName);

            ThrowOnErrors = false;

            CustomDimensions = new List<string>();
            CustomMetrics = new List<Int64?>();
        }

        private static string GenerateCacheBreaker()
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            return random.Next(100000000, 999999999).ToString(CultureInfo.InvariantCulture);
        }

        public void SetCustom(int index, string dimension)
        {
            if (index < 1 || index > 200)
                throw new ArgumentOutOfRangeException(string.Format("position {0} - {1}", index, "Must be between 1 and 200"));

            CustomDimensions.Insert(index-1, dimension);
        }
        
        public void SetCustom(int index, Int64 metric)
        {
            if (index < 1 || index > 200)
                throw new ArgumentOutOfRangeException(string.Format("position {0} - {1}", index, "Must be between 1 and 200"));

            CustomMetrics.Insert(index - 1, metric);
        }

        private Dictionary<string,string> GetCustomParameters()
        {
            var parameters = new Dictionary<string, string>();

            for (var i=0; i < CustomDimensions.Count; i++)
            {   
                if (!string.IsNullOrEmpty(CustomDimensions[i]))
                {
                    parameters.Add(string.Format("cd{0}", i+1), CustomDimensions[i]);
                }
            }

            for (var i=0; i < CustomMetrics.Count; i++)
            {
                if (CustomMetrics[i] != null)
                {
                    parameters.Add(string.Format("cm{0}", i+1), CustomMetrics[i].Value.ToString(CultureInfo.InvariantCulture));
                }
            }

            return parameters;
        }
    }
}

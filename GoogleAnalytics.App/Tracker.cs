using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public IAnalyticsSession AnalyticsSession { get; set; }

        public string AppName { get; set; }
        public string AppId { get; set; }
        public string AppVersion { get; set; }

        public string Language { get; set; }
        public string UserAgent { get; set; }
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
            AnalyticsSession = analyticsSession;

            ClientId = AnalyticsSession.GetClientId();
            AppName = AnalyticsSession.GetAppName();
            AppVersion = AnalyticsSession.GetAppVersion();
            ScreenResolution = AnalyticsSession.GetScreenResolution();
            ScreenColors = AnalyticsSession.GetScreenColors();

#if WINDOWS_PHONE
            const string osplatform = "Windows Phone";
            var osversion = Environment.OSVersion.Version.ToString();
#else
            const string osplatform = "Windows RT";
            const string osversion = "8";
#endif

            Language = CultureInfo.CurrentUICulture.Name;
            UserAgent = string.Format("GoogleAnalytics/2.0b3 ({0}; U; {1}; {2})", osplatform, osversion, Language);

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

        private static byte[] GetRequestBytes(Dictionary<string,string> postParameters) {
            if (postParameters == null || postParameters.Count == 0)
                return new byte[0];
            var sb = new StringBuilder();
            foreach (var parameter in postParameters)
                sb.Append(Uri.EscapeUriString(parameter.Key) + "=" + Uri.EscapeUriString(parameter.Value) + "&");
            sb.Length = sb.Length - 1;
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private async Task<TrackingResult> RequestUrlAsync(string url, Dictionary<string, string> parameters) {
            var requestBytes = GetRequestBytes(parameters);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.UserAgent = UserAgent;
            WebResponse response = null;
            try
            {
                using (var oStream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
                {
                    oStream.Write(requestBytes, 0, requestBytes.Length);
                }
                response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                return new TrackingResult
                {
                    Url = url,
                    Parameters = parameters,
                    Success = true
                };
            }
            catch (Exception e)
            {
                if (ThrowOnErrors)
                    throw;

                return new TrackingResult
                {
                    Url = url,
                    Parameters = parameters,
                    Success = false,
                    Exception = e
                };
            }
            finally
            {
                var disposableResult = response as IDisposable;
                if (disposableResult != null)
                {
                    disposableResult.Dispose();
                }
            }
        }
    }
}

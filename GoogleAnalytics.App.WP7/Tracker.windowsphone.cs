using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAnalytics.App
{
    public partial class Tracker
    {
#if WINDOWS_PHONE
        public Tracker(string trackingId)
            : this(trackingId, new WindowsPhoneAnalyticsSession())
        {
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
            try {
                using (var oStream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null)) {
                    oStream.Write(requestBytes, 0, requestBytes.Length);
                }
                response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                return new TrackingResult {
                    Url = url,
                    Parameters = parameters,
                    Success = true
                };
            } catch (Exception e) {
                if (ThrowOnErrors)
                    throw;

                return new TrackingResult {
                    Url = url,
                    Parameters = parameters,
                    Success = false,
                    Exception = e
                };
            } finally {
                var disposableResult = response as IDisposable;
                if (disposableResult != null) {
                    disposableResult.Dispose();
                }
            }
        }

#endif
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleAnalytics.App {
    public partial class Tracker {
#if NETFX_CORE
        public Tracker(string trackingId)
            : this(trackingId, new WindowsRTAnalyticsSession()) {
        }

        private async Task<TrackingResult> RequestUrlAsync(string url, Dictionary<string, string> parameters) {
            var request = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            message.Headers.Add("User-Agent", UserAgent);
            message.Content = new FormUrlEncodedContent(parameters);
            
            HttpResponseMessage response = null;
            try {
                response = await request.SendAsync(message);
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

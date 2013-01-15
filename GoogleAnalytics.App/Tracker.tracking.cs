using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleAnalytics.App
{
    public partial class Tracker
    {
        public async Task<TrackingResult> TrackViewAsync(string screen)
        {
            AppScreen = screen;

            var parameters = GetDefaultParameters();
            parameters.Add("t", "appview");
            parameters.Add("z", GenerateCacheBreaker());

            return await RequestUrlAsync(UseHttps ? EndpointUrlSsl : EndpointUrl, parameters);
        }

        public async Task<TrackingResult> TrackEventAsync(string category, string action, string label = null, Int64? value = null)
        {
            var parameters = GetDefaultParameters();
            parameters.Add("t", "event");

            if (!string.IsNullOrEmpty(category)) parameters.Add("ec", category);
            if (!string.IsNullOrEmpty(action)) parameters.Add("ea", action);
            if (!string.IsNullOrEmpty(label)) parameters.Add("el", label);
            if (value != null) parameters.Add("ev", value.Value.ToString(CultureInfo.InvariantCulture));
            
            parameters.Add("z", GenerateCacheBreaker());

            return await RequestUrlAsync(UseHttps ? EndpointUrlSsl : EndpointUrl, parameters);
        }

        public async Task<TrackingResult[]> TrackTransactionAsync(Transaction tran)
        {
            var parameters = GetDefaultParameters();
            parameters.Add("t", "tran");
            parameters.Add("ti", tran.OrderId);

            if (!string.IsNullOrEmpty(tran.StoreName)) parameters.Add("ta", tran.Shipping);
            if (!string.IsNullOrEmpty(tran.Total)) parameters.Add("tr", tran.Total);
            if (!string.IsNullOrEmpty(tran.Shipping)) parameters.Add("ts", tran.Shipping);
            if (!string.IsNullOrEmpty(tran.Tax)) parameters.Add("tt", tran.Tax);

            parameters.Add("z", GenerateCacheBreaker());

            var tasks = new List<Task<TrackingResult>> { RequestUrlAsync(UseHttps ? EndpointUrlSsl : EndpointUrl, parameters) };
            if (tran.Items != null)
            {
                tasks.AddRange(tran.Items.Select(item => TrackTransactionItemAsync(tran.OrderId, item)));
            }
            return await TaskEx.WhenAll(tasks);
        }

        private async Task<TrackingResult> TrackTransactionItemAsync(string orderId, TransactionItem item)
        {
            var parameters = GetDefaultParameters();
            parameters.Add("t", "item");
            parameters.Add("ti", orderId);

            if (!string.IsNullOrEmpty(item.Price)) parameters.Add("ip", item.Price);
            if (item.Quantity != null) parameters.Add("iq", item.Quantity.Value.ToString(CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(item.Code)) parameters.Add("ic", item.Code);
            if (!string.IsNullOrEmpty(item.Name)) parameters.Add("in", item.Name);
            if (!string.IsNullOrEmpty(item.Category)) parameters.Add("iv", item.Category);

            parameters.Add("z", GenerateCacheBreaker());

            return await RequestUrlAsync(UseHttps ? EndpointUrlSsl : EndpointUrl, parameters);
        }

        private Dictionary<string,string> GetDefaultParameters() {
            var parameters = new Dictionary<string,string>
            {
                { "ul", Language },
                { "_v", "mi1b3" },
                { "cid", ClientId },
                { "cd", AppScreen },
                { "tid", TrackingId },
                { "v", ProtocolVersion },
                { "qt", "0" }
            };

            if (Anonymize) parameters.Add("aip", "1");
            if (!string.IsNullOrEmpty(AppName)) parameters.Add("an", AppName);
            if (!string.IsNullOrEmpty(AppVersion)) parameters.Add("av", AppVersion);
            if (!string.IsNullOrEmpty(ScreenColors)) parameters.Add("sd", ScreenColors);
            if (!string.IsNullOrEmpty(ScreenResolution)) parameters.Add("sr", ScreenResolution);

            foreach (var parameter in GetCustomParameters())
            {
                parameters.Add(parameter.Key, parameter.Value);
            }

            return parameters;
        }
    }
}
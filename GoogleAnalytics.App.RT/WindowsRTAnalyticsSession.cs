using System;
using Windows.Storage;
using Windows.UI.Xaml;

namespace GoogleAnalytics.App
{
    public class WindowsRTAnalyticsSession
        : IAnalyticsSession
    {
        private const string StorageKeyUniqueId = "GoogleAnalytics.ClientID";

        private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        public string GetClientId()
        {
            if (!_settings.Values.ContainsKey(StorageKeyUniqueId))
            {
                _settings.Values.Add(StorageKeyUniqueId, Guid.NewGuid().ToString());
            }
            return (string)_settings.Values[StorageKeyUniqueId];
        }

        public string GetAppName()
        {
            return ManifestAppInfo.Name;
        }

        public string GetAppVersion()
        {

            return ManifestAppInfo.Version;
        }

        public string GetScreenResolution()
        {
            return string.Format("{0}x{1}", Window.Current.Bounds.Width, Window.Current.Bounds.Height);
        }

        public string GetScreenColors()
        {
            return null;
        }

        public string GetOsPlatform()
        {
            return "Windows";
        }

        public string GetOsVersion()
        {
            return "Windows NT 6.2";
        }

        public string GetDeviceName()
        {
            var deviceInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            return string.IsNullOrEmpty(deviceInfo.SystemSku) ? string.Format("{0} {1}", deviceInfo.SystemManufacturer, deviceInfo.SystemProductName) : deviceInfo.SystemSku;
        }
    }
}

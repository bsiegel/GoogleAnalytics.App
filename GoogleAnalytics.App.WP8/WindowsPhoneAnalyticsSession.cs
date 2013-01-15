using System;
using System.IO.IsolatedStorage;

namespace GoogleAnalytics.App
{
    public class WindowsPhoneAnalyticsSession
        : IAnalyticsSession
    {
        private const string StorageKeyUniqueId = "GoogleAnalytics.ClientID";

        private readonly IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;

        public string GetClientId()
        {
            if (!_settings.Contains(StorageKeyUniqueId))
            {
                _settings.Add(StorageKeyUniqueId, Guid.NewGuid().ToString());
            }
            return (string)_settings[StorageKeyUniqueId];
        }

        public string GetAppName()
        {
            return ManifestAppInfo.Title;
        }

        public string GetAppVersion()
        {

            return ManifestAppInfo.Version;
        }

        public string GetScreenResolution()
        {
            return string.Format("{0}x{1}", System.Windows.Application.Current.Host.Content.ActualWidth, System.Windows.Application.Current.Host.Content.ActualHeight);
        }

        public string GetScreenColors()
        {
            return string.Format("{0}-bit", ManifestAppInfo.BitsPerPixel ?? "24");
        }
    }
}

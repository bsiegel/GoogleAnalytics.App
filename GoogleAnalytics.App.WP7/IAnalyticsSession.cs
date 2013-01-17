namespace GoogleAnalytics.App
{
    public interface IAnalyticsSession
    {
        string GetClientId();

        string GetAppName();

        string GetAppVersion();

        string GetScreenResolution();

        string GetScreenColors();

        string GetOsPlatform();

        string GetOsVersion();

        string GetDeviceName();
    }
}
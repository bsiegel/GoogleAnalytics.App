namespace GoogleAnalytics.App
{
    public partial class Tracker
    {
#if WINDOWS_PHONE
        public Tracker(string trackingId)
            : this(trackingId, new WindowsPhoneAnalyticsSession())
        {
        }
#endif
    }
}

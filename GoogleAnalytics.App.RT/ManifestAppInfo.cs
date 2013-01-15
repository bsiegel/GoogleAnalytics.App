using System.Collections.Generic;
using System.Xml.Linq;

namespace GoogleAnalytics.App
{
    public class ManifestAppInfo
    {
        private static readonly Dictionary<string, string> Properties;

        static ManifestAppInfo()
        {
            Properties = new Dictionary<string, string>();
            var appManifestXml = XDocument.Load("Package.appxmanifest");
            using (var rdr = appManifestXml.CreateReader(ReaderOptions.None))
            {
                rdr.ReadToDescendant("Identity");
                if (!rdr.IsStartElement()) return;

                rdr.MoveToFirstAttribute();
                while (rdr.MoveToNextAttribute())
                {
                    Properties.Add(rdr.Name, rdr.Value);
                }
            }
        }

        public static string Name
        {
            get
            {
                string appName;
                Properties.TryGetValue("Name", out appName);
                return appName;
            }
        }

        public static string Version
        {
            get
            {
                string appVersion;
                Properties.TryGetValue("Version", out appVersion);
                return appVersion;
            }
        }
    }
}
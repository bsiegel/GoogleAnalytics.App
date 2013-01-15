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
            var appManifestXml = XDocument.Load("WMAppManifest.xml");
            using (var rdr = appManifestXml.CreateReader(ReaderOptions.None))
            {
                rdr.ReadToDescendant("App");
                if (!rdr.IsStartElement()) return;

                rdr.MoveToFirstAttribute();
                while (rdr.MoveToNextAttribute())
                {
                    Properties.Add(rdr.Name, rdr.Value);
                }
            }
        }

        public static string Title
        {
            get
            {
                string appName;
                Properties.TryGetValue("Title", out appName);
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

        public static string BitsPerPixel
        {
            get
            {
                string colorDepth;
                Properties.TryGetValue("BitsPerPixel", out colorDepth);
                return colorDepth;
            }
        }
    }
}
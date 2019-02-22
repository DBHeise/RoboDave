using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace RoboDave.Forensic
{

    public class BrowserHistory
    {
        public String User { get; set; }
        public String Browser { get; set; }
        //public DateTime Timestamp { get; set; }
        public String Url { get; set; }
    }

    [Cmdlet(VerbsCommon.Get, "BrowserHistory", SupportsShouldProcess = true)]
    [OutputType(typeof(BrowserHistory[]))]
    public class BrowserHistoryCmdlet : PSCmdlet
    {

        [Parameter(Position = 0, Mandatory = false, HelpMessage = "Which browser to use (default: all)")]
        [ValidateSet("all", "ie", "chrome", "firefox")]
        public String Browser { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.Browser)
            {
                case "ie":
                    GetIETypedUrls();
                    break;
                case "chrome":
                    GetChromeHistory();
                    break;
                case "firefox":
                    GetFirefoxHistory();
                    break;
                default:
                    GetIETypedUrls();
                    GetChromeHistory();
                    GetFirefoxHistory();
                    break;
            }
        }


        private static String ieregexUserMatch = @"S-1-5-21-[0-9]+-[0-9]+-[0-9]+-[0-9]+$";
        private void GetIETypedUrls()
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(ieregexUserMatch);

            foreach (String subkey in Microsoft.Win32.Registry.Users.GetSubKeyNames())
            {
                if (regex.IsMatch(subkey))
                {
                    var user = new System.Security.Principal.SecurityIdentifier(subkey).Translate(typeof(System.Security.Principal.NTAccount));
                    using (var key = Microsoft.Win32.Registry.Users.OpenSubKey(subkey + "\\Software\\Microsoft\\Internet Explorer\\TypedURLs"))
                    {
                        foreach (String valName in key.GetValueNames())
                        {
                            String val = key.GetValue(valName) as String;
                            var o = new BrowserHistory()
                            {
                                User = user.Value,
                                Browser = "IE_Typed",
                                Url = val
                            };
                            WriteObject(o);
                        }
                    }
                }
            }
        }


        private static String safeReadFile(String path)
        {
            String ans = null;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(file))
            {
                ans = reader.ReadToEnd();
            }
            return ans;
        }


        private static String chromeHistoryMatch = @"(htt(p|s))://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)*?";
        private void GetChromeHistory()
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(chromeHistoryMatch);
            String baseUserFolder = Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\Users");
            foreach (var userfolder in System.IO.Directory.GetDirectories(baseUserFolder))
            {
                String user = userfolder.Replace(baseUserFolder, "").Replace("\\", "");
                String folder = System.IO.Path.Combine(userfolder, "AppData", "Local", "Google", "Chrome", "User Data", "Default");
                if (System.IO.Directory.Exists(folder))
                {
                    String history = safeReadFile(System.IO.Path.Combine(folder, "History"));
                    foreach (System.Text.RegularExpressions.Match match in regex.Matches(history))
                    {
                        var o = new BrowserHistory()
                        {
                            User = user,
                            Browser = "Chrome",
                            Url = match.Value
                        };
                        WriteObject(o);
                    }

                }
            }
        }

        private static String firefoxHistoryMatch = @"(htt(p|s))://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)*?";
        private void GetFirefoxHistory()
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(firefoxHistoryMatch);
            String baseUserFolder = Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\Users");
            foreach (var userfolder in System.IO.Directory.GetDirectories(baseUserFolder))
            {
                String user = userfolder.Replace(baseUserFolder, "").Replace("\\", ""); ;
                String folder = System.IO.Path.Combine(userfolder, "AppData", "Roaming", "Mozilla", "Firefox", "Profiles");
                if (System.IO.Directory.Exists(folder))
                {
                    foreach(var profile in Directory.GetDirectories(folder, "*.default"))
                    {
                        String history = safeReadFile(Path.Combine(profile, "places.sqlite"));
                        foreach (System.Text.RegularExpressions.Match match in regex.Matches(history))
                        {
                            var o = new BrowserHistory()
                            {
                                User = user,
                                Browser = "Firefox",
                                Url = match.Value
                            };
                            WriteObject(o);
                        }
                    }
                }

            }
        }
    }
}

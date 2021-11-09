using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace RoboDave.Forensic
{
    /// <summary>
    /// Browser History - a simple object containing browser history information
    /// </summary>
    public class BrowserHistory
    {
        /// <summary>
        /// The user and/or profile
        /// </summary>
        public String User { get; set; }
        
        /// <summary>
        /// The browser / source
        /// </summary>
        public String Browser { get; set; }
        
        /// <summary>
        /// The timestamp it was visitied (not always present)
        /// </summary>
        public DateTime? Timestamp { get; set; }
        
        /// <summary>
        /// The URL visited
        /// </summary>
        public String Url { get; set; }
    }

    /// <summary>
    /// <para type="synopsis">Retrieves the local browser history</para>
    /// <para type="description">Uses different methods for IE/Edge, Chrome, and Firefox browsers to retrieve a list of URLs that have been visisted</para>
    /// <example>
    ///     <code>Get-BrowserHistory</code>
    ///     <para>Retrieves all the browser history on the local machine</para>
    /// </example>
    /// <example>
    ///     <code>Get-BrowserHistory -Browser IE</code>
    ///     <para>Retrieves the IE/Edge browser history (from WinINet and TypedURLs) on the local machine for the current user</para>
    /// </example>
    /// <example>
    ///     <code>Get-BrowserHistory -Browser Chrome</code>
    ///     <para>Retrieves the Chrome browser history on the local machine for all users and profiles</para>
    /// </example>
    /// <example>
    ///     <code>Get-BrowserHistory -Browser Firefox</code>
    ///     <para>Retrieves the IE/Edge browser history on the local machine for the current user/profile</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "BrowserHistory", SupportsShouldProcess = true)]
    [OutputType(typeof(BrowserHistory[]))]
    public class BrowserHistoryCmdlet : PSCmdlet
    {
        /// <summary>
        /// <para type="description"></para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "Which browser to use (default: all)")]
        [ValidateSet("all", "ie", "chrome", "firefox")]
        public String Browser { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.Browser)
            {
                case "ie":
                    GetIETypedUrls();
                    GetIEHistory2();
                    break;
                case "chrome":
                    GetChromeHistory();
                    break;
                case "firefox":
                    GetFirefoxHistory();
                    break;
                default:
                    GetIETypedUrls();
                    GetIEHistory2();
                    GetChromeHistory();
                    GetFirefoxHistory();
                    break;
            }
        }

        private void GetIEHistory2()
        {
            BrowserHistory_ie ieh = new BrowserHistory_ie();
            var his1 = ieh.GetHistory_WinINet();
            foreach(var o in his1)
            {
                WriteObject(o);
            }
            var his2 = ieh.GetHistory_IE();
            foreach(var o in his2)
            {
                WriteObject(o);
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
                String[] locs = new String[] { "Local", "LocalLow", "Roaming" };
                foreach (var l in locs)
                {
                    String chromeFolder = System.IO.Path.Combine(userfolder, "AppData", l, "Google", "Chrome", "User Data");
                    if (System.IO.Directory.Exists(chromeFolder))
                    {
                        List<String> profiles = new List<string>(new String[] { Path.Combine(chromeFolder, "Default") });
                        profiles.AddRange(Directory.GetDirectories(chromeFolder, "*Profile*"));
                        
                        foreach (var folder in profiles) {
                            String profile = Path.GetFileName(folder);
                            if (System.IO.Directory.Exists(folder))
                            {
                                String historyFile = Path.Combine(folder, "History");
                                if (File.Exists(historyFile))
                                {
                                    String history = safeReadFile(historyFile);
                                    foreach (System.Text.RegularExpressions.Match match in regex.Matches(history))
                                    {
                                        var o = new BrowserHistory()
                                        {
                                            User = user + ":" + profile,
                                            Browser = "Chrome",
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

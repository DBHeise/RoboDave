
namespace RoboDave.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Contains information about a remotely provided IPAddress
    /// </summary>
    public struct RemoteAddressInfo
    {
        /// <summary>
        /// Name of provider of IPAddress information
        /// </summary>
        public String Provider { get; private set; }

        /// <summary>
        /// General status of the result
        /// </summary>
        public String Status { get; private set; }

        /// <summary>
        /// IPAddress returned by the service
        /// </summary>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// Raw return result of the service
        /// </summary>
        public String Raw { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider">name of provider</param>
        /// <param name="status">status of service call</param>
        /// <param name="address">IPAddress</param>
        public RemoteAddressInfo(String provider, String status, IPAddress address)
        {
            this.Provider = provider;
            this.Status = status;
            this.Address = address;
            this.Raw = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider">name of provider</param>
        /// <param name="status">status of service call</param>
        /// <param name="address">IPAddress or raw data</param>
        public RemoteAddressInfo(String provider, String status, String address)
        {
            this.Provider = provider;
            this.Status = status;
            this.Raw = address;
            this.Address = null;
            IPAddress a;
            if (status == "Success")
            {
                if (IPAddress.TryParse(address.Trim(), out a))
                {
                    this.Address = a;
                }
                else
                {
                    var match = RemoteAddressesCmdlet.ipv4Regex.Match(address);
                    if (match != null && match.Success)
                    {
                        this.Address = IPAddress.Parse(match.Value);
                    }
                    else
                    {
                        this.Status = "Invalid IPAddress";
                    }
                }
            }

        }
    }

    /// <summary>
    /// <para type="synopsis">Uses Remote Endpoints/Services to determine the remote IP Address</para>
    /// <para type="description">Calls various endpoints to determine the remotely visible IP Address of this system</para>
    /// <example>
    ///     <code>Get-RemoteAddresses</code>
    ///     <para>Calls the default remote services/endpoints</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "RemoteAddresses", SupportsShouldProcess = true)]
    [OutputType(typeof(RemoteAddressInfo))]
    public class RemoteAddressesCmdlet : PSCmdlet
    {
        internal static Regex ipv4Regex = new Regex(@"(?:(?:\d|[01]?\d\d|2[0-4]\d|25[0-5])\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d\d|\d)(?:\/\d{1,2})?");

        public RemoteAddressesCmdlet()
        {
            this.RemoteEndpoints = new string[] {
                "http://icanhazip.com",
                "http://www.whatismyip.com/",
                "http://api.hostip.info/get_html.php",
                "http://checkip.dyndns.com",
                "https://myexternalip.com/raw",
                "dns://resolver1.opendns.com/myip.opendns.com"
            };
        }

        /// <summary>
        /// List of remote endpoints/services to call
        /// </summary>
        [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "List of remote endpoints")]
        public String[] RemoteEndpoints { get; set; }

        /// <summary>
        /// ProcessRecord - core powershell function
        /// </summary>
        protected override void ProcessRecord()
        {
            foreach (var server in this.RemoteEndpoints)
            {
                Uri u = new Uri(server);

                WriteVerbose("Using Remote Endpoint: " + u.ToString());

                String response = null;
                String status = null;
                String provider = u.Host;

                if (u.Scheme == "http" || u.Scheme == "https")
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            response = client.DownloadString(u);
                            status = "Success";
                        }
                        catch (WebException we)
                        {
                            status = "Exception: " + we.Status;
                            if (we.InnerException != null)
                            {
                                if (we.InnerException is System.Net.Sockets.SocketException)
                                {
                                    status = "SocketException: " + we.InnerException.Message;
                                }
                                else
                                {
                                    status = we.InnerException.GetType().Name + ": " + we.InnerException.Message;
                                }
                            }
                            else
                            {
                                var webResponse = we.Response;
                                if (webResponse != null)
                                {
                                    status = "HttpResponse: " + ((HttpWebResponse)webResponse).StatusCode.ToString();
                                    Stream stream = webResponse.GetResponseStream();
                                    if (stream != null)
                                    {
                                        StreamReader reader = new StreamReader(stream, true);
                                        response = reader.ReadToEnd();
                                        reader.Dispose();
                                        stream.Dispose();
                                    }
                                    webResponse.Dispose();
                                }

                            }
                        }
                    }
                }
                else if (u.Scheme == "dns")
                {                    
                    String dnsserver = u.Host;
                    String lookup = u.LocalPath.Trim('/');
                 
                    var proc = new System.Diagnostics.Process();
                    proc.StartInfo = new System.Diagnostics.ProcessStartInfo("nslookup.exe", lookup + " " + dnsserver);
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.EnableRaisingEvents = false;
                    proc.Start();
                    proc.WaitForExit();
                    response = proc.StandardOutput.ReadToEnd();
                    status = "Success";

                    response = response.Substring(response.IndexOf("\r\n\r\n")).Trim();
                } else
                {
                    status = "Bad Protocol";
                }


                WriteObject(new RemoteAddressInfo(provider, status, response));

            }

        }
    }
}

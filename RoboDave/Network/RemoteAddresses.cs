
namespace RoboDave.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;


    [Cmdlet(VerbsCommon.Get, "RemoteAddresses", SupportsShouldProcess = true)]
    [OutputType(typeof(IPAddress))]
    public class RemoteAddressesCmdlet : PSCmdlet
    {

        private String[] remoteServers;

        public RemoteAddressesCmdlet()
        {
            this.remoteServers = new string[] {"http://icanhazip.com","http://freegeoip.net/xml/","http://www.whatismyip.com/","http://api.hostip.info/get_html.php","http://myip.dnsdynamic.org","http://checkip.dyndns.com"};
        }


        protected override void ProcessRecord()
        {

            Uri remoteServer = new Uri(Rando.RandomPick(this.remoteServers));
            WriteVerbose("Using Remote Service: " + remoteServer.ToString());
            String address;
            using (WebClient client = new WebClient())
            {
                address = client.DownloadString(remoteServer).Trim();
            }
            WriteObject(IPAddress.Parse(address));
        }
    }
}

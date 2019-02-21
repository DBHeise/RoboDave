
namespace RoboDave.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Net.NetworkInformation;

    [Cmdlet(VerbsCommon.Get, "LocalhostFQDN", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class LocalhostFQDNCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            WriteObject(string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName));
        }
    }
}

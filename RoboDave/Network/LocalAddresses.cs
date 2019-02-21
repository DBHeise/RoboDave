
namespace RoboDave.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    [Cmdlet(VerbsCommon.Get, "LocalAddresses", SupportsShouldProcess = true)]
    [OutputType(typeof(IPAddress[]))]
    public class LocalAddressesCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (NetworkInterface net in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties props = net.GetIPProperties();

                foreach (UnicastIPAddressInformation info in props.UnicastAddresses)
                {
                   
                        WriteObject(info.Address);
                }
            }
        }
    }
}

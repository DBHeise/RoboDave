
namespace RoboDave.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    /// <summary>
    /// <para type="synopsis">Returns all the local IPAddresses</para>
    /// <para type="description">Enumerates all network interfaces and returns all IPAddresses of the local system</para>
    /// <example>
    ///     <code>Get-LocalAddresses</code>
    ///     <para>gets the local IPAddresses</para>
    /// </example>    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LocalAddresses", SupportsShouldProcess = true)]
    [OutputType(typeof(IPAddress[]))]
    public class LocalAddressesCmdlet : PSCmdlet
    {
        /// <summary>
        /// ProcessRecord - core powershell function
        /// </summary>
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

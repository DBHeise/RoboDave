
namespace RoboDave.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Net.NetworkInformation;


    /// <summary>
    /// <para type="synopsis">Returns the local host's fully qualified domain name</para>
    /// <para type="description">Concatenates the local machine name and the local domain name</para>
    /// <example>
    ///     <code>Get-LocalhostFQDN</code>
    ///     <para>returns the fully qualified domain name of the localhost</para>
    /// </example>    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LocalhostFQDN", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class LocalhostFQDNCmdlet : PSCmdlet
    {
        /// <summary>
        /// ProcessRecord - core powershell function
        /// </summary>
        protected override void ProcessRecord()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            WriteObject(string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName));
        }
    }
}

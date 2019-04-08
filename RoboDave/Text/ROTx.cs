using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Text
{
    public class ROTx
    {
    }

    [Cmdlet(VerbsData.Convert, "ROT", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class ROTXCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Input string", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public String Input { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Shifting amount")]
        public int Shift { get; set; }

        protected override void ProcessRecord()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Input.Length; i++)
            {
                int c = (int)Input[i];
                c += this.Shift;
                sb.Append((char)c);
            }
            WriteObject(sb.ToString());
        }
    }
}

using RoboDave.Random;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators
{
    [Cmdlet(VerbsCommon.New, "RandomFile", SupportsShouldProcess = true)]
    [OutputType(typeof(FileInfo))]
    public class RandomFileCmdlet: PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public UInt64 Size { get; set; }

        [Parameter(Mandatory = true)]
        public String OutputFile { get; set; }

        protected override void ProcessRecord()
        {
            System.IO.File.WriteAllBytes(this.OutputFile, Rando.GetBytes(this.Size));
            WriteObject(new FileInfo(this.OutputFile));
        }
    }
}

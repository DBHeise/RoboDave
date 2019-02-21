

namespace RoboDave.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Management.Automation;
    using RoboDave.Random;


    internal static class MadLibHelper
    {
        internal static MadLib madlib;
        static MadLibHelper()
        {
            madlib = new MadLib();
        }
    }


    [Cmdlet(VerbsOther.Use, "MadLib", SupportsShouldProcess = true)]    
    public class UseMadLibCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The input to Madlibize")]
        public String Input { get; set; }

        protected override void ProcessRecord()
        {            
            WriteObject(MadLibHelper.madlib.Generate(this.Input));
        }
    }


    [Cmdlet(VerbsCommon.New, "MadLib", SupportsShouldProcess = true)]
    public class NewMadLibCmdlet : PSCmdlet
    {

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The input to Madlibize")]
        public String Input { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(MadLibHelper.madlib.GenerateMadLib(this.Input));
        }
    }


}

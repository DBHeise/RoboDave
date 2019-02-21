
namespace RoboDave.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;
    using System.Collections;


    [Cmdlet(VerbsCommon.Get, "RandomMEID", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class RandomMEIDCmdlet : PSCmdlet
    {
        private Random rnd = new Random();

        public RandomMEIDCmdlet()
        {
            this.Prepender = "";
            this.Length = 14;
        }

        [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "String to prepend to random MEID")]
        public String Prepender { get; set; }

        [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Length of MEID")]
        public UInt16 Length { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                char[] validChars = "0123456789ABCDEF".ToCharArray();                
                StringBuilder ans = new StringBuilder();
                ans.Append(this.Prepender);                
                while (ans.Length != this.Length)
                {
                    int index = rnd.Next(0, validChars.Length);
                    ans.Append(validChars[index]);
                }

                //14 digit hex string
                WriteObject(ans.ToString());
            }
            catch (Exception e)
            {
                WriteError(new ErrorRecord(e, "ProcessRecord", ErrorCategory.NotSpecified, this));
            }
        }
    }
}

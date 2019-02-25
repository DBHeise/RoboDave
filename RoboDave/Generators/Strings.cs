

namespace RoboDave.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Management.Automation;
    using RoboDave.Random;

    [Cmdlet(VerbsCommon.New, "RandomString", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class RandomStringCmdlet : PSCmdlet
    {
        public RandomStringCmdlet() : base()
        {
            this.StringType = StringType.AlphaNumeric;
            this.Length = 20;
        }

        #region Parameters

        [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "String Type")]
        public StringType StringType { get; set; }

        [Parameter(Position = 1, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "String Length")]
        public UInt32 Length { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            try
            {
                WriteObject(StringGenerator.GetString(this.StringType, this.Length));
            }
            catch (Exception e)
            {
                WriteError(new ErrorRecord(e, "ProcessRecord", ErrorCategory.NotSpecified, this));
            }
        }
    }
}

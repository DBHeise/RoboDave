
namespace RoboDave.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "RandomDateTime", SupportsShouldProcess = true)]
    [OutputType(typeof(DateTime))]
    public class RandomDateTimeCmdlet : PSCmdlet
    {
        private Random rnd = new Random();

        protected override void ProcessRecord()
        {
            try
            {
                DateTime ans = DateTime.Now
                    .AddYears(rnd.Next(1, 1))
                    .AddMonths(rnd.Next(-11, 11))
                    .AddDays(rnd.Next(-30, 30))
                    .AddHours(rnd.Next(-23, 23))
                    .AddMinutes(rnd.Next(-59, 59))
                    .AddSeconds(rnd.Next(-59, 59));
                WriteObject(ans);

            }
            catch (Exception e)
            {
                WriteError(new ErrorRecord(e, "ProcessRecord", ErrorCategory.NotSpecified, this));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators
{
    [Cmdlet(VerbsCommon.Get, "RandomExcuse", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class RandomExcuseCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            string sentence = @"
[interjection] [adjective] [person], 
    Sorry, but I cannot [Verb_PresentTense] the [thing] because I am currently [Verb_Progressive] the [adjective] [thing].
Sincerely,
   Your [adjective] [relation].
";
            WriteObject(MadLibHelper.madlib.Generate(sentence));
        }
    }
}

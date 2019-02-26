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
        public RandomFileCmdlet()
        {
            this.Size = 1024;
            this.Seperator = Environment.NewLine;
            this.StringType = StringType.Random;
        }

        [Parameter(Mandatory = false, HelpMessage = "String to use between certian string types")]
        public String Seperator { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Type of string to generate")]
        public StringType StringType { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Size of file to create")]
        public UInt64 Size { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Output file")]
        public String OutputFile { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.StringType)
            {
                case StringType.AaZz:
                case StringType.AlphaNumeric:
                case StringType.ASCII:
                case StringType.ANSI:
                case StringType.Hex:
                case StringType.LowerCase:
                case StringType.UpperCase:
                case StringType.Unicode:
                case StringType.Digits:
                    System.IO.File.WriteAllText(this.OutputFile, StringGenerator.GetString(this.StringType, this.Size));
                    break;
                case StringType.Name:
                case StringType.Word:
                case StringType.Sentence:
                case StringType.Email:
                case StringType.EmailSimple:
                case StringType.Domain:
                case StringType.TLD:
                case StringType.Uri:
                    UInt64 writtenSize = 0;
                    while (writtenSize < this.Size)
                    {
                        String data = StringGenerator.GetString(this.StringType) + this.Seperator;
                        writtenSize += (UInt64)data.Length;
                        System.IO.File.AppendAllText(this.OutputFile, data);
                    }
                    break;
                case StringType.Random:
                default:
                    System.IO.File.WriteAllBytes(this.OutputFile, Rando.GetBytes(this.Size));                    
                    break;
            }

            WriteObject(new FileInfo(this.OutputFile));
        }
    }
}

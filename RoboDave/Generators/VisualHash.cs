using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators
{
    [Cmdlet(VerbsCommon.Get, "VisualHash", SupportsShouldProcess = true, DefaultParameterSetName = "text")]
    [OutputType(typeof(Bitmap))]
    public class VisualHash : PSCmdlet
    {
        public VisualHash()
        {
            this.Width = 512;
            this.Height = 512;
        }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Visual Hash Type")]
        public VHash.HashType Type { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Image Width")]
        public UInt16 Width { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Image Height")]
        public UInt16 Height { get; set; }


        [Parameter(ParameterSetName = "text", ValueFromRemainingArguments = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Input to create a visual hash")]
        public String Text { get; set; }

        [Parameter(ParameterSetName = "bytes", ValueFromRemainingArguments = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Input to create a visual hash")]
        public Byte[] Data { get; set; }

        [Parameter(ParameterSetName = "file", ValueFromRemainingArguments = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Input to create a visual hash")]
        public FileInfo File { get; set; }


        protected override void ProcessRecord()
        {
            var hasher = VHash.VisualHasher.CreateHasher(this.Type, this.Width, this.Height);
            Bitmap bmp = null;
            switch (this.ParameterSetName)
            {
                case "bytes":
                    bmp = hasher.Hash(this.Data);
                    break;
                case "file":
                    byte[] bytes = System.IO.File.ReadAllBytes(this.File.FullName);
                    bmp = hasher.Hash(bytes);
                    break;
                case "text":
                default:
                    bmp = hasher.Hash(this.Text);
                    break;
            }
            
            WriteObject(bmp);
        }
    }
}

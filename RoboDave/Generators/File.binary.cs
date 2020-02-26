using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators
{
    /// <summary>
    /// <para type="synopsis">Generates a random binary file</para>
    /// <para type="discription">Generates a random file of size and data specified by the user</para>
    /// <example>
    ///     <code>New-RandomBinaryFile -Size 10Gb -OutputFile C:\temp\randomfile.bin</code>
    ///     <para>Generates a 10Gb file located at 'C:\temp\randomfile.bin' filled with random bytes</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomBinaryFile -Size 10Mb -OutputFile C:\temp\randomfile.bin -RepeatByte 0x0</code>
    ///     <para>Generates a 10Mb file located at 'C:\temp\randomfile.bin' filled with the zero byte</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomBinaryFile -Size 10Mb -OutputFile C:\temp\randomfile.bin -RepeatByte @(0x90,0x90,0x90,0xC3)</code>
    ///     <para>Generates a 10Mb file located at 'C:\temp\randomfile.bin' filled with the repeated byte pattern 90 90 90 C3</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomBinaryFile -Size 5120 -OutputFile C:\temp\randomfile.bin -RepeatString "ABCDEFGHIJKLMNOPQRSTUVWXYZ"</code>
    ///     <para>Generates a 5120 byte file located at 'C:\temp\randomfile.bin' filled with the repeated string "ABCDEFGHIJKLMNOPQRSTUVWXYZ"</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomBinaryFile", SupportsShouldProcess = true, DefaultParameterSetName = "random")]
    [OutputType(typeof(FileInfo))]
    public class RandomBinaryFileCmdlet : PSCmdlet
    {
        /// <summary>
        /// default constructor for RandomBinaryFileCmdlet
        /// </summary>
        public RandomBinaryFileCmdlet()
        {
            this.Size = 1024;
            this.BlockSize = 1024;
        }

        /// <summary>
        /// Size of blocks to generate at a time
        /// </summary>
        public UInt16 BlockSize { get; set; }

        /// <summary>
        /// Size of the file (or minimum size in some cases)
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Size of file to create")]
        public UInt64 Size { get; set; }

        /// <summary>
        /// Location to write data
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Output file")]
        public String OutputFile { get; set; }


        /// <summary>
        /// Pattern to use to fill the file
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "repeat", HelpMessage = "Repeated Byte Pattern")]
        public Byte[] RepeatBytes { get; set; }

        /// <summary>
        /// Pattern to use to fill the file
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "repeatstr", HelpMessage = "Repeated String Pattern")]
        public String RepeatString 
        { 
            get { return System.Text.Encoding.Default.GetString(this.RepeatBytes); } 
            set { this.RepeatBytes = System.Text.Encoding.Default.GetBytes(value); } 
        }


        protected override void BeginProcessing()
        {
            if (String.IsNullOrWhiteSpace(this.OutputFile))
            {
                this.OutputFile = Path.GetTempFileName();
            }
            base.BeginProcessing();
        }

        private int repeteIndex = 0;
        private Byte[] extractpattern(int size)
        {
            List<byte> ans = new List<byte>(size);
            int idx = 0;
            while (idx < size) {

                ans.Add(this.RepeatBytes[repeteIndex]);
                idx++;
                repeteIndex = (repeteIndex + 1) % this.RepeatBytes.Length;
            }
            return ans.ToArray();
        }

        protected override void ProcessRecord()
        {
            this.repeteIndex = 0;
            UInt64 writtenSize = 0;
            using (var writer = new FileStream(this.OutputFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                while (writtenSize < this.Size)
                {
                    Byte[] data;
                    ulong bsize = this.BlockSize;
                    if (writtenSize + BlockSize > this.Size)
                    {
                        bsize = this.Size - writtenSize;
                    }

                    switch (this.ParameterSetName.ToLowerInvariant())
                    {
                        case "repeatstr":
                        case "repeat":
                            {
                                data = this.extractpattern((int)bsize);
                            }
                            break;
                        case "random":
                        default:
                            {
                                data = Rando.GetBytes(bsize);
                            }
                            break;
                    }

                    writtenSize += (UInt64)data.Length;
                    writer.Write(data, 0, data.Length);
                }
            }
            WriteObject(new FileInfo(this.OutputFile));            
        }

    }
}

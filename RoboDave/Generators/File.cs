
namespace RoboDave.Generators
{
    using RoboDave.Random;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// <para type="synopsis">Generates a random file</para>
    /// <para type="description">Generates a random file of size and data specified by the user</para>
    /// <example>
    ///     <code>New-RandomFile -OutputFile C:\temp\randomfile.bin</code>
    ///     <para>Generates a file of 1024 random bytes located at 'C:\temp\randomfile.bin'</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomFile -Size 1Mb -StringType Hex -OutputFile C:\temp\randomhex.txt</code>
    ///     <para>Generates a 1Mb file of hex characters</para>
    /// </example>    
    /// <example>
    ///     <code>New-RandomFile -Size 10Kb -StringType Word -Seperator "|" -OutputFile C:\temp\randomwords.txt</code>
    ///     <para>Generates a file at 'C:\temp\randomwords.txt' of at least 10Kb bytes filled with random words seperated by a pipe</para>
    /// </example>    
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomFile", SupportsShouldProcess = true)]
    [OutputType(typeof(FileInfo))]
    public class RandomFileCmdlet : PSCmdlet
    {
        /// <summary>
        /// RandomFileCmdlet Constructor
        /// </summary>
        public RandomFileCmdlet()
        {
            this.Size = 1024;
            this.Seperator = Environment.NewLine;
            this.StringType = StringType.Random;
        }

        /// <summary>
        /// Seperator string (used only during certain types of StringType)
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "String to use between certian string types")]
        public String Seperator { get; set; }

        /// <summary>
        /// Type of String/data to generate
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Type of string to generate")]
        public StringType StringType { get; set; }

        /// <summary>
        /// Size of the file (or minimum size in some cases)
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Size of file to create")]
        public UInt64 Size { get; set; }

        /// <summary>
        /// Location to write data
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "Output file")]
        public String OutputFile { get; set; }

        /// <summary>
        /// ProcessRecord - primary Cmdlet func
        /// </summary>
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

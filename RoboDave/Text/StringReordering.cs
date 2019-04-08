
namespace RoboDave.Text
{

    using RoboDave.Text.Languages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    public class StringReordering
    {
        public UInt16 MinPartLength { get; set; }
        public UInt16 MaxPartLength { get; set; }

        public StringReordering()
        {
            this.MinPartLength = 1;
            this.MaxPartLength = 5;
        }

        private void Shuffle(ref String[] list, out int[] order)
        {
            int n = list.Length;
            order = new int[n];
            String[] old = new string[n];
            for (int i = 0; i < n; i++)
            {
                order[i] = i;
                old[i] = list[i];
            }
            Rando.Shuffle(order);
            for (int i = 0; i < n; i++)
            {
                list[order[i]] = old[i];
            }
        }

        public String Encode(String input)
        {
            //1. extract input into parts
            int start = 0;
            List<String> parts = new List<String>();
            while (start < input.Length)
            {
                int pLen = Rando.RandomInt(this.MinPartLength, this.MaxPartLength);
                if (start + pLen >= input.Length)
                {
                    parts.Add(input.Substring(start));
                }
                else
                {
                    parts.Add(input.Substring(start, pLen));
                }
                start += pLen;
            }

            //2. shuffle order of parts
            String[] list = parts.ToArray();
            int[] order;
            this.Shuffle(ref list, out order);

            //3. return concatenation of parts and ordering in given language
            return LanguageFactory.EncodeToStringReorder(Language.Powershell, list, order);
        }

        public String Decode(String input)
        {
            String[] parts;
            int[] order;
            LanguageFactory.ParseStringReorder(Language.Powershell, input, out parts, out order);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < order.Length; i++)
            {
                sb.Append(parts[order[i]]);
            }

            return sb.ToString();
        }

    }

    [Cmdlet(VerbsData.ConvertTo, "StringReorderedEncodedString", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class StringReorderingEncoderCmdlet : PSCmdlet
    {
        private StringReordering sr;

        [Parameter(Mandatory = true, HelpMessage = "Input string", ValueFromPipeline = true)]
        public String Input { get; set; }

        protected override void BeginProcessing()
        {
            this.sr = new StringReordering();
        }
        protected override void ProcessRecord()
        {
            WriteObject(this.sr.Encode(this.Input));
        }
    }

    [Cmdlet(VerbsData.ConvertFrom, "StringReorderedEncodedString", SupportsShouldProcess = true)]
    [OutputType(typeof(String))]
    public class StringReorderingDecoderCmdlet : PSCmdlet
    {
        private StringReordering sr;

        [Parameter(Mandatory = true, HelpMessage = "Input string", ValueFromPipeline = true)]
        public String Input { get; set; }

        protected override void BeginProcessing()
        {
            this.sr = new StringReordering();
        }
        protected override void ProcessRecord()
        {
            WriteObject(this.sr.Decode(this.Input));
        }
    }

}

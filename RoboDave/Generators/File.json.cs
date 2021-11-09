namespace RoboDave.Generators
{
    using RoboDave.Random;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// <para type="synopsis">Generates a random JSON file</para>
    /// <para type="description">Generates a random JSON file</para>
    /// <example>
    ///     <code></code>
    ///     <para></para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomJSONFile", SupportsShouldProcess = true)]
    [OutputType(typeof(FileInfo))]
    public class RandomJsonFileCmdlet : PSCmdlet
    {
        /// <summary>
        /// default cTor
        /// </summary>
        public RandomJsonFileCmdlet() : base()
        {
            this.MaxDepth = 5;
            this.MaxWidth = 3;
        }

        /// <summary>
        /// Location to write data
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "Output file")]
        public String OutputFile { get; set; }

        /// <summary>
        /// Max number of children per item
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Max Tree width")]
        public UInt16 MaxWidth { get; set; }


        /// <summary>
        /// Max recursion depth
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Max Tree depth")]
        public UInt16 MaxDepth { get; set; }

        private String newKey()
        {
            return StringGenerator.GetString(StringType.Word);
        }

        private string newValue(int currentDepth)
        {
            if (currentDepth > this.MaxDepth)
            {
                return "\"" + StringGenerator.GetString(StringType.Word) + "\"";
            }

            string value = null;

            String type = Rando.RandomPick<String>(new String[] { "string", "string", "number", "number", "boolean", "datetime", "array", "object" });
            switch (type)
            {
                case "string":
                    value = "\"" + StringGenerator.GetString(StringType.Word) + "\"";
                    break;
                case "number":
                    value = Rando.RandomInt(int.MinValue, int.MaxValue).ToString();
                    break;
                case "boolean":
                    value = Rando.RandomBoolean().ToString().ToLower();
                    break;
                case "datetime":
                    DateTime dt = DateTime.Now
                                     .AddYears(Rando.RandomInt(-100, 100))
                                     .AddMonths(Rando.RandomInt(-11, 11))
                                     .AddDays(Rando.RandomInt(-30, 30))
                                     .AddHours(Rando.RandomInt(-23, 23))
                                     .AddMinutes(Rando.RandomInt(-59, 59))
                                     .AddSeconds(Rando.RandomInt(-59, 59));
                    value = "\"" + dt.ToString("yyyy-MM-ddTH:mm:ss.fffK") + "\"";
                    break;
                case "array":
                    value = "[";
                    int numItems = Rando.RandomInt(1, this.MaxWidth);
                    for (int i = 0; i < numItems; i++)
                    {
                        value += newValue(currentDepth + 1);
                        if (i+1<numItems)
                        {
                            value += ",";
                        }
                    }
                    value += "]";
                    break;
                case "object":
                    value = newObject(currentDepth + 1);
                    break;
            }
            return value;
        }

        private string newObject(int currentDepth)
        {
            StringBuilder sb = new StringBuilder();
            if (currentDepth > this.MaxDepth)
            {
                return "{}";
            }

            sb.Append("{");
            int numItems = Rando.RandomInt(1, this.MaxWidth);
            for (int i = 0; i < numItems; i++)
            {
                String key = newKey();
                String value = newValue(currentDepth + 1);
                sb.AppendFormat("\"{0}\":{1}", key, value);
                if (i + 1 < numItems)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// ProcessRecord - primary Cmdlet func
        /// </summary>
        protected override void ProcessRecord()
        {
            using (var writer = new StreamWriter(this.OutputFile))
            {
                writer.Write(newObject(0));
            }
            WriteObject(new FileInfo(this.OutputFile));

        }
    }
}
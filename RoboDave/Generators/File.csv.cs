
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
    /// <para type="synopsis">Generates a random CSV file</para>
    /// <para type="description">Generates a random CSV file </para>
    /// <example>
    ///     <code>New-RandomCSVFile -RowCount 10 -OutputFile E:\temp\random.csv</code>
    ///     <para>Generates a random CSV file with 10 rows and a randomly generated columns and data</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomCSVFile -IncludeHeader $true -RowCount 1000000 -OutputFile E:\temp\random.csv -Columns @{"one"="IPv4"; "two"="Word"; "three"="EmailSimple:10"; "four"="Hex:5"; "five"="datetime:yyyy-MM-ddTHH:mm:ss"}</code>
    ///     <para>Generates a 1 million row CSV file with the five columns of the specified data types. The columns are garuenteed of the specified type, but NOT in the specified order</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomCSVFile", SupportsShouldProcess = true)]
    [OutputType(typeof(FileInfo))]
    public class RandomCSVFileCmdlet : PSCmdlet
    {
        /// <summary>
        /// Default cTor
        /// </summary>
        public RandomCSVFileCmdlet() : base()
        {
            this.IncludeHeader = true;
            this.QuoteAll = false;
            this.Seperator = ',';
        }

        /// <summary>
        /// Include the header row
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Include the header row")]
        public Boolean IncludeHeader { get; set; }

        /// <summary>
        /// Quote All columns (TRUE= all columns are quoted, FALSE= NO columns are quoted, NULL= only columns that need quotes are quoted)
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Quote All columns (TRUE= all columns are quoted, FALSE= NO columns are quoted, NULL= only columns that need quotes are quoted)")]
        public Boolean? QuoteAll { get; set; }

        /// <summary>
        /// Character to use to seperate items (default=',')
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Character to use to seperate items (default=',')")]
        public Char Seperator { get; set; }

        /// <summary>
        /// Number of CSV rows to generate
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "Number of CSV rows to generate")]
        public UInt64 RowCount { get; set; }

        /// <summary>
        /// Location to write data
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "Output file")]
        public String OutputFile { get; set; }


        /// <summary>
        /// CSV Column definitions
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "CSV Column definitions")]
        public Hashtable Columns { get; set; }

        private String[] getKeyList()
        {
            List<String> ans = new List<String>();
            if (this.Columns == null || this.Columns.Count == 0)
            {
                this.Columns = new Hashtable();
                int count = Rando.RandomInt(3, 15);
                for (int i = 0; i < count; i++)
                {
                    String key = StringGenerator.GetString(StringType.Word);
                    String type = Rando.RandomPick<String>(new String[] { "AaZz:10", "Digits:5", "Hex:8", "EmailSimple:5", "Domain", "Name", "Word", "IPAddress", "DateTime:yyyy-MM-ddTHH:mm:ss" });
                    ans.Add(key);
                    this.Columns.Add(key, type);
                }
            }
            else
            {
                foreach (var key in this.Columns.Keys)
                {
                    ans.Add((String)key);
                }
            }
            return ans.ToArray();
        }


        /// <summary>
        /// ProcessRecord - primary Cmdlet func
        /// </summary>
        protected override void ProcessRecord()
        {
            bool numColumns = this.Columns == null;

            using (var writer = new StreamWriter(this.OutputFile))
            {
                String[] keys = getKeyList();

                if (this.IncludeHeader)
                {
                    writer.WriteLine(String.Join(this.Seperator.ToString(), keys));
                }
                for (ulong row = 0; row < this.RowCount; row++)
                {
                    for (int col = 0; col < keys.Length; col++)
                    {
                        if (col != 0)
                        {
                            writer.Write(this.Seperator);
                        }

                        var key = keys[col];
                        var val = (String)this.Columns[key];
                        String[] valParts = val.SplitAtFirst(':');
                        uint length = 0;
                        if (valParts.Length > 1)
                        {
                            val = valParts[0];
                            uint.TryParse(valParts[1], out length);
                        }

                        StringType st;

                        if (Enum.TryParse<StringType>(val, out st))
                        {
                            if (length > 0)
                                val = StringGenerator.GetString(st, length);
                            else
                                val = StringGenerator.GetString(st);
                        }
                        else
                        {
                            switch (val.ToLowerInvariant())
                            {
                                case "datetime":
                                    DateTime dt = DateTime.Now
                                       .AddYears(Rando.RandomInt(1, 1))
                                       .AddMonths(Rando.RandomInt(-11, 11))
                                       .AddDays(Rando.RandomInt(-30, 30))
                                       .AddHours(Rando.RandomInt(-23, 23))
                                       .AddMinutes(Rando.RandomInt(-59, 59))
                                       .AddSeconds(Rando.RandomInt(-59, 59));
                                    val = dt.ToString(valParts[1]);
                                    break;
                            }
                        }

                        if (this.QuoteAll.HasValue)
                        {
                            if (this.QuoteAll.Value)
                            {
                                writer.Write("\"");
                                writer.Write(val);
                                writer.Write("\"");
                            }
                            else
                            {
                                writer.Write(val);
                            }
                        }
                        else
                        {
                            if (val.Contains(" ") || val.Contains(this.Seperator))
                            {
                                writer.Write("\"");
                                writer.Write(val);
                                writer.Write("\"");
                            }
                            else
                            {
                                writer.Write(val);
                            }
                        }

                    }
                    writer.WriteLine();
                }

            }

            if (numColumns)
            {
                this.Columns = null;
            }

            WriteObject(new FileInfo(this.OutputFile));

        }
    }
}
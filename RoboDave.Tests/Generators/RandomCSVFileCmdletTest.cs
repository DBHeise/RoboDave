
namespace RoboDave.Tests.Generators
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RoboDave.Generators;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;


    [TestClass]
    public class RandomCSVFileCmdletTest : RoboDave.Tests.PSCmdletTest
    {
        [TestMethod]
        public void cTor()
        {
            var cit = new RandomCSVFileCmdlet();
            Assert.IsNotNull(cit);
            Assert.IsTrue(cit is PSCmdlet);
        }

        [TestMethod]
        public void Example001()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);

            int rowCount = Rando.RandomInt(5, 25);

            ps.AddCommand("New-RandomCSVFile")
                .AddParameter("RowCount", rowCount)
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            VerifyCSVFile(tmpFile, ",", rowCount + 1, null, null);
        }

        [TestMethod]
        public void Example002()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);

            int rowCount = Rando.RandomInt(1000, 100000);

            var ht = new Hashtable();
            ht.Add("one", "IPv4");
            ht.Add("two", "Word");
            ht.Add("three", "EmailSimple:10");
            ht.Add("four", "Hex:5");
            ht.Add("five", "datetime:yyyy-MM-ddTHH:mm:ss");

            ps.AddCommand("New-RandomCSVFile")
                .AddParameter("RowCount", rowCount)
                .AddParameter("IncludeHeader", true)
                .AddParameter("Columns", ht)
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            VerifyCSVFile(tmpFile, ",", rowCount + 1, 5, null);
        }

        [TestMethod]
        public void IncludeHeader()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);

            int rowCount = Rando.RandomInt(5, 25);

            ps.AddCommand("New-RandomCSVFile")
                .AddParameter("RowCount", rowCount)
                .AddParameter("IncludeHeader", false)
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            VerifyCSVFile(tmpFile, ",", rowCount, null, null);
        }

        [TestMethod]
        public void QuoteAll_Null()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);

            int rowCount = Rando.RandomInt(50, 100);

            var ht = new Hashtable();
            ht.Add("one", "Sentence");
            ht.Add("two", "Domain");
            ht.Add("three", "Name");

            ps.AddCommand("New-RandomCSVFile")
                .AddParameter("RowCount", rowCount)
                .AddParameter("IncludeHeader", true)
                .AddParameter("QuoteAll", null)
                .AddParameter("Columns", ht)
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            VerifyCSVFile(tmpFile, ",", rowCount + 1, 3, null);
        }

        [TestMethod]
        public void QuoteAll_True()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);

            int rowCount = Rando.RandomInt(50, 100);

            var ht = new Hashtable();
            ht.Add("one", "Sentence");
            ht.Add("two", "Domain");
            ht.Add("three", "Name");

            ps.AddCommand("New-RandomCSVFile")
                .AddParameter("RowCount", rowCount)
                .AddParameter("IncludeHeader", true)
                .AddParameter("QuoteAll", true)
                .AddParameter("Columns", ht)
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            VerifyCSVFile(tmpFile, ",", rowCount + 1, 3, true);
        }

        [TestMethod]
        public void QuoteAll_False()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);

            int rowCount = Rando.RandomInt(50, 100);

            var ht = new Hashtable();
            ht.Add("one", "Sentence");
            ht.Add("two", "Domain");
            ht.Add("three", "Name");

            ps.AddCommand("New-RandomCSVFile")
                .AddParameter("RowCount", rowCount)
                .AddParameter("IncludeHeader", true)
                .AddParameter("QuoteAll", false)
                .AddParameter("Columns", ht)
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            VerifyCSVFile(tmpFile, ",", rowCount+1, null, false);
        }



        public static void VerifyCSVFile(String csvFile, string delimiter, int totalRows, int? colCount, Boolean? quoteall)
        {
            Assert.IsTrue(File.Exists(csvFile));

            int count = 0;
            
            using (var reader = new StreamReader(csvFile))
            using (var parser = new CsvHelper.CsvParser(reader, System.Globalization.CultureInfo.InvariantCulture))            
            {
                parser.Configuration.Delimiter = delimiter;                
                while (true)
                {
                    
                    var records = parser.Read();
                    if (records == null)
                    {
                        break;
                    }

                    count++;

                    if (colCount.HasValue)
                    {
                        Assert.AreEqual(colCount.Value, records.Length);
                    }

                    if (quoteall.HasValue)
                    {
                        if (quoteall.Value)
                        {
                        }
                        else
                        {
                        }
                    }
                    else
                    {

                    }
                }
            }
            Assert.AreEqual(totalRows, count);
        }
    }
}

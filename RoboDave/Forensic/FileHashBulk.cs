using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Forensic
{

    public class HashResult
    {
        public String Algorithm { get; private set; }        
        public String Hash { get; private set; }
        public String Path { get; private set; }

        public HashResult(String name, Byte[] value, String path)
        {
            this.Algorithm = name;            
            this.Hash = BitConverter.ToString(value).Replace("-", String.Empty);
            this.Path = path;
        }
    }

    [Cmdlet(VerbsCommon.Get, "FileHashBulk", SupportsShouldProcess = true)]
    [OutputType(typeof(HashResult))]
    public class FileHashCmdlet : PSCmdlet
    {
        public FileHashCmdlet()
        {
            this.Algorithms = new string[] { "md5", "sha1", "sha256" };
        }

        [Parameter(Mandatory = false, HelpMessage = "List of hashes to compute")]
        public String[] Algorithms { get; set; } //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.create?view=netframework-4.7#System_Security_Cryptography_HashAlgorithm_Create_System_String_

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName =true, HelpMessage = "List of algorithms")]
        [Alias("FullName")]
        public String[] InputFiles { get; set; }

        private Dictionary<String, HashAlgorithm> map;

        protected override void BeginProcessing()
        {
            map = new Dictionary<string, HashAlgorithm>();

            //Create hashers
            foreach (var hsh in this.Algorithms)
            {
                map.Add(hsh, HashAlgorithm.Create(hsh));
            }
        }

        protected override void ProcessRecord()
        {
            //Calculate hashes
            foreach (string file in this.InputFiles)
            {
                WriteVerbose("Processing file: " + file);
                using (FileStream stream = File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
                {
                    foreach (var h in map)
                    {
                        var hr = new HashResult(h.Key, h.Value.ComputeHash(stream), file);
                        stream.Seek(0, SeekOrigin.Begin); //have to reset the stream in order for the hash to get computed again
                        WriteObject(hr);
                    }
                }
            }
        }

        protected override void EndProcessing()
        {
            //Dispose hashers
            foreach (var h in map)
            {
                h.Value.Dispose();
            }
        }
    }
}

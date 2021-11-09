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

    /// <summary>
    /// HashResult - an object to hold file hash information
    /// </summary>
    public class HashResult
    {
        /// <summary>
        /// The Algorithm used
        /// </summary>
        public String Algorithm { get; private set; }        
        /// <summary>
        /// The Hash value (as a HEX string)
        /// </summary>
        public String Hash { get; private set; }
        /// <summary>
        /// The File that was hashed
        /// </summary>
        public String Path { get; private set; }

        /// <summary>
        /// HashResult Constructor
        /// </summary>
        /// <param name="name">hash algorithm used</param>
        /// <param name="value">hash value</param>
        /// <param name="path">file that was hashed</param>
        public HashResult(String name, Byte[] value, String path)
        {
            this.Algorithm = name;            
            this.Hash = BitConverter.ToString(value).Replace("-", String.Empty);
            this.Path = path;
        }
    }

    /// <summary>
    /// <para type="synopsis">Retrieves multiple hashes of a single file</para>
    /// <para type="description">Calculates the hash of an input with multiple algorithms</para>
    /// <example>
    ///     <code>Get-FileHashBulk C:\foo.bar</code>
    ///     <para>Calculates the md5, sha1, and sha256 hashes of the C:\foo.bar file</para>
    /// </example>
    /// <example>
    ///     <code>Get-ChildItem C:\temp | Get-FileHashBulk -Algorithms @("md5", "sha512")</code>
    ///     <para>Calculates the md5 and sha512 hashes of all the files in C:\temp</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "FileHashBulk", SupportsShouldProcess = true)]
    [OutputType(typeof(HashResult))]
    public class FileHashCmdlet : PSCmdlet
    {
        /// <summary>
        /// FileHashCmdlet Constructor
        /// </summary>
        public FileHashCmdlet()
        {
            this.Algorithms = new string[] { "md5", "sha1", "sha256" };
        }

        /// <summary>
        /// Algorithms to calculate hashes
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "List of hashes to compute")]
        public String[] Algorithms { get; set; } //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.create?view=netframework-4.7#System_Security_Cryptography_HashAlgorithm_Create_System_String_

        /// <summary>
        /// The input file(s)
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName =true, HelpMessage = "List of algorithms")]
        [Alias("FullName")]
        public String[] InputFiles { get; set; }

        private Dictionary<String, HashAlgorithm> map;
        
        /// <summary>
        /// BeginProcessing - powershell setup
        /// </summary>
        protected override void BeginProcessing()
        {
            map = new Dictionary<string, HashAlgorithm>();

            //Create hashers
            foreach (var hsh in this.Algorithms)
            {
                map.Add(hsh, HashAlgorithm.Create(hsh));
            }
        }

        /// <summary>
        /// ProcessRecord - core powershell action
        /// </summary>
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

        /// <summary>
        /// EndProcessing - powershell cleanup
        /// </summary>
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators.VHash
{
    public class HashedFlag : Flag
    {


        public HashedFlag(ushort width, ushort height) : base(width, height) 
        {
            this.InternalHasher = HashAlgorithm.Create("SHA256");
        }

        public HashAlgorithm InternalHasher { get; set; }

        public override Bitmap Hash(byte[] data)
        {
            this.InternalHasher.Initialize();
            byte[] hashed = this.InternalHasher.ComputeHash(data);
            return base.Hash(hashed);
        }
    }
}

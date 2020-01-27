using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators.VHash
{
    //https://cs.stackexchange.com/questions/40714/looking-for-an-algorithm-to-generate-an-identicon-avatar-from-genome-data
    //https://stackoverflow.com/questions/3587569/easy-to-remember-fingerprints-for-data/3601278#3601278
    
    /// <summary>
    /// Type of Visual Hash
    /// </summary>
    public enum HashType
    {
        /// <summary>
        /// Direct - directly convert data (bytes) into 8Bit indexed BMP
        /// </summary>
        Direct,
        /// <summary>
        /// Flag - directly convert data (bytes) into FLAG
        /// </summary>
        Flag,
        /// <summary>
        /// HashedFlag - uses hash of data to turn into FLAG
        /// </summary>
        HashedFlag,

        /// <summary>
        /// Emoji 
        /// </summary>
        Emoji,

        //Honeycomb,
        //ASCII, //Keyart - https://github.com/atoponce/keyart - BSD License
        //Identicon, // https://github.com/stewartlord/identicon.js - BSD License
        //Inkblot, //Hashblot - https://github.com/stuartpb/hashblot - Unlicense
        //Fractal, //Visprint - http://www.tastyrabbit.net/visprint/ - GNU License
        //Keyhole, // https://github.com/mailpile/Mailpile/blob/master/scripts/colorprints.py - AGPLv3 License
        //VizHashGD, // https://sebsauvage.net/wiki/doku.php?id=php:vizhash_gd - zlib/libpng OSI licence
        //JrSnowflake, //https://wiki.debian.org/JrSnowflake - ??
        //QRCode, //QRCode.js - https://github.com/davidshimjs/qrcodejs - MIT License

        //Wavatar, //https://wordpress.org/plugins/wavatars/ - ??
        //MonsterID, //https://github.com/splitbrain/monsterID https://github.com/sandfoxme/monsterid - MIT License
        //RoboHash // https://robohash.org/ ??? - ??
    }

    public abstract class VisualHasher
    {
        public UInt16 Width { get; protected set; }
        public UInt16 Height { get; protected set; }
        public VisualHasher(ushort width, ushort height)
        {
            this.Width = width;
            this.Height = height;
        }

        protected virtual Bitmap GenerateBaseBitmap()
        {
            return new Bitmap(this.Width, this.Height);
        }

        public virtual Bitmap Hash(String data)
        {
            Byte[] dataBytes = System.Text.Encoding.Default.GetBytes(data);
            return this.Hash(dataBytes);
        }

        public abstract Bitmap Hash(Byte[] data);


        #region Static Methods

        public static VisualHasher CreateHasher(HashType name, UInt16 width, UInt16 height)
        {
            VisualHasher ans = null;
            switch (name)
            {
                case HashType.Emoji:
                    ans = new Emoji(width, height);
                    break;
                case HashType.HashedFlag:
                    ans = new HashedFlag(width, height);
                    break;
                case HashType.Flag:
                    ans = new Flag(width, height);
                    break;
                case HashType.Direct:
                    ans = new Direct(width, height);
                    break;
                default:
                    break;
            }
            return ans;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators.VHash
{
    public class Emoji : VisualHasher
    {
        //public static byte[] StringToByteArray(String hex)
        //{
        //    int NumberChars = hex.Length;
        //    byte[] bytes = new byte[NumberChars / 2];
        //    for (int i = 0; i < NumberChars; i += 2)
        //        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //    return bytes;
        //}

        public Emoji(ushort width, ushort height) : base(width, height)
        {
            this.StartChar = "🌀";
        }        

        public String StartChar { get; set; }


        private String AddBytes(byte data)
        {
            int num = Char.ConvertToUtf32(this.StartChar, 0);
            num += data;
            return Char.ConvertFromUtf32(num).ToString();
        }

        public override Bitmap Hash(byte[] data)
        {
            var bmp = this.GenerateBaseBitmap();

            ushort gridsize = (ushort)Math.Ceiling(Math.Sqrt((float)data.Length));
            ushort pixelSize = (ushort)(Math.Min(this.Width, this.Height) / gridsize);
            List<RectangleF> blocks = RandomImage.GetGridBlocks(this.Width, this.Height, pixelSize);

            using (var gfx = Graphics.FromImage(bmp))
            using (var enumerator = blocks.GetEnumerator())
            {
                for (int i = 0; i < data.Length; i++)
                {
                    enumerator.MoveNext();
                    String s = AddBytes(data[i]);
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        ShapesArtist.DrawEmoji(gfx, brush, enumerator.Current, s);
                    }                    
                }
            }
            return bmp;
        }
    }
}

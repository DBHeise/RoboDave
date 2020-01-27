using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators.VHash
{
    public class Flag : VisualHasher
    {
        public Flag(ushort width, ushort height) : base(width, height) { }


        public override Bitmap Hash(byte[] data)
        {
            var bmp = this.GenerateBaseBitmap();

            float blockHeight = (float)this.Height / ((float)data.Length /3);
            float y = 0f;
            using (var gfx = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < data.Length; i += 3)
                {                    
                    RectangleF rect = new RectangleF(0, y, this.Width, blockHeight);
                    
                    int red = data[i];
                    int green = (i + 1 < data.Length ? data[i + 1] : 0);
                    int blue = (i + 2 < data.Length ? data[i + 2] : 0);

                    using (Brush b = new SolidBrush(Color.FromArgb(red, green, blue)))
                    {
                        gfx.FillRectangle(b, rect);
                    }
                    y += blockHeight;
                }
            }

            return bmp;
        }
    }
}

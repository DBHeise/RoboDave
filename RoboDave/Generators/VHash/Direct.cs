using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators.VHash
{
    public class Direct : VisualHasher
    {
        public Direct(ushort width, ushort height) : base(width, height) { }

        public override Bitmap Hash(byte[] data)
        {

            int width = (int)Math.Ceiling(Math.Sqrt((float)data.Length));
            int height = width;
            var bmp = BitmapHelper.CreateBitmap(width, height, data);

            //var bmp = this.GenerateBaseBitmap();
            //float scale = Math.Min(this.Width / (float)width, this.Height / (float)height);

            //var gfx = Graphics.FromImage(bmp);

            //gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            //gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            //gfx.DrawImage(obmp, 0, 0, this.Width, this.Height);


            return bmp;
        }
    }
}

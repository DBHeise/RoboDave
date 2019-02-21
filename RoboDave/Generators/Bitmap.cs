

namespace RoboDave.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Management.Automation;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class BitmapHelper
    {
        internal static Bitmap CreateBitmap(int width, int height, byte[] data)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

    }

    [Cmdlet(VerbsCommon.New, "RandomBitmap", SupportsShouldProcess = true)]
    [OutputType(typeof(Bitmap))]    
    public class RandomBitmapCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Width")]
        public UInt16 Width { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Height")]
        public UInt16 Height { get; set; }

        protected override void ProcessRecord()
        {
            byte[] buffer = Rando.GetBytes(this.Width * this.Height);
            var bmp = BitmapHelper.CreateBitmap(this.Width, this.Height, buffer);

            WriteObject(bmp);
        }


    }

    [Cmdlet(VerbsCommon.Get, "BitmapFromFile", SupportsShouldProcess = true)]
    [OutputType(typeof(Bitmap))]
    public class BitmapFromFileCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory =true, ValueFromPipeline = true, HelpMessage = "Input file")]
        public String InputFile { get; set; }

        protected override void ProcessRecord()
        {
            byte[] buffer = System.IO.File.ReadAllBytes(this.InputFile);
            int width = (int)Math.Ceiling(Math.Sqrt((float)buffer.Length));
            int height = width;
            var bmp = BitmapHelper.CreateBitmap(width, height, buffer);
            WriteObject(bmp);
        }
    }
}

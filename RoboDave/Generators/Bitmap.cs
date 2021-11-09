

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
        internal static Bitmap CreateBitmap(int width, int height, byte[] data, PixelFormat format = PixelFormat.Format8bppIndexed)
        {
            Bitmap bmp = new Bitmap(width, height, format);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

    }

    /// <summary>
    /// <para type="synopsis">Generates a random bitmap</para>
    /// <para type="discription">Generates a random bitmap (8 bits per pixel, indexed)</para>
    /// <example>
    ///     <code>New-RandomBitmap</code>
    ///     <para>Generates a random bitmap 100 by 100 pixels</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomBitmap -Width 512 -Height 1024</code>
    ///     <para>Generates a random bitmap 512 by 1024 pixels</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomBitmap", SupportsShouldProcess = true)]
    [OutputType(typeof(Bitmap))]
    public class RandomBitmapCmdlet : PSCmdlet
    {
        /// <summary>
        /// RandomBitmapCmdlet Constructor
        /// </summary>
        public RandomBitmapCmdlet() : base()
        {
            this.Width = 100;
            this.Height = 100;
        }

        /// <summary>
        /// Width of the image to generate (in pixels)
        /// </summary>
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Width")]
        public UInt16 Width { get; set; }

        /// <summary>
        /// Height of the image to generate (in pixels)
        /// </summary>
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Height")]
        public UInt16 Height { get; set; }

        /// <summary>
        /// ProcessRecord - core powershell function
        /// </summary>
        protected override void ProcessRecord()
        {
            byte[] buffer = Rando.GetBytes(this.Width * this.Height);
            var bmp = BitmapHelper.CreateBitmap(this.Width, this.Height, buffer);

            WriteObject(bmp);
        }


    }
    /// <summary>
    /// <para type="synopsis">Returns a bitmap from a file</para>
    /// <para type="description">takes an input file and turns it into a bitmap using the bytes of the file as data</para>
    /// <example>
    ///     <code>Get-BitmapFromFile -InputFile C:\foo.bar</code>
    ///     <para>Generates a bitmap from the C:\foo.bar file</para>
    /// </example>
    /// <example>
    ///     <code>Get-ChildItem E:\Temp\*.txt | Get-BitmapFromFile</code>
    ///     <para>Takes all the files in the E:\Temp folder and returns bitmaps for each of them</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "BitmapFromFile", SupportsShouldProcess = true)]
    [OutputType(typeof(Bitmap))]
    public class BitmapFromFileCmdlet : PSCmdlet
    {
        /// <summary>
        /// The file to use as data for the bitmap
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "Input file")]
        [Alias("FullName")]
        public String InputFile { get; set; }

        /// <summary>
        /// Cmdlet ProcessRecord - main function for cmdlet work
        /// </summary>
        protected override void ProcessRecord()
        {
            byte[] buffer = System.IO.File.ReadAllBytes(this.InputFile);
            int width = (int)Math.Ceiling(Math.Sqrt((float)buffer.Length));
            int height = width;
            var bmp = BitmapHelper.CreateBitmap(width, height, buffer);
            WriteObject(bmp);
        }
    }

    /// <summary>
    /// TypedImage - specifies the algorithm for image generation
    /// </summary>
    public enum TypedImage
    {
        /// <summary>
        /// Random bits of color
        /// </summary>
        Random,

        /// <summary>
        /// Blocks of random color
        /// </summary>
        Pixel,

        /// <summary>
        /// Simple Shapes drawn randomly
        /// </summary>
        SimpleShape,

        /// <summary>
        /// Inserts simple shapes in blocks
        /// </summary>
        GridShapes
    }



    /// <summary>
    /// <para type="synopsis">Generates a new random image based on given patterns</para>
    /// <para type="description"></para>
    /// <example>
    ///     <code>New-RandomImage -Type Random</code>
    ///     <para>Creates a random bitmap</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomImage -Type Pixel -PixelSize 64 -Width 1024 -Height 1024</code>
    ///     <para>Creates an image of size 1024x1024 with 'blocks' of 64 pixels</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomImage -Type SimpleShape -Width 1024 -Height 1024 -Shape Circle -ShapeCount 2 -IsFilled $true</code>
    ///     <para>Creates an image of size 1024x1024 two filled circles of random size and color</para>
    /// </example>
    /// <example>
    ///     <code>New-RandomImage -Type SimpleShape -Width 1024 -Height 1024 -Shape RandomPolygon</code>
    ///     <para>Creates an image of size 1024x1024 with one random polygon</para>
    /// </example>    
    /// <example>
    ///     <code>New-RandomImage -Type SimpleShape -Width 1024 -Height 1024 -Shape RandomShape -ShapeCount 20 -IsFilled $true</code>
    ///     <para>Creates an image of size 1024x1024 with twenty random filled shapes of random size and color</para>
    /// </example>    
    /// <example>
    ///     <code>New-RandomImage -Type GridShapes -Width 1024 -Height 1024 -PixelSize 64 -Shape RandomShape -IsFilled $true </code>
    ///     <para>Creates an image of size 1024x1024 with 16x16 blocks each with a random filled shape</para>
    /// </example>    
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomImage", SupportsShouldProcess = true, DefaultParameterSetName = "Random")]
    [OutputType(typeof(Bitmap))]
    public class RandomImage : PSCmdlet
    {
        /// <summary>
        /// RandomImage Cmdlet constructor
        /// </summary>
        public RandomImage()
        {
            this.ShapeCount = 1;
            this.IsFilled = false;
            this.Shape = Shapes.Circle;
            this.Width = 512;
            this.Height = 512;
            this.PixelSize = 32;
        }

        /// <summary>
        /// <para type="description">Defines which algorithm to use to generate the image</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Type of image to generate, e.g. Random")]
        public TypedImage Type { get; set; }

        /// <summary>
        /// <para type="description">The Width (in pixels) of the bitmap</para>
        /// </summary>
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Width")]
        public UInt16 Width { get; set; }

        /// <summary>
        /// <para type="description">The Height (in pixels) of the bitmap</para>
        /// </summary>
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Height")]
        public UInt16 Height { get; set; }

        /// <summary>
        /// <para type="description">The nubmer of pixels per grid area (used only in Shape=Pixel and Shape=GridShapes)</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Size of 'pixels'")]
        public UInt16 PixelSize { get; set; }

        /// <summary>
        /// <para type="description">The Shape to use (used only in Shape=SimpleShape and Shape=GridShapes)</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Shape to draw")]
        public Shapes Shape { get; set; }

        /// <summary>
        /// <para type="description">Is the drawn shaped filled (used only in Shape=SimpleShape and Shape=GridShapes)</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Should the shape be filled")]
        public Boolean IsFilled { get; set; }

        /// <summary>
        /// <para type="description">The number of shapes to draw (used only in Shape=SimpleShape)</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Number of shapes to draw")]
        public UInt16 ShapeCount { get; set; }

        /// <summary>
        /// GetGridBlocks - splits the area (width by height) into blocks of pixelSize
        /// </summary>
        /// <param name="width">Width of the orignial area</param>
        /// <param name="height">Height of the orginial area</param>
        /// <param name="pixelSize">Number of pixels to form a block</param>
        /// <returns></returns>
        public static List<RectangleF> GetGridBlocks(UInt16 width, UInt16 height, UInt16 pixelSize)
        {
            var ans = new List<RectangleF>();

            //Setup grid rectangles
            for (int y = 0; y < height; y += pixelSize)
            {
                for (int x = 0; x < width; x += pixelSize)
                {
                    var rect = new RectangleF(x, y, pixelSize, pixelSize);
                    ans.Add(rect);
                }
            }
            return ans;

        }

        /// <summary>
        /// ProcessRecord - primary powershell cmdlet entrance
        /// </summary>
        protected override void ProcessRecord()
        {
            Bitmap bmp = null;
            switch (this.Type)
            {
                case TypedImage.SimpleShape:
                    {
                        bmp = new Bitmap(this.Width, this.Height);
                        var unit = GraphicsUnit.Pixel;
                        var rect = bmp.GetBounds(ref unit);
                        using (var gfx = Graphics.FromImage(bmp))
                        {
                            for (int i = 0; i < this.ShapeCount; i++)
                            {
                                ShapesArtist.DrawShape(gfx, rect, this.Shape, true, this.IsFilled);
                            }
                        }
                    }
                    break;
                case TypedImage.Pixel:
                    {
                        byte[] data = new byte[this.Width * this.Height];
                        byte[] c = null;

                        for (int y = 0; y < this.Height; y++)
                        {
                            if (y % this.PixelSize == 0)
                            {
                                c = Rando.GetBytes(this.Width / this.PixelSize);
                            }

                            for (int x = 0; x < this.Width; x++)
                            {
                                int idx = (y * this.Width) + x;

                                data[idx] = c[x / this.PixelSize];
                            }
                        }

                        bmp = BitmapHelper.CreateBitmap(this.Width, this.Height, data);

                        // Alternate method to do the same thing

                        //bmp = new Bitmap(this.Width, this.Height);
                        //List<RectangleF> gridBlocks = getGridBlocks();
                        //using (var gfx = Graphics.FromImage(bmp))
                        //{
                        //    foreach (var grid in gridBlocks)
                        //    {
                        //        using (var brush = new SolidBrush(Rando.RandomColor()))
                        //        {
                        //            ShapesArtist.DrawRectangle(gfx, brush, null, grid, true);
                        //        }
                        //    }
                        //}

                    }
                    break;
                case TypedImage.GridShapes:
                    {
                        bmp = new Bitmap(this.Width, this.Height);
                        List<RectangleF> gridBlocks = GetGridBlocks(this.Width, this.Height, this.PixelSize);

                        //create images for each grid rectangle
                        using (var gfx = Graphics.FromImage(bmp))
                        {
                            foreach (var grid in gridBlocks)
                            {
                                ShapesArtist.DrawShape(gfx, grid, this.Shape, false, this.IsFilled);
                            }
                        }
                    }
                    break;
                default:
                    {
                        byte[] buffer = Rando.GetBytes(this.Width * this.Height);
                        bmp = BitmapHelper.CreateBitmap(this.Width, this.Height, buffer);
                    }
                    break;
            }
            WriteObject(bmp);
        }
    }
}

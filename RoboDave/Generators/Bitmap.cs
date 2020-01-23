

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
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "Input file")]
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

    public enum Shapes
    {
        Square,
        Rectangle,
        Circle,
        Ellipse,
        Diamond,
        RandomPolygon,
        RandomShape
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
    ///     <code>New-RandomImage -Type SimpleShape -Width 1024 -Height 1024 -Shape RandomPolygon -PolygonPointCount 22</code>
    ///     <para>Creates an image of size 1024x1024 with one random polygon with 22 points</para>
    /// </example>    
    /// <example>
    ///     <code>New-RandomImage -Type SimpleShape -Width 1024 -Height 1024 -Shape RandomShape -ShapeCount 20 -IsFilled $true</code>
    ///     <para>Creates an image of size 1024x1024 with twenty random filled shapes of random size and color</para>
    /// </example>    
    /// <example>
    ///     <code>New-RandomImage -Type GridShapes -Width 1024 -Height 1024 -PixelSize 64 -Shape RandomShape -IsFilled $true -PolygonPointCount 5</code>
    ///     <para>Creates an image of size 1024x1024 with 16x16 blocks each with a random filled shape (and if its a polygon then it will have 5 points)</para>
    /// </example>    
    /// </summary>
    [Cmdlet(VerbsCommon.New, "RandomImage", SupportsShouldProcess = true, DefaultParameterSetName = "Random")]
    [OutputType(typeof(Bitmap))]
    public class RandomImage : PSCmdlet
    {
        public RandomImage()
        {
            this.ShapeCount = 1;
            this.IsFilled = false;
            this.Shape = Shapes.Circle;
            this.Width = 512;
            this.Height = 512;
            this.PixelSize = 32;
            this.PolygonPointCount = 5;
        }

        /// <summary>
        /// <para type="description">Defines which algorithm to use to generate the image</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Type of image to generate, e.g. Random")]
        public TypedImage Type { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Width")]
        public UInt16 Width { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Height")]
        public UInt16 Height { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Size of 'pixels'")]
        public UInt16 PixelSize { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Shape to draw")]
        public Shapes Shape { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Should the shape be filled")]
        public Boolean IsFilled { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Number of shapes to draw")]
        public UInt16 ShapeCount { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Number of points in the random polygon")]
        public UInt16 PolygonPointCount { get; set; }


        private static Shapes GetRandomShape()
        {
            List<Shapes> list = Enum.GetValues(typeof(Shapes)).Cast<Shapes>().ToList();
            list.Remove(Shapes.RandomShape);
            return Rando.RandomPick<Shapes>(list);
        }

        private static void drawShape(Graphics gfx, RectangleF area, Shapes shape, Boolean isfilled, UInt16 polygonPointCount)
        {
            Shapes trueshape = shape;
            if (trueshape == Shapes.RandomShape)
            {
                trueshape = GetRandomShape();
            }
            using (var brush = new SolidBrush(Rando.RandomColor()))
            using (var pen = new Pen(brush, Rando.RandomFloat(0.5f, 10.0f)))
            {
                switch (trueshape)
                {
                    case Shapes.Square:
                        {
                            float x = Rando.RandomFloat(area.X, area.X + area.Width);
                            float y = Rando.RandomFloat(area.Y, area.Y + area.Height);
                            float width = Rando.RandomFloat(1, Math.Min(area.Width, area.Height));
                            var rect = new RectangleF(x, y, width, width);
                            if (isfilled)
                            {
                                gfx.FillRectangle(brush, rect);
                            }
                            else
                            {
                                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                            }
                        }
                        break;
                    case Shapes.Rectangle:
                        {
                            float x = Rando.RandomFloat(area.X, area.X + area.Width);
                            float y = Rando.RandomFloat(area.Y, area.Y + area.Height);
                            float width = Rando.RandomFloat(1, area.Width);
                            float height = Rando.RandomFloat(1, area.Height);
                            var rect = new RectangleF(x, y, width, height);
                            if (isfilled)
                            {
                                gfx.FillRectangle(brush, rect);
                            }
                            else
                            {
                                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                            }
                        }
                        break;
                    case Shapes.Circle:
                        {
                            float cx = Rando.RandomFloat(area.X, area.X + area.Width);
                            float cy = Rando.RandomFloat(area.Y, area.Y + area.Height);
                            float radius = Rando.RandomFloat(1, Math.Min(area.Width, area.Height));
                            var rect = new RectangleF(cx - (radius / 2), cy - (radius / 2), radius, radius);
                            if (isfilled)
                            {
                                gfx.FillEllipse(brush, rect);
                            }
                            else
                            {
                                gfx.DrawEllipse(pen, rect);
                            }
                        }
                        break;
                    case Shapes.Ellipse:
                        {
                            float cx = Rando.RandomFloat(area.X, area.X + area.Width);
                            float cy = Rando.RandomFloat(area.Y, area.Y + area.Height);
                            float width = Rando.RandomFloat(1, area.Width);
                            float height = Rando.RandomFloat(1, area.Height);
                            var rect = new RectangleF(cx - (width / 2), cy - (height / 2), width, height);
                            if (isfilled)
                            {
                                gfx.FillEllipse(brush, rect);
                            }
                            else
                            {
                                gfx.DrawEllipse(pen, rect);
                            }
                        }
                        break;
                    case Shapes.Diamond:
                        {
                            float cx = Rando.RandomFloat(area.X, area.X + area.Width);
                            float cy = Rando.RandomFloat(area.Y, area.Y + area.Height);
                            float radius = Rando.RandomFloat(1, Math.Min(area.Width, area.Height));
                            PointF[] points = new PointF[]
                            {
                                    new PointF(cx, cy-radius),
                                    new PointF(cx+radius, cy),
                                    new PointF(cx, cy+radius),
                                    new PointF(cx-radius, cy)
                            };
                            if (isfilled)
                            {
                                gfx.FillPolygon(brush, points);
                            }
                            else
                            {
                                gfx.DrawPolygon(pen, points);
                            }
                        }
                        break;
                    case Shapes.RandomPolygon:
                        {
                            List<PointF> points = new List<PointF>();
                            for (float i = 0; i < polygonPointCount; i++)
                            {
                                float x = Rando.RandomFloat(area.X, area.X + area.Width);
                                float y = Rando.RandomFloat(area.Y, area.Y + area.Height);
                                points.Add(new PointF(x,y));
                            }
                            if (isfilled)
                            {
                                gfx.FillPolygon(brush, points.ToArray());
                            }
                            else
                            {
                                gfx.DrawPolygon(pen, points.ToArray());
                            }
                        }
                        break;
                }
            }
        }

        private static void drawShape(Bitmap bmp, RectangleF area, Shapes shape, Boolean isfilled, UInt16 polygonPointCount)
        {
            using (var gfx = Graphics.FromImage(bmp))
            {
                drawShape(gfx, area, shape, isfilled, polygonPointCount);
                gfx.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
            }
        }

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
                                drawShape(gfx, rect, this.Shape, this.IsFilled, this.PolygonPointCount);
                            }
                        }
                    }
                    break;
                case TypedImage.Pixel:
                    {
                        int smallWidth = (int)(this.Width / this.PixelSize);
                        int smallHeight = (int)(this.Height / this.PixelSize);
                        int smallsize = (int)(smallWidth * smallHeight);
                        byte[] dataSmall = Rando.GetBytes(smallsize);

                        byte[] data = new byte[this.Width * this.Height];

                        for (int x = 0; x < this.Width; x++)
                        {
                            for (int y = 0; y < this.Height; y++)
                            {
                                int idx = (x * this.Width) + y;

                                int idxXSmall = (int)(x / this.PixelSize);
                                int idxYSmall = (int)(y / this.PixelSize);
                                int idxsmall = (idxXSmall * smallWidth) + idxYSmall;

                                data[idx] = dataSmall[idxsmall % smallsize];
                            }
                        }

                        bmp = BitmapHelper.CreateBitmap(this.Width, this.Height, data);
                    }
                    break;
                case TypedImage.GridShapes:
                    {
                        bmp = new Bitmap(this.Width, this.Height);
                        List<RectangleF> gridBlocks = new List<RectangleF>();

                        //Setup grid rectangles
                        for (int y = 0; y < this.Height; y += this.PixelSize)
                        {
                            for (int x = 0; x < this.Width; x += this.PixelSize)
                            {
                                var rect = new RectangleF(x, y, this.PixelSize, this.PixelSize);
                                gridBlocks.Add(rect);
                            }
                        }

                        //create images for each grid rectangle
                        using (var gfx = Graphics.FromImage(bmp))
                        {
                            foreach (var grid in gridBlocks)
                            {
                                drawShape(gfx, grid, this.Shape, this.IsFilled, this.PolygonPointCount);
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

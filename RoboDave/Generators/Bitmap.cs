

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
        Random,
        Pixel,
        SimpleShape
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

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Type of Image to Generate")]
        public TypedImage Type { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Width")]
        public UInt16 Width { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Image Height")]
        public UInt16 Height { get; set; }

        [Parameter(ParameterSetName = "Pixel", ValueFromPipelineByPropertyName = true, HelpMessage = "Size of 'pixels'")]
        public UInt16 PixelSize { get; set; }

        [Parameter(ParameterSetName = "SimpleShape", ValueFromPipelineByPropertyName = true, HelpMessage = "Shape to draw")]
        public Shapes Shape { get; set; }

        [Parameter(ParameterSetName = "SimpleShape", ValueFromPipelineByPropertyName = true, HelpMessage = "Should the shape be filled")]
        public Boolean IsFilled { get; set; }

        [Parameter(ParameterSetName = "SimpleShape", ValueFromPipelineByPropertyName = true, HelpMessage = "Number of shapes to draw")]
        public UInt16 ShapeCount { get; set; }

        [Parameter(ParameterSetName = "SimpleShape", ValueFromPipelineByPropertyName = true, HelpMessage = "Number of points in the random polygon")]
        public UInt16 PolygonPointCount { get; set; }


        private static Shapes GetRandomShape()
        {
            List<Shapes> list = Enum.GetValues(typeof(Shapes)).Cast<Shapes>().ToList();
            list.Remove(Shapes.RandomShape);
            return Rando.RandomPick<Shapes>(list);
        }

        private static void drawShape(Bitmap bmp, Shapes shape, Boolean isfilled, UInt16 polygonPointCount)
        {
            Shapes trueshape = shape;
            if (trueshape == Shapes.RandomShape)
            {
                trueshape = GetRandomShape();
            }

            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(Rando.RandomColor()))
            using (var pen = new Pen(brush, Rando.RandomFloat(0.5f, 10.0f)))
            {
                switch (trueshape)
                {
                    case Shapes.Square:
                        {
                            int x = Rando.RandomInt(0, Math.Min(bmp.Width, bmp.Height));
                            int width = Rando.RandomInt(1, Math.Min(bmp.Width, bmp.Height) - x);
                            var rect = new Rectangle(x, x, width, width);
                            if (isfilled)
                            {
                                gfx.FillRectangle(brush, rect);
                            }
                            else
                            {
                                gfx.DrawRectangle(pen, rect);
                            }
                        }
                        break;
                    case Shapes.Rectangle:
                        {
                            int x = Rando.RandomInt(0, bmp.Width);
                            int y = Rando.RandomInt(0, bmp.Height);
                            int width = Rando.RandomInt(1, bmp.Width - x);
                            int height = Rando.RandomInt(1, bmp.Height - y);
                            var rect = new Rectangle(x, y, width, height);
                            if (isfilled)
                            {
                                gfx.FillRectangle(brush, rect);
                            }
                            else
                            {
                                gfx.DrawRectangle(pen, rect);
                            }
                        }
                        break;
                    case Shapes.Circle:
                        {
                            int cx = Rando.RandomInt(0, bmp.Width);
                            int cy = Rando.RandomInt(0, bmp.Height);
                            int radius = Rando.RandomInt(1, Math.Min(bmp.Width - cx, bmp.Height - cy));
                            var rect = new Rectangle(cx - (radius / 2), cy - (radius / 2), radius, radius);
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
                            int cx = Rando.RandomInt(0, bmp.Width);
                            int cy = Rando.RandomInt(0, bmp.Height);
                            int width = Rando.RandomInt(1, bmp.Width - cx);
                            int height = Rando.RandomInt(1, bmp.Height - cy);
                            var rect = new Rectangle(cx - (width / 2), cy - (height / 2), width, height);
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
                            int cx = Rando.RandomInt(0, bmp.Width);
                            int cy = Rando.RandomInt(0, bmp.Height);
                            int radius = Rando.RandomInt(1, Math.Min(bmp.Width - cx, bmp.Height - cy));
                            Point[] points = new Point[]
                            {
                                    new Point(cx, cy-radius),
                                    new Point(cx+radius, cy),
                                    new Point(cx, cy+radius),
                                    new Point(cx-radius, cy)
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
                            List<Point> points = new List<Point>();
                            for (int i = 0; i < polygonPointCount; i++)
                            {
                                points.Add(new Point(Rando.RandomInt(0, bmp.Width), Rando.RandomInt(0, bmp.Height)));
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
                gfx.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
            }

        }

        protected override void ProcessRecord()
        {
            Bitmap bmp;
            switch (this.Type)
            {
                case TypedImage.SimpleShape:
                    bmp = new Bitmap(this.Width, this.Height);
                    for (int i = 0; i < this.ShapeCount; i++)
                    {
                        drawShape(bmp, this.Shape, this.IsFilled, this.PolygonPointCount);
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
                default:
                    byte[] buffer = Rando.GetBytes(this.Width * this.Height);
                    bmp = BitmapHelper.CreateBitmap(this.Width, this.Height, buffer);
                    break;
            }
            WriteObject(bmp);
        }
    }
}

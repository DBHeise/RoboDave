using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators
{
    public enum Shapes
    {
        Square,
        Rectangle,
        Circle,
        Ellipse,
        Diamond,

        Triangle_Right,
        Triangle_CenterTop,
        Triangle_CenterBottom,
        Triangle_CenterLeft,
        Triangle_CenterRight,

        /*
            Parallelogram
            Traezoid
            Kite
            Trapezium
            Regular Pentagon
            Regular Hexagon
            Lemoine Hexagon
            Octagon
            Pentagram
            Hexagram
            Octagram

            Annulus (donut)
            Arbelos

            Crescent
            Circular Sector
            Vesica Piscis (fish bladder)
            Salinon
            Quatrefoil
            Trefoil
            Triqetra
            Heart
            Archimedean spiral
            Astroid
            Cardioid
            Deltoid
            Lemniscate
            Taijitu
            Tomoe
            Fish curve
            Clothoid
            Snowflake
            Nodary
            Sierpinski carpet
            Sierpinski triangle
            Rose Curve
         */

        RandomPolygon,
        RandomShape
    }

    public static class ShapesArtist
    {

        private static Shapes GetRandomShape()
        {
            List<Shapes> list = Enum.GetValues(typeof(Shapes)).Cast<Shapes>().ToList();
            list.Remove(Shapes.RandomShape);
            return Rando.RandomPick<Shapes>(list);
        }

        public static PointF[] GeneratePoints(Shapes shape, RectangleF bounds)
        {
            List<PointF> ans = new List<PointF>();

            float cx = bounds.X + (bounds.Width / 2);
            float cy = bounds.Y + (bounds.Height / 2);
            float radius = Math.Min(bounds.Width, bounds.Height) / 2;

            switch (shape)
            {
                case Shapes.Rectangle:
                case Shapes.Square:
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    break;
                case Shapes.Diamond:
                    ans.Add(new PointF(cx, cy - radius));
                    ans.Add(new PointF(cx + radius, cy));
                    ans.Add(new PointF(cx, cy + radius));
                    ans.Add(new PointF(cx - radius, cy));
                    break;
                case Shapes.Triangle_Right:
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    break;
                case Shapes.Triangle_CenterTop:
                    ans.Add(new PointF(cx, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y + bounds.Height));
                    ans.Add(new PointF(cx, bounds.Y));
                    break;
                case Shapes.Triangle_CenterBottom:
                    ans.Add(new PointF(cx, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y));
                    ans.Add(new PointF(cx, bounds.Y + bounds.Height));
                    break;
                case Shapes.Triangle_CenterLeft:
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, cy));
                    ans.Add(new PointF(bounds.X, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, bounds.Y));
                    break;
                case Shapes.Triangle_CenterRight:
                    ans.Add(new PointF(bounds.X, cy));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y));
                    ans.Add(new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height));
                    ans.Add(new PointF(bounds.X, cy));
                    break;
                case Shapes.RandomPolygon:
                    {
                        int count = Rando.RandomInt(3, 10);
                        for (int i = 0; i < count; i++)
                        {
                            float x, y;
                            Boolean isXBorder = Rando.RandomBoolean();
                            if (isXBorder)
                            {
                                if (Rando.RandomBoolean())
                                    x = bounds.X;
                                else
                                    x = bounds.X + bounds.Width;

                                y = Rando.RandomFloat(bounds.Y, bounds.Y + bounds.Height);
                            }
                            else
                            {
                                x = Rando.RandomFloat(bounds.X, bounds.X + bounds.Width);
                                if (Rando.RandomBoolean())
                                    y = bounds.Y;
                                else
                                    y = bounds.Y + bounds.Height;
                            }
                            ans.Add(new PointF(x, y));
                        }
                    }
                    break;
                default:
                    throw new NotSupportedException(String.Format("Generate Points does not support: {0}", shape.ToString()));
            }

            return ans.ToArray();
        }

     
        private static int lastSize = -1;
        private static bool nearlyEqual(float one, float two, float delta)
        {
            return Math.Abs(one - two) < delta;
        }
        private static Font growFontToFill(Graphics g, string graphicString, String fontName, float height)
        {
            Font testFont = null;

            int minFontSize = 1;
            int maxFontSize = int.MaxValue;
            
            int adjustedSize = (lastSize != -1 ? lastSize : minFontSize);
            for (; adjustedSize <= maxFontSize && adjustedSize >= minFontSize;)
            {
                testFont = new Font(fontName, adjustedSize);

                // Test the string with the new size
                SizeF adjustedSizeNew = g.MeasureString(graphicString, testFont);

                if (nearlyEqual(adjustedSizeNew.Height, height, 0.9f))
                {
                    // Good font, return it
                    lastSize = adjustedSize;
                    return testFont;
                }
                else if (adjustedSizeNew.Height > height)
                {
                    adjustedSize--;
                }
                else // adjustedSizeNew.Height < height
                {
                    adjustedSize++;
                }
            }

            throw new ArgumentOutOfRangeException("maxFontSize", "MaxFontSize is too small!");
        }

        public static void DrawEmoji(Graphics gfx, Brush brush, RectangleF rect, String emoji)
        {
            Font f = growFontToFill(gfx, emoji, "Segoe UI Symbol", rect.Height);

            gfx.DrawString(emoji, f, brush, rect);
        }

        public static void DrawEllipse(Graphics gfx, Brush brush, Pen pen, RectangleF rect, Boolean isFilled)
        {
            if (isFilled)
            {
                gfx.FillEllipse(brush, rect);
            }
            else
            {
                gfx.DrawEllipse(pen, rect);
            }
        }

        public static void DrawPolygon(Graphics gfx, Brush brush, Pen pen, PointF[] points, Boolean isFilled)
        {
            if (points.Length > 0)
            {
                if (isFilled)
                {
                    gfx.FillPolygon(brush, points);
                }
                else
                {
                    gfx.DrawPolygon(pen, points);
                }
            }
        }


        public static void DrawShape(Graphics gfx, RectangleF area, Shapes shape, Boolean randomWithin, Boolean isfilled)
        {
            Shapes trueshape = shape;
            if (trueshape == Shapes.RandomShape)
            {
                trueshape = GetRandomShape();
            }

            RectangleF rect = area;
            if (randomWithin)
            {
                float x = Rando.RandomFloat(area.X, area.X + area.Width);
                float y = Rando.RandomFloat(area.Y, area.Y + area.Height);
                float width = Rando.RandomFloat(1, area.Width);
                float height = Rando.RandomFloat(1, area.Height);
                rect = new RectangleF(x, y, width, height);
            }

            if (trueshape == Shapes.Square || trueshape == Shapes.Circle)
            {
                rect.Width = Math.Min(rect.Width, rect.Height);
                rect.Height = rect.Width;
            }

            using (var brush = new SolidBrush(Rando.RandomColor()))
            using (var pen = new Pen(brush, Rando.RandomFloat(0.5f, 10.0f)))
            {
                switch (trueshape)
                {

                    case Shapes.Circle:
                    case Shapes.Ellipse:
                        DrawEllipse(gfx, brush, pen, rect, isfilled);
                        break;
                    default:
                        {
                            PointF[] points = GeneratePoints(trueshape, rect);
                            DrawPolygon(gfx, brush, pen, points, isfilled);
                        }
                        break;
                }
            }
        }

        public static void DrawShape(Bitmap bmp, RectangleF area, Shapes shape, Boolean randomwithin, Boolean isfilled)
        {
            using (var gfx = Graphics.FromImage(bmp))
            {
                DrawShape(gfx, area, shape, randomwithin, isfilled);
                gfx.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
            }
        }

    }
}

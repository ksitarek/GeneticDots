using System;
using System.Drawing;

namespace GeneticDots.Domain
{
    public class Obstacle
    {
        public System.Windows.Point Start { get; }
        public System.Windows.Point End { get; }

        public Obstacle(System.Windows.Point start, System.Windows.Point end)
        {
            Start = start;
            End = end;
        }

        public Rectangle GetRectangle()
        {
            var rect = new Rectangle();
            rect.X = (int)Start.X;
            rect.Y = (int)Start.Y;

            rect.Width = (int)Math.Abs(End.X - Start.X);
            rect.Height = (int)Math.Abs(End.Y - Start.Y);

            return rect;
        }
    }
}
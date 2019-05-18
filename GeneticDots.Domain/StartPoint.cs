using System.Windows;

namespace GeneticDots.Domain
{
    public class StartPoint
    {
        public Point Position { get; }
        public double Radius { get; } = 5;

        public StartPoint(double x, double y, double radius = 5)
        {
            Position = new Point(x, y);
            Radius = radius;
        }
    }
}
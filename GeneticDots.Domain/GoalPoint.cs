using System.Windows;

namespace GeneticDots.Domain
{
    public class GoalPoint
    {
        public GoalPoint(double x, double y, double radius = 5)
        {
            Position = new Point(x, y);
            Radius = radius;
        }

        public Point Position { get; }

        public double Radius { get; }
    }
}
using System;
using System.Windows;

namespace GeneticDots.Domain
{
    public static class VectorExtensions
    {
        public static Vector Limit(this Vector vector, double max)
        {
            var squareMagnitude = vector.VectorSquaredMagnitude();
            if (squareMagnitude > Math.Pow(max, 2))
            {
                vector = Vector.Divide(vector, Math.Sqrt(squareMagnitude));
                vector = Vector.Multiply(max, vector);
            }

            return vector;
        }

        public static double VectorSquaredMagnitude(this Vector vector)
        {
            return Math.Pow(vector.X, 2)
                 + Math.Pow(vector.Y, 2);
        }
    }
}
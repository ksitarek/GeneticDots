using System;
using System.Windows;

namespace GeneticDots.Domain
{
    public class Brain
    {
        private readonly Vector[] _instructions;
        private readonly Random _random = new Random();

        public int Step { get; private set; } = 0;

        public Brain(int instructionsCnt)
        {
            _instructions = new Vector[instructionsCnt];

            // make instructions random
            for (var i = 0; i < instructionsCnt; i++)
            {
                _instructions[i] = GetRandomAngleVector();
            }
        }

        public Brain(Vector[] instructions)
        {
            _instructions = instructions;
        }

        public bool HasNextStep()
            => Step < _instructions.Length;

        public Vector GetNextStep()
        {
            if (!HasNextStep())
                return new Vector();

            return _instructions[Step++];
        }

        public Brain Clone()
        {
            return new Brain((Vector[])_instructions.Clone());
        }

        public void Mutate()
        {
            var chance = 0.01;
            for (var i = 0; i < _instructions.Length; i++)
            {
                double rand = _random.NextDouble();
                if (rand < chance)
                {
                    _instructions[i] = GetRandomAngleVector();
                }
            }
        }

        private Vector GetRandomAngleVector()
        {
            var randomDouble = _random.NextDouble();
            var twoPi = Math.PI * 2;
            var randomAngle = randomDouble * twoPi - Math.PI;

            var x = Math.Cos(randomAngle);
            var y = Math.Sin(randomAngle);

            return new Vector(x, y);
        }
    }
}
using System;
using System.Diagnostics;
using System.Windows;

namespace GeneticDots.Domain
{
    [DebuggerDisplay("Id: {Id}, Fitness: {Fitness}")]
    public class Dot
    {
        public Guid Id = Guid.NewGuid();

        public readonly Brain Brain;
        private readonly StartPoint _start;

        public bool IsDead { get; private set; } = false;
        public bool ReachedGoal { get; private set; } = false;

        public Point Position { get; private set; }
        public double Fitness { get; private set; }

        private Vector _velocity;
        private Vector _acceleration;

        public Dot(StartPoint start)
            : this(start, new Brain(1000))

        {
        }

        private Dot(StartPoint start, Brain brain)
        {
            Brain = brain;

            Position = new Point(start.Position.X, start.Position.Y);

            _velocity = new Vector(0, 0);
            _acceleration = new Vector(0, 0);
            _start = start;
        }

        public void Update()
        {
            if (IsDead || ReachedGoal)
                return;

            // can we make a move?
            if (Brain.HasNextStep())
            {
                Move();
            }
            else
            {
                // the dot died of old age
                SetIsDead();
                return;
            }
        }

        public Dot Clone()
        {
            var brainClone = Brain.Clone();
            var clone = new Dot(_start, brainClone);

            return clone;
        }

        private void Move()
        {
            _acceleration = Brain.GetNextStep();

            _velocity = Vector.Add(_velocity, _acceleration);
            _velocity = _velocity.Limit(5);

            Position = Vector.Add(_velocity, Position);
        }

        public void SetIsDead() => IsDead = true;

        public void SetReachedGoal() => ReachedGoal = true;

        public void SetFitness(double fitness) => Fitness = fitness;
    }
}
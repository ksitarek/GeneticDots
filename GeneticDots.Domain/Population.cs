using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GeneticDots.Domain
{
    public class Population
    {
        public Dot[] Dots;
        private readonly int _size;
        private readonly StartPoint _start;
        private readonly GoalPoint _goal;
        private readonly List<Obstacle> _obstacles;
        private int _currentStep = 0;

        private int _generation = 1;

        private PopulationScore _score;
        private Random _random = new Random();

        public Population(int size, StartPoint start, GoalPoint goal, List<Obstacle> obstacles)
        {
            _size = size;
            _start = start;
            _goal = goal;
            _obstacles = obstacles;

            Dots = new Dot[_size];
            for (var i = 0; i < size; i++)
                Dots[i] = new Dot(_start);

            _score = new PopulationScore(goal, Dots);
        }

        public void Update()
        {
            if (Dots.All(d => d.IsDead || d.ReachedGoal))
            {
                // all dots are dead, let's evaluate population
                _score.CalculateFitness();

                // get best dot and make make an evolution of it
                var newDots = new Dot[_size];
                newDots[0] = _score.BestDot.Clone();

                // generate rest of the dots
                for (var i = 1; i < _size; i++)
                {
                    var parent = SelectParent();
                    newDots[i] = parent.Clone();

                    // mutate dot's brain
                    newDots[i].Brain.Mutate();
                }
                _generation++;

                newDots.CopyTo(Dots, 0);
                _score = new PopulationScore(_goal, Dots);
            }

            foreach (var dot in Dots)
            {
                if (_score.MaxSteps.HasValue && _currentStep > _score.MaxSteps)
                {
                    dot.SetIsDead();
                }
                else
                {
                    dot.Update();
                }

                if (HasCollision(dot))
                {
                    dot.SetIsDead();
                }

                if (HaveReachedGoal(dot))
                {
                    // this is the first dot
                    dot.SetReachedGoal();

                    // kill all the others
                    foreach (var d in Dots.Where(d => d != dot))
                        d.SetIsDead();

                    // jump out of the loop
                    continue;
                }
            }

            _currentStep++;
        }

        private bool HaveReachedGoal(Dot dot)
        {
            // calculate distance to the goal
            var gpx = _goal.Position.X;
            var gpy = _goal.Position.Y;
            var dpx = dot.Position.X;
            var dpy = dot.Position.Y;

            var distance = Math.Sqrt(
                  Math.Pow(gpx - dpx, 2)
                + Math.Pow(gpy - dpy, 2)
            );

            return distance <= _goal.Radius;
        }

        private Dot SelectParent()
        {
            var randomFitness = _random.NextDouble() * _score.FitnessSum;

            var runningSum = 0.0;
            for (var i = 0; i < Dots.Length; i++)
            {
                runningSum += Dots[i].Fitness;
                if (runningSum > randomFitness)
                    return Dots[i];
            }

            return null;
        }

        public bool HasCollision(Dot dot)
        {
            var dpx = dot.Position.X;
            var dpy = dot.Position.Y;

            var collidingObstacles = _obstacles
                .Where(o => o.Start.X <= dpx && o.End.X >= dpx)
                .Where(o => o.Start.Y <= dpy && o.End.Y >= dpy);

            return collidingObstacles.Any();
        }
    }

    public class PopulationScore
    {
        public Dot BestDot { get; private set; }
        public double FitnessSum { get; private set; }

        private readonly GoalPoint _goal;
        private readonly Dot[] _dots;

        public int? MaxSteps { get; private set; }

        public PopulationScore(GoalPoint goal, Dot[] dots)
        {
            _goal = goal;
            _dots = dots;
        }

        public void CalculateFitness()
        {
            foreach (var dot in _dots)
            {
                CalculateFitness(dot);
                FitnessSum += dot.Fitness;

                // set best dot
                if (BestDot == null || BestDot.Fitness < dot.Fitness)
                    BestDot = dot;

                // if best dot have reached goal, set current step as max
                if (BestDot.ReachedGoal)
                    MaxSteps = BestDot.Brain.Step;
            }
        }

        private void CalculateFitness(Dot dot)
        {
            var fitness = 0.0;
            if (dot.ReachedGoal)
            {
                fitness = 1 / 16.0 + 10_000.0 / Math.Pow(dot.Brain.Step, 2);
            }
            else
            {
                var distance = CalculateDistanceToGoal(dot.Position);
                fitness = 1 / Math.Pow(distance, 2);
            }

            dot.SetFitness(fitness);
        }

        private double CalculateDistanceToGoal(Point point)
        {
            var gpx = _goal.Position.X;
            var gpy = _goal.Position.Y;

            return Math.Sqrt(
                    Math.Pow(gpx - point.X, 2)
                  + Math.Pow(gpy - point.Y, 2)
                );
        }
    }
}
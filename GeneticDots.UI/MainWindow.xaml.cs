using GeneticDots.Domain;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace GeneticDots.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Renderer _renderer;
        private StartPoint _startPoint;
        private GoalPoint _goalPoint;
        private List<Obstacle> _obstacles;
        private Population _population;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitializeGame();
            InitializeRenderer();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();
            timer.Tick += Draw;
        }

        private void InitializeGame()
        {
            var width = (int)Playground.Width;
            var height = (int)Playground.Height;

            _startPoint = new StartPoint(width / 2, height - 50, 15);
            _goalPoint = new GoalPoint(width / 2, 30, 15);

            // walls
            _obstacles = new List<Obstacle>() {
                new Obstacle(new Point(-9, 0), new Point(1, height)),
                new Obstacle(new Point(0, -9), new Point(width, 1)),
                new Obstacle(new Point(width-1, 0), new Point(width+9, height)),
                new Obstacle(new Point(0, height-1), new Point(width, height+9))
            };

            // a few obstacles
            _obstacles.Add(new Obstacle(new Point(300, 200), new Point(600, 300)));

            _population = new Population(10000, _startPoint, _goalPoint, _obstacles);
        }

        private void Draw(object sender, EventArgs e)
        {
            _population.Update();
            Playground.Source = _renderer.Draw(_population.Dots);
        }

        private void InitializeRenderer()
        {
            _renderer = new Renderer((int)Playground.Width, (int)Playground.Height);
            _renderer.SetStartPoint(_startPoint);
            _renderer.SetGoalPoint(_goalPoint);

            _renderer.AddObstacles(_obstacles);
        }
    }
}
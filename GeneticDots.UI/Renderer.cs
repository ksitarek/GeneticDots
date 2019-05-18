using GeneticDots.Domain;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace GeneticDots.UI
{
    public class Renderer
    {
        private int _width;
        private int _height;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private GoalPoint _goalPoint;
        private StartPoint _startPoint;
        private List<Obstacle> _obstacles;
        private readonly Color _backgroundColor = Color.White;

        private readonly Brush _startBrush = Brushes.Green;
        private readonly Brush _goalBrush = Brushes.Blue;
        private readonly Brush _dotBrush = Brushes.Red;
        private readonly Brush _obstacleBrush = Brushes.Black;

        public Renderer(int actualWidth, int actualHeight)
        {
            _width = actualWidth;
            _height = actualHeight;

            InitializeBitmap();
        }

        public void SetGoalPoint(GoalPoint goalPoint)
            => _goalPoint = goalPoint;

        public void SetStartPoint(StartPoint startPoint)
            => _startPoint = startPoint;

        public void AddObstacles(List<Obstacle> obstacles)
            => _obstacles = obstacles;

        public System.Windows.Media.ImageSource Draw(Dot[] dots)
        {
            _graphics.Clear(_backgroundColor);

            DrawStart();
            DrawGoal();
            DrawObstacles();

            foreach (var dot in dots)
                DrawDot(dot);

            return ConvertBitmapToImageSource();
        }

        private void DrawObstacles()
        {
            var rectangles = _obstacles
                .Select(o => o.GetRectangle())
                .ToArray();

            _graphics.FillRectangles(_obstacleBrush, rectangles);
        }

        private void DrawDot(Dot dot)
        {
            var dotPosition = dot.Position;

            _graphics.FillEllipse(_dotBrush,
                (float)dotPosition.X - 3,
                (float)dotPosition.Y - 3,
                6, 6);
        }

        private System.Windows.Media.ImageSource ConvertBitmapToImageSource()
        {
            using (var ms = new MemoryStream())
            {
                _bitmap.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void InitializeBitmap()
        {
            _bitmap = new Bitmap(_width, _height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(_backgroundColor);
        }

        private void DrawStart()
        {
            _graphics.FillEllipse(_startBrush,
                (float)(_startPoint.Position.X - _startPoint.Radius / 2),
                (float)(_startPoint.Position.Y - _startPoint.Radius / 2),
                (float)_startPoint.Radius,
                (float)_startPoint.Radius);
        }

        private void DrawGoal()
        {
            _graphics.FillEllipse(_goalBrush,
                (float)(_goalPoint.Position.X - _goalPoint.Radius / 2),
                (float)(_goalPoint.Position.Y - _goalPoint.Radius / 2),
                (float)_goalPoint.Radius,
                (float)_goalPoint.Radius);
        }
    }
}
using MazeSolver.Métier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeSolver.Ihm.Thread
{
    public class PathSearchDisplay : IThreadDispatcher
    {
        private readonly DispatcherTimer dispatcherTimer;
        private readonly MazeController mazeController;
        private readonly List<Square> pathSearchSquares;
        private const long NANO_SECOND_TIMER = 100000; //~0.1s
        private bool isDisplaying;
        private readonly Dictionary<Square, int> nbApparition;
        public PathSearchDisplay(MazeController mazeController)
        {
            this.mazeController = mazeController;
            nbApparition = new Dictionary<Square, int>();
            isDisplaying = true;
            pathSearchSquares = new List<Square>();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Display;
            dispatcherTimer.Interval = new TimeSpan(NANO_SECOND_TIMER);
            Initialisation();
        }

        private void Initialisation()
        {
            pathSearchSquares.Clear();
            foreach(Square s in mazeController.PathSearchSquares)
            {
                if (s.Type == SquareType.PATH && !pathSearchSquares.Contains(s))
                {
                    pathSearchSquares.Add(s);
                    nbApparition.TryAdd(s, 0);
                }
            }
        }

        public void Display(object sender, EventArgs e)
        {
            if (pathSearchSquares.Count != 0)
            {
                Square square = pathSearchSquares[0];
                pathSearchSquares.RemoveAt(0);                
                Rectangle rectangle = mazeController.GetRectangle(square);
                rectangle.Fill = GetBrushes(square);              
            }
            else
            {
                StopThread();
            }
        }

        private Brush GetBrushes(Square square)
        {
            Brush b;
            switch(nbApparition[square])
            {
                case 0: b = Brushes.Cyan;  break;
                case 1: b = Brushes.DarkCyan;  break;
                case 2: b = Brushes.LightBlue; break;
                case 3: b = Brushes.Blue; break;
                case 4: b = Brushes.Black; break;
                default: b = Brushes.Black; break;
            }
            nbApparition[square] = nbApparition[square] + 1;
            return b;
        }

        public void StartThread()
        {
            dispatcherTimer.Start();
        }

        public void StopThread()
        {
            dispatcherTimer.Stop();
            isDisplaying = false;
        }
        public bool IsDisplaying { get => isDisplaying; set => isDisplaying = value; }
    }
}

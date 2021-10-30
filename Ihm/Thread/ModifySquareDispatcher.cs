using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Shapes;
using MazeSolver.Métier;

namespace MazeSolver.Ihm.Thread
{
    /// <summary>
    /// Classe Implémentant IThreadDispatcher. C'est un thread permettant d'afficher pas à pas la génération du labyrinthe
    /// </summary>
    public class ModifySquareDispatcher : IThreadDispatcher
    {
        private readonly DispatcherTimer dispatcher;
        private const long NANO_SECOND_TIMER = 100000000; //~10s
        private readonly Dictionary<Square, Rectangle> mazeRectangle;
        private readonly MazeController mazeController;

        public ModifySquareDispatcher(MazeController mazeController, Dictionary<Square, Rectangle> mazeRectangle)
        {
            this.mazeController = mazeController;
            this.mazeRectangle = mazeRectangle;
            dispatcher = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcher.Tick += new EventHandler(Display);
            dispatcher.Interval = new TimeSpan(NANO_SECOND_TIMER /(long)Math.Pow((Settings.GetInstance().MazeSize), 2));
        }

        public void Display(object sender, EventArgs e)
        {
            if (mazeController.SquareToDisplay.Count > 0)
            {
                Square s = mazeController.SquareToDisplay[0];
                mazeController.SquareToDisplay.RemoveAt(0);
                Rectangle r = mazeRectangle[s];
                r.Fill = mazeController.GetSquareFill(s);
            }
            else
            {
                StopThread();
            }
        }

        public void StartThread()
        {
            dispatcher.Start();
        }

        public void StopThread()
        {
            dispatcher.Stop();
        }
    }
}

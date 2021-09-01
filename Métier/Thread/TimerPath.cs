﻿using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MazeSolver.Métier.Thread
{
    /// <summary>
    /// Classe représentant un timer pour CompareWindow
    /// </summary>
    public class TimerPath
    {
        private readonly PathDisplayer pathDisplayer;           //objet qui fait apparaitre le chemin
        private readonly Label timerLabel;                      //label affichant le timer
        private readonly DispatcherTimer dispatcherTimer;       //objet Thread permettant de mettre à jour le Label
        private readonly DateTime startTime;                    //Date of the start of the Thread
        private const long NANO_SECOND_TIMER = 100000;          // this * 100 ~= 0.01 second
                                                                // *100 because TimeSpan use ticks, 1 tick = 100 nanosecond

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="pathDisplayer">pathDisplayer associé au timer</param>
        /// <param name="label">label affichant le timer</param>
        public TimerPath(PathDisplayer pathDisplayer, Label label)
        {
            this.pathDisplayer = pathDisplayer;
            this.timerLabel = label;
            this.dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Tick += new EventHandler(DispatchTime);
            dispatcherTimer.Interval = new TimeSpan(NANO_SECOND_TIMER);
            startTime = DateTime.Now;
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Méthode utilisé par le thread pour afficher le timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatchTime(object sender, EventArgs e)
        {
            if(pathDisplayer.IsDispatching)
            {
                DateTime dateTime = DateTime.Now;
                TimeSpan time = dateTime - startTime;
                timerLabel.Content = time.TotalSeconds.ToString("F2");
            }
            else
            {
                dispatcherTimer.Stop();
            }
        }
    }
}

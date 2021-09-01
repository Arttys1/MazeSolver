using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MazeSolver.Métier.Thread
{
    public class TimerPath
    {
        private readonly PathDisplayer pathDisplayer;
        private readonly Label label;
        private readonly DispatcherTimer dispatcherTimer;
        private readonly DateTime startTime;
        private const long NANO_SECOND_TIMER = 100000;

        public TimerPath(PathDisplayer pathDisplayer, Label label)
        {
            this.pathDisplayer = pathDisplayer;
            this.label = label;
            this.dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Tick += new EventHandler(DispatchTime);
            dispatcherTimer.Interval = new TimeSpan(NANO_SECOND_TIMER);
            startTime = DateTime.Now;
            dispatcherTimer.Start();
        }

        private void DispatchTime(object sender, EventArgs e)
        {
            if(pathDisplayer.IsDispatching)
            {
                DateTime dateTime = DateTime.Now;
                TimeSpan time = dateTime - startTime;
                label.Content = time.TotalSeconds.ToString("F2");
            }
            else
            {
                dispatcherTimer.Stop();
            }
        }
    }
}

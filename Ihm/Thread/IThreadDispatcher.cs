using System;

namespace MazeSolver.Ihm.Thread
{
    public interface IThreadDispatcher
    {
        void Display(object sender, EventArgs e);
        void StartThread();
        void StopThread();
    }
}
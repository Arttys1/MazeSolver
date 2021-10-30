using System;

namespace MazeSolver.Ihm.Thread
{
    /// <summary>
    /// Interface représentant les threads utilisé dans ce programme.
    /// </summary>
    public interface IThreadDispatcher
    {
        /// <summary>
        /// Méthode affichant quelque chose. Appeler lors des Ticks de la classe DispatcherTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Display(object sender, EventArgs e);

        /// <summary>
        /// Méthode commençant le thread.
        /// </summary>
        void StartThread();

        /// <summary>
        /// Méthode terminant le thread.
        /// </summary>
        void StopThread();
    }
}
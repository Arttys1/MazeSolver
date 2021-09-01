﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeSolver.Métier.Thread
{
    /// <summary>
    /// Classe gérant la gestion de l'affichage du chemin
    /// </summary>
    public class PathDisplayer
    {
        private readonly List<Square> path;                         //Chemin du labyrinthe
        private readonly DispatcherTimer dispatcher;                //Objet permettant d'afficher 1 à 1 les éléments du chemin
        private readonly Grid grid;                                 //Grille d'affichage
        private readonly Maze maze;                                 //Labyrinthe concerné
        private bool isDispatching;
        private const long NANO_SECOND_TIMER = 1000000;     // this * 100 ~= 0.1 second
                                                            // *100 because TimeSpan use ticks, 1 tick = 100 nanosecond

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="path">Chemin pour résoudre la labyrinthe</param>
        /// <param name="grid">Grille d'affichage</param>
        /// <param name="maze">labyrinthe</param>
        public PathDisplayer(List<Square> path, Grid grid, Maze maze)
        {
            this.grid = grid;
            this.maze = maze;
            this.path = path;
            isDispatching = true;
            dispatcher = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcher.Tick += new EventHandler(DisplayPath);
            dispatcher.Interval = new TimeSpan(NANO_SECOND_TIMER);
        }

        /// <summary>
        /// Méthode permettant d'afficher un rectangle du chemin.
        /// Elle sera Appelés plusieurs fois par le DispatcherTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayPath(object sender, EventArgs e)
        {
            if (path.Count > 0)
            {
                Square square = path[0];
                path.RemoveAt(0);
                Rectangle r = maze.GetRectangle(square);
                
                if (r != null)
                {
                    double squareSize = Math.Max(Settings.GetInstance().SquareSize / 3, 2);     //Side of these squares must be minimum 2.
                    Rectangle rectangle = new Rectangle
                    {
                        Height = squareSize,
                        Width = squareSize,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(r.Margin.Left + squareSize, r.Margin.Top + squareSize, 0, 0),
                        Fill = Brushes.Red,
                    };

                    grid.Children.Add(rectangle);
                }
            }
            else
            {
                StopThread();
            }
        }

        /// <summary>
        /// Méthode permettant de commencer l'affichage
        /// </summary>
        public void StartThread()
        {
            dispatcher.Start();
        }

        /// <summary>
        /// Méthode permettant l'arret du DispatcherTimer
        /// </summary>
        private void StopThread()
        {
            dispatcher.Stop();
            isDispatching = false;
        }

        public bool IsDispatching => isDispatching;
    }
}

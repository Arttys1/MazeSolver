using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.PathSearchAlgorithm
{
    public abstract class PathSearchAlgorithm
    {
        private readonly MazeController mazeController;             //intermédiaire entre l'ihm et le model
        private readonly Dictionary<Square, Square> predecesseur;   //Predeccesseur d'une case
        private readonly Maze maze;                                 //labyrinthe

        protected PathSearchAlgorithm(MazeController mazeController)
        {
            this.mazeController = mazeController;
            maze = mazeController.Maze;
            predecesseur = new Dictionary<Square, Square>();            
        }

        /// <summary>
        /// Méthode permettant de calculer les distances de chaque case du labyrinthe de l'arrivé.
        /// </summary>
        /// <param name="start">Le départ du labyrinthe</param>
        public abstract void CalculDistanceMaze(Square start);

        /// <summary>
        /// Méthode renvoyant le chemin pour aller à une case donnée
        /// </summary>
        /// <param name="end">représente la case jusqu'ou l'on souhaite allée</param>
        /// <returns>le chemin pour aller à une case donnée</returns>
        public virtual List<Square> GetPath(Square end)
        {
            List<Square> path = new List<Square>() { end };
            Square pred = end;

            while (predecesseur[pred] != null)
            {
                path.Add(predecesseur[pred]);
                pred = predecesseur[pred];
            }

            path.Add(Maze.Start);
            path.Reverse();
            return path;
        }
        /// <summary>
        /// Accesseur du mazeController
        /// </summary>
        protected MazeController MazeController => mazeController;
        /// <summary>
        /// Accesseur du labyrinthe
        /// </summary>
        protected Maze Maze => maze;
        /// <summary>
        /// Accesseur de predecesseur
        /// </summary>
        protected Dictionary<Square, Square> Predecesseur => predecesseur;

        /// <summary>
        /// méthode permettant d'ajuster le predecesseur
        /// </summary>
        /// <param name="a">la case</param>
        /// <param name="b">le predecesseur</param>
        protected void SetPredecesseur(Square a, Square b)
        {
            Predecesseur[a] = b;
        }

    }
}

using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe abstraite implémentant MazeBuildingAlgorithm, représentant les algorithmes de génération de labyrinte ayant besoin de connaître les cases visités
    /// </summary>
    public abstract class VisitingAlgorithm : MazeBuildingAlgorithm
    {
        private readonly Dictionary<Square, bool> isVisited;    //Les cases visités du labyrinthe

        protected VisitingAlgorithm(MazeController mazeController, List<Square> walls, List<Square> paths) : base(mazeController, walls, paths)
        {
            isVisited = new Dictionary<Square, bool>();
        }

        /// <summary>
        /// Accesseur de la liste des cases visités
        /// </summary>
        public Dictionary<Square, bool> IsVisited => isVisited;

        /// <summary>
        /// Méthode permettant l'initialisation de l'algorithme. Peut être Surchargé ou masqué.
        /// </summary>
        protected virtual void Initialisation()
        {
            isVisited.Clear();
            foreach (Square square in Paths)
            {
                isVisited.Add(square, false);
            }
        }

        /// <summary>
        /// Méthode permettant de vérifier si toutes les cases sont visitées
        /// </summary>
        /// <returns>true si toutes les cases sont visitées, false sinon</returns>
        protected virtual bool IsFinished()
        {
            bool res = true;
            foreach (bool b in isVisited.Values)
            {
                if (!b)
                {
                    res = false;
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// Méthode fusionnant deux case ainsi que le murs qui les séparent.
        /// </summary>
        /// <param name="a">Case à fusionner</param>
        /// <param name="b">Case à fusionner</param>
        protected virtual void CombineTowSquare(Square a, Square b)
        {
            Square wall = null;
            foreach (Square VoisinA in a.Voisins)
            {
                foreach (Square VoisinB in b.Voisins)
                {
                    if (VoisinA == VoisinB)
                    {
                        wall = VoisinB;
                    }
                }
            }

            if (wall != null)
            {
                wall.Type = SquareType.PATH;
                wall.MazeNumber = b.MazeNumber;
                a.MazeNumber = b.MazeNumber;
                Paths.Add(wall);
                Walls.Remove(wall);
                MazeController.AddSquareToDisplay(wall);
                MazeController.AddSquareToDisplay(a);                
                //MazeController.AddSquareToDisplay(b);
                
            }
        }

        /// <summary>
        /// Méthode renvoyant les cases parcourables autour d'une case donnée.
        /// Elle renvoie seulement les cases parcourables et non visités
        /// </summary>
        /// <param name="square">case centrale</param>
        /// <returns>Les cases parcourables, non visités autour d'une case donnée</returns>
        protected virtual List<Square> GetAdjacentsSquares(Square square)
        {
            List<Square> adjacentsSquares = new List<Square>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Square secondSquare;
                Square firstSquare = Maze.GetSquare(square.Coordinates.GetNeighbor(direction));
                if (firstSquare != null && firstSquare.Type != SquareType.BORDURE)
                {
                    secondSquare = Maze.GetSquare(firstSquare.Coordinates.GetNeighbor(direction));

                    if (secondSquare != null && secondSquare.Type != SquareType.BORDURE)
                    {
                        adjacentsSquares.Add(secondSquare);
                    }
                }
            }

            return adjacentsSquares;
        }
    }
}


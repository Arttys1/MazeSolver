using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
namespace MazeSolver.Métier.Algorithme.PathSearchAlgorithm
{
    /// <summary>
    /// Classe, hérité de PathSearchAlgorithm, représentant l'algorithme de A*.
    /// Cet algorithme est une amélioration de Dijkstra.
    /// Lien vers le wikipédia détaillant l'algorithme : 
    /// "https://fr.wikipedia.org/wiki/Algorithme_A*"
    /// </summary>
    public class AStar : PathSearchAlgorithm
    {
        private readonly Dictionary<Square, int> distances; //Distance d'une case par rapport au départ
        private const int infini = int.MaxValue - 1000;     //valeur représentant l'infini. Nombre que l'on suppose ne jamais atteindre.

        public AStar(MazeController mazeController) : base(mazeController)
        {
            distances = new Dictionary<Square, int>();
            foreach(Square s in Maze.GetAllSquares())
            {
                distances.Add(s, infini);
                SetPredecesseur(s, null);
            }
            distances[Maze.Start] = 0;            
        }

        /// <summary>
        /// Renvoie le nombre case séparant la case en paramètre de la case arrivé.
        /// </summary>
        /// <param name="s">Case que l'on souhaite</param>
        /// <returns>Le nombre case séparant la case en paramètre de la case arrivé.</returns>
        private int GetHScore(Square s)
        {
            return Math.Abs(Maze.End.Coordinates.X - s.Coordinates.X) + Math.Abs(Maze.End.Coordinates.Y - s.Coordinates.Y);
        }
        /// <summary>
        /// Renvoie le nombre case séparant la case en paramètre de la case départ.
        /// </summary>
        /// <param name="s">La case que l'on souhaite.</param>
        /// <returns>Renvoie le nombre case séparant la case en paramètre de la case départ.</returns>
        private int GetGScore(Square s)
        {
            return Math.Abs(Maze.Start.Coordinates.X - s.Coordinates.X) + Math.Abs(Maze.Start.Coordinates.Y - s.Coordinates.Y);
        }
        /// <summary>
        /// Renvoie la somme du nombre de case entre le départ la case passé en paramètre et l'arrivé.
        /// </summary>
        /// <param name="s">La case que l'on souhaite.</param>
        /// <returns>Renvoie la somme du nombre de case entre le départ la case passé en paramètre et l'arrivé.</returns>
        private int GetFScore(Square s)
        {
            return GetGScore(s) + GetHScore(s);
        }

        /// <summary>
        /// Renvoie toutes les cases voisines traversables.
        /// </summary>
        /// <param name="square">La case centrale.</param>
        /// <returns>Les cases voisines traversables.</returns>
        private List<Square> GetAdjacentesSquares(Square square)
        {
            List<Square> adjacentsSquares = new List<Square>();

            foreach(Square s in square.Voisins)
            {
                if(s.Type == SquareType.PATH)
                {
                    adjacentsSquares.Add(s);
                }
            }

            return adjacentsSquares;
        }

        public override void CalculDistanceMaze(Square start)
        {
            MazeController.AddPathSearchSquares(start);
            List<Square> openList = new List<Square>();
            List<Square> closedList = new List<Square>();
            bool trouver = false;
            openList.Add(start);
            Square current;

            while (openList.Count != 0 && !trouver)
            {
                current = openList[0];
                openList.RemoveAt(0);
                if (current != Maze.End)
                {
                    closedList.Add(current);
                    foreach (Square neighbor in GetAdjacentesSquares(current))
                    {
                        if (!closedList.Contains(neighbor))
                        {
                            if (!openList.Contains(neighbor))
                            {
                                openList.Add(neighbor);
                                openList.Sort((square1, square2) => GetHScore(square1).CompareTo(GetHScore(square2)));
                                SetPredecesseur(neighbor, current);
                                MazeController.AddPathSearchSquares(neighbor);
                                int gScore = GetGScore(neighbor);
                                distances[neighbor] = gScore;
                            }
                            else
                            {
                                UpdateSquareScore(neighbor);
                            }
                        }
                    }
                }
                else
                {
                    trouver = true;
                }
            }
        }

        /// <summary>
        /// Méthode actualisant les distances d'une case.
        /// </summary>
        /// <param name="neighbor">La case à actualisé.</param>
        private void UpdateSquareScore(Square neighbor)
        {
            if(Predecesseur[neighbor] != null)
            {
                int gScore = GetGScore(neighbor);
                if(distances[neighbor] > gScore)
                {
                    distances[neighbor] = gScore;
                    MazeController.AddPathSearchSquares(neighbor);
                    UpdateSquareScore(Predecesseur[neighbor]);
                }
            }
        }
    }
}

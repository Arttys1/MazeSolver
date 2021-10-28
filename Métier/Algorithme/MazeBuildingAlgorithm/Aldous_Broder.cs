using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    public class Aldous_Broder : MazeBuildingAlgorithm
    {
        private readonly Dictionary<Square, bool> isVisited;
        public Aldous_Broder(Maze maze, List<Square> walls, List<Square> paths) : base(maze, walls, paths)
        {
            isVisited = new Dictionary<Square, bool>();
        }

        private void Initialisation()
        {
            isVisited.Clear();
            foreach(Square square in Paths)
            {
                isVisited.Add(square, false);
            }
        }

        /// <summary>
        /// Méthode permettant de vérifier si toutes les cases sont visitées
        /// </summary>
        /// <returns>true si toutes les cases sont visitées, false sinon</returns>
        private bool IsFinished()
        {
            bool res = true;
            foreach(bool b in isVisited.Values)
            {
                if(!b)
                {
                    res = false;
                }
            }
            return res;
        }
        
        public override void CreateMaze()
        {
            Initialisation();
            Random random = new Random();
            Square currentCell = Paths[random.Next(Paths.Count)];
            isVisited[currentCell] = true;
            while(!IsFinished())
            {
                List<Square> neighbor = GetAdjacentsSquares(currentCell);
                Square nextCell = neighbor[random.Next(neighbor.Count)];
                if(!isVisited[nextCell])
                {
                    CombineTowSquare(currentCell, nextCell);
                    isVisited[nextCell] = true;
                }
                currentCell = nextCell;
            }

            int number = random.Next();
            foreach(Square square in Paths)
            {
                square.MazeNumber = number;
            }
            Maze.Start.MazeNumber = number;
            Maze.End.MazeNumber = number;
        }

        /// <summary>
        /// Méthode fusionnant deux case ainsi que le murs qui les séparent.
        /// </summary>
        /// <param name="a">Case à fusionner</param>
        /// <param name="b">Case à fusionner</param>
        private void CombineTowSquare(Square a, Square b)
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
            }
        }

        /// <summary>
        /// Méthode renvoyant les cases parcourables autour d'une case donnée.
        /// Elle renvoie seulement les cases parcourables et non visités
        /// </summary>
        /// <param name="square">case centrale</param>
        /// <returns>Les cases parcourables, non visités autour d'une case donnée</returns>
        private List<Square> GetAdjacentsSquares(Square square)
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

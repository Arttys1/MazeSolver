using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe hérité de VisitingAlgorithm.
    /// Elle permet de créer un labyrinthe selon l'algorithme d'Aldous-Broder.
    /// Lien vers le wikipédia détaillant l'algorithme :
    /// https://en.wikipedia.org/wiki/Maze_generation_algorithm
    /// </summary>
    public class Aldous_Broder : VisitingAlgorithm
    {
        public Aldous_Broder(MazeController mazeController, List<Square> walls, List<Square> paths) : base(mazeController, walls, paths) { }

        public override void CreateMaze()
        {
            Initialisation();
            Random random = new Random();
            Square currentCell = Paths[random.Next(Paths.Count)];
            IsVisited[currentCell] = true;
            while (!IsFinished())
            {
                List<Square> neighbor = GetAdjacentsSquares(currentCell);
                Square nextCell = neighbor[random.Next(neighbor.Count)];
                if (!IsVisited[nextCell])
                {
                    CombineTowSquare(currentCell, nextCell);
                    IsVisited[nextCell] = true;
                }
                currentCell = nextCell;
            }

            int number = random.Next();
            foreach (Square square in Paths)
            {
                square.MazeNumber = number;
            }
            Maze.Start.MazeNumber = number;
            Maze.End.MazeNumber = number;
            AddStartEndToDisplayer();
        }

        protected override void CombineTowSquare(Square a, Square b)
        {
            base.CombineTowSquare(a, b);
            MazeController.AddSquareToDisplay(b);
        }

    }
}

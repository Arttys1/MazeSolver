using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe hérité de VisitingAlgorithm.
    /// Elle permet de créer un labyrinthe selon l'algorithme de Wilson.
    /// Lien vers le wikipédia détaillant l'algorithme :
    /// https://en.wikipedia.org/wiki/Maze_generation_algorithm
    /// </summary>
    public class Wilson : VisitingAlgorithm
    {
        public Wilson(MazeController mazeController, List<Square> walls, List<Square> paths) : base(mazeController, walls, paths) { }

        public override void CreateMaze()
        {
            Initialisation();
            Random r = new Random();
            Square currentCell;
            Square nextCell;
            while(!IsFinished())
            {
                currentCell = Paths[r.Next(Paths.Count)];
                IsVisited[currentCell] = true;
                List<Square> neighbors = GetAdjacentsSquares(currentCell);
                nextCell = neighbors[r.Next(neighbors.Count)];

                while (!IsVisited[nextCell])
                {
                    CombineTowSquare(currentCell, nextCell);
                    IsVisited[nextCell] = true;
                    currentCell = nextCell;
                    neighbors = GetAdjacentsSquares(currentCell);
                    nextCell = neighbors[r.Next(neighbors.Count)];
                }
            }

            AddStartEndToDisplayer();
        }
    }
}

using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.PathSearchAlgorithm
{
    public abstract class PathSearchAlgorithm
    {
        private readonly MazeController mazeController;
        private readonly Maze maze;

        protected PathSearchAlgorithm(MazeController mazeController)
        {
            this.mazeController = mazeController;
            maze = mazeController.Maze;
        }

        protected MazeController MazeController => mazeController;

        protected Maze Maze => maze;

        public abstract void CalculDistanceMaze(Square start);
        public abstract List<Square> GetPath(Square end);

    }
}

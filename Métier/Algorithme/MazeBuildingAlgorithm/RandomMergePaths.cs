using MazeSolver.Ihm;
using System;
using System.Collections.Generic;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe hérité de MazeBuildingAlgorithm.
    /// Elle permet de créer un labyrinthe selon l'agorithme de la fusion aléatoire de chemins.
    /// Lien vers un wikipédia détaillant l'algorithme:
    /// https://fr.wikipedia.org/wiki/Modélisation_mathématique_de_labyrinthe
    /// </summary>
    public class RandomMergePaths : MazeBuildingAlgorithm
    {


        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="maze">le labyrinthe</param>
        /// <param name="walls">les murs intérieurs du labyrinthe</param>
        /// <param name="paths">les cases parcourables du labyrinthe</param>
        public RandomMergePaths(MazeController mazeController, List<Square> walls, List<Square> paths) : base(mazeController, walls, paths) { }
        public override void CreateMaze()
        {
            while (!IsFinish())
            {
                Square wall = Walls[new Random().Next(Walls.Count)];
                OpenWall(wall);
            }
            AddStartEndToDisplayer();
        }

        /// <summary>
        /// Méthode permettant d'ouvrir un mur et de rassembler les deux cases
        /// </summary>
        /// <param name="wall">mur à ouvrir</param>
        private void OpenWall(Square wall)
        {
            Square voisinA = null;
            Square voisinB = null;
            if(Maze.GetSquare(wall.Coordinates.GetNeighbor(Direction.HAUT)).Type == SquareType.PATH)
            {
                voisinA = Maze.GetSquare(wall.Coordinates.GetNeighbor(Direction.HAUT));
                voisinB = Maze.GetSquare(wall.Coordinates.GetNeighbor(Direction.BAS));
            }
            else if (Maze.GetSquare(wall.Coordinates.GetNeighbor(Direction.GAUCHE)).Type == SquareType.PATH)
            {
                voisinA = Maze.GetSquare(wall.Coordinates.GetNeighbor(Direction.GAUCHE));
                voisinB = Maze.GetSquare(wall.Coordinates.GetNeighbor(Direction.DROITE));
            }

            if (voisinA != null && voisinB != null)
            {
                if (voisinA.MazeNumber != voisinB.MazeNumber)
                {
                    voisinA.MazeNumber = voisinB.MazeNumber;
                    wall.MazeNumber = voisinB.MazeNumber;
                    wall.Type = SquareType.PATH;
                    Paths.Add(wall);
                    Walls.Remove(wall);
                    MazeController.AddSquareToDisplay(voisinA);
                    MazeController.AddSquareToDisplay(voisinB);
                    MazeController.AddSquareToDisplay(wall);
                    ContinuousPath(voisinA);
                    ContinuousPath(voisinB);
                }
            }
        }

        /// <summary>
        /// Méthode permettant de fusionner deux segments pour qu'ils aient le même identifiant
        /// </summary>
        /// <param name="start">case de départ</param>
        private void ContinuousPath(Square start)
        {
            List<Square> toDo = new List<Square>() { start };

            while (toDo.Count != 0)
            {
                Square current = toDo[0];
                toDo.RemoveAt(0);
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    Square cellNext = Maze.GetSquare(current.Coordinates.GetNeighbor(dir));
                    if (cellNext != null && cellNext.Type == SquareType.PATH && cellNext.MazeNumber != current.MazeNumber)
                    {
                        cellNext.MazeNumber = current.MazeNumber;
                        toDo.Add(cellNext);
                    }
                }
            }
        }

        /// <summary>
        /// Méthode déterminant si l'algorithme est finit
        /// </summary>
        /// <returns>true si toutes les cases parcourables ont le mêmes identifiants</returns>
        private bool IsFinish()
        {
            bool finish = true;
            int value = Paths[0].MazeNumber;
            
            foreach(Square square in Paths)
            {
                if(square.MazeNumber != value)
                {
                    finish = false;
                    break;
                }
            }

            return finish;
        }
    }
}

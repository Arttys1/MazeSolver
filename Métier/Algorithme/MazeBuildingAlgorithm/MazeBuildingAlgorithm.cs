using MazeSolver.Ihm;
using System.Collections.Generic;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe abstraite représentant un algorithme de création de labyrinthe
    /// </summary>
    public abstract class MazeBuildingAlgorithm
    {
        private readonly Maze maze;                     //labyrinthe
        private readonly List<Square> walls;            //les murs intérieurs du labyrinthe
        private readonly List<Square> paths;            //les cases parcourables du labyrinthe
        private readonly MazeController mazeController; //L'intermédiaire en l'ihm et le model

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="maze">le labyrinthe</param>
        /// <param name="walls">les murs intérieurs du labyrinthe</param>
        /// <param name="paths">les cases parcourables du labyrinthe</param>
        protected MazeBuildingAlgorithm(MazeController mazeController, List<Square> walls, List<Square> paths)
        {
            this.mazeController = mazeController;
            maze = mazeController.Maze;
            this.walls = walls;
            this.paths = paths;
        }

        /// <summary>
        /// Méthode permettant de créer le labyrinthe
        /// </summary>
        public abstract void CreateMaze();

        /// <summary>
        ///Accesseur du labyrinthe 
        /// </summary>
        protected Maze Maze => maze;

        /// <summary>
        /// Accesseur des murs
        /// </summary>
        protected List<Square> Walls => walls;

        /// <summary>
        /// Accesseur des cases parcourables
        /// </summary>
        protected List<Square> Paths => paths;  

        /// <summary>
        /// Accesseur du mazeController
        /// </summary>
        protected MazeController MazeController => mazeController;

        /// <summary>
        /// Méthode permettant d'ajouter le départ à l'arriver à la liste d'affichage du thread
        /// </summary>
        protected void AddStartEndToDisplayer()
        {
            MazeController.AddSquareToDisplay(Maze.Start);
            MazeController.AddSquareToDisplay(Maze.End);
        }
    }
}

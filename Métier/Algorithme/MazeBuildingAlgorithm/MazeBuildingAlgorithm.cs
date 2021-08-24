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

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="maze">le labyrinthe</param>
        /// <param name="walls">les murs intérieurs du labyrinthe</param>
        /// <param name="paths">les cases parcourables du labyrinthe</param>
        protected MazeBuildingAlgorithm(Maze maze, List<Square> walls, List<Square> paths)
        {
            this.maze = maze;
            this.walls = walls;
            this.paths = paths;
        }

        /// <summary>
        /// Méthode permettant de créer le labyrinthe
        /// </summary>
        public abstract void CreateMaze();

        protected Maze Maze => maze;        //Accesseur du labyrinthe 

        protected List<Square> Walls => walls;  //Accesseur des murs

        protected List<Square> Paths => paths;  //Accesseur des cases parcourables
    }
}

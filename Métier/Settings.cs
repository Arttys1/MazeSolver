using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;

namespace MazeSolver.Métier
{
    /// <summary>
    /// Classe contenant les paramètres. C'est un singleton.
    /// </summary>
    public class Settings
    {
        private static Settings instance = null;    //Instance de l'objet, il est unique.

        private double mazeSize;
        private MazeBuildingAlgorithmType mazeBuildingAlgorithmType;
        private bool isComplexMaze;
        private double squareSize;

        /// <summary>
        /// Constructeur privée, accessible uniquement depuis la méthode getInstance.
        /// </summary>
        private Settings()
        {
            squareSize = 20;
            mazeSize = 25;
            mazeBuildingAlgorithmType = MazeBuildingAlgorithmType.ExhaustiveExploration;
            isComplexMaze = false;
        }

        /// <summary>
        /// Méthode renvoyant l'unique instance de l'objet.
        /// </summary>
        /// <returns>L'instance de l'objet</returns>
        public static Settings GetInstance()
        {
            if(instance == null)
            {
                instance = new Settings();
            }

            return instance;
        }

        public double MazeSize
        {
            get => mazeSize;
            set { mazeSize = value; squareSize = System.Math.Round(500 / value); }
        }
        public bool IsComplexMaze { get => isComplexMaze; set => isComplexMaze = value; }
        public double SquareSize { get => squareSize; }
        public MazeBuildingAlgorithmType MazeBuildingAlgorithmType { get => mazeBuildingAlgorithmType; set => mazeBuildingAlgorithmType = value; }
    }
}

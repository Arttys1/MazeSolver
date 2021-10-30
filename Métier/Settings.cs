using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;

namespace MazeSolver.Métier
{
    /// <summary>
    /// Classe contenant les paramètres. C'est un singleton.
    /// </summary>
    public class Settings
    {
        private static Settings instance = null;    //Instance de l'objet, il est unique.

        private double squareSize;                                      //représente la taille d'un coté d'une rectangle
        private double mazeSize;                                        //représente le nombre de case d'un côté du labyrinthe
        private MazeBuildingAlgorithmType mazeBuildingAlgorithmType;    //représente quel algorithme de construction de labyrinthe va être utilisé.
        private bool isComplexMaze;                                     //représente si le labyrinthe sera "simple" ou "complexe" (chemin unique ou non)
        private bool instantGeneration;                                 //représente si le labyrinthe sera générer instannément ou s'il sera généré pas à pas

        /// <summary>
        /// Constructeur privée, accessible uniquement depuis la méthode getInstance.
        /// </summary>
        private Settings()
        {
            squareSize = 20;
            mazeSize = 25;
            mazeBuildingAlgorithmType = MazeBuildingAlgorithmType.ExhaustiveExploration;
            isComplexMaze = false;
            instantGeneration = true;
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

        /// <summary>
        /// Accesseur et mutateur de la taille du labyrinthe.
        /// Modifie la taille des rectangles pour que la labyrinthe face environ 500 pixels de côté.
        /// </summary>
        public double MazeSize
        {
            get => mazeSize;
            set { mazeSize = value; squareSize = System.Math.Round(500 / value); }
        }

        /// <summary>
        /// Accesseur et mutateur de isComplexMaze
        /// </summary>
        public bool IsComplexMaze { get => isComplexMaze; set => isComplexMaze = value; }

        /// <summary>
        /// Accesseur de SquareSize
        /// </summary>
        public double SquareSize { get => squareSize; }

        /// <summary>
        /// Accesseur et mutateur de mazeBuildingAlgorithmType
        /// </summary>
        public MazeBuildingAlgorithmType MazeBuildingAlgorithmType { get => mazeBuildingAlgorithmType; set => mazeBuildingAlgorithmType = value; }

        /// <summary>
        /// Accesseur et mutateur du booléen instantGeneration
        /// </summary>
        public bool InstantGeneration { get => instantGeneration; set => instantGeneration = value; }
    }
}

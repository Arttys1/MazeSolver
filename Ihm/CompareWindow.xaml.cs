using MazeSolver.Métier;
using MazeSolver.Métier.Algorithme;
using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;
using MazeSolver.Métier.Thread;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MazeSolver.Ihm
{
    /// <summary>
    /// Logique d'interaction pour CompareWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        private readonly Maze mazeExhaustiveExploration;        //labyrinthe où sera utilisé l'algorithme d'exploration exhaustive
        private readonly Maze mazeRandomMergePath;              //labyrinthe où sera utilisé l'algorithme de fusion aléatoire des chemins

        /// <summary>
        /// Constructeur
        /// </summary>
        public CompareWindow()
        {
            InitializeComponent();
            mazeExhaustiveExploration = new Maze(gridExhaustiveExploration);
            mazeRandomMergePath = new Maze(gridRandomMergePath);
        }

        /// <summary>
        /// Méthode pour générer les labyrinthes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateMazes(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;

            mazeExhaustiveExploration.CreateRandomMaze(MazeBuildingAlgorithmType.ExhaustiveExploration);
            mazeRandomMergePath.CreateRandomMaze(MazeBuildingAlgorithmType.RandomMergePaths);

            mazeExhaustiveExploration.UpdateMaze();
            mazeRandomMergePath.UpdateMaze();
        }

        /// <summary>
        /// Méthode pour résoudre les labyrinthes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResolveMazes(object sender, RoutedEventArgs e)
        {
            
            Button button = (Button)sender;
            button.IsEnabled = false;

            Dijkstra dijkstra = new Dijkstra(mazeRandomMergePath);
            dijkstra.CalculDistanceMaze(mazeRandomMergePath.Start);
            List<Square> firstPath = dijkstra.GetPath(mazeRandomMergePath.End);

            Dijkstra dijkstraSecond = new Dijkstra(mazeExhaustiveExploration);
            dijkstraSecond.CalculDistanceMaze(mazeExhaustiveExploration.Start);
            List<Square> secondPath = dijkstraSecond.GetPath(mazeExhaustiveExploration.End);


            new PathDisplayer(firstPath, gridRandomMergePath, mazeRandomMergePath).StartThread();
            new PathDisplayer(secondPath, gridExhaustiveExploration, mazeExhaustiveExploration).StartThread();
            
        }
    }
}

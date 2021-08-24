using MazeSolver.Métier;
using MazeSolver.Métier.Algorithme;
using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;
using MazeSolver.Métier.Thread;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeSolver.Ihm
{
    /// <summary>
    /// Logique d'interaction pour CompareWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        private const double SQUARESIZE = 20;

        private readonly Maze mazeExhaustiveExploration;
        private readonly Maze mazeRandomMergePath;

        public CompareWindow()
        {
            InitializeComponent();
            mazeExhaustiveExploration = new Maze(gridExhaustiveExploration);
            mazeRandomMergePath = new Maze(gridRandomMergePath);
        }

        private void GenerateMazes(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;

            mazeExhaustiveExploration.CreateRandomMaze(MazeBuildingAlgorithmType.ExhaustiveExploration);
            mazeRandomMergePath.CreateRandomMaze(MazeBuildingAlgorithmType.RandomMergePaths);

            mazeExhaustiveExploration.UpdateMaze();
            mazeRandomMergePath.UpdateMaze();
        }
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

using MazeSolver.Ihm;
using MazeSolver.Métier;
using MazeSolver.Métier.Algorithme;
using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;
using MazeSolver.Métier.Thread;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace MazeSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Maze maze;      // le labyrinthe        

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            maze = new Maze(grid);
            AddMouseEnter();
        }

        /// <summary>
        /// Méthode permettant de créer le labyrinthe à partir de la grille
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateRandomMaze(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                try
                {
                    button.IsEnabled = false;

                    maze.CreateRandomMaze(Settings.GetInstance().MazeBuildingAlgorithmType);

                    maze.UpdateMaze();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                    button.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Méthode qui ajoute à tous les rectangles un evenement mouseEnter
        /// </summary>
        private void AddMouseEnter()
        {
            foreach (Rectangle rectangle in maze.GetAllRectangle())
            {
                rectangle.MouseEnter += Rectangle_MouseEnter;
            }
        }

        /// <summary>
        /// Méthode affichant les coordonnées de la case pointé par le curseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            double squareSize = Settings.GetInstance().SquareSize;
            double x = (rectangle.Margin.Left - 10) / squareSize;
            double y = (rectangle.Margin.Top - 10) / squareSize;
            CoordinatesLabel.Content = "(" + x + "/" + y + ")";
        }

        /// <summary>
        /// Méthode permettant de résoudre le labyrinthe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResolveMaze(object sender, RoutedEventArgs e)
        {
            Dijkstra dijkstra = new Dijkstra(maze);
            dijkstra.CalculDistanceMaze(maze.Start);
            PathDisplayer pathDisplayer = new PathDisplayer(dijkstra.GetPath(maze.End), grid, maze);
            pathDisplayer.StartThread();
        }

        /// <summary>
        /// Méthode permettant de reset le labyrinthe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetMaze(object sender, RoutedEventArgs e)
        {
            maze = new Maze(grid);
            AddMouseEnter();
            ButtonRandomMaze.IsEnabled = true;
        }

        /// <summary>
        /// Méthode permettant d'ouvrir la fenetre qui compare les deux algorithmes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareTwoAlgorithm(object sender, RoutedEventArgs e)
        {
            CompareWindow compareWindow = new CompareWindow();
            compareWindow.Show();
        }

        /// <summary>
        /// Méthode gérant l'évenement de changer la selection du comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Settings settings = Settings.GetInstance();

            switch (comboBox.SelectedIndex)
            {
                case 0: settings.MazeSize = 11; break;
                case 1: settings.MazeSize = 25; break;
                case 2: settings.MazeSize = 51; break;
                case 3: settings.MazeSize = 75; break;
                case 4: settings.MazeSize = 101; break;
                default: throw new System.Exception("SelectedIndex non-implémented !");
            }

            if (SquareSizeLabel != null)
            {
                SquareSizeLabel.Content = settings.SquareSize;
            }
            ResetMaze(null, null);
        }

        /// <summary>
        /// Méthode gérant l'évenement de cliquer sur la checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComplexMazeCheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.IsChecked == true)
            {
                Settings.GetInstance().IsComplexMaze = true;
            }
            else
            {
                Settings.GetInstance().IsComplexMaze = false;
            }
        }

        private void InitAlgoType(object sender, System.EventArgs e)
        {
            CBAlgoType.Items.Clear();
            foreach (MazeBuildingAlgorithmType algo in Enum.GetValues(typeof(MazeBuildingAlgorithmType)))
            {
                CBAlgoType.Items.Add(algo);
            }
            CBAlgoType.SelectedIndex = 0;
        }

        private void AlgoTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cb)
            {
                try
                {
                    MazeBuildingAlgorithmType type = (MazeBuildingAlgorithmType)cb.SelectedItem;
                    Settings.GetInstance().MazeBuildingAlgorithmType = type;

                } catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                } 
            }
        }
    } 
}

using MazeSolver.Ihm;
using MazeSolver.Métier;
using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;
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
        private readonly MazeController mazeController; //Intermédiaire entre l'ihm et le model

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            mazeController = new MazeController(new Maze(), grid);
            AddMouseEnter();
            mazeController.AddChangeStartEnd();
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

                    mazeController.CreateRandomMaze(Settings.GetInstance().MazeBuildingAlgorithmType);
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
            foreach (Rectangle rectangle in mazeController.GetAllRectangle())
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
            mazeController.ResolveMaze();
        }

        /// <summary>
        /// Méthode permettant de reset le labyrinthe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetMaze(object sender, RoutedEventArgs e)
        {
            if (mazeController != null)
            {
                mazeController.ResetMaze(new Maze());
                AddMouseEnter();
                mazeController.AddChangeStartEnd();
                ButtonRandomMaze.IsEnabled = true;
            }
        }

        /// <summary>
        /// Méthode gérant l'évenement de changer la selection du comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                Settings settings = Settings.GetInstance();

                settings.MazeSize = comboBox.SelectedIndex switch
                {
                    0 => 11,
                    1 => 25,
                    2 => 51,
                    3 => 75,
                    4 => 101,
                    _ => throw new System.Exception("SelectedIndex non-implemented !"),
                };
                if (SquareSizeLabel != null)
                {
                    SquareSizeLabel.Content = settings.SquareSize;
                }
                ResetMaze(null, null);
            }
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

        /// <summary>
        /// Méthode appelé à l'initialisation de la fenêtre remplissant la ComboBox des différents types d'algorithme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitAlgoType(object sender, System.EventArgs e)
        {
            CBAlgoType.Items.Clear();
            foreach (MazeBuildingAlgorithmType algo in Enum.GetValues(typeof(MazeBuildingAlgorithmType)))
            {
                CBAlgoType.Items.Add(algo);
            }
            CBAlgoType.SelectedIndex = 0;
        }

        /// <summary>
        /// Méthode appelé lors du changement du choix de l'algorithme de création
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    mazeController.ResetMaze(new Maze());
                }
            }
        }

        /// <summary>
        /// Méthode appelé lors du Chech ou du UnCheck de la CheckBox. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckInstantGeneration(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked == true)
                {
                    Settings.GetInstance().InstantGeneration = true;
                }
                else
                {
                    Settings.GetInstance().InstantGeneration = false;
                }
            }
        }
    } 
}

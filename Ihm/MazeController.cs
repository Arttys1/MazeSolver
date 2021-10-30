using MazeSolver.Ihm.Thread;
using MazeSolver.Métier;
using MazeSolver.Métier.Algorithme;
using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeSolver.Ihm
{
    /// <summary>
    /// Classe intermédiaire entre l'ihm et le model. Elle permet de controller le labyrinthe.
    /// </summary>
    public class MazeController
    {
        private readonly Grid grid;                                         //La grille d'affichage du labyrinthe
        private Maze maze;                                                  //le labyrinthe
        private readonly Dictionary<Square, Rectangle> mazeRectangle;       //Les rectangles associés à leurs cases
        private readonly List<Square> squareToDisplay;                      //Les cases à afficher dans le thread
        private readonly List<IThreadDispatcher> threads;                   //Les Threads en court

        public MazeController(Maze maze, Grid grid)
        {
            this.maze = maze;
            this.grid = grid;
            threads = new List<IThreadDispatcher>();
            mazeRectangle = new Dictionary<Square, Rectangle>();
            squareToDisplay = new List<Square>();
            DisplayMaze();
        }

        /// <summary>
        /// Méthode affichant la grille du labyrinthe
        /// </summary>
        private void DisplayMaze()
        {
            grid.Children.Clear();
            mazeRectangle.Clear();
            double squareSize = Settings.GetInstance().SquareSize;
            foreach (Square square in maze.GetAllSquares())
            {
                Rectangle rectangle = new Rectangle
                {
                    Height = squareSize,
                    Width = squareSize,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(10 + square.Coordinates.X * squareSize, 10 + square.Coordinates.Y * squareSize, 0, 0),
                    Fill = GetSquareFill(square),
                };
                mazeRectangle.Add(square, rectangle);
                grid.Children.Add(rectangle);
            }
        }

        /// <summary>
        /// Méthode permettant d'actualiser l'affichage des cases
        /// </summary>
        public void UpdateMaze()
        {
            foreach (Square square in maze.GetAllSquares())
            {
                GetRectangle(square).Fill = GetSquareFill(square);
            }
        }

        /// <summary>
        /// Méthode renvoyant un Rectangle selon sa case
        /// </summary>
        /// <param name="square">La case du rectangle souhaité</param>
        /// <returns>Le rectangle selon la case en paramètre</returns>
        public Rectangle GetRectangle(Square square)
        {
            mazeRectangle.TryGetValue(square, out Rectangle value);
            return value;
        }

        /// <summary>
        /// Méthode permettant la résolution du labyrinthe
        /// </summary>
        public void ResolveMaze()
        {
            Dijkstra dijkstra = new Dijkstra(maze);
            dijkstra.CalculDistanceMaze(maze.Start);
            PathDisplayer pathDisplayer = new PathDisplayer(dijkstra.GetPath(maze.End), grid, this);
            pathDisplayer.StartThread();
            threads.Add(pathDisplayer);
        }

        /// <summary>
        /// Méthode permettant de réinitialiser le labyrinthe
        /// </summary>
        /// <param name="maze">Le nouveau labyrinthe</param>
        public void ResetMaze(Maze maze)
        {
            this.maze = maze;
            squareToDisplay.Clear();
            foreach(IThreadDispatcher thread in threads)
            {
                thread.StopThread();
            }
            threads.Clear();
            DisplayMaze();
        }

        /// <summary>
        /// Méthode permettant de colorier un rectanlge selon certain son type est son numéro
        /// </summary>
        /// <param name="square">Case du rectangle</param>
        /// <returns>La couleur du rectangle</returns>
        public Brush GetSquareFill(Square square)
        {
            Brush brush = null;

            switch (square.Type)
            {
                case SquareType.BORDURE:
                case SquareType.WALL: brush = Brushes.Black; break;
                case SquareType.PATH:
                    switch (square.MazeNumber % 11)
                    {
                        case 0: brush = Brushes.LightCoral; break;
                        case 1: brush = Brushes.Yellow; break;
                        case 2: brush = Brushes.DimGray; break;
                        case 3: brush = Brushes.Blue; break;
                        case 4: brush = Brushes.Purple; break;
                        case 5: brush = Brushes.Green; break;
                        case 6: brush = Brushes.Chartreuse; break;
                        case 7: brush = Brushes.Chocolate; break;
                        case 8: brush = Brushes.DarkMagenta; break;
                        case 9: brush = Brushes.Cyan; break;
                        case 10: brush = Brushes.Crimson; break;
                    }
                    break;
                default: throw new Exception("not implemented type Square");
            }

            return brush;
        }

        /// <summary>
        /// Méthode permettant de créer le labyrinthe.
        /// 2 algorithmes ont été implémentés dans le programme.
        /// </summary>
        /// <param name="mazeBuildingAlgorithmType">L'algorithme qui créera le labyrinthe</param>
        public void CreateRandomMaze(MazeBuildingAlgorithmType mazeBuildingAlgorithmType)
        {
            MazeBuildingAlgorithm algorithm;
            Settings settings = Settings.GetInstance();

            switch (mazeBuildingAlgorithmType)
            {
                case MazeBuildingAlgorithmType.RandomMergePaths: algorithm = new RandomMergePaths(this, maze.Walls, maze.Paths); break;
                case MazeBuildingAlgorithmType.ExhaustiveExploration: algorithm = new ExplorationExhaustive(this, maze.Walls, maze.Paths); break;
                case MazeBuildingAlgorithmType.AldousBroder: algorithm = new Aldous_Broder(this, maze.Walls, maze.Paths); break;
                case MazeBuildingAlgorithmType.Wilson: algorithm = new Wilson(this, maze.Walls, maze.Paths); break;
                default: throw new Exception("Unimplemented maze solving algorithm");
            }

            algorithm.CreateMaze();
            if (settings.IsComplexMaze)
            {
                maze.ComplexifyMaze();
            }

            if(settings.InstantGeneration)
            {
                DisplayMaze();
            }
            else
            {
                ModifySquareDispatcher modifySquareDispatcher = new ModifySquareDispatcher(this, mazeRectangle);
                threads.Add(modifySquareDispatcher);
                modifySquareDispatcher.StartThread();
            }            
        }

        /// <summary>
        /// Méthode renvoyant tous les rectangles 
        /// </summary>
        /// <returns>Tous les rectangles</returns>
        public Dictionary<Square, Rectangle>.ValueCollection GetAllRectangle()
        {
            return mazeRectangle.Values;
        }

        /// <summary>
        /// Accesseur du labyrinthe
        /// </summary>
        public Maze Maze => maze;

        /// <summary>
        /// Accesseur de la liste d'affichage du thread
        /// </summary>
        public List<Square> SquareToDisplay => squareToDisplay;

        /// <summary>
        /// Méthode permettant l'ajout dans la liste d'affichage du thread
        /// </summary>
        /// <param name="s">La cellule à ajouter</param>
        public void AddSquareToDisplay(Square s) => squareToDisplay.Add(s);
    }
}

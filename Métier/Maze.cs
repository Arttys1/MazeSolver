using MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeSolver.Métier
{
    /// <summary>
    /// Classe gérant le labyrinthe.
    /// </summary>
    public class Maze
    {
        private readonly Grid grid;                                         //La grille d'affichage du labyrinthe
        private readonly Dictionary<Square, Rectangle> mazeRectangle;       //Les rectangles associés à leurs cases
        private readonly Dictionary<Coordinates, Square> mazeSquare;        //Les cases du labyrinthe associé à leurs coordonnées
        private readonly List<Square> walls;                                //La listes de tous les murs intérieurs du labyrinthe
        private readonly List<Square> paths;                                //La listes de tous les cases parcourables du labyrinthe
        private Square start;                                               //La case de départ du labyrinthe
        private Square end;                                                 //La case d'arrivé du labyrinthe

        private readonly int MAZESIZE = (int)Settings.GetInstance().MazeSize;    //Constante représentant la taille d'un coté du labyrinthe
                                                                                //Le nombre de case total est donc MAZESIZE²

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Maze(Grid grid)
        {
            this.grid = grid;
            mazeSquare = new Dictionary<Coordinates, Square>();
            mazeRectangle = new Dictionary<Square, Rectangle>();
            walls = new List<Square>();
            paths = new List<Square>();
            GridGenerator();
            DisplayMaze();
        }

        /// <summary>
        /// Méthode permettant de generer la grille de départ du labyrinthe
        /// </summary>
        private void GridGenerator()
        {
            int nb = 0;
            for (int i = 0; i < MAZESIZE; i++) 
            {
                for (int j = 0; j < MAZESIZE; j++)
                {
                    Coordinates coordinates = new Coordinates(i, j);
                    Square square = new Square(coordinates);


                    if ((i == 0) || (i == MAZESIZE - 1) || (j == 0) || (j == MAZESIZE - 1))
                    {
                        square.Type = SquareType.BORDURE;
                    }
                    else if (i % 2 == 1 && j % 2 == 1)
                    {
                        square.Type = SquareType.PATH;
                        square.MazeNumber = nb++;
                        paths.Add(square);
                    }
                    else
                    {
                        if (square.Coordinates.X % 2 != 0 || square.Coordinates.Y % 2 != 0)
                        {
                            walls.Add(square);
                        }
                        else
                        {
                            square.Type = SquareType.BORDURE;
                        }
                    }

                    mazeSquare.Add(coordinates, square);
                }
            }

            foreach(Coordinates coordinates in mazeSquare.Keys) //Permet d'ajouter les voisins aux cases
            {
                for (int d = 0; d < 4; d++)
                {
                    Coordinates cooVoisin = coordinates.GetNeighbor((Direction)d);
                    Square square = GetSquare(cooVoisin);
                    if(square != null)
                    {
                        GetSquare(coordinates).AddVoisin(square);
                    }
                }
            }


            start = GetSquare(new Coordinates(0, 1));
            end = GetSquare(new Coordinates(MAZESIZE - 1, MAZESIZE - 2));

            start.Type = SquareType.PATH;
            start.MazeNumber = GetSquare(new Coordinates(1, 1)).MazeNumber;

            end.Type = SquareType.PATH;
            end.MazeNumber = GetSquare(new Coordinates(MAZESIZE - 2, MAZESIZE - 2)).MazeNumber;
        }

        /// <summary>
        /// Méthode affichant la grille du labyrinthe
        /// </summary>
        private void DisplayMaze()
        {
            grid.Children.Clear();
            mazeRectangle.Clear();
            double squareSize = Settings.GetInstance().SquareSize;
            foreach (Square square in GetAllSquares())
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
            foreach (Square square in GetAllSquares())
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
        /// Méthode permettant de colorier un rectanlge selon certain son type est son numéro
        /// </summary>
        /// <param name="square">Case du rectangle</param>
        /// <returns>La couleur du rectangle</returns>
        private Brush GetSquareFill(Square square)
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

            switch (mazeBuildingAlgorithmType)
            {
                case MazeBuildingAlgorithmType.RandomMergePaths: algorithm = new RandomMergePaths(this, walls, paths); break;
                case MazeBuildingAlgorithmType.ExhaustiveExploration: algorithm = new ExplorationExhaustive(this, walls, paths); break;
                case MazeBuildingAlgorithmType.AldousBroder: algorithm = new Aldous_Broder(this, walls, paths); break;
                default: throw new Exception("Unimplemented maze solving algorithm");
            }

            algorithm.CreateMaze();
            if(Settings.GetInstance().IsComplexMaze)
            {
                ComplexifyMaze();
            }
        }

        /// <summary>
        /// Méthode permettant de retourner toutes les cases de la grille
        /// </summary>
        /// <returns>Toutes les cases de la grille</returns>
        public Dictionary<Coordinates, Square>.ValueCollection GetAllSquares() => mazeSquare.Values;

        /// <summary>
        /// Méthode renvoyant une case selon sa coordonnée.
        /// </summary>
        /// <param name="coordinates">La coordonnée de la case souhaitée</param>
        /// <returns>La case associé à la coordonnée en paramètre</returns>
        public Square GetSquare(Coordinates coordinates)
        {
            mazeSquare.TryGetValue(coordinates, out Square value);
            return value;
        }
        public Square Start => start;   //Accesseur renvoyant la case de départ du labyrinthe
        public Square End => end;       //Accesseur renvoyant la case d'arrivé du labyrinthe

        private void ComplexifyMaze()
        {
            for (int i = 0; i < walls.Count / 5; i++)
            {
                Square wall = walls[new Random().Next(walls.Count)];
                wall.Type = SquareType.PATH;
                wall.MazeNumber = paths[0].MazeNumber;
                walls.Remove(wall);
                paths.Add(wall);
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
    }
}


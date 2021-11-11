using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace MazeSolver.Métier
{
    /// <summary>
    /// Classe gérant le labyrinthe.
    /// </summary>
    public class Maze
    {
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
        public Maze()
        {
            mazeSquare = new Dictionary<Coordinates, Square>();
            walls = new List<Square>();
            paths = new List<Square>();
            GridGenerator();;
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
        /// <summary>
        /// Accesseur et mutateur renvoyant la case de départ du labyrinthe
        /// </summary>
        public Square Start { get => start; set => start = value; }

        /// <summary>
        /// //Accesseur et mutateur renvoyant la case d'arrivé du labyrinthe
        /// </summary>
        public Square End { get => end; set => end = value; }       

        /// <summary>
        /// Accesseur renvoyant les murs du labyrinthe
        /// </summary>
        public List<Square> Walls => walls;

        /// <summary>
        /// Accesseur renvoyant les chemins du labyrinthe
        /// </summary>
        public List<Square> Paths => paths;

        /// <summary>
        /// Méthode détruisant 1/5ème des murs du labyrinthe. Cela permet d'avoir plusieurs chemins
        /// </summary>
        public List<Square> ComplexifyMaze()
        {
            List<Square> w = new List<Square>();
            for (int i = 0; i < walls.Count / 5; i++)
            {
                Square wall = walls[new Random().Next(walls.Count)];
                wall.Type = SquareType.PATH;
                wall.MazeNumber = paths[0].MazeNumber;
                walls.Remove(wall);
                paths.Add(wall);
                w.Add(wall);
            }
            return w;
        }

        /// <summary>
        /// Méthode permettant de changer l'arrivé.
        /// </summary>
        /// <param name="r">Le rectangle qui sera la nouvelle arrivé</param>
        /// <returns>La nouvelle et l'ancienne arrivé</returns>
        public (Square, Square) ChangeEnd(Rectangle r)
        {
            return ChangeEntry(r, false);
        }

        /// <summary>
        /// Méthode permettant de changer le départ.
        /// </summary>
        /// <param name="r">Le rectangle qui sera le nouveau départ</param>
        /// <returns>Le nouveau et l'ancien départ</returns>
        public (Square, Square) ChangeStart(Rectangle r)
        {
            return ChangeEntry(r, true);
        }

        /// <summary>
        /// Méthode permettant de changer une entré.
        /// </summary>
        /// <param name="r">Le rectangle qui sera la nouvelle entré</param>
        /// <param name="isStart">Booléen représentant si c'est le départ ou l'arrivé</param>
        /// <returns>La nouvelle et l'ancienne entré</returns>
        private (Square, Square) ChangeEntry(Rectangle r, bool isStart)
        {
            Square newEntry = GetSquareWithRectangle(r);
            Square previousEntry;
            if(isStart)
            {
                previousEntry = Start;
                Start = newEntry;
            }
            else
            {
                previousEntry = End;
                End = newEntry;
            }
            previousEntry.Type = SquareType.BORDURE;

            newEntry.MazeNumber = Paths[0].MazeNumber;
            newEntry.Type = SquareType.PATH;
            return (newEntry, previousEntry);
        }


        /// <summary>
        /// Méthode permettant de récupérer une case avec son rectangle correspondant. La méthode utilise les coordonnées du rectangle pour connaître sa case.
        /// </summary>
        /// <param name="r">Le rectangle de la case souhaité</param>
        /// <returns>La case du rectangle en paramètre</returns>
        private Square GetSquareWithRectangle(Rectangle r)
        {
            double squareSize = Settings.GetInstance().SquareSize;

            int x = (int)((r.Margin.Left - 10) / squareSize);
            int y = (int)((r.Margin.Top - 10) / squareSize);

            return GetSquare(new Coordinates(x, y));
        }
    }
}


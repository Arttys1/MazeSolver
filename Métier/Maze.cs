using System;
using System.Collections.Generic;

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
        /// Accesseur renvoyant la case de départ du labyrinthe
        /// </summary>
        public Square Start => start;

        /// <summary>
        /// //Accesseur renvoyant la case d'arrivé du labyrinthe
        /// </summary>
        public Square End => end;       

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
        public void ComplexifyMaze()
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
    }
}


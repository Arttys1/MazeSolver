using MazeSolver.Ihm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver.Métier.Algorithme.PathSearchAlgorithm
{
    /// <summary>
    /// Classe représentant le parcours en largeur permettant de trouver un chemin dans un graphe non pondéré.
    /// Lien vers le wikipédia détailant l'algorithme :
    /// https://fr.wikipedia.org/wiki/Algorithme_de_parcours_en_largeur
    /// </summary>
    class BreadFirst : PathSearchAlgorithm
    {
        private readonly Dictionary<Square, bool> isVisited;

        public BreadFirst(MazeController mazeController) : base(mazeController)
        {
            this.isVisited = new Dictionary<Square, bool>();
            foreach(Square s in Maze.GetAllSquares())
            {
                isVisited.Add(s, false);
                SetPredecesseur(s, null);
            }
        }

        public override void CalculDistanceMaze(Square start)
        {
            Queue<Square> squares = new Queue<Square>();
            Square square = start;

            squares.Enqueue(square);
            isVisited[square] = true;
            while(squares.Count != 0 && square != Maze.End)
            {
                square = squares.Dequeue();
                MazeController.AddPathSearchSquares(square);
                foreach(Square voisin in square.Voisins)
                {
                    if(voisin.Type == SquareType.PATH && !isVisited[voisin])
                    {
                        squares.Enqueue(voisin);
                        isVisited[voisin] = true;
                        SetPredecesseur(voisin, square);
                    }
                }
            }

        }
    }
}

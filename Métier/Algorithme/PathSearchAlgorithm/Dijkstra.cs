using MazeSolver.Ihm;
using System.Collections.Generic;

namespace MazeSolver.Métier.Algorithme.PathSearchAlgorithm
{
    /// <summary>
    /// Classe représentant l'algorithme de Dijkstra permettant de trouver le plus court chemin.
    /// Lien vers le wikipédia détaillant l'algorithme :
    /// https://fr.wikipedia.org/wiki/Algorithme_de_Dijkstra
    /// </summary>
    public class Dijkstra : PathSearchAlgorithm
    {
        private readonly Dictionary<Square, int> distances;         //Distance de chaque case
        private readonly Dictionary<Square, bool> estVisite;        //est-ce que les cases sont visités
        private const int infini = int.MaxValue - 10000;            //Constante représentant l'infini

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="maze">le labyrinthe</param>
        public Dijkstra(MazeController mazeController) : base(mazeController)
        {            
            distances = new Dictionary<Square, int>();
            estVisite = new Dictionary<Square, bool>();
        }

        /// <summary>
        /// Méthode Initialisant l'algorithme
        /// </summary>
        /// <param name="start">la case de départ</param>
        private void Initialisation(Square start)
        {
            distances.Clear();
            estVisite.Clear();
            Predecesseur.Clear();
            foreach (Square square in Maze.GetAllSquares())
            {
                distances.Add(square, infini);
                estVisite.Add(square, false);
                Predecesseur.Add(square, null);
            }
            SetDistance(start, 0);
        }

        /// <summary>
        /// Méthode renvoyant la case la plus proche
        /// </summary>
        /// <returns>la plus proche</returns>
        private Square GetNearestSquare()
        {
            Square s = null;
            int distanceMin = infini;

            foreach (Square square in Maze.GetAllSquares())
            {
                if (!estVisite[square] && GetDistance(square) < distanceMin)
                {
                    distanceMin = GetDistance(square);
                    s = square;
                }
            }

            return s;
        }

        /// <summary>
        /// Méthode permettant d'actualiser les distances des cases
        /// </summary>
        /// <param name="a">Premiere case</param>
        /// <param name="b">Deuxieme case</param>
        private void Relachement(Square a, Square b)
        {
            if (GetDistance(b) > GetDistance(a) + CoutMouvementVers(a))
            {
                SetDistance(b, GetDistance(a) + CoutMouvementVers(a));
                SetPredecesseur(b, a);

                MazeController.AddPathSearchSquares(b);
                MazeController.AddPathSearchSquares(a);
            }
        }

        /// <summary>
        /// Méthode renvoyant le cout pour aller vers une case.
        /// </summary>
        /// <param name="a">la case</param>
        /// <returns>Si elle est parcourable, renvoie 1 sinon renvoie infini</returns>
        private int CoutMouvementVers(Square a)
        {
            int cout = 1;

            if (a.Type != SquareType.PATH)
            {
                cout = infini;
            }
            return cout;
        }

        /// <summary>
        /// Méthode calculant la distance de chaque case du départ
        /// </summary>
        /// <param name="start">le départ</param>
        public override void CalculDistanceMaze(Square start)
        {
            Initialisation(start);
            Square nearestSquare = GetNearestSquare();

            while (nearestSquare != null && nearestSquare != Maze.End)
            {
                estVisite[nearestSquare] = true;

                foreach (Square voisin in nearestSquare.Voisins)
                {
                    Relachement(nearestSquare, voisin);
                }

                nearestSquare = GetNearestSquare();
            }

        }

        /// <summary>
        /// Méthode permettant d'ajuster une distance dans le conteneurs
        /// </summary>
        /// <param name="square">la case</param>
        /// <param name="distance">la distance</param>
        private void SetDistance(Square square, int distance)
        {
            distances[square] = distance;
        }

        /// <summary>
        /// Méthode permettant de renvoyer la distance
        /// </summary>
        /// <param name="square">la case</param>
        /// <returns>la distance</returns>
        private int GetDistance(Square square)
        {
            distances.TryGetValue(square, out int value);
            return value;
        }        
    }
}

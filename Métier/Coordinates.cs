using System;

namespace MazeSolver.Métier
{
    /// <summary>
    /// Classe représentant les coordonnées d'une case.
    /// La coordonnée 0, 0 est en haut à gauche.
    /// </summary>
    public class Coordinates
    {
        private readonly int x;             //Axe X
        private readonly int y;             //Axe Y

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="x">axe X</param>
        /// <param name="y">axe Y</param>
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Méthode renvoyant une coordonnée adjacente selon une direction
        /// </summary>
        /// <param name="dir">Direction de la coordonnée souhaité</param>
        /// <returns>La coordonnée adjacente selon la direction en paramètre</returns>
        public Coordinates GetNeighbor(Direction dir)
        {
            Coordinates coordinates = null;

            switch (dir)
            {
                case Direction.HAUT: coordinates = new Coordinates(x - 1, y); break;
                case Direction.DROITE: coordinates = new Coordinates(x, y + 1); break;
                case Direction.BAS: coordinates = new Coordinates(x + 1, y); break;
                case Direction.GAUCHE: coordinates = new Coordinates(x, y - 1); break;
            }

            return coordinates;
        }

        public int X => x;  //Accesseur du X
        public int Y => y;  //Accesseur du Y

        //Surchage de l'opérateur == concernant les Coordonnées.
        public override bool Equals(object obj)
        {
            return obj is Coordinates coordinates &&
                   x == coordinates.x &&
                   y == coordinates.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}

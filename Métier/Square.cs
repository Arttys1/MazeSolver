using System.Collections.Generic;

namespace MazeSolver.Métier
{
    /// <summary>
    /// Classe réprésentant une case du labyrinthe.
    /// </summary>
    public class Square
    {
        private readonly Coordinates coordinates;       //Coordonnées de la case
        private int mazeNumber;                         //Identifiant de la case
        private SquareType type;                        //Type de la Case
        private readonly List<Square> voisins;          //Voisins de la case

        /// <summary>
        /// Constructeur initialisant les coordonnées.
        /// </summary>
        /// <param name="coordinates">Coordonnées de la case</param>
        public Square(Coordinates coordinates)
        {
            voisins = new List<Square>();
            this.coordinates = coordinates;
            this.MazeNumber = -1;
            type = SquareType.WALL;            
        }

        public List<Square> Voisins => new List<Square>(voisins);       //Accesseur de la liste des voisins. Renvoie une copie
        public void AddVoisin(Square voisin) => voisins.Add(voisin);    //Méthode permettant d'ajouter un voisin
        public int MazeNumber { get => mazeNumber; set => mazeNumber = value; } //Accesseur et mutateur de l'identifiant de la Case
        public Coordinates Coordinates => coordinates;                  //Accesseur des coordonnées de la case
        public SquareType Type { get => type; set => type = value; }    //Accesseur et mutateur du type de la case
    }
}

using System;
using System.Collections.Generic;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe hérité de MazeBuildingAlgorithm.
    /// Elle permet de créer un labyrinthe selon l'agorithme de l'exploration exhaustive.
    /// Lien vers un wikipédia détaillant l'algorithme:
    /// https://fr.wikipedia.org/wiki/Modélisation_mathématique_de_labyrinthe
    /// </summary>
    public class ExplorationExhaustive : MazeBuildingAlgorithm
    {
        private readonly Dictionary<Square, bool> estVisite;                //représente si les cases ont été visités ou non
        private readonly Dictionary<Square, Square> precedent;              //représente la case précendente.

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="maze">le labyrinthe</param>
        /// <param name="walls">les murs intérieurs du labyrinthe</param>
        /// <param name="paths">les cases parcourables du labyrinthe</param>
        public ExplorationExhaustive(Maze maze, List<Square> walls, List<Square> paths) : base(maze, walls, paths)
        {
            estVisite = new Dictionary<Square, bool>();
            precedent = new Dictionary<Square, Square>();
        }

        /// <summary>
        /// Méthode initialisant toutes les cases comme non visité
        /// </summary>
        private void Initialisation()
        {
            estVisite.Clear();
            foreach (Square square in Paths)
            {
                if (square != Maze.Start || square != Maze.End)
                {
                    estVisite.Add(square, false);
                }
            }
        }

        public override void CreateMaze()
        {
            Initialisation();
            Square square = Paths[new Random().Next(Paths.Count)];
            MazeCreationRecursive(square);

            Maze.Start.MazeNumber = Paths[0].MazeNumber;
            Maze.End.MazeNumber = Maze.Start.MazeNumber;
        }

        /// <summary>
        /// Méthode qui est le point d'entrer de la méthode récursive du même nom
        /// </summary>
        /// <param name="start">Case départ</param>
        private void MazeCreationRecursive(Square start)
        {
            List<Square> CasesVoisines = GetAdjacentsSquares(start);
            estVisite[start] = true;
            Square nextSquare = CasesVoisines[new Random().Next(CasesVoisines.Count)];
            CombineTowSquare(start, nextSquare);
            precedent[nextSquare] = start;
            MazeCreationRecursive(start, nextSquare);
        }

        /// <summary>
        /// Méthode récursive permettant de créer un labyrinthe.
        /// L'algorithme est le suivant : On choisit une case au hasard.
        /// Puis on regarde quelles sont les cellules voisines possibles et non visitées.
        /// S'il y a au moins une possibilité, on en choisit une au hasard, on ouvre le mur et on recommence avec la nouvelle cellule.
        /// S'il n'y en pas, on revient à la case précédente et on recommence.
        /// Lorsque l'on est revenu à la case de départ et qu'il n'y a plus de possibilités, le labyrinthe est terminé.
        /// </summary>
        /// <param name="start">case départ</param>
        /// <param name="actualSquare">case actuel de la récursivité</param>
        private void MazeCreationRecursive(Square start, Square actualSquare)
        {
            if (start != actualSquare ) 
            {                
                List<Square> CasesVoisines = GetAdjacentsSquares(actualSquare);
                estVisite[actualSquare] = true;

                if (CasesVoisines.Count != 0)
                {
                    Square nextSquare = CasesVoisines[new Random().Next(CasesVoisines.Count)];
                    CombineTowSquare(nextSquare, actualSquare);
                    precedent[nextSquare] = actualSquare;
                    MazeCreationRecursive(start, nextSquare);
                }
                else
                {
                    Square pred = precedent[actualSquare];
                    MazeCreationRecursive(start, pred);
                }
            }
        }

        /// <summary>
        /// Méthode fusionnant deux case ainsi que le murs qui les séparent.
        /// </summary>
        /// <param name="a">Case à fusionner</param>
        /// <param name="b">Case à fusionner</param>
        private void CombineTowSquare(Square a, Square b)
        {
            Square wall = null;
            foreach(Square VoisinA in a.Voisins)
            {
                foreach(Square VoisinB in b.Voisins)
                {
                    if(VoisinA == VoisinB)
                    {
                        wall = VoisinB;                        
                    }                    
                }
            }

            if(wall != null)
            {
                wall.Type = SquareType.PATH;
                wall.MazeNumber = b.MazeNumber;
                a.MazeNumber = b.MazeNumber;
                Paths.Add(wall);
                Walls.Remove(wall);
            }
        }

        /// <summary>
        /// Méthode renvoyant les cases parcourables autour d'une case donnée.
        /// Elle renvoie seulement les cases parcourables et non visités
        /// </summary>
        /// <param name="square">case centrale</param>
        /// <returns>Les cases parcourables, non visités autour d'une case donnée</returns>
        private List<Square> GetAdjacentsSquares(Square square)
        {
            List<Square> adjacentsSquares = new List<Square>();
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Square secondSquare;
                Square firstSquare = Maze.GetSquare(square.Coordinates.GetNeighbor(direction));
                if (firstSquare != null && firstSquare.Type != SquareType.BORDURE)
                {
                    secondSquare = Maze.GetSquare(firstSquare.Coordinates.GetNeighbor(direction));

                    if (secondSquare != null && !estVisite[secondSquare] && secondSquare.Type != SquareType.BORDURE)
                    {
                        adjacentsSquares.Add(secondSquare);
                    }
                }

            }

            return adjacentsSquares;
        }
    }
}

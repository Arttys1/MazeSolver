using MazeSolver.Ihm;
using System;
using System.Collections.Generic;

namespace MazeSolver.Métier.Algorithme.MazeBuildingAlgorithm
{
    /// <summary>
    /// Classe hérité de VisitingAlgorithm.
    /// Elle permet de créer un labyrinthe selon l'agorithme de l'exploration exhaustive.
    /// Lien vers un wikipédia détaillant l'algorithme:
    /// https://fr.wikipedia.org/wiki/Modélisation_mathématique_de_labyrinthe
    /// </summary>
    public class ExplorationExhaustive : VisitingAlgorithm
    {
        private readonly Dictionary<Square, Square> precedent;              //représente la case précendente.

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="maze">le labyrinthe</param>
        /// <param name="walls">les murs intérieurs du labyrinthe</param>
        /// <param name="paths">les cases parcourables du labyrinthe</param>
        public ExplorationExhaustive(MazeController mazeController, List<Square> walls, List<Square> paths) : base(mazeController, walls, paths)
        {
            precedent = new Dictionary<Square, Square>();
        }
        /// <summary>
        /// Méthode initialisant toutes les cases comme non visité
        /// </summary>
        private new void Initialisation()
        {
            IsVisited.Clear();
            foreach (Square square in Paths)
            {
                if (square != Maze.Start || square != Maze.End)
                {
                    IsVisited.Add(square, false);
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
            AddStartEndToDisplayer();
        }

        /// <summary>
        /// Méthode qui est le point d'entrer de la méthode récursive du même nom
        /// </summary>
        /// <param name="start">Case départ</param>
        private void MazeCreationRecursive(Square start)
        {
            List<Square> CasesVoisines = GetAdjacentsSquares(start);
            IsVisited[start] = true;
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
                IsVisited[actualSquare] = true;

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
        /// Méthode renvoyant les cases parcourables autour d'une case donnée.
        /// Elle renvoie seulement les cases parcourables et non visités
        /// </summary>
        /// <param name="square">case centrale</param>
        /// <returns>Les cases parcourables, non visités autour d'une case donnée</returns>
        private new List<Square> GetAdjacentsSquares(Square square)
        {
            List<Square> adjacentsSquares = new List<Square>();
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Square secondSquare;
                Square firstSquare = Maze.GetSquare(square.Coordinates.GetNeighbor(direction));
                if (firstSquare != null && firstSquare.Type != SquareType.BORDURE)
                {
                    secondSquare = Maze.GetSquare(firstSquare.Coordinates.GetNeighbor(direction));

                    if (secondSquare != null && !IsVisited[secondSquare] && secondSquare.Type != SquareType.BORDURE)
                    {
                        adjacentsSquares.Add(secondSquare);
                    }
                }
            }

            return adjacentsSquares;
        }
    }
}

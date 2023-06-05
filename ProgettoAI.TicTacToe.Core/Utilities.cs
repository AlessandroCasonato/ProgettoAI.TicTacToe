using ProgettoAI.TicTacToe.Core.Models;
using System.Numerics;

namespace ProgettoAI.TicTacToe.Core
{
    public static class Utilities
    {
        public const char MIN_CHAR = 'O';
        public const char MAX_CHAR = 'X';
        public const int LOOKAHEAD = 2;
        public enum Status
        {
            Empty,
            ThreeMax,
            ThreeMin,
            TwoMaxOneMin,
            TwoMaxOneEmpty,
            TwoMinOneMax,
            TwoMinOneEmpty,
            OneMaxTwoEmpty,
            OneMinTwoEmpty,
            OneMaxOneMinOneEmpty,
        };
        public static Dictionary<Status, int> UtilityValue = new Dictionary<Status, int>()
        {
            { Status.ThreeMax, 100 },
            { Status.ThreeMin, -100 },
            { Status.TwoMaxOneEmpty, 10 },
            { Status.TwoMinOneEmpty, -10 },
            { Status.TwoMaxOneMin, 9 },
            { Status.TwoMinOneMax, -9 },
            { Status.OneMaxTwoEmpty, 1 },
            { Status.OneMinTwoEmpty, -1 },
            { Status.OneMaxOneMinOneEmpty, 0 },
            { Status.Empty, 0 }
        };

        /// <summary>
        /// Funzione che calcola e ritorna la mossa ottimale per MAX in base alla funzione di utilità e lookahead di 2.
        /// </summary>
        /// <param name="tilesState">Lo stato attuale della griglia di gioco</param>
        /// <returns>La mossa scelta da MAX.</returns>
        public static Move GetNextMove(TilesState tilesState)
        {
            // Vengono espanse le possibili mosse
            var moves = new List<Move>();
            foreach (var tile in tilesState.EmptyTiles)
            {
                var move = new Move(tilesState, tile.Item1, tile.Item2, MAX_CHAR);
                
                move.NextMoves = new();
                
                foreach (var t in move.TilesStateWithMove.EmptyTiles)
                {
                    var playerMove = new Move(move.TilesStateWithMove, t.Item1, t.Item2, MIN_CHAR);
                    playerMove.NextMoves = new();
                    foreach (var p in playerMove.TilesStateWithMove.EmptyTiles)
                    {
                        playerMove.NextMoves.Add(new Move(playerMove.TilesStateWithMove, p.Item1, p.Item2, MAX_CHAR));
                    }
                    //Rimuovo tutte le mosse possibili di MIN che non siano ottimali per MIN
                    playerMove.NextMoves.OrderByDescending(x => x.Utility).ToList();
                    move.NextMoves.Add(playerMove);
                }

                move.NextMoves.RemoveAll(x => x.Utility > move.NextMoves.Min(x => x.Utility));
                moves.Add(move);
            }
            moves = moves.OrderByDescending(x => x.Utility).ToList();

            Move chosenMove = null, lookaheadOfChosenMove = null;
            //Viene scelta la mossa migliore per MAX tra quelle espanse
            foreach(var move in moves)
            {
                if (move.IsAWinningMove())
                    return move;
                else
                {
                    //In questo caso significa che è l'ultima mossa della partita
                    if (!move.NextMoves.Any())
                        return move;

                    //Se la mossa porta alla vittoria di MIN nel prossimo turno viene restituita per minimizzare le probabilità di sconfitta
                    if (move.NextMoves.FirstOrDefault().IsAWinningMove())
                        return move.NextMoves.First();
                    else
                    {
                        // MAX presuppone che MIN giochi al meglio, scegliendo quindi la mossa con massima utilità per MIN (o minima per MAX)
                        foreach (var m in move.NextMoves.FirstOrDefault().NextMoves)
                        {
                            if (chosenMove is null) 
                            {
                                chosenMove = move;
                                lookaheadOfChosenMove = m;
                            }
                            else if (lookaheadOfChosenMove.Utility < m.Utility)
                            {
                                chosenMove = move;
                                lookaheadOfChosenMove = m;
                            }
                        }
                    }
                }
            }

            return chosenMove ?? moves.First();
        }


        /// <summary>
        /// Controlla se le coordinate della casella di gioco siano su una diagonale e ritorna quale diagonale sia eventualmente
        /// </summary>
        /// <param name="x">Ascissa della casella controllata.</param>
        /// <param name="y">Ordinata della casella controllata.</param>
        /// <returns>0 se la casella appartiene alla diagonale che parte da 0,0 e termina in 2,2, 
        /// 1 se appartiene all'altra diagonale, 
        /// 2 se appartiene ad entrambe,
        /// -1 altrimenti</returns>
        public static int GetDiagonal((uint x, uint y) tile)
        {
            (uint, uint)[] firstDiag = new (uint, uint)[3] { (0, 0), (1, 1), (2, 2) };
            (uint, uint)[] secondDiag = new (uint, uint)[3] { (0, 2), (1, 1), (2, 0) };
            var firstDCheck = firstDiag.Contains(tile);
            var secondDCheck = secondDiag.Contains(tile);
            if (firstDCheck && secondDCheck)
                return 2;
            else if (secondDCheck)
                return 1;
            else if (firstDCheck)
                return 0;
            else
                return -1;
        }

        /// <summary>
        /// Ritorna il simbolo del giocatore avversario a quello passato come parametro
        /// </summary>
        /// <param name="player">Il simbolo del giocatore attuale</param>
        /// <returns>Il simbolo del giocatore avversario</returns>
        public static char GetOppositePlayer(char player)
        {
            var oppositePlayer = player == MAX_CHAR ? MIN_CHAR : MAX_CHAR;
            return oppositePlayer;
        }
    }
}

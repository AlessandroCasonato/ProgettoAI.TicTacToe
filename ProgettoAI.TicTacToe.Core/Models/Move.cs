using System.Numerics;
using System.Web;
using Status = ProgettoAI.TicTacToe.Core.Utilities.Status;

namespace ProgettoAI.TicTacToe.Core.Models
{
    /// <summary>
    /// Mossa intrapresa da uno dei due giocatori, definita dalla posizione sulla griglia e dall'utilità attesa.
    /// </summary>
    public class Move
    {
        public uint MovePositionX { get; set; }
        public uint MovePositionY { get; set; }
        public int Utility { get; set; }
        public char Player { get; set; }
        public TilesState TilesStateWithMove { get; set; }
        public List<Move> NextMoves { get; set; }

        public Move(TilesState tilesStateBeforeMove, uint movePositionX, uint movePositionY, char player)
        {
            //Validazione
            if (movePositionX > 2 || movePositionY > 2)
                throw new ArgumentException("Coordinate non correttamente impostate");

            // Player = player;
            MovePositionX = movePositionX;
            MovePositionY = movePositionY;
            Player = player;
            TilesStateWithMove = new TilesState(tilesStateBeforeMove);
            Utility = CalculateSingleUtility();
        }
        public bool IsAWinningMove()
        {
            if (Player == Utilities.MAX_CHAR)
            {
                if (TilesStateWithMove.Rows[MovePositionX].LineStatus == Status.ThreeMax ||
                    TilesStateWithMove.Columns[MovePositionY].LineStatus == Status.ThreeMax ||
                    TilesStateWithMove.Diagonals[0].LineStatus == Status.ThreeMax ||
                    TilesStateWithMove.Diagonals[1].LineStatus == Status.ThreeMax)
                    return true;
                else
                    return false;
            }
            else
            {
                if (TilesStateWithMove.Rows[MovePositionX].LineStatus == Status.ThreeMin ||
                    TilesStateWithMove.Columns[MovePositionY].LineStatus == Status.ThreeMin ||
                    TilesStateWithMove.Diagonals[0].LineStatus == Status.ThreeMin ||
                    TilesStateWithMove.Diagonals[1].LineStatus == Status.ThreeMin)
                    return true;
                else
                    return false;
            }
        }

        private int CalculateSingleUtility()
        {
            var utility = 0;
            TilesStateWithMove.EmptyTiles.Remove(TilesStateWithMove.EmptyTiles.First(m => m.Item1.Equals(MovePositionX) && m.Item2.Equals(MovePositionY)));
            if (Player == Utilities.MAX_CHAR)
            {
                TilesStateWithMove.ComputerTiles.Add((MovePositionX, MovePositionY));
                TilesStateWithMove.Rows[MovePositionX].AddMaxChar();
                TilesStateWithMove.Columns[MovePositionY].AddMaxChar();
                var diag = Utilities.GetDiagonal((MovePositionX, MovePositionY));
                if (diag == 0 || diag == 2)
                    TilesStateWithMove.Diagonals[0].AddMaxChar();
                if (diag == 1 || diag == 2)
                    TilesStateWithMove.Diagonals[1].AddMaxChar();
            }
            else
            {
                TilesStateWithMove.PlayerTiles.Add((MovePositionX, MovePositionY));
                TilesStateWithMove.Rows[MovePositionX].AddMinChar();
                TilesStateWithMove.Columns[MovePositionY].AddMinChar();
                var diag = Utilities.GetDiagonal((MovePositionX, MovePositionY));
                if (diag == 0 || diag == 2)
                    TilesStateWithMove.Diagonals[0].AddMinChar();
                if (diag == 1 || diag == 2)
                    TilesStateWithMove.Diagonals[1].AddMinChar();
            }
            var allLines = TilesStateWithMove.Rows.Concat(TilesStateWithMove.Columns.Concat(TilesStateWithMove.Diagonals));
            foreach (var line in allLines)
            {
                utility += line.Utility;
            }
            return utility;
        }
    }
}

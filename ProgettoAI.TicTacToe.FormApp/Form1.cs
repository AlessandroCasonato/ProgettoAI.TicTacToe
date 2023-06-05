using ProgettoAI.TicTacToe.Core;
using ProgettoAI.TicTacToe.Core.Models;
using System.Numerics;

namespace ProgettoAI.TicTacToe.FormApp
{
    public partial class Form1 : Form
    {
        private char[,] tiles = new char[3, 3];
        private Button[] buttons; 
        private TilesState tilesState;
        public Form1()
        {
            InitializeComponent();
            buttons = new Button[] { btn00, btn01, btn02, btn10, btn11, btn12, btn20, btn21, btn22 };
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            PlayerMove(0, 0, (Button)sender);
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            PlayerMove(0, 1, (Button)sender);
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            PlayerMove(0, 2, (Button)sender);
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            PlayerMove(1, 0, (Button)sender);
        }

        private void btn11_Click(object sender, EventArgs e)
        {
            PlayerMove(1, 1, (Button)sender);
        }

        private void btn12_Click(object sender, EventArgs e)
        {
            PlayerMove(1, 2, (Button)sender);
        }

        private void btn20_Click(object sender, EventArgs e)
        {
            PlayerMove(2, 0, (Button)sender);
        }

        private void btn21_Click(object sender, EventArgs e)
        {
            PlayerMove(2, 1, (Button)sender);
        }

        private void btn22_Click(object sender, EventArgs e)
        {
            PlayerMove(2, 2, (Button)sender);
        }

        private void newGameMaxBtn_Click(object sender, EventArgs e)
        {
            ClearBoard();
            tilesState = new();
            ComputerMove();
        }

        private void newGameMinBtn_Click(object sender, EventArgs e)
        {
            ClearBoard();
            tilesState = new();
        }

        private void PlayerMove(uint x, uint y, Button sender)
        {
            if (CheckIfEmpty((x, y)))
            {
                var count = tilesState.EmptyTiles.Count;
                if (!ExecuteMove(x, y, Utilities.MIN_CHAR) && count > 1)
                    ComputerMove();
               
            }
        }

        private void ComputerMove()
        {
            var computerMove = Utilities.GetNextMove(tilesState);
            ExecuteMove(computerMove.MovePositionX, computerMove.MovePositionY, Utilities.MAX_CHAR);
        }

        /// <summary>
        /// Aggiorna la griglia di gioco in base alla nuova mossa
        /// </summary>
        /// <param name="x">Coordinata x della nuova mossa.</param>
        /// <param name="y">Coordinata y della nuova mossa.</param>
        /// <param name="player">Simbolo del giocatore che effettua la nuova mossa</param>
        /// <param name="sender">Bottone da aggiornare nel Form.</param>
        /// <returns>True se la partita è terminata, false altrimenti.</returns>
        private bool ExecuteMove(uint x, uint y, char player)
        {
            tiles[x, y] = player;
            var btn = GetButtonFromCoords((x, y));
            btn.Text = player.ToString();
            // sender.Enabled = false;
            if (player.Equals(Utilities.MAX_CHAR))
            {
                btn.ForeColor = Color.Red;
                tilesState.ComputerTiles.Add((x, y));
                tilesState.Rows[x].AddMaxChar();
                tilesState.Columns[y].AddMaxChar();
                var diag = Utilities.GetDiagonal((x, y));
                if (diag == 0 || diag == 2 )
                    tilesState.Diagonals[0].AddMaxChar();
                if (diag == 1 || diag == 2)
                    tilesState.Diagonals[1].AddMaxChar();
                //Controllo se è una mossa vincente
                if (tilesState.Rows[x].LineStatus == Utilities.Status.ThreeMax ||
                    tilesState.Columns[y].LineStatus == Utilities.Status.ThreeMax ||
                    tilesState.Diagonals[0].LineStatus == Utilities.Status.ThreeMax ||
                    tilesState.Diagonals[1].LineStatus == Utilities.Status.ThreeMax)
                {
                    MessageBox.Show("Vittoria del giocatore MAX", "Hai perso");
                    return true;
                }
            } 
            else
            {
                btn.ForeColor = Color.Blue;
                tilesState.PlayerTiles.Add((x, y));
                tilesState.Rows[x].AddMinChar();
                tilesState.Columns[y].AddMinChar();
                var diag = Utilities.GetDiagonal((x, y));
                if (diag == 0 || diag == 2)
                    tilesState.Diagonals[0].AddMinChar();
                if (diag == 1 || diag == 2)
                    tilesState.Diagonals[1].AddMinChar();
                if (tilesState.Rows[x].LineStatus == Utilities.Status.ThreeMin ||
                    tilesState.Columns[y].LineStatus == Utilities.Status.ThreeMin ||
                    tilesState.Diagonals[0].LineStatus == Utilities.Status.ThreeMin ||
                    tilesState.Diagonals[1].LineStatus == Utilities.Status.ThreeMin)
                {
                    MessageBox.Show("Vittoria del giocatore MIN", "Hai vinto!");
                    return true;
                }
                        
            }
                
            tilesState.EmptyTiles.Remove(tilesState.EmptyTiles.First(g => g.Item1.Equals(x) && g.Item2.Equals(y)));
            return false;
        }

        private Button GetButtonFromCoords((uint movePositionX, uint movePositionY) coords)
        {
            switch (coords)
            {
                case (0, 0):
                    return btn00;
                case (0, 1):
                    return btn01;
                case (0, 2):
                    return btn02;
                case (1, 0):
                    return btn10;
                case (1, 1):
                    return btn11;
                case (1, 2):
                    return btn12;
                case (2, 0):
                    return btn20;
                case (2, 1):
                    return btn21;
                case (2, 2):
                    return btn22;
                default:
                    throw new ArgumentOutOfRangeException("Coordinate non valide");
            }
        }

        private void ClearBoard()
        {
            foreach(var btn in buttons)
            {
                btn.Enabled = true;
                btn.Text = string.Empty;
            }
        }

        private bool CheckIfEmpty((uint x, uint y) coords)
        {
            return String.IsNullOrWhiteSpace(GetButtonFromCoords(coords).Text);
        }

    }
}
namespace ProgettoAI.TicTacToe.Core.Models
{
    public class TilesState
    {
        public List<(uint, uint)> PlayerTiles { get; set; } = new();
        public List<(uint, uint)> ComputerTiles { get; set; } = new();
        public List<(uint, uint)> EmptyTiles { get; set; } = new()
        {
            (0, 0), (1, 0), (2, 0),
            (0, 1), (1, 1), (2, 1),
            (0, 2), (1, 2), (2, 2),
        };

        public Line[] Rows = new Line[3] { new Line { StartIndex = 0 }, new Line { StartIndex = 1 }, new Line { StartIndex = 2 } };
        public Line[] Columns = new Line[3] { new Line { StartIndex = 0 }, new Line { StartIndex = 1 }, new Line { StartIndex = 2 } };
        public Line[] Diagonals = new Line[2] { new Line { StartIndex = 0 }, new Line { StartIndex = 2 } };

        public TilesState()
        {

        }

        public TilesState(TilesState tilesState)
        {
            PlayerTiles = new();
            ComputerTiles = new();
            EmptyTiles = new();
            Rows = new Line[3];
            Columns = new Line[3];
            Diagonals = new Line[2];
            foreach (var playerTile in tilesState.PlayerTiles)
            {
                PlayerTiles.Add(playerTile);
            }
            foreach (var computerTile in tilesState.ComputerTiles)
            {
                ComputerTiles.Add(computerTile);
            }
            foreach (var emptyTile in tilesState.EmptyTiles)
            {
                EmptyTiles.Add(emptyTile);
            }
            for (int i = 0; i < 3; i++)
            {
                Rows[i] = new(tilesState.Rows[i]);
                Columns[i] = new(tilesState.Columns[i]);
                if (i < 2)
                    Diagonals[i] = new(tilesState.Diagonals[i]);
            }
        }

    }
}

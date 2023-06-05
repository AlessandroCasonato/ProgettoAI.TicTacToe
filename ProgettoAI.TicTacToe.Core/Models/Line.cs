using Status = ProgettoAI.TicTacToe.Core.Utilities.Status;

namespace ProgettoAI.TicTacToe.Core.Models
{
    public class Line
    {
        public uint EmptyTiles { get; set; } = 3;
        public uint MaxCharTiles { get; set; } = 0;
        public uint MinCharTiles { get; set; } = 0;
        public Status LineStatus { get; set; } = Status.Empty;
        public int Utility { get; set; } = 0;
        public uint StartIndex { get; set; }
        
        public void AddMaxChar()
        {
            MaxCharTiles++;
            EmptyTiles--;
            UpdateLineStatus();
        }
        public void AddMinChar()
        {
            MinCharTiles++;
            EmptyTiles--;
            UpdateLineStatus();
        }

        public void UpdateLineStatus()
        {
            if (EmptyTiles + MaxCharTiles + MinCharTiles > 3)
                throw new ArgumentOutOfRangeException("Il numero di caselle nella linea è 3, e non è possibile sforare tale valore");
            if (EmptyTiles == 3)
                LineStatus = Status.Empty;
            else if (MaxCharTiles == 3)
                LineStatus = Status.ThreeMax;
            else if (MinCharTiles == 3)
                LineStatus = Status.ThreeMin;
            else if (MaxCharTiles == 2)
            {
                if (MinCharTiles == 1)
                    LineStatus = Status.TwoMaxOneMin;
                else if (EmptyTiles == 1)
                    LineStatus = Status.TwoMaxOneEmpty;
            }
            else if (MinCharTiles == 2)
            {
                if (MaxCharTiles == 1)
                    LineStatus = Status.TwoMinOneMax;
                else if (EmptyTiles == 1)
                    LineStatus = Status.TwoMinOneEmpty;
            }
            else if (EmptyTiles == 2)
            {
                if (MaxCharTiles == 1)
                    LineStatus = Status.OneMaxTwoEmpty;
                else if (MinCharTiles == 1)
                    LineStatus = Status.OneMinTwoEmpty;
            }
            else 
                LineStatus = Status.OneMaxOneMinOneEmpty;

            Utility = Utilities.UtilityValue[LineStatus];
        }

        public Line()
        {

        }

        public Line(Line line)
        {
            EmptyTiles= line.EmptyTiles;
            MinCharTiles= line.MinCharTiles;
            MaxCharTiles= line.MaxCharTiles;
            LineStatus= line.LineStatus;
            StartIndex= line.StartIndex;
        }
    }
}

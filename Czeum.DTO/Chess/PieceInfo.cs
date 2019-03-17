using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DTO.Chess
{
    public class PieceInfo
    {
        public Color Color { get; set; }
        public PieceType Type { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}

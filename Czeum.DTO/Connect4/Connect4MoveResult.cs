using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;

namespace Czeum.DTO.Connect4
{
    public class Connect4MoveResult : MoveResult
    {
        public Item[,] Board { get; set; }
    }
}

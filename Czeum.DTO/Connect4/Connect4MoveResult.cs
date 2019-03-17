using System;
using System.Collections.Generic;
using System.Text;
using Connect4.Abstractions;

namespace Czeum.DTO.Connect4
{
    public class Connect4MoveResult : MoveResult
    {
        public Item[,] Board { get; set; }
    }
}

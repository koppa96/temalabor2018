using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    public abstract class MoveResult
    {
        public Status Status { get; set; }
    }
}

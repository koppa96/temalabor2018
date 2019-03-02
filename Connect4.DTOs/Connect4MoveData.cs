using Connect4.Abstractions;
using Connect4.Connect4Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.DTOs
{
    public class Connect4MoveData : MoveData
    {
        public int Column { get; set; }
        public Item Item { get; set; }

        public override IGameService FindGameService(IEnumerable<IGameService> services)
        {
            return FindGameServiceTyped<Connect4Service>(services);
        }
    }
}

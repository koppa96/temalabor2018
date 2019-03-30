using Czeum.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Connect4
{
    public class Connect4MoveData : MoveData
    {
        public int Column { get; set; }
    }
}

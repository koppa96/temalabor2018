using System;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DTO.Connect4;

namespace Czeum.DAL.Entities
{
    public class SerializedConnect4Board : SerializedBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

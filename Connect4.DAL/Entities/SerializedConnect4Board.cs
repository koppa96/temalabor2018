using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.DAL.Entities
{
    public class SerializedConnect4Board : SerializedBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

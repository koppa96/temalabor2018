using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Connect4.ChessLogic
{
    public class Direction
    {
        public int RowDirection { get; private set; }
        public int ColumnDirection { get; private set; }
        public Func<int, int, Field, bool> Predicate { get; private set; }

        private Direction()
        {
        }

        public static Direction Left => new Direction()
        {
            RowDirection = 0,
            ColumnDirection = -1,
            Predicate = (i, j, f) => j > f.Column
        };

        public static Direction Right => new Direction()
        {
            RowDirection = 0,
            ColumnDirection = 1,
            Predicate = (i, j, f) => j < f.Column
        };

        public static Direction Above => new Direction()
        {
            RowDirection = -1,
            ColumnDirection = 0,
            Predicate = (i, j, f) => i > f.Row
        };

        public static Direction Below => new Direction()
        {
            RowDirection = 1,
            ColumnDirection = 0,
            Predicate = (i, j, f) => i < f.Row
        };

        public static Direction AboveLeft => new Direction()
        {
            RowDirection = -1,
            ColumnDirection = -1,
            Predicate = (i, j, f) => Above.Predicate(i, j, f) && Left.Predicate(i, j, f)
        };

        public static Direction AboveRight => new Direction()
        {
            RowDirection = -1,
            ColumnDirection = 1,
            Predicate = (i, j, f) => Above.Predicate(i, j, f) && Right.Predicate(i, j, f)
        };

        public static Direction BelowRight => new Direction()
        {
            RowDirection = 1,
            ColumnDirection = 1,
            Predicate = (i, j, f) => Below.Predicate(i, j, f) && Right.Predicate(i, j, f)
        };

        public static Direction BelowLeft => new Direction()
        {
            RowDirection = 1,
            ColumnDirection = -1,
            Predicate = (i, j, f) => Below.Predicate(i, j, f) && Left.Predicate(i, j, f)
        };

        public static Direction GuessDirection(Field from, Field to)
        {
            if (from.Row == to.Row && from.Column == to.Column)
            {
                throw new ArgumentException("The given fields have the same position.");
            }

            if (to.Row > from.Row)
            {
                if (to.Column > from.Column)
                {
                    return BelowRight;
                }

                if (to.Column < from.Column)
                {
                    return BelowLeft;
                }

                return Below;
            }
            else if (to.Row < from.Row)
            {
                if (to.Column > from.Column)
                {
                    return AboveRight;
                }

                if (to.Column < from.Column)
                {
                    return AboveLeft;
                }

                return Above;
            }
            else
            {
                if (to.Column > from.Column)
                {
                    return Right;
                }

                return Left;
            }
        }
    }
}

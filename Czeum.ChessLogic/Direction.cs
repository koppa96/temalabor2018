using System;

namespace Czeum.ChessLogic
{
    public class Direction
    {
        public int RowDirection { get; private set; }
        public int ColumnDirection { get; private set; }
        public Func<int, int, Field, bool> Predicate { get; private set; }

        private Direction(int rowDirection, int columnDirection, Func<int, int, Field, bool> predicate)
        {
            RowDirection = rowDirection;
            ColumnDirection = columnDirection;
            Predicate = predicate;
        }

        public static Direction Left => new Direction(0, -1, (i, j, f) => j > f.Column);

        public static Direction Right => new Direction(0, 1, (i, j, f) => j < f.Column);

        public static Direction Above => new Direction(-1, 0, (i, j, f) => i > f.Row);

        public static Direction Below => new Direction(1, 0, (i, j, f) => i < f.Row);

        public static Direction AboveLeft =>
            new Direction(-1, -1, (i, j, f) => Above.Predicate(i, j, f) && Left.Predicate(i, j, f));

        public static Direction AboveRight =>
            new Direction(-1, 1, (i, j, f) => Above.Predicate(i, j, f) && Right.Predicate(i, j, f));

        public static Direction BelowRight =>
            new Direction(1, 1, (i, j, f) => Below.Predicate(i, j, f) && Right.Predicate(i, j, f));

        public static Direction BelowLeft =>
            new Direction(1, -1, (i, j, f) => Below.Predicate(i, j, f) && Left.Predicate(i, j, f));

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

                return to.Column < from.Column ? BelowLeft : Below;
            }

            if (to.Row < from.Row)
            {
                if (to.Column > from.Column)
                {
                    return AboveRight;
                }

                return to.Column < from.Column ? AboveLeft : Above;
            }

            return to.Column > from.Column ? Right : Left;
        }
    }
}

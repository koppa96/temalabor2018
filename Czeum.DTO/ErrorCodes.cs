namespace Czeum.DTO
{
    public static class ErrorCodes
    {
        public const string InvalidBoardSize = nameof(InvalidBoardSize);
        public const string AlreadyInLobby = nameof(AlreadyInLobby);
        public const string AlreadyQueuing = nameof(AlreadyQueuing);
        public const string NoSuchLobby = nameof(NoSuchLobby);
        public const string NoRightToChange = nameof(NoRightToChange);
        public const string CouldNotJoinLobby = nameof(CouldNotJoinLobby);
    }
}
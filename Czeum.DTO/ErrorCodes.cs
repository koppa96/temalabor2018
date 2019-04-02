namespace Czeum.DTO
{
    public static class ErrorCodes
    {
        //Lobby management related errors
        public const string InvalidBoardSize = nameof(InvalidBoardSize);
        public const string AlreadyInLobby = nameof(AlreadyInLobby);
        public const string AlreadyQueuing = nameof(AlreadyQueuing);
        public const string NoSuchLobby = nameof(NoSuchLobby);
        public const string NoRightToChange = nameof(NoRightToChange);
        public const string CouldNotJoinLobby = nameof(CouldNotJoinLobby);
        public const string InvalidLobbyType = nameof(InvalidLobbyType);
        public const string NotEnoughPlayers = nameof(NotEnoughPlayers);
        public const string CannotSendMessage = nameof(CannotSendMessage);
        
        //Friend management related errors
        public const string NoSuchUser = nameof(NoSuchUser);
        public const string AlreadyFriends = nameof(AlreadyFriends);
        public const string AlreadyRequested = nameof(AlreadyRequested);
        public const string NoSuchRequest = nameof(NoSuchRequest);
        public const string NoSuchFriendship = nameof(NoSuchFriendship);
        
        //Game related errors
        public const string NoSuchMatch = nameof(NoSuchMatch);
        public const string NotYourTurn = nameof(NotYourTurn);
        public const string NotYourMatch = nameof(NotYourMatch);
        public const string GameNotSupported = nameof(GameNotSupported);
    }
}
namespace Czeum.Api.Common
{
    public static class ApiResources
    {
        public const string BasePath = "api";
        
        public static class Accounts
        {
            public const string BasePath = ApiResources.BasePath + "/accounts";
        }
        
        public static class Boards
        {
            public const string BasePath = ApiResources.BasePath + "/boards";
        }

        public static class Friends
        {
            public const string BasePath = ApiResources.BasePath + "/friends";

            public static class FriendRequests
            {
                public const string BasePath = ApiResources.Friends.BasePath + "/friend-requests";
            }

            public static class Friendships
            {
                public const string BasePath = ApiResources.Friends.BasePath + "/friendships";
            }
        }
        
        public static class Lobbies
        {
            public const string BasePath = ApiResources.BasePath + "/lobbies";
        }

        public static class Messages
        {
            public const string BasePath = ApiResources.BasePath + "/messages";
            
            public static class Lobby
            {
                public const string BasePath = ApiResources.Messages.BasePath + "/lobby";
            }
        
            public static class Match
            {
                public const string BasePath = ApiResources.Messages.BasePath + "/match";
            }
        }
        
        public static class Matches
        {
            public const string BasePath = ApiResources.BasePath + "/matches";
        }
        
        public static class SoloQueue
        {
            public const string BasePath = ApiResources.BasePath + "solo-queue";
        }
    }
}
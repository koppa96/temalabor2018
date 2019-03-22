namespace Czeum.Server.Services
{
    public interface ISoloQueueService
    {
        void JoinSoloQueue(string user);
        void LeaveSoloQueue(string user);
        string[] PopFirstTwoPlayers();
        bool IsQueuing(string user);
    }
}
using System.Threading.Tasks;

namespace Czeum.Server.Hubs
{
    public interface IErrorClient
    {
        Task ReceiveError(string errorCode);
    }
}
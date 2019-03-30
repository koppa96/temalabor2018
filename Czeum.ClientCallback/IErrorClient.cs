using System.Threading.Tasks;

namespace Czeum.ClientCallback
{
    public interface IErrorClient
    {
        Task ReceiveError(string errorCode);
    }
}
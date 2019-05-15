using System.Threading.Tasks;

namespace Czeum.ClientCallback
{
    /// <summary>
    /// An interface used by the GameHub to notify its clients about errors.
    /// </summary>
    public interface IErrorClient
    {
        Task ReceiveError(string errorCode);
    }
}
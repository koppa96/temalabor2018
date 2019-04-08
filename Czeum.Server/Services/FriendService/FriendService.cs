using System.Collections.Generic;
using Czeum.DAL;

namespace Czeum.Server.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FriendService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<string> GetFriendsOf(string user)
        {
            return _unitOfWork.FriendRepository.GetFriendsOf(user);
        }

        public void AcceptRequest(string sender, string receiver)
        {
            _unitOfWork.FriendRepository.AcceptRequest(sender, receiver);
            _unitOfWork.Save();
        }

        public void RemoveFriend(string user, string friend)
        {
            _unitOfWork.FriendRepository.RemoveFriend(user, friend);
            _unitOfWork.Save();
        }

        public void AddRequest(string sender, string receiver)
        {
            _unitOfWork.FriendRepository.AddRequest(sender, receiver);
            _unitOfWork.Save();
        }

        public void RemoveRequest(string sender, string receiver)
        {
            _unitOfWork.FriendRepository.RemoveRequest(sender, receiver);
            _unitOfWork.Save();
        }

        public List<string> GetRequestsSentBy(string user)
        {
            return _unitOfWork.FriendRepository.GetRequestsSentBy(user);
        }

        public List<string> GetRequestsReceivedBy(string user)
        {
            return _unitOfWork.FriendRepository.GetRequestsReceivedBy(user);
        }
    }
}
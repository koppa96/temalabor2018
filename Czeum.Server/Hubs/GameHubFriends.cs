using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.DTO;
using Czeum.DTO.UserManagement;

namespace Czeum.Server.Hubs
{
    public partial class GameHub
    {
        public async Task<List<string>> SendFriendRequest(string receiver)
        {
            try
            {
                _friendRepository.AddRequest(Context.UserIdentifier, receiver);
                await Clients.User(receiver).ReceiveRequest(Context.UserIdentifier);
            }
            catch (ArgumentOutOfRangeException e)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchUser);
            }
            catch (ArgumentException e)
            {
                if (e.Message.Contains("request"))
                {
                    await Clients.Caller.ReceiveError(ErrorCodes.AlreadyRequested);
                }
                else if (e.Message.Contains("friendship"))
                {
                    await Clients.Caller.ReceiveError(ErrorCodes.AlreadyFriends);
                }
            }
            
            return _friendRepository.GetRequestsSentBy(Context.UserIdentifier);
        }

        public async Task<Friend> AcceptRequest(string sender)
        {
            try
            {
                _friendRepository.AcceptRequest(sender, Context.UserIdentifier);
            }
            catch (ArgumentException)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchRequest);
                return null;
            }

            await Clients.User(sender).FriendAdded(new Friend { IsOnline = true, Username = Context.UserIdentifier });
            return new Friend 
            {
                IsOnline = _onlineUserTracker.IsOnline(sender),
                Username = sender
            };
        }

        public async Task RejectRequest(string sender)
        {
            try
            {
                _friendRepository.RemoveRequest(sender, Context.UserIdentifier);
            }
            catch (ArgumentException e)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchRequest);
                return;
            }

            await Clients.User(sender).RequestRejected(Context.UserIdentifier);
        }

        public async Task RemoveFriend(string friend)
        {
            try
            {
                _friendRepository.RemoveFriend(Context.UserIdentifier, friend);
            }
            catch (ArgumentException e)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchFriendship);
                return;
            }

            await Clients.User(friend).FriendRemoved(Context.UserIdentifier);
        }
    }
}
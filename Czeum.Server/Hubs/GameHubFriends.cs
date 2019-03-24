using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.DTO;
using Czeum.DTO.UserManagement;

namespace Czeum.Server.Hubs
{
    public partial class GameHub
    {
        public async Task SendFriendRequest(string receiver)
        {
            try
            {
                _friendRepository.AddRequest(Context.UserIdentifier, receiver);
                await Clients.User(receiver).ReceiveRequest(Context.UserIdentifier);
                await Clients.Caller.SuccessfulRequest(receiver);
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
        }

        public async Task AcceptRequest(string sender)
        {
            try
            {
                _friendRepository.AcceptRequest(sender, Context.UserIdentifier);
                
                await Clients.User(sender).FriendAdded(new Friend
                {
                    IsOnline = true, 
                    Username = Context.UserIdentifier
                });
                
                await Clients.Caller.FriendAdded(new Friend {
                    IsOnline = _onlineUserTracker.IsOnline(sender),
                    Username = sender
                });
            }
            catch (ArgumentException)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchRequest);
            }
        }

        public async Task RejectRequest(string sender)
        {
            try
            {
                _friendRepository.RemoveRequest(sender, Context.UserIdentifier);
                
                await Clients.User(sender).RequestRejected(Context.UserIdentifier);
                await Clients.Caller.SuccessfulRejection(sender);
            }
            catch (ArgumentException e)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchRequest);
            }
        }

        public async Task RemoveFriend(string friend)
        {
            try
            {
                _friendRepository.RemoveFriend(Context.UserIdentifier, friend);
                
                await Clients.User(friend).FriendRemoved(Context.UserIdentifier);
                await Clients.Caller.FriendRemoved(friend);
            }
            catch (ArgumentException e)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchFriendship);
            }
        }
    }
}
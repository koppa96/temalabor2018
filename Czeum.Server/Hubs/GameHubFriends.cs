using System;
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
                await _friendService.AddRequestAsync(Context.UserIdentifier, receiver);
                await Clients.User(receiver).ReceiveRequest(Context.UserIdentifier);
                await Clients.Caller.SuccessfulRequest(receiver);
            }
            catch (ArgumentOutOfRangeException)
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
                await _friendService.AcceptRequestAsync(sender, Context.UserIdentifier);
                
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
                await _friendService.RemoveRequestAsync(sender, Context.UserIdentifier);
                
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
                await _friendService.RemoveFriendAsync(Context.UserIdentifier, friend);
                
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
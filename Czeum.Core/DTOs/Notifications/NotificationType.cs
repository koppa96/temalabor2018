using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Core.DTOs.Notifications
{
    public enum NotificationType
    {
        InviteReceived = 0,
        FriendRequestReceived = 1,
        Poked = 2,
        FriendRequestAccepted = 3,
        AchivementUnlocked = 4
    }
}

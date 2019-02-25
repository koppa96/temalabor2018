using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.DAL.Entities
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public ApplicationUser User1 { get; set; }
        public ApplicationUser User2 { get; set; }
    }
}

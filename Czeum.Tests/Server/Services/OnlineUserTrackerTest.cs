using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Server.Services.OnlineUsers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Server.Services
{
    [TestClass]
    public class OnlineUserTrackerTest
    {
        private IOnlineUserTracker onlineUserTracker;

        [TestInitialize]
        public void Init()
        {
            onlineUserTracker = new OnlineUserTracker();
        }

        [TestMethod]
        [DataRow("TestUser")]
        public void AddOnlineUser(string username)
        {
            onlineUserTracker.PutUser(username);
            Assert.IsTrue(onlineUserTracker.IsOnline(username));
        }

        [TestMethod]
        [DataRow("TestUser")]
        public void RemoveOnlineUser(string username)
        {
            onlineUserTracker.PutUser(username);
            onlineUserTracker.RemoveUser(username);
            Assert.IsFalse(onlineUserTracker.IsOnline(username));
        }

        [TestMethod]
        [DataRow("TestUser")]
        public void TryPutMultipleUsersWithSameName(string username)
        {
            onlineUserTracker.PutUser(username);
            onlineUserTracker.PutUser(username);
            Assert.AreEqual(1, onlineUserTracker.GetUsers().Count);

            onlineUserTracker.RemoveUser(username);
            Assert.IsFalse(onlineUserTracker.IsOnline(username));
        }
    }
}

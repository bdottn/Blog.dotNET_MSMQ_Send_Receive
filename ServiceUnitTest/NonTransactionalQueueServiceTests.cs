using ExpectedObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Service.Protocol;
using System;
using System.Messaging;

namespace Service.UnitTest
{
    [TestClass]
    public sealed class NonTransactionalQueueServiceTests
    {
        // private string remoteQueuePath = @"FormatName:DIRECT=OS:{ServerName}\private$\NonTransactionalQueue";
        // private string remoteQueuePath = @"FormatName:DIRECT=TCP:{ServerIP}\private$\NonTransactionalQueue";

        private string queuePath = @".\private$\NonTransactionalQueue";

        private IQueueService service;

        [TestInitialize]
        public void TestInitialize()
        {
            this.service = new NonTransactionalQueueService(this.queuePath);

            this.CleanQueue();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.service = null;

            this.CleanQueue();
        }

        private void CleanQueue()
        {
            var queue = new MessageQueue(this.queuePath);

            queue.Purge();
        }

        [TestMethod]
        public void SendTest_傳送Student_預期佇列中的第一個訊息有一樣的Student()
        {
            var student =
                new Student()
                {
                    Id = Guid.NewGuid(),
                    Name = "Echo",
                    Height = 178,
                    Weight = 60,
                };

            var expected = student;

            this.service.Send(student);

            // 利用 Peek 取得佇列中的第一個訊息
            var queue = new MessageQueue(this.queuePath);

            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Student), });

            var message = queue.Peek(TimeSpan.FromSeconds(10));

            var actual = (Student)message.Body;

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void SendReceiveTest_傳送Student_預期接收一樣的Student()
        {
            var student =
                new Student()
                {
                    Id = Guid.NewGuid(),
                    Name = "David",
                    Height = 165,
                    Weight = 85,
                };

            var expected = student;

            this.service.Send(student);

            var actual = this.service.Receive(TimeSpan.FromSeconds(10));

            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }
}
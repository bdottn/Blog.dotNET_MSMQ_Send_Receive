using Model;
using Service.Protocol;
using System;
using System.Messaging;

namespace Service
{
    public sealed class NonTransactionalQueueService : IQueueService
    {
        private readonly MessageQueue messageQueue;

        public NonTransactionalQueueService(string queuePath)
        {
            this.messageQueue = new MessageQueue(queuePath);
        }

        public void Send(Student student)
        {
            this.messageQueue.Send(student);
        }

        public Student Receive(TimeSpan timeout)
        {
            this.messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Student), });

            var message = this.messageQueue.Receive(timeout);

            return (Student)message.Body;
        }
    }
}
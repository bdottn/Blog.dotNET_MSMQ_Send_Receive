using Model;
using Service.Protocol;
using System;
using System.Messaging;

namespace Service
{
    public sealed class TransactionalQueueService : IQueueService
    {
        private readonly MessageQueue messageQueue;
        private readonly MessageQueueTransaction transaction;

        public TransactionalQueueService(string queuePath)
        {
            this.messageQueue = new MessageQueue(queuePath);

            this.transaction = new MessageQueueTransaction();
        }

        public void Send(Student student)
        {
            this.transaction.Begin();

            this.messageQueue.Send(student, this.transaction);

            this.transaction.Commit();
        }

        public Student Receive(TimeSpan timeout)
        {
            this.messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Student), });

            this.transaction.Begin();

            var message = this.messageQueue.Receive(timeout, this.transaction);

            this.transaction.Commit();

            return (Student)message.Body;
        }
    }
}
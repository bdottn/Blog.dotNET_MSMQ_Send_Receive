using Model;
using System;

namespace Service.Protocol
{
    public interface IQueueService
    {
        void Send(Student student);

        Student Receive(TimeSpan timeout);
    }
}
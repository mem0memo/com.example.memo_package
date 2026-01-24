using UnityEngine;

namespace mm
{
    public interface IMessageService : IService
    {
        void Send(string message, Color color);
    }
}

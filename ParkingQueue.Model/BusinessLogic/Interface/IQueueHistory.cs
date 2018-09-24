using System;

namespace ParkingQueue.Model
{
    /// <summary>Элемент в истории электронной очереди на выдачу автомобиля</summary>
    public interface IQueueHistory: IQueue
    {
        /// <summary>Причина исключения из очереди</summary>
        string OutputReasonId { get; set; }
    }
}
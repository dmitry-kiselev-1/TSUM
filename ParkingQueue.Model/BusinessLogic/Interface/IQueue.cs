using System;

namespace ParkingQueue.Model
{
    /// <summary>Элемент электронной очереди на выдачу автомобиля</summary>
    public interface IQueue
    {
        /// <summary>Время фактического включения в очередь</summary>
        DateTime Input { get; set; }
        
        /// <summary>Номер парковочной карты</summary>
        string ParkingCard { get; set; }
        
        /// <summary>
        /// На этапе Queue: время ожидания (прогноз исключения из очереди);
        /// На этапе QueueHistory: время фактического исключения из очереди
        /// </summary>
        DateTime? Output { get; set; }
        
        /// <summary>Количество ротаций (пропусков клиентом своей очереди)</summary>
        byte? Rotation { get; set; }
        
        /// <summary>Отметка времени первой ротации</summary>
        DateTime? RotationStart { get; set; }
        
        /// <summary>
        /// На этапе Queue: позиция в очереди;
        /// На этапе QueueHistory: позиция на момент фактического исключения из очереди
        /// </summary>
        int? Position { get; set; }
    }
}
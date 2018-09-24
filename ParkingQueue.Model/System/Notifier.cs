using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ParkingQueue.Model.Annotations;
using ParkingQueue.Model.ParkingQueue;

namespace ParkingQueue.Model
{
    /// <summary>Уведомляет об изменении очереди</summary>
    public static class Notifier
    {
        /// <summary>Событие для подписки на изменение очереди</summary>
        public static event EventHandler QueueChanged;

        /// <summary>Генерация события, уведомляющего об изменении очереди</summary>
        internal static void OnQueueChanged(QueueChangedEventArgs eventArgs)
        {
            QueueChanged?.Invoke(null, eventArgs);
        }
    }

    /// <summary>Информация о серверном событии</summary>
    public class QueueChangedEventArgs : EventArgs
    {
        /// <summary>Причина изменения очереди</summary>
        public QueueChangedEnum Reason;

        /// <summary>Элементы очереди</summary>
        public IEnumerable<Model.IQueue> Queue;

        /// <summary>Время ожидания Клиента (до ротации)</summary>
        public int RotationTimeLimitParam = Model.QueueManager.RotationTimeLimitParam;
    }

    /// <summary>Причина изменения очереди</summary>
    public enum QueueChangedEnum : byte
    {
        /// <summary>Элемент добавлен</summary>
        EnQueue,
        /// <summary>Элемент удалён</summary>
        DeQueue,
        /// <summary>Ротация элемента</summary>
        Rotation
    }
}

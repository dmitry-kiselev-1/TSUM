using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkingQueue.Model.ParkingQueue;

namespace ParkingQueue.Model
{
    /// <summary>
    /// Элемент электронной очереди на выдачу автомобиля (Active Record Pattern, за исключением Delete).
    /// Уникальным идентификатором сущности является свойство ParkingCard.
    /// </summary>
    [Table("Queue")]
    [KnownType(typeof(Queue))]
    [JsonObject(MemberSerialization.OptIn)]
    //[Synchronization]
    public partial class Queue: QueueBase, IQueue, IEquatable<Queue>
    {
        /// <summary>Возвращает все элементы очереди</summary>
        public IEnumerable<Model.IQueue> Get()
        {
            try
            {
                using (Repository repository = new Repository())
                {
                    return QueueManager.Recalculate(repository.Queue.ToList());
                }
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return null;
            }
        }

        /// <summary>Возвращает заданное количество элементов очереди</summary>
        /// <param name="count">Количество первых элементов</param>
        public IEnumerable<Model.IQueue> Get(int count)
        {
            try
            {
                return Get().Take(count);
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return null;
            }
        }
        
        /// <summary>Возвращает информацию о заданном элементе в очереди</summary>
        /// <param name="id">Номер парковочной карты</param>
        public Model.IQueue Get(string id)
        {
            try
            {
                // быстрая проверка - если такого элемента нет, пересчёт очереди не нужен:
                using (Repository repository = new Repository())
                {
                    if (repository.Queue.Find(id) == null)
                        return null;
                }

                return Get().FirstOrDefault(t => t.ParkingCard == id);
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return null;
            }
        }

        /// <summary>Добавляет уникальный элемент в очередь</summary>
        public Model.IQueue Put()
        {
            try
            {
                if (QueueManager.EnQueueTran(this))
                {
                    // возврат информации о добавленном элементе
                    return new Model.Queue().Get(this.ParkingCard);
                }
                return null;
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return null;
            }
        }

        /// <summary>Удаляет заданный элемент из очереди</summary>
        /// <param name="id">Номер парковочной карты</param>
        /// <param name="reason">Причина исключения из очереди</param>
        public Model.IQueueHistory Delete(string id, OutputReasonEnum reason)
        {
            try
            {
                // для удаления элемента необходима полная информация о нём:
                var entity = new Model.Queue().Get(id);

                if (QueueManager.DeQueueTran(entity, reason))
                {
                    // возврат информации об удалённом элементе
                    return new QueueHistory()
                    {
                        Input = entity.Input,
                        ParkingCard = entity.ParkingCard
                    }.Get();
                }
                return null;
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return null;
            }
        }

        /// <summary>Перезапуск или остановка таймера</summary>
        public bool Start(bool start)
        {
            try
            {
                return QueueManager.ResetTimer(start);
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return false;
            }
        }

        /// <summary>Запуск одной ротации</summary>
        public bool Rotate()
        {
            try
            {
                return QueueManager.RotateTran(once: true);
            }
            catch (Exception e)
            {
                Model.Log.Put(message: e.Message, exception: e);
                return false;
            }
        }

        public bool Equals(Queue other)
        {
            if (other == null)
                return false;

            if (this.GetHashCode() == other.GetHashCode())
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.ParkingCard.GetHashCode();
        }
    }
}

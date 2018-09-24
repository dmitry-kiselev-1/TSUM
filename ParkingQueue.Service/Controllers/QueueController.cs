using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using ParkingQueue.Model;
using ParkingQueue.Model.ParkingQueue;

namespace ParkingQueue.Service.Controllers
{
    /// <summary>API электронной очереди</summary>
    //[Authorize]
    public class QueueController : ControllerBase
    {
        /// <summary>Возвращает все элементы очереди</summary>
        /// <returns>
        /// [
        ///     {"parkingCard":"ParkingCard_0001","output":"2016-02-24T16:26:31.032","rotation":0,"position":1},
        ///     {"parkingCard":"ParkingCard_0002","output":"2016-02-24T16:31:31.032","rotation":0,"position":2}
        /// ]        
        /// </returns>
        [ResponseType(typeof(IEnumerable<Queue>))]
        public IEnumerable<Model.IQueue> Get()
        {
            return new Model.Queue().Get();
        }

        /// <summary>Возвращает заданное количество элементов очереди</summary>
        /// <param name="count">Количество первых элементов</param>
        /// <returns>
        /// [
        ///     {"parkingCard":"ParkingCard_0001","output":"2016-02-24T16:26:31.032","rotation":0,"position":1},
        ///     {"parkingCard":"ParkingCard_0002","output":"2016-02-24T16:31:31.032","rotation":0,"position":2}
        /// ]        
        /// </returns>
        [ResponseType(typeof(IEnumerable<Queue>))]
        public IEnumerable<Model.IQueue> Get([FromUri] int count)
        {
            return new Model.Queue().Get(count);
        }

        /// <summary>Возвращает информацию о заданном элементе в очереди</summary>
        /// <param name="id">Номер парковочной карты</param>
        /// <returns>
        /// {"parkingCard":"ParkingCard_0001","output":"2016-02-24T16:26:31.032","rotation":0,"position":1}
        /// </returns>
        [ResponseType(typeof(Queue))]
        public Model.IQueue Get(string id)
        {
            return new Model.Queue().Get(id);
        }

        /// <summary>Добавляет уникальный элемент в очередь</summary>
        /// <param name="id">Номер парковочной карты</param>
        /// <param name="value">В настоящее время не используется</param>
        /// <returns>
        /// {"parkingCard":"ParkingCard_0001","output":"2016-02-24T16:26:31.032","rotation":0,"position":1}
        /// </returns>
        [ResponseType(typeof(Queue))]
        public Model.IQueue Put(string id, [FromBody] Model.IQueue value)
        {
            return new Model.Queue()
            {
                ParkingCard = id
            }
            .Put();
        }

        /// <summary>Удаляет заданный элемент из очереди</summary>
        /// <param name="id">Номер парковочной карты</param>
        /// <param name="reason">Причина исключения из очереди</param>
        /// <returns>
        /// {"parkingCard":"ParkingCard_0001","output":"2016-02-24T16:26:31.032","rotation":0,"position":1,"outputReasonId"="V"}
        /// </returns>
        [ResponseType(typeof(QueueHistory))]
        public Model.IQueueHistory Delete(string id, [FromUri] OutputReasonEnum reason)
        {
            return new Model.Queue().Delete(id, reason);

            /* Active Record Pattern:
            return new Model.Queue()
            {
                ParkingCard = id,
                OutputReasonId = reason
            }
            .Delete();
            */
        }

        /// <summary>Запуск или остановка таймера</summary>
        /// <example>POST api/Queue?start=true</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool Post([FromUri] bool start)
        {
            return new Model.Queue().Start(start);
        }

        /// <summary>Запуск одной ротации</summary>
        /// <example>POST api/Queue?rotation=1</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool Post([FromUri] int rotation)
        {
            return new Model.Queue().Rotate();
        }
    }
}

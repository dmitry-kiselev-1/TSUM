using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ParkingQueue.Model
{
    /// <summary>
    /// Элемент в истории электронной очереди (Active Record Pattern).
    /// Уникальным идентификатором сущности являются свойства OutputReason + ParkingCard.
    /// </summary>
    [Table("QueueHistory")]
    [KnownType(typeof(QueueHistory))]
    [JsonObject(MemberSerialization.OptIn)]
    public partial class QueueHistory : QueueBase, IQueueHistory
    {
        /// <summary>Причина исключения из очереди</summary>
        [Required]
        public OutputReason OutputReason { get; set; }

        [ForeignKey("OutputReason")]
        [JsonProperty(Order = 6)]
        public string OutputReasonId { get; set; }

        /// <summary>Возвращает информацию о заданном элементе в очереди</summary>
        public Model.IQueueHistory Get()
        {
            using (Repository repository = new Repository())
            {
                return repository.QueueHistory.Find(this.Input, this.ParkingCard);
            }
        }
    }
}

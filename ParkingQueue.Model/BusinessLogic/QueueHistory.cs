using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ParkingQueue.Model
{
    /// <summary>
    /// ������� � ������� ����������� ������� (Active Record Pattern).
    /// ���������� ��������������� �������� �������� �������� OutputReason + ParkingCard.
    /// </summary>
    [Table("QueueHistory")]
    [KnownType(typeof(QueueHistory))]
    [JsonObject(MemberSerialization.OptIn)]
    public partial class QueueHistory : QueueBase, IQueueHistory
    {
        /// <summary>������� ���������� �� �������</summary>
        [Required]
        public OutputReason OutputReason { get; set; }

        [ForeignKey("OutputReason")]
        [JsonProperty(Order = 6)]
        public string OutputReasonId { get; set; }

        /// <summary>���������� ���������� � �������� �������� � �������</summary>
        public Model.IQueueHistory Get()
        {
            using (Repository repository = new Repository())
            {
                return repository.QueueHistory.Find(this.Input, this.ParkingCard);
            }
        }
    }
}

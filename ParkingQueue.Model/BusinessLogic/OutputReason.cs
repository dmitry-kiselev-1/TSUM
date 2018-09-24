using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ParkingQueue.Model
{
    /// <summary>���������� ������ ���������� �� �������</summary>
    [Table("OutputReason")]
    [KnownType(typeof(OutputReason))]
    public partial class OutputReason
    {
        [Key]
        [Required]
        [StringLength(1)]
        [Column(Order = 0)]
        [Display(Name = "������������� �������")]
        public string Id { get; set; }

        [Required]
        [StringLength(16)]
        [Column(Order = 1)]
        [Display(Name = "�������� �������")]
        public string Code { get; set; }

        [StringLength(128)]
        [Column(Order = 2)]
        [Display(Name = "�������� �������")]
        public string Description { get; set; }

        public virtual ICollection<QueueHistory> QueueHistory { get; set; }

        public OutputReason()
        {
            QueueHistory = new HashSet<QueueHistory>();
        }
    }

    namespace ParkingQueue
    {
        /// <summary>
        /// ��������� ������� ���������� �� �������
        /// </summary>
        public enum OutputReasonEnum : byte
        {
            /// <summary>�� - ���������� ������ �������</summary>
            Rotation,
            /// <summary>��� - ������� ������� �������� �������</summary>
            Peek,
            /// <summary>��� - ������� ������� ��� �������</summary>
            Vip,
            /// <summary>��� - ������ �� ������ ������ � ��� ���������� �� ��������</summary>
            Time
        }
    }

}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ParkingQueue.Model
{
    /// <summary>Справочник причин исключения из очереди</summary>
    [Table("OutputReason")]
    [KnownType(typeof(OutputReason))]
    public partial class OutputReason
    {
        [Key]
        [Required]
        [StringLength(1)]
        [Column(Order = 0)]
        [Display(Name = "Идентификатор причины")]
        public string Id { get; set; }

        [Required]
        [StringLength(16)]
        [Column(Order = 1)]
        [Display(Name = "Название причины")]
        public string Code { get; set; }

        [StringLength(128)]
        [Column(Order = 2)]
        [Display(Name = "Описание причины")]
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
        /// Возможные причины исключения из очереди
        /// </summary>
        public enum OutputReasonEnum : byte
        {
            /// <summary>ЭО - превышение лимита ротаций</summary>
            Rotation,
            /// <summary>СУД - Пропуск Клиента согласно очереди</summary>
            Peek,
            /// <summary>СУД - Пропуск Клиента вне очереди</summary>
            Vip,
            /// <summary>СУД - Клиент не забрал машину и она возвращена на парковку</summary>
            Time
        }
    }

}

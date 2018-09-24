using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

//using Newtonso

namespace ParkingQueue.Model
{
    /// <summary>Элемент электронной очереди</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class QueueBase
    {
        /// <summary>Время фактического включения в очередь</summary>
        [Required]
        [Column(Order = 0)]
        public DateTime Input { get; set; }

        /// <summary>Номер парковочной карты</summary>
        [Required]
        [StringLength(16)]
        [Column(Order = 1)]
        [JsonProperty(Order = 1)]
        public string ParkingCard { get; set; }

        /// <summary>
        /// На этапе Queue: вычисляемое время ожидания (прогноз исключения из очереди);
        /// На этапе QueueHistory: время фактического исключения из очереди
        /// </summary>
        [Column(Order = 2)]
        [JsonProperty(Order = 2)]
        public DateTime? Output { get; set; }
        
        /// <summary>Количество ротаций (пропусков клиентом своей очереди)</summary>
        [Column(Order = 3)]
        [JsonProperty(Order = 4/*, NullValueHandling = NullValueHandling.Ignore*/)]
        public byte? Rotation { get; set; }

        /// <summary>
        /// На этапе Queue: вычисляемая позиция в очереди (принудительная позиция сохраняется в БД);
        /// На этапе QueueHistory: позиция на момент фактического исключения из очереди
        /// </summary>
        [Column(Order = 4)]
        [JsonProperty(Order = 3)]
        public int? Position { get; set; }

        /// <summary>Отметка времени первой ротации</summary>
        [Column(Order = 5)]
        //[JsonProperty(Order = 5, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? RotationStart { get; set; }

        /// <summary>
        /// Зарезервировано для передачи дополнительной информации вызывающей системе
        /// </summary>
        [NotMapped]
        [StringLength(128)]
        [Column(Order = 100)]
        [JsonProperty(Order = 100, NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }
    }
}
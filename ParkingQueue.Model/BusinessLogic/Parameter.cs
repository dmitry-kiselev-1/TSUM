using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace ParkingQueue.Model
{
    /// <summary>Параметр системы (Active Record Pattern).</summary>
    [Table("Parameter")]
    [KnownType(typeof(Parameter))]
    public partial class Parameter : IParameter
    {
        [Key]
        [Required]
        [StringLength(32)]
        [Column(Order = 0)]
        public string Code { get; set; }

        [Required]
        [StringLength(32)]
        [Column(Order = 1)]
        public string Value { get; set; }

        [StringLength(128)]
        [Column(Order = 2)]
        public string Description { get; set; }

        #region methods

        /// <summary>Возвращает список параметров</summary>
        public IEnumerable<Model.IParameter> Get()
        {
            using (Repository repository = new Repository())
            {
                return repository.Parameter.ToList();
            }
        }

        /// <summary>Возвращает информацию о параметре</summary>
        public Model.IParameter Get(string id)
        {
            using (Repository repository = new Repository())
            {
                return repository.Parameter.Find(id);
            }
        }

        /// <summary>Добавляет параметр системы</summary>
        public void Put()
        {
            using (Repository repository = new Repository())
            {
                repository.Parameter.Add(
                    new Parameter()
                    {
                        Code = this.Code,
                        Value = this.Value,
                        Description = this.Description
                    });

                repository.SaveChanges();
            }
        }

        #endregion
    }

    namespace ParkingQueue
    {
        /// <summary>
        /// Возможные параметры системы
        /// </summary>
        public enum ParameterEnum : byte
        {
            /// <summary>Расчётное время ожидания одного автомобиля</summary>
            OutputForecastParam,

            /// <summary>Лимит времени ожидания, по истечении которого считается, что Клиент пропустил свою очередь</summary>
            RotationTimeLimitParam,
            
            /// <summary>Лимит ротаций - количество пропусков Клиентом своей очереди до его исключения</summary>
            RotationCountLimitParam,

            /// <summary>Позиция, на которую помещается ротируемый (опоздавший) Клиент</summary>
            RotationPlaceParam,

            /// <summary>Количество видимых Клиенту позиций в очереди</summary>
            ViewLimitParam
        }
    }
}
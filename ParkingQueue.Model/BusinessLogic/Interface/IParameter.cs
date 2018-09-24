using System.Collections.Generic;

namespace ParkingQueue.Model
{
    /// <summary>Параметр системы</summary>
    public interface IParameter
    {
        /// <summary>Название параметра</summary>
        string Code { get; set; }
        /// <summary>Описание параметра.</summary>
        string Description { get; set; }

        /// <summary>Значение параметра</summary>
        string Value { get; set; }
    }
}
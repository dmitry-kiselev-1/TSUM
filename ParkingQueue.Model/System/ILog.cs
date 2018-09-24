using System;
using System.Threading.Tasks;

namespace ParkingQueue.Model
{
    public interface ILog
    {
        Int64 Id { get; set; }
        DateTime Date { get; set; }
        string Message { get; set; }
        string Exception { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParkingQueue.Model
{
    /// <summary>Журнал ошибок системы.</summary>
    [Table("Log")]
    [KnownType(typeof(Log))]
    public class Log : ILog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column(Order = 0)]
        public Int64 Id { get; set; }

        [Required]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Required]
        //[StringLength(512)]
        [Column(Order = 2)]
        public string Message { get; set; }

        [Required]
        //[StringLength(2048)]
        [Column(Order = 3)]
        public string Exception { get; set; }

        public static async Task Put(string message, Exception exception)
        {
            try
            {
                using (Repository repository = new Repository())
                {
                    repository.Log.Add(new Log() {Date = DateTime.Now, Message = message, Exception = exception.ToString()});
                    //await repository.SaveChangesAsync();
                    repository.SaveChanges();
                }
            }
            catch (Exception e)
            {
                string source = "ParkingQueue.Service";
                string log = "ParkingQueue.Service";
                string logMessage = @" Исключение не удалось сохранить в БД: \n\n" + e.Message + @";\n\n Подробности: \n\n" + e.ToString();

                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, log);

                //await Task.Run(() =>
                //    EventLog.WriteEntry(source, logMessage, EventLogEntryType.Error));
                EventLog.WriteEntry(source, logMessage, EventLogEntryType.Error);
            }
        }
    }
}

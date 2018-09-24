using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;

namespace ParkingQueue.Service.Controllers
{
    //[Authorize]
    public class LogController : ControllerBase
    {
        /// <summary>Создаёт запись в журнале ошибок</summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task Put(string message, Exception exception)
        {
            return Model.Log.Put(message, exception);
        }

        /*
        /// <summary>Возвращает записи журнала ошибок</summary>
        /// <example>GET api/Log</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<Model.ILog> Get()
        {
            return new Model.Log().Get();
        }
        */
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;

namespace ParkingQueue.Service.Controllers
{
    //[Authorize]
    public class ParameterController : ControllerBase
    {
        /// <summary>Возвращает список параметров</summary>
        /// <example>GET api/Parameter</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<Model.IParameter> Get()
        {
            return new Model.Parameter().Get();
        }

        /// <summary>Возвращает информацию о параметре</summary>
        /// <param name="id">Код параметра</param>
        /// <example>GET api/Parameter/1</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public Model.IParameter Get(string id)
        {
            return new Model.Parameter().Get(id);
        }
    }
}

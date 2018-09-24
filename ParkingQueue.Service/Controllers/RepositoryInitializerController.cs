using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using ParkingQueue.Model;

namespace ParkingQueue.Service.Controllers
{
    //[Authorize]
    public class RepositoryInitializerController : ControllerBase
    {
        /// <summary>Инициализация БД</summary>
        /// <param name="withTestData">Генерировать тестовые данные в БД</param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Get([FromUri] string withTestData = "false")
        {
            new Model.RepositoryInitializer().Initialize(withTestData);
        }
    }
}

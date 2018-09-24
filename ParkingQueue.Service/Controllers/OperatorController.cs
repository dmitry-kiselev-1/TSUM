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
    public class OperatorController : ControllerBase
    {
        /// <summary>Возвращает список операторов</summary>
        /// <example>GET api/Operator</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<Model.IOperator> Get()
        {
            return new Model.Operator().Get();
        }

        /// <summary>Возвращает информацию о заданном операторе</summary>
        /// <param name="id">AD login</param>
        /// <example>GET api/Operator/1</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public Model.IOperator Get(string id)
        {
            return new Model.Operator().Get(id);
        }

        /// <summary>Добавляет оператора</summary>
        /// <param name="id">AD login</param>
        /// <param name="value">данные оператора</param>
        /// <example>PUT api/Operator/1</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Put(string id, [FromBody] Model.IOperator value)
        {
            new Model.Operator()
            {
                Login = id,
                Name = value.Name,
                IsAdmin = value.IsAdmin
            }.Put();
        }

        /// <summary>Удаляет заданного оператороа</summary>
        /// <param name="id">AD login</param>
        /// <example>DELETE api/Operator/1</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Delete(string id)
        {
            new Model.Operator()
            {
                Login = id
            }.Delete();
        }
    }
}

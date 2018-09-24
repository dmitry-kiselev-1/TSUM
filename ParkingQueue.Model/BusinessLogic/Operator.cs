using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace ParkingQueue.Model
{
    /// <summary>Пользователь системы, оператор (Active Record Pattern).</summary>
    [Table("Operator")]
    [KnownType(typeof(Operator))]
    public partial class Operator : IOperator
    {
        [Key]
        [Required]
        [StringLength(32)]
        [Column(Order = 0)]
        public string Login { get; set; }

        [Column(Order = 1)]
        public bool IsAdmin { get; set; }

        [Required]
        [StringLength(128)]
        [Column(Order = 2)]
        public string Name { get; set; }

        /// <summary>Возвращает список операторов</summary>
        public IEnumerable<Model.IOperator> Get()
        {
            using (Repository repository = new Repository())
            {
                return repository.Operator.ToList();
            }
        }

        /// <summary>Возвращает информацию о заданном операторе</summary>
        /// <param name="id">AD login</param>
        public Model.IOperator Get(string id)
        {
            using (Repository repository = new Repository())
            {
                return repository.Operator.Find(id);
            }
        }

        /// <summary>Добавляет оператора</summary>
        public void Put()
        {
            using (Repository repository = new Repository())
            {
                repository.Operator.Add(
                    new Operator()
                    {
                        Login = this.Login,
                        Name = this.Name,
                        IsAdmin = this.IsAdmin
                    });

                repository.SaveChanges();
            }
        }

        /// <summary>Удаляет заданного оператороа</summary>
        public void Delete()
        {
            using (Repository repository = new Repository())
            {
                var entity = repository.Operator.Find(this.Login);
                repository.Operator.Remove(entity);
                repository.SaveChanges();
            }
        }
    }
}

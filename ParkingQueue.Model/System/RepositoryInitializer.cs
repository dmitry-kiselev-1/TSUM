using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using ParkingQueue.Model.ParkingQueue;

namespace ParkingQueue.Model
{
    /// <summary>Инициализатор хранилища (БД)</summary>
    public class RepositoryInitializer
    {
        /// <summary>Инициализирует хранилище (БД) - только если его нет</summary>
        /// <example>GET api/RepositoryInitializer?withTestData=false</example>
        public void Initialize(string withTestData = "false")
        {
            var isTestingMode = bool.Parse(withTestData);

            using (Repository repository = new Repository())
            {
                if (repository.Database.Exists()) return;

                repository.Database.Create();

                // генерация данных, необходимых системе для работы (настройки и справочники):
                repository.OutputReason.AddRange(
                    new List<OutputReason>()
                    {
                        new OutputReason() {Id = "R", Code = OutputReasonEnum.Rotation.ToString(), Description = "Превышение лимита ротаций"},
                        new OutputReason() {Id = "P", Code = OutputReasonEnum.Peek.ToString(), Description = "Пропуск Клиента согласно очереди"},
                        new OutputReason() {Id = "V", Code = OutputReasonEnum.Vip.ToString(), Description = "Пропуск Клиента вне очереди"},
                        new OutputReason() {Id = "T", Code = OutputReasonEnum.Time.ToString(), Description = "Клиент не забрал машину и она возвращена на парковку"}
                    });

                repository.SaveChanges();
            }

            using (Repository repository = new Repository())
            {
                repository.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    @"ALTER DATABASE [ParkingQueue] SET RECOVERY SIMPLE WITH NO_WAIT;");

                repository.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    @"ALTER DATABASE [ParkingQueue] SET ALLOW_SNAPSHOT_ISOLATION ON;");
            }

            new Model.Parameter()
            {
                Code = ParameterEnum.OutputForecastParam.ToString(),
                Value = "300",
                Description = "Расчётное время ожидания одного автомобиля (в секундах)"
            }
            .Put();

            new Model.Parameter()
            {
                Code = ParameterEnum.RotationCountLimitParam.ToString(),
                Value = "3",
                Description = "Лимит ротаций - количество пропусков Клиентом своей очереди до его исключения"
            }.Put();

            new Model.Parameter()
            {
                Code = ParameterEnum.RotationPlaceParam.ToString(),
                Value = "5",
                Description = "Позиция, на которую помещается ротируемый (опоздавший) Клиент"
            }.Put();

            new Model.Parameter()
            {
                Code = ParameterEnum.ViewLimitParam.ToString(),
                Value = "20",
                Description = "Количество видимых Клиенту позиций в очереди"
            }.Put();

            new Model.Parameter()
            {
                Code = ParameterEnum.RotationTimeLimitParam.ToString(),
                Value = isTestingMode ? "10" : "120",
                Description = "Лимит времени ожидания (в секундах), по истечении которого считается, что Клиент пропустил свою очередь"
            }.Put();

            // генерация тестовых данных:

            if (!isTestingMode) return;

            for (int i = 1; i <= 5; i++)
            {
                new Model.Operator()
                {
                    Login = "Login_" + i.ToString("00"),
                    IsAdmin = false,
                    Name = "Name_" + i.ToString("00"),
                }.Put();
            }

            for (int i = 1; i <= 1000; i++)
            {
                Task.Delay(1);
                new Model.Queue()
                {
                    ParkingCard = "ParkingCard_" + i.ToString("0000")
                }
                .Put();
            }
        }
    }
}
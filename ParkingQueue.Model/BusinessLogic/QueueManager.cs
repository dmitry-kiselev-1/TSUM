using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ParkingQueue.Model.ParkingQueue;

namespace ParkingQueue.Model
{
    /// <summary>Инкапсулирует логику пересчёта и ротаций очереди</summary>
    //[Synchronization]
    static class QueueManager
    {
        /// <summary>Таймер ротаций</summary>
        private static readonly System.Timers.Timer RotationTimer = 
            new System.Timers.Timer(){AutoReset = false};

        internal static readonly int RotationTimeLimitParam =
                int.Parse(new Parameter().Get(ParameterEnum.RotationTimeLimitParam.ToString()).Value);

        internal static readonly int RotationCountLimitParam =
                int.Parse(new Parameter().Get(ParameterEnum.RotationCountLimitParam.ToString()).Value);

        internal static readonly int OutputForecastParam =
                int.Parse(new Parameter().Get(ParameterEnum.OutputForecastParam.ToString()).Value);

        internal static readonly int ViewLimitParam =
                int.Parse(new Parameter().Get(ParameterEnum.ViewLimitParam.ToString()).Value);

        internal static readonly int RotationPlaceParam =
                int.Parse(new Parameter().Get(ParameterEnum.RotationPlaceParam.ToString()).Value);

        const IsolationLevel TranIsolationLevel = IsolationLevel.Snapshot;

        private static readonly object Locker = new object();

        static QueueManager()
        {
            RotationTimer.Interval = RotationTimeLimitParam * 1000;
            RotationTimer.Elapsed += (sender, args) => { RotateTran(); };
        }

        /// <summary>Включает элемент в очередь, транзакцией</summary>
        /// <param name="entity">Элемент</param>
        /// <returns>Успех операции</returns>
        internal static bool EnQueueTran(Model.IQueue entity)
        {
            lock (Locker)
            {
                if (entity == null) return false;

                using (Repository repository = new Repository())
                {
                    using (var tran = repository.Database.BeginTransaction(TranIsolationLevel))
                    {
                        // проверка на дубли:
                        if (repository.Queue.Find(entity.ParkingCard) != null)
                            return false;

                        // добавление элемента в очередь:
                        repository.Queue.Add(new Queue()
                                             {
                                                 Input = DateTime.Now,
                                                 ParkingCard = entity.ParkingCard
                                             });
                        repository.SaveChanges();

                        // отправка всех изменений модели в БД:
                        tran.Commit();
                    }
                }

                Model.Notifier.OnQueueChanged(new QueueChangedEventArgs()
                {
                    Reason = QueueChangedEnum.EnQueue,
                    Queue = new Model.Queue().Get(ViewLimitParam)
                });

                return true;
            }
        }

        /// <summary>Исключает элемент из очереди, транзакцией</summary>
        /// <param name="entity">Элемент</param>
        /// <param name="reason">Причина исключения из очереди</param>
        /// <param name="extRepository">Экземпляр репозитория при внешней транзакции</param>
        /// <param name="extTransaction">Экземпляр транзакции при внешней транзакции</param>
        /// <returns>Успех операции</returns>
        internal static bool DeQueueTran(Model.IQueue entity, OutputReasonEnum reason,
            Repository extRepository = null, DbContextTransaction extTransaction = null)
        {
            lock (Locker)
            {
                if (entity == null) return false;

                using (Repository repository = extRepository ?? new Repository())
                {
                    using (var tran = extTransaction ?? repository.Database.BeginTransaction(TranIsolationLevel))
                    {
                        // необходимо убедиться, что элемент существует в текущей транзакции:
                        var entityTran = repository.Queue.Find(entity.ParkingCard);
                        if (entityTran == null)
                            return false;

                        var outputReason = repository.OutputReason
                            .First(t => t.Code == reason.ToString());

                        // копирование элемента в историю:
                        if (repository.QueueHistory.Find(entity.Input, entity.ParkingCard) == null)
                        {
                            repository.QueueHistory.Add(new QueueHistory()
                            {
                                Input = entity.Input,
                                Output = DateTime.Now,
                                ParkingCard = entity.ParkingCard,
                                Rotation = entity.Rotation,
                                RotationStart = entity.RotationStart,
                                Position = entity.Position,
                                OutputReason = outputReason
                            });
                        }

                        // удаление элемента из очереди:
                        repository.Queue.Remove(entityTran);

                        // сохранение всех изменений модели:
                        repository.SaveChanges();

                        // пересчёт фиксированных позиций:
                        switch (reason)
                        {
                            case OutputReasonEnum.Rotation:
                                break;

                            case OutputReasonEnum.Peek:
                                FixedPositionsMoveUp(repository);
                                break;

                            case OutputReasonEnum.Vip:
                            case OutputReasonEnum.Time:
                                if (entity.Position <= RotationPlaceParam)
                                {
                                    FixedPositionsMoveUp(repository);
                                }
                                break;
                        }

                        // отправка всех изменений модели в БД:
                        tran.Commit();
                    }
                }

                Model.Notifier.OnQueueChanged(new QueueChangedEventArgs()
                {
                    Reason = QueueChangedEnum.DeQueue,
                    Queue = new Model.Queue().Get(ViewLimitParam)
                });

                return true;
            }
        }

        /// <summary>
        /// Логика ротации (состояние: количество ротаций и фиксированная позиция). 
        /// Данное состояние хранится в БД.
        /// </summary>
        internal static bool RotateTran(bool once = false)
        {
            lock (Locker)
            {
                // остановка таймера:
                ResetTimer(started: false);

                using (Repository repository = new Repository())
                {
                    using (var tran = repository.Database.BeginTransaction(TranIsolationLevel))
                    {
                        // проверка наличия элементов в текущей транзакции:
                        if (!repository.Queue.Any())
                            return false;

                        // извлечение первого элемента:
                        // ToDo: не в рамках текущей транзакции!!!
                        var id = new Model.Queue().Get(1).First().ParkingCard;
                        var firstEntity = repository.Queue.First(t => t.ParkingCard == id);

                        // увеличение счётчика ротаций:
                        firstEntity.Rotation = 
                            (byte?) (firstEntity.Rotation.HasValue 
                                ? firstEntity.Rotation + 1 
                                : 1);

                        // отметка времени первой ротации:
                        if (firstEntity.RotationStart == null)
                            firstEntity.RotationStart = DateTime.Now;

                        repository.SaveChanges();

                        // пересчёт фиксированных позиций:
                        FixedPositionsMoveUp(repository);

                        // присвоение принудительной позиции:
                        firstEntity.Position = RotationPlaceParam;
                        repository.SaveChanges();

                        if (firstEntity.Rotation >= RotationCountLimitParam)
                        {
                            DeQueueTran(firstEntity, OutputReasonEnum.Rotation, repository, tran);
                        }
                        else
                        {
                            tran.Commit();

                            Model.Notifier.OnQueueChanged(new QueueChangedEventArgs()
                            {
                                Reason = QueueChangedEnum.Rotation,
                                Queue = new Model.Queue().Get(ViewLimitParam)
                            });

                        }
                    }
                }

                // запуск таймера:
                if (!once) ResetTimer(started: true);

                return true;
            }
        }

        /// <summary>
        /// Пересчёт очереди (состояние: позиция и время ожидания)
        /// Данное состояние не хранится в БД.
        /// </summary>
        /// <param name="source">Извлечённая из БД очередь для пересчёта</param>
        /// <param name="forceOutput">Принудительная установка времени ожидания первому клиенту</param>
        /// <returns>Пересчитанный объект очереди в памяти</returns>
        internal static IEnumerable<Model.IQueue> Recalculate(IEnumerable<Model.IQueue> source, DateTime? forceOutput = null)
        {
            IEnumerable<Model.IQueue> result = new List<IQueue>();

            // фиксированные позиции:
            var fixedPositions = source.Cast<Queue>().Where(t => t.Position.HasValue).OrderByDescending(t => t.Position).ToList();

            if (fixedPositions.Count == 0)
            {
                result = SortAndCalculatePositions(source, SortPositionsEnum.Input, forceOutput);
            }
            else
            {
                result = SortAndCalculatePositions(source, SortPositionsEnum.Input, forceOutput);

                foreach (var fixedPosition in fixedPositions)
                {
                    // обычные позиции смещаются вверх, уступая место принудительной:
                    foreach (var movingPosition in result
                        .Except(fixedPositions)
                        .Where(t => t.Position <= fixedPosition.Position)
                        .OrderBy(t => t.Position))
                    {
                        movingPosition.Position--;
                    }

                    // в освободившееся место помещается принудительная позиция:
                    var resultPosition = result.First(t => t.ParkingCard == fixedPosition.ParkingCard);
                    resultPosition.Position = fixedPosition.Position;
                }

                // пересчёт времени с учётом фиксированных позиций:
                result = SortAndCalculatePositions(result, SortPositionsEnum.Position, forceOutput);
            }

            return result.OrderBy(t => t.Position).ToList();
        }

        /// <summary>Сортирует и вычисляет позиции</summary>
        /// <param name="queue">Извлечённая из БД очередь для пересчёта</param>
        /// <param name="sortBy">Применяемая сортировка</param>
        /// <param name="forceOutput">Принудительная установка времени ожидания первому клиенту</param>
        /// <returns></returns>
        private static IEnumerable<Model.IQueue> SortAndCalculatePositions(
            IEnumerable<Model.IQueue> queue, SortPositionsEnum sortBy, DateTime? forceOutput = null)
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, int.Parse(now.Millisecond.ToString(new string('0', Repository.EntityDatetimePrecision))));

            var result = new List<Model.IQueue>();

            if (sortBy == SortPositionsEnum.Input)
            {
                result = queue.OrderBy(t => t.Input).Select((t, index) => new Model.Queue()
                {
                    Position = index + 1,
                    Output = forceOutput? // расчётное время первой позиции равно текущему
                        .AddSeconds(OutputForecastParam*(index)) 
                        ?? now.AddSeconds(OutputForecastParam*(index)),
                    Input = t.Input,
                    ParkingCard = t.ParkingCard,
                    Rotation = t.Rotation,
                    RotationStart = t.RotationStart
                }).Cast<Model.IQueue>().ToList();
            }

            if (sortBy == SortPositionsEnum.Position)
            {
                result = queue.OrderBy(t => t.Position).Select((t, index) => new Model.Queue()
                {
                    Position = t.Position,
                    Output = forceOutput? // расчётное время первой позиции равно текущему
                        .AddSeconds(OutputForecastParam*(index)) 
                        ?? now.AddSeconds(OutputForecastParam*(index)),
                    Input = t.Input,
                    ParkingCard = t.ParkingCard,
                    Rotation = t.Rotation
                }).Cast<Model.IQueue>().ToList();
            }

            return result;
        }

        /// <summary>На основе какой сортировки вычисляется очередь</summary>
        private enum SortPositionsEnum : byte
        {
            /// <summary>Сортировка по времени (вычисляется позиция)</summary>
            Input,

            /// <summary>Сортировка по позиции (вычисляется время)</summary>
            Position
        }

        /// <summary>Пересчёт фиксированных позиций (смещение их вверх в очереди)</summary>
        /// <param name="repository">Ссылка на объект репозитория</param>
        private static void FixedPositionsMoveUp(Repository repository)
        {
            // пересчёт фиксированных позиций:
            foreach (var position in repository.Queue.Where(t => t.Position.HasValue))
            {
                position.Position--;
            }
            repository.SaveChanges();
        }

        /// <summary>Перезапуск или остановка таймера</summary>
        internal static bool ResetTimer(bool started)
        {
            if (started)
            {
                RotationTimer.Enabled = true;
                RotationTimer.Start();
            }
            else
            {
                RotationTimer.Enabled = false;
                RotationTimer.Stop();
            }
            return RotationTimer.Enabled;
        }
    }
}
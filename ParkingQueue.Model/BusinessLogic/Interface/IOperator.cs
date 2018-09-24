namespace ParkingQueue.Model
{
    /// <summary>Оператор (пользователь системы)</summary>
    public interface IOperator
    {
        bool IsAdmin { get; set; }
        string Login { get; set; }
        string Name { get; set; }
    }
}
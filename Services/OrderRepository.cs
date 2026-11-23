using DeliveryApi.Models;

namespace DeliveryApi.Services;

/// <summary>Список заказов (в памяти).</summary>
public class OrderRepository
{
    private readonly List<Order> _orders = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public Order Add(string customerName, DeliveryResult result)
    {
        lock (_lock)
        {
            var order = new Order
            {
                Id = _nextId++,
                CustomerName = customerName.Trim(),
                Status = "Создан",
                CreatedAt = DateTime.UtcNow,
                Result = result
            };
            _orders.Add(order);
            return order;
        }
    }

    public IReadOnlyList<Order> GetAll()
    {
        lock (_lock)
        {
            return _orders.ToList();
        }
    }

    public Order? GetById(int id)
    {
        lock (_lock)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }
    }
}

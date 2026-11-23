using DeliveryApi.Models;

namespace DeliveryApi.Services;

/// <summary>История расчётов (в памяти).</summary>
public class HistoryStorage
{
    private readonly List<DeliveryResult> _records = new();
    private readonly object _lock = new();

    public void Add(DeliveryResult result)
    {
        lock (_lock)
        {
            _records.Add(result);
        }
    }

    public IReadOnlyList<DeliveryResult> GetAll()
    {
        lock (_lock)
        {
            return _records.ToList();
        }
    }
}

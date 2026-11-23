using DeliveryApi.Models;

namespace DeliveryApi.Services;

/// <summary>Справочник городов с координатами (хранится в коде).</summary>
public class CityService
{
    private readonly List<City> _cities = new()
    {
        new City("Москва", 55.7558, 37.6173),
        new City("Санкт-Петербург", 59.9343, 30.3351),
        new City("Нижний Новгород", 56.2965, 43.9361),
        new City("Казань", 55.7961, 49.1064),
        new City("Екатеринбург", 56.8389, 60.6057),
        new City("Новосибирск", 55.0084, 82.9357),
        new City("Воронеж", 51.6720, 39.1843),
        new City("Самара", 53.1959, 50.1002),
        new City("Ростов-на-Дону", 47.2357, 39.7015),
        new City("Краснодар", 45.0355, 38.9753)
    };

    /// <summary>Возвращает весь справочник.</summary>
    public IReadOnlyList<City> GetAll() => _cities;

    /// <summary>Ищет город по названию без учёта регистра.</summary>
    public City? Find(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return _cities.FirstOrDefault(c => c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

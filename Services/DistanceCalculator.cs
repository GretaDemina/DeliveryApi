using DeliveryApi.Models;
using Microsoft.Extensions.Options;

namespace DeliveryApi.Services;

/// <summary>Расчёт расстояния между городами по координатам.</summary>
public class DistanceCalculator
{
    private const double EarthRadiusKm = 6371.0;
    private readonly TariffSettings _tariff;

    public DistanceCalculator(IOptions<TariffSettings> tariff)
    {
        _tariff = tariff.Value;
    }

    /// <summary>Расстояние по прямой (формула гаверсинусов), км.</summary>
    public double Haversine(City a, City b)
    {
        double lat1 = ToRadians(a.Latitude);
        double lat2 = ToRadians(b.Latitude);
        double dLat = lat2 - lat1;
        double dLon = ToRadians(b.Longitude - a.Longitude);

        double h = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                 + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        return 2 * EarthRadiusKm * Math.Asin(Math.Sqrt(h));
    }

    /// <summary>Расстояние по дорогам: прямая × дорожный коэффициент, округлённое до километра.</summary>
    public int ToRoadDistance(double straightKm)
    {
        return (int)Math.Round(straightKm * _tariff.RoadCoefficient, MidpointRounding.AwayFromZero);
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}

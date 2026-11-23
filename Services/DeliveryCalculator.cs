using DeliveryApi.Models;
using Microsoft.Extensions.Options;

namespace DeliveryApi.Services;

/// <summary>
/// Расчёт стоимости и срока доставки.
/// стоимость = (A + B + C) × K_тип × (1 − скидка) × (1 + страховка)
/// A = базовая плата; B = расчётный вес × плата за кг; C = расчётный вес × расстояние × ставка за кг·км.
/// </summary>
public class DeliveryCalculator
{
    private readonly TariffSettings _tariff;
    private readonly CityService _cities;
    private readonly DistanceCalculator _distance;

    public DeliveryCalculator(IOptions<TariffSettings> tariff, CityService cities, DistanceCalculator distance)
    {
        _tariff = tariff.Value;
        _cities = cities;
        _distance = distance;
    }

    /// <summary>Выполняет расчёт. При неверных городах выбрасывает ArgumentException.</summary>
    public DeliveryResult Calculate(DeliveryRequest request)
    {
        var origin = _cities.Find(request.FromCity)
                   ?? throw new ArgumentException($"Город отправления «{request.FromCity}» не найден в справочнике");
        var destination = _cities.Find(request.ToCity)
                 ?? throw new ArgumentException($"Город назначения «{request.ToCity}» не найден в справочнике");

        if (origin.Name == destination.Name)
        {
            throw new ArgumentException("Города отправления и назначения должны различаться");
        }

        // 1. Расстояние
        double straightKm = _distance.Haversine(origin, destination);
        int distanceKm = _distance.ToRoadDistance(straightKm);

        // 2. Расчётный вес: максимум из фактического и объёмного
        double volumetric = 0;
        if (request.Length > 0 && request.Width > 0 && request.Height > 0)
        {
            volumetric = request.Length * request.Width * request.Height / _tariff.VolumetricDivisor;
        }
        decimal chargeableWeight = (decimal)Math.Round(Math.Max(request.Weight, volumetric), 2);

        // 3. Статьи стоимости
        decimal baseFee = _tariff.BaseFee;
        decimal weightFee = Math.Round(chargeableWeight * _tariff.HandlingPerKg, 2);
        decimal distanceFee = Math.Round(chargeableWeight * distanceKm * _tariff.RatePerKgKm, 2);
        decimal coefficient = request.Type == DeliveryType.Express ? _tariff.ExpressCoef : 1m;

        decimal subtotal = (baseFee + weightFee + distanceFee) * coefficient;

        // 4. Скидка за большой вес и страховка
        decimal discount = request.Weight > _tariff.DiscountWeightKg
            ? Math.Round(subtotal * _tariff.DiscountPercent / 100m, 2)
            : 0m;
        decimal afterDiscount = subtotal - discount;
        decimal insuranceCost = request.Insurance
            ? Math.Round(afterDiscount * _tariff.InsurancePercent / 100m, 2)
            : 0m;

        decimal cost = Math.Round(afterDiscount + insuranceCost, 0, MidpointRounding.AwayFromZero);

        // 5. Срок доставки
        int days = request.Type == DeliveryType.Express
            ? Math.Max(1, (int)Math.Ceiling(distanceKm / (double)_tariff.ExpressKmPerDay))
            : 1 + (int)Math.Ceiling(distanceKm / (double)_tariff.StandardKmPerDay);

        return new DeliveryResult
        {
            FromCity = origin.Name,
            ToCity = destination.Name,
            StraightKm = Math.Round(straightKm, 1),
            DistanceKm = distanceKm,
            ChargeableWeight = chargeableWeight,
            BaseFee = baseFee,
            WeightFee = weightFee,
            DistanceFee = distanceFee,
            TypeCoefficient = coefficient,
            Discount = discount,
            InsuranceCost = insuranceCost,
            Cost = cost,
            Days = days,
            Type = request.Type,
            Currency = "RUB"
        };
    }
}

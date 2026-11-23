namespace DeliveryApi.Models;

/// <summary>Тарифные ставки. Значения по умолчанию совпадают с практической работой №1.</summary>
public class TariffSettings
{
    /// <summary>Базовая плата за посылку, руб.</summary>
    public decimal BaseFee { get; set; } = 250m;

    /// <summary>Плата за работу с грузом, руб. за 1 кг.</summary>
    public decimal HandlingPerKg { get; set; } = 15m;

    /// <summary>Ставка перевозки, руб. за 1 кг на 1 км.</summary>
    public decimal RatePerKgKm { get; set; } = 0.02m;

    /// <summary>Коэффициент экспресс-доставки.</summary>
    public decimal ExpressCoef { get; set; } = 1.5m;

    /// <summary>Вес (кг), начиная с которого действует скидка.</summary>
    public double DiscountWeightKg { get; set; } = 50;

    /// <summary>Размер скидки за большой вес, %.</summary>
    public decimal DiscountPercent { get; set; } = 10m;

    /// <summary>Страховой сбор, % от стоимости.</summary>
    public decimal InsurancePercent { get; set; } = 2m;

    /// <summary>Делитель для объёмного веса (Д × Ш × В в см ÷ делитель).</summary>
    public double VolumetricDivisor { get; set; } = 5000;

    /// <summary>Дорожный коэффициент: расстояние по дорогам / расстояние по прямой.</summary>
    public double RoadCoefficient { get; set; } = 1.15;

    /// <summary>Км в сутки для стандартной доставки.</summary>
    public int StandardKmPerDay { get; set; } = 500;

    /// <summary>Км в сутки для экспресс-доставки.</summary>
    public int ExpressKmPerDay { get; set; } = 1000;
}

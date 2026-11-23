namespace DeliveryApi.Models;

/// <summary>Результат расчёта доставки с разбивкой стоимости.</summary>
public class DeliveryResult
{
    public string FromCity { get; set; } = string.Empty;
    public string ToCity { get; set; } = string.Empty;

    /// <summary>Расстояние по прямой, км.</summary>
    public double StraightKm { get; set; }

    /// <summary>Расстояние по дорогам, км.</summary>
    public int DistanceKm { get; set; }

    /// <summary>Расчётный вес (максимум из фактического и объёмного), кг.</summary>
    public decimal ChargeableWeight { get; set; }

    /// <summary>A. Базовая плата, руб.</summary>
    public decimal BaseFee { get; set; }

    /// <summary>B. Работа с грузом, руб.</summary>
    public decimal WeightFee { get; set; }

    /// <summary>C. Перевозка, руб.</summary>
    public decimal DistanceFee { get; set; }

    /// <summary>Коэффициент типа доставки.</summary>
    public decimal TypeCoefficient { get; set; }

    /// <summary>Скидка за большой вес, руб.</summary>
    public decimal Discount { get; set; }

    /// <summary>Стоимость страховки, руб.</summary>
    public decimal InsuranceCost { get; set; }

    /// <summary>Итоговая стоимость, руб.</summary>
    public decimal Cost { get; set; }

    /// <summary>Срок доставки, дней.</summary>
    public int Days { get; set; }

    public DeliveryType Type { get; set; }

    public string Currency { get; set; } = "RUB";

    /// <summary>Сумма в выбранной валюте (если конвертация выполнена).</summary>
    public decimal? ConvertedCost { get; set; }

    public string? ConvertedCurrency { get; set; }

    /// <summary>Предупреждение, например при недоступности сервиса конвертации.</summary>
    public string? Warning { get; set; }

    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}

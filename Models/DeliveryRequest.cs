using System.ComponentModel.DataAnnotations;

namespace DeliveryApi.Models;

/// <summary>Запрос на расчёт доставки.</summary>
public class DeliveryRequest
{
    /// <summary>Город отправления (из справочника городов).</summary>
    [Required(ErrorMessage = "Укажите город отправления")]
    public string FromCity { get; set; } = string.Empty;

    /// <summary>Город назначения (из справочника городов).</summary>
    [Required(ErrorMessage = "Укажите город назначения")]
    public string ToCity { get; set; } = string.Empty;

    /// <summary>Фактический вес посылки, кг.</summary>
    [Range(0.01, 10000, ErrorMessage = "Вес должен быть больше нуля")]
    public double Weight { get; set; }

    /// <summary>Длина, см (необязательно).</summary>
    [Range(0, 1000, ErrorMessage = "Длина должна быть от 0 до 1000 см")]
    public double Length { get; set; }

    /// <summary>Ширина, см (необязательно).</summary>
    [Range(0, 1000, ErrorMessage = "Ширина должна быть от 0 до 1000 см")]
    public double Width { get; set; }

    /// <summary>Высота, см (необязательно).</summary>
    [Range(0, 1000, ErrorMessage = "Высота должна быть от 0 до 1000 см")]
    public double Height { get; set; }

    /// <summary>Тип доставки: standard или express.</summary>
    public DeliveryType Type { get; set; } = DeliveryType.Standard;

    /// <summary>Нужна ли страховка.</summary>
    public bool Insurance { get; set; }

    /// <summary>Валюта для пересчёта (USD, EUR). Если не указана, возвращается только сумма в рублях.</summary>
    public string? Currency { get; set; }
}

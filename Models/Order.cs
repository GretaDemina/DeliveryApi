using System.ComponentModel.DataAnnotations;

namespace DeliveryApi.Models;

/// <summary>Запрос на оформление заказа: данные расчёта и имя клиента.</summary>
public class CreateOrderRequest : DeliveryRequest
{
    [Required(ErrorMessage = "Укажите имя клиента")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
    public string CustomerName { get; set; } = string.Empty;
}

/// <summary>Оформленный заказ.</summary>
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Status { get; set; } = "Создан";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DeliveryResult Result { get; set; } = new();
}

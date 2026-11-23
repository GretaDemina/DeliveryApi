using System.Globalization;
using System.Text.Json;

namespace DeliveryApi.Services;

/// <summary>
/// Клиент внешнего API конвертации валют.
/// 1) Если в настройках указан CurrencyApi:ClassmateUrl, запрос уходит в API одногруппника:
///    GET {ClassmateUrl}?amount=1700&amp;from=RUB&amp;to=USD  →  { "result": 18.5 }
///    (формат нужно согласовать с одногруппником и при необходимости поправить метод ConvertViaClassmateAsync).
/// 2) Если API одногруппника не задан или недоступен, используется открытое API ЦБ РФ.
/// </summary>
public class CurrencyConverterClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly ILogger<CurrencyConverterClient> _logger;

    public CurrencyConverterClient(HttpClient http, IConfiguration config, ILogger<CurrencyConverterClient> logger)
    {
        _http = http;
        _config = config;
        _logger = logger;
    }

    /// <summary>Переводит сумму в рублях в указанную валюту. При ошибке выбрасывает исключение.</summary>
    public async Task<decimal> ConvertAsync(decimal amountRub, string currency, CancellationToken ct = default)
    {
        currency = currency.Trim().ToUpperInvariant();

        var classmateUrl = _config["CurrencyApi:ClassmateUrl"];
        if (!string.IsNullOrWhiteSpace(classmateUrl))
        {
            try
            {
                return await ConvertViaClassmateAsync(classmateUrl, amountRub, currency, ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "API одногруппника недоступно, используется API ЦБ РФ");
            }
        }

        return await ConvertViaCbrAsync(amountRub, currency, ct);
    }

    private async Task<decimal> ConvertViaClassmateAsync(string baseUrl, decimal amountRub, string currency, CancellationToken ct)
    {
        var amount = amountRub.ToString(CultureInfo.InvariantCulture);
        var url = $"{baseUrl}?amount={amount}&from=RUB&to={currency}";

        using var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("result").GetDecimal();
    }

    private async Task<decimal> ConvertViaCbrAsync(decimal amountRub, string currency, CancellationToken ct)
    {
        var url = _config["CurrencyApi:CbrUrl"] ?? "https://www.cbr-xml-daily.ru/daily_json.js";

        using var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.GetProperty("Valute").TryGetProperty(currency, out var valute))
        {
            throw new InvalidOperationException($"Валюта {currency} не поддерживается");
        }

        decimal value = valute.GetProperty("Value").GetDecimal();
        int nominal = valute.GetProperty("Nominal").GetInt32();
        decimal rubPerUnit = value / nominal;

        return Math.Round(amountRub / rubPerUnit, 2);
    }
}

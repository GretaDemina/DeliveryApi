# DeliveryApi — документация веб-сервиса

**Практическая работа №5. Документирование веб-сервиса.**

- **Работающий API:** https://delivery-api-gusanova.msk1.amvera.tech
- **Swagger UI:** https://delivery-api-gusanova.msk1.amvera.tech/swagger/index.html
- **OpenAPI JSON:** https://delivery-api-gusanova.msk1.amvera.tech/swagger/v1/swagger.json
- **Репозиторий:** https://github.com/GretaDemina/DeliveryApi

## 1. Назначение и пользователи

DeliveryApi — учебный веб-сервис для расчёта ориентировочной стоимости и срока доставки посылок между городами России, оформления заказов и просмотра истории расчётов. Пользователи — отправители посылок, специалисты службы доставки и разработчики клиентских приложений.

## 2. Технологии и архитектура

C#, ASP.NET Core 8 Web API, контроллеры, Swashbuckle.AspNetCore 6.6.2 (Swagger/OpenAPI), HttpClient, JSON, IOptions и appsettings.json. Docker и Amvera для развертывания; GitHub для хранения кода. База данных **не используется**: история и заказы хранятся в оперативной памяти процесса и могут пропасть при перезапуске. Авторизация отсутствует: не используйте настоящие персональные данные. Для валютного пересчёта вызывается внешний сервис; при его недоступности возвращаются рубли и warning.

## 3. Маршруты API

| Метод | Путь | Данные запроса | Ответ |
|---|---|---|---|
| `GET` | `/api/delivery/cities` | Без параметров | 200, JSON-массив городов |
| `GET` | `/api/delivery/tariffs` | Без параметров | 200, JSON с тарифными настройками |
| `GET` | `/api/delivery/distance` | Query: from, to (названия городов) | 200, JSON с straightKm и distanceKm; 404 |
| `POST` | `/api/delivery/calculate` | JSON: fromCity, toCity, weight, length, width, height, type, insurance, currency | 200, JSON с cost, days и детализацией; 400 |
| `GET` | `/api/delivery/history` | Без параметров | 200, JSON-массив расчётов |
| `POST` | `/api/orders` | JSON: customerName и поля расчёта | 201, JSON заказа; 400 |
| `GET` | `/api/orders` | Без параметров | 200, JSON-массив заказов |
| `GET` | `/api/orders/{id}` | Path: id (целое число) | 200, JSON заказа; 404 |

**Параметры расчёта:** `fromCity` и `toCity` — обязательные названия городов; `weight` — вес в кг (положительное число); `length`, `width`, `height` — габариты в см (необязательные, по умолчанию 0); `type` — `standard` или `express`; `insurance` — true/false; `currency` — необязательная строка, например `USD`.

### Пример расчёта

```http
POST /api/delivery/calculate
Content-Type: application/json
```

```json
{
  "fromCity": "Москва",
  "toCity": "Нижний Новгород",
  "weight": 10,
  "length": 40,
  "width": 30,
  "height": 20,
  "type": "express",
  "insurance": false
}
```

Ожидаемый основной результат: `HTTP 200`, `distanceKm: 457`, `cost: 737`, `days: 1`, `type: "express"`. Ответ содержит дополнительные поля (`baseFee`, `weightFee`, `distanceFee`, `discount`, `insuranceCost` и др.).

### Пример ошибки

`GET /api/delivery/distance?from=Москва&to=НесуществующийГород` → `HTTP 404`:

```json
{"error":"Один из городов не найден в справочнике"}
```

`POST /api/orders` с валидными данными и `customerName` → `HTTP 201 Created`, возвращает `id`, `status`, `createdAt`, `result`.

## 4. Коды ответов

- **200 OK** — успешный запрос;
- **201 Created** — заказ создан;
- **400 Bad Request** — ошибка валидации или бизнес-правил (например, отрицательный вес или одинаковые города);
- **404 Not Found** — город или заказ не найден;
- **405 Method Not Allowed** — неподдерживаемый метод для маршрута.

## 5. Запуск локально

Требуется .NET SDK 8. Из корня проекта:

```bash
dotnet restore
dotnet run --urls http://localhost:8080
```

Открыть `http://localhost:8080/swagger/index.html`. Для просмотра спецификации — `http://localhost:8080/swagger/v1/swagger.json`.

## 6. Развёртывание в Amvera

1. Загрузить файлы проекта в GitHub; в корне должны быть `Dockerfile`, `DeliveryApi.csproj`, `Program.cs`, `amvera.yml`.
2. В Amvera создать или открыть приложение `delivery-api`, выбрать сборку Docker, путь `Dockerfile`, порт контейнера `8080`.
3. Загрузить обновлённый код в привязанный репозиторий приложения или выполнить доступную в интерфейсе синхронизацию с GitHub; дождаться **успешной сборки** и запуска.
4. Проверить домен и открыть публичный Swagger UI по ссылке выше.

При изменении кода требуется **новая сборка / развертывание**. Локальный README сам по себе не появляется на GitHub, пока его не загрузят в репозиторий.

## 7. Документирование с помощью Swagger

В `Program.cs` зарегистрированы `AddEndpointsApiExplorer()` и `AddSwaggerGen(...)`; вызваны `UseSwagger()` и `UseSwaggerUI()`. В `DeliveryApi.csproj` включена генерация XML-документации, а `IncludeXmlComments(...)` подключает её к Swagger. Описания `summary`, `remarks`, `param`, `response` находятся в комментариях контроллеров. Swagger UI показывает маршруты, HTTP-методы, схемы данных, поля и позволяет выполнять запросы через **Try it out**.

**Примечание:** расширенные XML-описания появятся на публичном Swagger только после загрузки этой версии проекта и повторного развёртывания. Само открытие Swagger подтверждено пользователем; актуальная публичная конфигурация до обновления может отличаться.

## 8. Ограничения и безопасность

Учебный сервис не имеет аутентификации, ограничений запросов и постоянной БД. Не вводите реальные персональные данные. Полученный ранее запрос валютной конвертации вернул `200` с `warning` из-за недоступного внешнего курса, время ответа составило 5,71 с.

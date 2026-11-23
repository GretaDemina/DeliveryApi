# DeliveryApi — практическая работа №4

ASP.NET Core 8, расчёт стоимости доставки, справочники, история, заказы.

## Локально

`dotnet run --urls http://localhost:8080`

Swagger: http://localhost:8080/swagger

## GitHub

Создайте пустой репозиторий DeliveryApi в своём аккаунте GitHub.
В терминале из папки проекта выполните:

```sh
git init
git add .
git commit -m "Delivery API practice 4"
git branch -M main
git remote add origin https://github.com/ВАШ_ЛОГИН/DeliveryApi.git
git push -u origin main
```

## Amvera

Создайте приложение, проверьте стоимость пробного тарифа и доступный баланс.
Загрузите содержимое этой папки в репозиторий приложения по инструкции Amvera.
Dockerfile собирает .NET 8, amvera.yml указывает порт 8080.
После запуска подключите бесплатный домен в настройках сети и откройте /swagger.

https://docs.amvera.ru/applications/configuration/docker.html
https://docs.amvera.ru/applications/quick-start.html
https://docs.amvera.ru/general/price.html

## Postman

Импортируйте DeliveryApi.postman_collection.json. В Variables установите baseUrl
равным адресу развернутого сервиса без завершающего слеша. Запустите коллекцию
в Runner последовательно. Переменная orderId заполняется при создании заказа.
Для публичного сервиса используйте только вымышленные данные: API не имеет
авторизации, заказы и история временно хранятся в памяти.

Снимите скриншоты запросов и ответов; сохраните результаты Runner и время ответа.

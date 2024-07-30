# Интернет-магазин

## Описание

Этот проект представляет собой REST API для управления интернет-магазином, включающий несколько микросервисов: Пользователи, Товары, Заказы, Платежи и API Gateway. Сервис поддерживает CRUD операции и предоставляет возможность интеграции с API ePayment.kz для обработки платежей.

## Цели

- Управление пользователями, товарами, заказами и платежами.
- Обеспечение единого входа через API Gateway.
- Интеграция с API ePayment.kz для обработки платежей.
- Контейнеризация и деплой с использованием Docker и Render.

## Микросервисы

### Пользователи

- **Идентификатор:** Уникальный идентификатор пользователя.
- **Имя:** Полное имя пользователя.
- **Email:** Электронная почта пользователя.
- **Адрес:** Адрес пользователя.
- **Дата регистрации:** Дата регистрации пользователя.
- **Роль:** Роль пользователя в системе (например, администратор, клиент).

### Товары

- **Идентификатор:** Уникальный идентификатор товара.
- **Название:** Название товара.
- **Описание:** Краткое описание товара.
- **Цена:** Цена товара.
- **Категория:** Категория товара.
- **Количество на складе:** Количество товара на складе.
- **Дата добавления:** Дата добавления товара.

### Заказы

- **Идентификатор:** Уникальный идентификатор заказа.
- **Пользователь:** Идентификатор пользователя, сделавшего заказ.
- **Товары:** Список идентификаторов товаров в заказе.
- **Общая стоимость:** Общая стоимость заказа.
- **Дата заказа:** Дата создания заказа.
- **Статус заказа:** Статус заказа (новый, в обработке, выполнен).

### Платежи

- **Идентификатор:** Уникальный идентификатор платежа.
- **Пользователь:** Идентификатор пользователя, сделавшего платеж.
- **Заказ:** Идентификатор заказа, за который произведен платеж.
- **Сумма:** Сумма платежа.
- **Дата платежа:** Дата совершения платежа.
- **Статус платежа:** Статус платежа (успешный, неуспешный).
- **Интеграция с ePayment.kz:** Реализован функционал оплаты через API ePayment.kz.

### API Gateway

- **Описание:** Обеспечивает единый вход для всех запросов и маршрутизацию к соответствующим микросервисам.
- **Логика маршрутизации:** Направляет запросы в зависимости от пути и метода HTTP к нужным микросервисам.

## Пути API

### /users

- `GET /users` - Получить список всех пользователей.
- `POST /users` - Создать нового пользователя.
- `GET /users/{id}` - Получить данные конкретного пользователя.
- `PUT /users/{id}` - Обновить данные конкретного пользователя.
- `DELETE /users/{id}` - Удалить конкретного пользователя.
- `GET /users/search?name={name}` - Найти пользователей по имени.
- `GET /users/search?email={email}` - Найти пользователей по электронной почте.

### /products

- `GET /products` - Получить список всех товаров.
- `POST /products` - Создать новый товар.
- `GET /products/{id}` - Получить данные конкретного товара.
- `PUT /products/{id}` - Обновить данные конкретного товара.
- `DELETE /products/{id}` - Удалить конкретный товар.
- `GET /products/search?name={name}` - Найти товары по названию.
- `GET /products/search?category={category}` - Найти товары по категории.

### /orders

- `GET /orders` - Получить список всех заказов.
- `POST /orders` - Создать новый заказ.
- `GET /orders/{id}` - Получить данные конкретного заказа.
- `PUT /orders/{id}` - Обновить данные конкретного заказа.
- `DELETE /orders/{id}` - Удалить конкретный заказ.
- `GET /orders/search?user={userId}` - Найти заказы по идентификатору пользователя.
- `GET /orders/search?status={status}` - Найти заказы по статусу.

### /payments

- `GET /payments` - Получить список всех платежей.
- `POST /payments` - Создать новый платеж, используя API ePayment.kz.
- `GET /payments/{id}` - Получить данные конкретного платежа.
- `PUT /payments/{id}` - Обновить данные конкретного платежа.
- `DELETE /payments/{id}` - Удалить конкретный платеж.
- `GET /payments/search?user={userId}` - Найти платежи по идентификатору пользователя.
- `GET /payments/search?order={orderId}` - Найти платежи по идентификатору заказа.
- `GET /payments/search?status={status}` - Найти платежи по статусу.

## Технические требования

- **CRUD операции:** Реализованы для пользователей, товаров, заказов и платежей.
- **База данных:** PostgreSQL для хранения данных.
- **Контейнеризация:** Используются Docker и Docker Compose для контейнеризации микросервисов.
- **Автоматизация:** Makefile для автоматизации команд.
- **Тестирование:** Юнит-тесты для API каждого микросервиса.
- **Деплой:** Задеплоено на Render.

## Установка и запуск

1. **Клонируйте репозиторий:**

    ```bash
    git clone https://github.com/ваш_пользователь/интернет-магазин.git
    cd интернет-магазин
    ```

2. **Настройте Docker и Docker Compose:**

    ```bash
    docker-compose up --build
    ```

3. **Запустите Makefile для автоматизации:**

    ```bash
    make all
    ```

4. **Доступ к API:**

    - Пользователи: `http://localhost:8080/users`
    - Товары: `http://localhost:8080/products`
    - Заказы: `http://localhost:8080/orders`
    - Платежи: `http://localhost:8080/payments`

## Документация

- **Документация по API** доступна через Swagger UI по адресу: `http://localhost:8080/swagger-ui.html`.

## Развертывание на Render

1. **Создайте проект на Render и настройте автоматический деплой:**

    - Настройте GitHub интеграцию.
    - Укажите `docker-compose.yml` для развертывания.

2. **Деплой:**

    - Убедитесь, что проект успешно развернут и доступен по указанному URL.

## Интеграция с ePayment.kz

- Реализован функционал оплаты через API ePayment.kz в микросервисе "Payment".
- Подробности и настройка интеграции описаны в документации по API ePayment.kz.

# Spoleto.Marking.TsPiot

[![](https://img.shields.io/github/license/spoleto-software/Spoleto.Marking.TsPiot)](https://github.com/spoleto-software/Spoleto.Marking.TsPiot/blob/main/LICENSE)
[![](https://img.shields.io/nuget/v/Spoleto.Marking.TsPiot)](https://www.nuget.org/packages/Spoleto.Marking.TsPiot/)
![Build](https://github.com/spoleto-software/Spoleto.Marking.TsPiot/actions/workflows/ci.yml/badge.svg)

.NET 8 клиент для интеграции с Драйвером ТС ПИоТ (ЕСМ) через gRPC / REST.

---

## 📌 Описание

Пакет предназначен для интеграции программного обеспечения рабочего места кассира (ПМСР) с Драйвером ТС ПИоТ в рамках требований системы маркировки «Честный ЗНАК».

ТС ПИоТ выступает как единая точка входа для проверки кодов маркировки и берет на себя взаимодействие с ГИС МТ.

---

## 🎯 Назначение

- Проверка кодов маркировки
- Получение статуса товара
- Поддержка онлайн и офлайн режимов (ЛМ ЧЗ)
- Обработка ошибок и блокировка некорректных операций

---

## 🏗 Архитектура взаимодействия

### Общий сценарий

1. Пользователь сканирует код маркировки  
2. Код передаётся в ТС ПИоТ
3. ТС ПИоТ возвращает результат:
   - ✅ разрешить продажу
   - ❌ запретить продажу

---

## 🔌 Протокол взаимодействия

ТС ПИоТ предоставляет локальный API:

- gRPC
- REST

---

## 🚀 Установка

```bash
dotnet add package Spoleto.Marking.TsPiot
```

## ⚙️ Конфигурация

Пример appsettings.json:

```json
{
  "TsPiotClientOptions": {
    "ServiceUrl": "https://esm-emu.ao-esp.ru/",
    "RetryOptions": {
      "AttemptTimeoutSeconds": 30,
      "TotalTimeoutSeconds": 180,
      "BaseDelayMs": 500,
      "RetryCount": 3
    },
    "AppOptions": {
      "Name": "MyApp",
      "Version": "1.0",
      "Id": "my-app-id",
      "Token": "token"
    }
  }
}
```

---

## ⚙️ Инициализация

```csharp
var options = configuration
    .GetSection("TsPiotClientOptions")
    .Get<TsPiotClientOptions>();
```

## 🔄 Создание клиента

### REST
```csharp
ITsPiotClient client = new TsPiotRestClient(options);
```

### gRPC
```csharp
ITsPiotClient client = new TsPiotGrpcClient(options);
```


## ✅ Примеры использования

### Проверка кодов

```csharp
var codes = new[]
{
    "код1",
    "код2"
};

var result = await client.CheckCodesAsync(codes);

if (result != null)
{
    Console.WriteLine("Проверка выполнена");
}
```

### Проверка аварийного режима

```csharp
var result = await client.CheckCodesAsync(codes);

if (result.IsEmergencyMode)
{
    Console.WriteLine("Аварийный режим (код ответа 203)");
}
```

### Обработка ошибок

```csharp
try
{
    var result = await client.CheckCodesAsync(codes);
}
catch (TsPiotException ex)
{
    Console.WriteLine($"Ошибка проверки: {ex.Message}");
}
```

## 🧪 Примеры (как в тестах)

### Успешный сценарий

```csharp
var settings = ConfigurationHelper.GetOptions();
var client = new TsPiotRestClient(settings);

var codes = ConfigurationHelper.GetSuccessfulCodes();

var res = await client.CheckCodesAsync(codes);

Assert.That(res, Is.Not.Null);
```

### Аварийный режим (203)

```csharp
var settings = ConfigurationHelper.GetOptions();
var client = new TsPiotGrpcClient(settings);

var codes = ConfigurationHelper.GetSuccessfulCodes203();

var res = await client.CheckCodesAsync(codes);

Assert.That(res.IsEmergencyMode, Is.True);
```

### Ошибка проверки

```csharp
var settings = ConfigurationHelper.GetOptions();
var client = new TsPiotRestClient(settings);

var codes = ConfigurationHelper.GetUnsuccessfulCodes();

Assert.ThrowsAsync<TsPiotException>(async () =>
{
    await client.CheckCodesAsync(codes);
});
```

## ⚠️ Важно

- ТС ПИоТ должен быть доступен по ServiceUrl
- Клиент не взаимодействует напрямую с ГИС МТ
- Все проверки проходят через ТС ПИоТ
- Retry/Timeout настраиваются через RetryOptions

## 📄 Лицензия

MIT
# Кастом ТК302-ФБ (Custom TK302-FB Printer)

[Описание устройства на официальном сайте производителя](https://ladon.ru/kontrolno-kassovaya-tekhnika-kkt/kkt-custom-tk302-fb-ispolnenie-2/)

[Драйвер](https://ladon.ru/upload/iblock/fd6/custom_tk302_fb_driver_v.2.30.00.zip)


## Запросы

### Издать звуковой сигнал

`POST /api/beep`

### Получить статус ККТ

`GET /api/status`

### Открыть смену

`POST /api/shift/open`

### Закрыть кассовую смену

`POST /api/shift/close`

### Напечатать чек

`POST /api/print/receipt`

### Напечатать чек возврата

`POST /api/print/receipt/return`

### Напечатать слип-чек

`POST /api/print/slip`

### Напечатать билет

`POST /api/print/ticket`

### Напечатать x-отчет

`POST /api/print/report/x`


## Установка

Инструкция по установке на сервере Windows. Проверено на свежеустановленном Windows Server 2016 Standart (1607) x64. Параметрамы конфигурации: 2x2.2ГГц, 2Гб RAM, 30Гб HDD.

1. В Диспетчере Серверов добавить роль "Веб-сервер (IIS)" ([скриншот](Source/Images/s01.jpg)), при установке все настройки оставить по умолчанию.

2. Скачать и установить среду выполнения - [ASP.NET Core 3.1 Runtime](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-aspnetcore-3.1.21-windows-hosting-bundle-installer). После установки перезагрузите сервер.

3. Разместить папку с приложением по адресу `C:\inetpub\TK302FBPrinter`.

4. Настроить права досупа в папку для веб-сервера. Для этого открыть свойства папки в Проводнике, на вкладке "Безопастность" добавить пользователя `IIS_IUSRS` с правами на "Изменение", "Чтение и выполнение", "Список содержимого папки", "Чтение", "Запись" как показано на [скриншоте](Source/Images/s03.jpg).

5. В Диспетчере служб IIS остановить созданный по умолчанию веб-сайт "Default Web Site" (кнопкой "Остановить" на правой панели) и создать новый веб-сайт ([скриншот](Source/Images/s02.jpg)). При создании указать настройки, как показано на [скриншоте](Source/Images/s04.jpg). Запустить созданный веб-сайт.

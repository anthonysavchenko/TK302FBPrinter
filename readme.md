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

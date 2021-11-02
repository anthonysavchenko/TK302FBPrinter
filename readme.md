# Кастом ТК302-ФБ (Custom TK302-FB Printer)

[Описание устройства на официальном сайте производителя](https://ladon.ru/kontrolno-kassovaya-tekhnika-kkt/kkt-custom-tk302-fb-ispolnenie-2/)

[Драйвер](https://ladon.ru/upload/iblock/fd6/custom_tk302_fb_driver_v.2.30.00.zip)


## Запросы

### Издать звуковой сигнал

`POST /beep`

### Получить статус ККТ

`GET /status`

### Напечатать чек

`POST /print/check`

### Напечатать чек возврата

`POST /print/check/return`

### Напечатать слип-чек

`POST /print/slip`

### Напечатать билет

`POST /print/ticket`

### Напечатать x-отчет

`POST /print/report/x`

### Закрыть кассовую смену

`POST /shift/close`

using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class PrinterStatusDto
    {
        // Ошибка принтера
        [Required]
        public bool PrinterError { get; set; }

        // Крышка открыта
        [Required]
        public bool CoverOpen { get; set; }
        
        // Наличие бумаги
        [Required]
        public bool PaperPresent { get; set; }

        // Бумага скоро закончится
        [Required]
        public bool PaperNearEnd { get; set; }

        // Ошибка отрезчика
        [Required]
        public bool CutterError { get; set; }

        // Смена открыта
        [Required]
        public bool ShiftOpen { get; set; }

        // Дата не установлена
        [Required]
        public bool DateNotSet { get; set; }

        // Печать
        [Required]
        public bool Printing { get; set; }

        // Требуется перезагрузка
        [Required]
        public bool ResetNeeded { get; set; }

        // Замятие
        [Required]
        public bool PaperJam { get; set; }

        // Режим ожидания
        [Required]
        public bool PrinterIdle { get; set; }

        // Ожидание обновления ПО
        [Required]
        public bool FWUpadteWaiting { get; set; }

        // Чек в принтере       
        [Required]
        public bool TicketOut { get; set; }

        // Виртуальный NPE
        [Required]
        public bool VirtualPaperNearEnd { get; set; }

        // Режим инициализации
        [Required]
        public bool HWInitJumperOn { get; set; }

        // Регистрация
        [Required]
        public bool Serialized { get; set; }
    }

    public class PrinterStatusResultDto : ExecutionResultDto
    {
        public PrinterStatusResultDto(string errorDescription = null, PrinterStatusDto status = null)
            : base(errorDescription)
        {
            if (!string.IsNullOrEmpty(errorDescription))
            {
                PrinterStatus = null;
                return;
            }
            PrinterStatus = status;
        }

        public PrinterStatusDto PrinterStatus { get; set; }
    }
}

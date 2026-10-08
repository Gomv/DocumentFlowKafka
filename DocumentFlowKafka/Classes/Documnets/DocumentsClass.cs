using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    public enum DocumentFlowStage
    {
        НеСоздан,
        Создан,
        ПодготовленКОтправке,
        Отправлен,
        ПринятОператором,
        ПереданПолучателю,
        ПолученПолучателем,
        ПодтвержденПолучателем,
        ТребуетУточнения,
        Аннулирован,
        Отклонен,
        Завершен,
        Ошибка
    }

    /// <summary>
    /// Типы документов согласно XSD-схеме Приложения 1 РОСЭУ.
    /// </summary>
    public enum DocumentType
    {
        // Неизвестный тип
        Неизвестный,

        // Основные типы
        Неформализованный,
        ОтказПодписи,

        // Товарные документы
        ТН,                    // Товарная накладная
        ТТН,                   // Товарно-транспортная накладная
        Торг12ТитулПродавца,
        Торг12ТитулПокупателя,
        ДокументОПередачеТоваровПриТорговыхОперацияхПродавец,
        ДокументОПередачеТоваровПриТорговыхОперацияхПокупатель,

        // Акты
        Акт,
        АктСверки,
        АктТитулЗаказчика,
        АктТитулИсполнителя,
        ДокументОПередачеРезультатовРаботИсполнитель,
        ДокументОПередачеРезультатовРаботЗаказчик,

        // Финансовые документы
        ПлатПоруч,            // Платёжное поручение
        Договор,
        Заказ,
        Счет,
        Ведомость,

        // Счета-фактуры
        СЧФ,                  // Счёт-фактура
        СЧФДОППродавец,
        СЧФДОППокупатель,
        ДОППродавец,
        ДОППокупатель,
        ДОППродавецНакладная,
        ДОППродавецАкт,
        ДОППокупательНакладная,
        ДОППокупательАкт,

        // Корректировочные документы
        КорСЧФ,               // Корректировочный счёт-фактура
        КорСЧФДИСПродавец,
        КорСЧФДИСПокупатель,
        КорДИСПродавец,
        КорДИСПокупатель,
        КорДИСПродавецНакладная,
        КорДИСПродавецАкт,
        КорДИСПокупательНакладная,
        КорДИСПокупательАкт,

        // Строительные документы
        КС11,                 // КС-11
        КС2,                  // КС-2
        КС3,                  // КС-3

        // Технологические документы обмена
        ИзвещениеПолучении,   // ИОП
        УведомлениеОбУточнении, // УОУ
        ПредложениеОбАннулировании, // ПОА

        // Прочее
        СтруктурированныеДанные,

        // Типы для логического сообщения и квитанций (не из XSD, но нужны в системе)
        ЛС,                   // Логическое сообщение
        ТК,                   // Технологическая квитанция
        ТрК,                  // Транспортная квитанция
        ПР                    // Приглашение
    }
    public abstract class DocumentsClass
    {
        public string Name { get; init; }

        public Types Type { get; init; }

        public DocumentType DocType { get; init; }

        public DocumentFlowStage Stage { get; set; } =
            DocumentFlowStage.НеСоздан;

        public int StageNumber { get; set; }

        public DateTime CreatedAt { get; init; } =
            DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public string? ErrorMessage { get; set; }

        // Полный XML логического сообщения — единственное, что доступно снаружи
        public string DocumentLogick { get => XmlDeclaration + HeaderLogick + BodyLogick; init; }

        // Полный XML описания сети — доступно снаружи
        public string DocumentDescription { get => XmlDeclaration + HeaderDescription + BodyDescription; init; }

        // Полный XML транспортной квитанции — доступно снаружи
        public string DocumentReceipt { get => XmlDeclaration + HeaderReceipt + BodyReceipt; init; }

        // Общие данные, доступные напрямую, без парсинга XML
        public string Sender { get; init; }
        public string Receiver { get; init; }
        public string DocumentId { get; init; }

        // Для ЛС и других контейнеров
        public DocumentsClass InnerDocument { get; init; }

        // Внутренняя структура XML логического сообщения — доступна только наследникам
        protected string HeaderLogick { get; set; } = string.Empty;
        protected string BodyLogick { get; set; } = string.Empty;

        // Внутренняя структура XML описания сети — доступна только наследникам
        protected string HeaderDescription { get; set; } = string.Empty;
        protected string BodyDescription { get; set; } = string.Empty;

        // Внутренняя структура XML транспортной квитанции — доступна только наследникам
        protected string HeaderReceipt { get; set; } = string.Empty;
        protected string BodyReceipt { get; set; } = string.Empty;

        // XML декларация
        protected string XmlDeclaration { get; } = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        protected virtual string GenerationHeaderLogick() => string.Empty;
        protected virtual string GenerationBodyLogick(string content = "") => string.Empty;
        public virtual string GenerationHeaderDescription() => string.Empty;
        public virtual string GenerationBodyDescription(string content = "") => string.Empty;
        public virtual string GenerationHeaderReceipt() => string.Empty;
        public virtual string GenerationBodyReceipt(string content = "") => string.Empty;
    }
}

using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Первичный документ (ДОП/ДИС) без счёта-фактуры.
    /// </summary>
    public class PrimaryDocument : DocumentsClass
    {
        public string Function { get; init; } // "ДОП" | "ДИС"
        public string SubType { get; init; } // "Продавец" | "Покупатель"
        public string DocKind { get; init; } // "Накладная" | "Акт" | ""
        public string DocNumber { get; init; }
        public DateTime DocDate { get; init; }

        public PrimaryDocument(string sender, string receiver, string documentId, string function, string subType, string docKind, string docNumber, DateTime docDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            Function = function;
            SubType = subType;
            DocKind = docKind;
            DocNumber = docNumber;
            DocDate = docDate;

            // Определяем DocType по подтипу
            DocType = (function.ToUpper(), subType.ToUpper(), docKind.ToUpper()) switch
            {
                ("ДОП", "ПРОДАВЕЦ", "НАКЛАДНАЯ") => DocumentType.ДОППродавецНакладная,
                ("ДОП", "ПРОДАВЕЦ", "АКТ") => DocumentType.ДОППродавецАкт,
                ("ДОП", "ПРОДАВЕЦ", _) => DocumentType.ДОППродавец,
                ("ДОП", "ПОКУПАТЕЛЬ", "НАКЛАДНАЯ") => DocumentType.ДОППокупательНакладная,
                ("ДОП", "ПОКУПАТЕЛЬ", "АКТ") => DocumentType.ДОППокупательАкт,
                ("ДОП", "ПОКУПАТЕЛЬ", _) => DocumentType.ДОППокупатель,
                ("ДИС", "ПРОДАВЕЦ", "НАКЛАДНАЯ") => DocumentType.КорДИСПродавецНакладная,
                ("ДИС", "ПРОДАВЕЦ", "АКТ") => DocumentType.КорДИСПродавецАкт,
                ("ДИС", "ПРОДАВЕЦ", _) => DocumentType.КорДИСПродавец,
                ("ДИС", "ПОКУПАТЕЛЬ", "НАКЛАДНАЯ") => DocumentType.КорДИСПокупательНакладная,
                ("ДИС", "ПОКУПАТЕЛЬ", "АКТ") => DocumentType.КорДИСПокупательАкт,
                ("ДИС", "ПОКУПАТЕЛЬ", _) => DocumentType.КорДИСПокупатель,
                _ => DocumentType.Неизвестный
            };

            Type = new Types { Name = $"{function}{subType}{(string.IsNullOrEmpty(docKind) ? "" : docKind)}" };
            Name = $"Первичный документ ({function}, {subType}, {docKind})";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:ПервичныйДокумент xmlns:xs="http://www.w3.org/2001/XMLSchema" 
                              Функция="{Function}" ПодТип="{SubType}" Вид="{DocKind}"
                              Ид="{DocumentId}" Номер="{DocNumber}" Дата="{DocDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:Функция>{Function}</xs:Функция>
            <xs:ПодТип>{SubType}</xs:ПодТип>
            <xs:ВидДокумента>{DocKind}</xs:ВидДокумента>
            <xs:Номер>{DocNumber}</xs:Номер>
            <xs:Дата>{DocDate:yyyy-MM-dd}</xs:Дата>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:ПервичныйДокумент>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="Первичный" Функция="{Function}" ПодТип="{SubType}" Ид="{DocumentId}">
          <Заголовок>
            <Описание>{Function} ({SubType}, {DocKind}) № {DocNumber} от {DocDate:dd.MM.yyyy}</Описание>
            <Отправитель>{Sender}</Отправитель>
            <Получатель>{Receiver}</Получатель>
        """;

        public override string GenerationBodyDescription(string content = "") => $"""
          </Заголовок>
          <Содержимое>
            {content}
          </Содержимое>
        </ОписаниеДокумента>
        """;

        public override string GenerationHeaderReceipt() => $"""
        <Квитанция Тип="Первичный" Функция="{Function}" ПодТип="{SubType}" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для {Function} ({SubType}, {DocKind}) № {DocNumber}</Информация>
            <ДатаФормирования>{DateTime.Now:dd.MM.yyyy HH:mm:ss}</ДатаФормирования>
        """;

        public override string GenerationBodyReceipt(string content = "") => $"""
          </Заголовок>
          <Содержимое>
            {content}
          </Содержимое>
        </Квитанция>
        """;
    }
}

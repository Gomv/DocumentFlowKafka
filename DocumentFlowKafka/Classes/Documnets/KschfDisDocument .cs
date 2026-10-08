using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// КСЧФ + документ об изменении стоимости (КСЧФДИС).
    /// </summary>
    public class KschfDisDocument : DocumentsClass
    {
        public string KschfNumber { get; init; }
        public DateTime KschfDate { get; init; }
        public string BaseScfNumber { get; init; }
        public DateTime BaseScfDate { get; init; }
        public string DisNumber { get; init; }
        public DateTime DisDate { get; init; }
        public string SubType { get; init; } // "Продавец" | "Покупатель"

        public KschfDisDocument(string sender, string receiver, string documentId, string kschfNumber, DateTime kschfDate, string baseScfNumber, DateTime baseScfDate, string disNumber, DateTime disDate, string subType, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            KschfNumber = kschfNumber;
            KschfDate = kschfDate;
            BaseScfNumber = baseScfNumber;
            BaseScfDate = baseScfDate;
            DisNumber = disNumber;
            DisDate = disDate;
            SubType = subType;
            DocType = subType == "Продавец" ? DocumentType.КорСЧФДИСПродавец : DocumentType.КорСЧФДИСПокупатель;
            Type = new Types { Name = $"КорСЧФДИС{subType}" };
            Name = $"Корректировочный счёт-фактура с ДИС ({subType})";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:КорСЧФДИС xmlns:xs="http://www.w3.org/2001/XMLSchema" Тип="{SubType}" Ид="{DocumentId}" 
                      КорНомер="{KschfNumber}" КорДата="{KschfDate:yyyy-MM-dd}"
                      БазНомер="{BaseScfNumber}" БазДата="{BaseScfDate:yyyy-MM-dd}"
                      ДИСНомер="{DisNumber}" ДИСДата="{DisDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:КорректировочныйСЧФ>
              <xs:Номер>{KschfNumber}</xs:Номер>
              <xs:Дата>{KschfDate:yyyy-MM-dd}</xs:Дата>
            </xs:КорректировочныйСЧФ>
            <xs:БазовыйСЧФ>
              <xs:Номер>{BaseScfNumber}</xs:Номер>
              <xs:Дата>{BaseScfDate:yyyy-MM-dd}</xs:Дата>
            </xs:БазовыйСЧФ>
            <xs:ДИС>
              <xs:Номер>{DisNumber}</xs:Номер>
              <xs:Дата>{DisDate:yyyy-MM-dd}</xs:Дата>
            </xs:ДИС>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:КорСЧФДИС>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="КорСЧФДИС" ПодТип="{SubType}" Ид="{DocumentId}">
          <Заголовок>
            <Описание>КорСЧФДИС ({SubType}): КорСЧФ № {KschfNumber}, ДИС № {DisNumber}</Описание>
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
        <Квитанция Тип="КорСЧФДИС" ПодТип="{SubType}" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для КорСЧФДИС ({SubType}) № {KschfNumber}/{DisNumber}</Информация>
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

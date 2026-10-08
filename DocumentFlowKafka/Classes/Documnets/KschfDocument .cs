using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Корректировочный счёт-фактура (КСЧФ).
    /// </summary>
    public class KschfDocument : DocumentsClass
    {
        public string KschfNumber { get; init; }
        public DateTime KschfDate { get; init; }
        public string BaseScfNumber { get; init; }
        public DateTime BaseScfDate { get; init; }

        public KschfDocument(string sender, string receiver, string documentId, string kschfNumber, DateTime kschfDate, string baseScfNumber, DateTime baseScfDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            KschfNumber = kschfNumber;
            KschfDate = kschfDate;
            BaseScfNumber = baseScfNumber;
            BaseScfDate = baseScfDate;
            DocType = DocumentType.КорСЧФ;
            Type = new Types { Name = "КорСЧФ" };
            Name = "Корректировочный счёт-фактура";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:КорСЧФ xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" 
                   КорНомер="{KschfNumber}" КорДата="{KschfDate:yyyy-MM-dd}"
                   БазНомер="{BaseScfNumber}" БазДата="{BaseScfDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:КорректировочныйСЧФ>
              <xs:Номер>{KschfNumber}</xs:Номер>
              <xs:Дата>{KschfDate:yyyy-MM-dd}</xs:Дата>
            </xs:КорректировочныйСЧФ>
            <xs:БазовыйСЧФ>
              <xs:Номер>{BaseScfNumber}</xs:Номер>
              <xs:Дата>{BaseScfDate:yyyy-MM-dd}</xs:Дата>
            </xs:БазовыйСЧФ>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:КорСЧФ>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="КорСЧФ" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Корректировочный счёт-фактура № {KschfNumber} от {KschfDate:dd.MM.yyyy} к СЧФ № {BaseScfNumber}</Описание>
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
        <Квитанция Тип="КорСЧФ" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для корректировочного счёта-фактуры № {KschfNumber}</Информация>
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

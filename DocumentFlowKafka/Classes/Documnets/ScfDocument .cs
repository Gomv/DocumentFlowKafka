using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Счёт-фактура (СЧФ).
    /// </summary>
    public class ScfDocument : DocumentsClass
    {
        public string ScfNumber { get; init; }
        public DateTime ScfDate { get; init; }

        public ScfDocument(string sender, string receiver, string documentId, string scfNumber, DateTime scfDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            ScfNumber = scfNumber;
            ScfDate = scfDate;
            DocType = DocumentType.СЧФ;
            Type = new Types { Name = "СЧФ" };
            Name = "Счёт-фактура";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:СЧФ xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" Номер="{ScfNumber}" Дата="{ScfDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:НомерСЧФ>{ScfNumber}</xs:НомерСЧФ>
            <xs:ДатаСЧФ>{ScfDate:yyyy-MM-dd}</xs:ДатаСЧФ>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:СЧФ>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="СЧФ" Ид="{DocumentId}" Номер="{ScfNumber}">
          <Заголовок>
            <Описание>Счёт-фактура № {ScfNumber} от {ScfDate:dd.MM.yyyy}</Описание>
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
        <Квитанция Тип="СЧФ" Ид="{DocumentId}" Номер="{ScfNumber}">
          <Заголовок>
            <Информация>Квитанция для счёта-фактуры № {ScfNumber}</Информация>
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

using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Уведомление об уточнении (УОУ).
    /// </summary>
    public class UouDocument : DocumentsClass
    {
        public string TargetDocumentId { get; init; }
        public string Reason { get; init; }
        public DateTime UouDate { get; init; }

        public UouDocument(string sender, string receiver, string documentId, string targetDocumentId, string reason, DateTime uouDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            TargetDocumentId = targetDocumentId;
            Reason = reason;
            UouDate = uouDate;
            DocType = DocumentType.УведомлениеОбУточнении;
            Type = new Types { Name = "УОУ" };
            Name = "Уведомление об уточнении";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:УОУ xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" ЦелевойИд="{TargetDocumentId}" Дата="{UouDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:ЦелевойДокументИд>{TargetDocumentId}</xs:ЦелевойДокументИд>
            <xs:Причина>{Reason}</xs:Причина>
            <xs:ДатаУведомления>{UouDate:yyyy-MM-dd}</xs:ДатаУведомления>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:УОУ>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="УОУ" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Уведомление об уточнении для документа № {TargetDocumentId}: {Reason}</Описание>
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
        <Квитанция Тип="УОУ" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для УОУ № {DocumentId}</Информация>
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

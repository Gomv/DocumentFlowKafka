using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Отказ в подписи.
    /// </summary>
    public class RejectDocument : DocumentsClass
    {
        public string TargetDocumentId { get; init; }
        public string Reason { get; init; }
        public DateTime RejectDate { get; init; }

        public RejectDocument(string sender, string receiver, string documentId, string targetDocumentId, string reason, DateTime rejectDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            TargetDocumentId = targetDocumentId;
            Reason = reason;
            RejectDate = rejectDate;
            DocType = DocumentType.ОтказПодписи;
            Type = new Types { Name = "Отказ" };
            Name = "Отказ в подписи";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:Отказ xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" ЦелевойИд="{TargetDocumentId}" Дата="{RejectDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:ЦелевойДокументИд>{TargetDocumentId}</xs:ЦелевойДокументИд>
            <xs:Причина>{Reason}</xs:Причина>
            <xs:ДатаОтказа>{RejectDate:yyyy-MM-dd}</xs:ДатаОтказа>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:Отказ>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="Отказ" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Отказ в подписи для документа № {TargetDocumentId}: {Reason}</Описание>
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
        <Квитанция Тип="Отказ" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для отказа в подписи № {DocumentId}</Информация>
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

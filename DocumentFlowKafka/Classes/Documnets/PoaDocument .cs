using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Предложение об аннулировании (ПОА).
    /// </summary>
    public class PoaDocument : DocumentsClass
    {
        public string TargetDocumentId { get; init; }
        public string TargetDocumentType { get; init; }
        public string Reason { get; init; }

        public PoaDocument(string sender, string receiver, string documentId, string targetDocumentId, string targetDocumentType, string reason, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            TargetDocumentId = targetDocumentId;
            TargetDocumentType = targetDocumentType;
            Reason = reason;
            DocType = DocumentType.ПредложениеОбАннулировании;
            Type = new Types { Name = "ПОА" };
            Name = "Предложение об аннулировании";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:ПОА xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" ЦелевойИд="{TargetDocumentId}" ЦелевойТип="{TargetDocumentType}">
          <xs:Заголовок>
            <xs:ЦелевойДокументИд>{TargetDocumentId}</xs:ЦелевойДокументИд>
            <xs:ЦелевойДокументТип>{TargetDocumentType}</xs:ЦелевойДокументТип>
            <xs:Причина>{Reason}</xs:Причина>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:ПОА>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="ПОА" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Предложение об аннулировании для {TargetDocumentType} № {TargetDocumentId}: {Reason}</Описание>
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
        <Квитанция Тип="ПОА" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для ПОА № {DocumentId}</Информация>
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

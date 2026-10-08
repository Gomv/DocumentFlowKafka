using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Извещение о получении (ИОП).
    /// </summary>
    public class IopDocument : DocumentsClass
    {
        public string TargetDocumentId { get; init; }
        public DateTime IopDate { get; init; }

        public IopDocument(string sender, string receiver, string documentId, string targetDocumentId, DateTime iopDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            TargetDocumentId = targetDocumentId;
            IopDate = iopDate;
            DocType = DocumentType.ИзвещениеПолучении;
            Type = new Types { Name = "ИОП" };
            Name = "Извещение о получении";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:ИОП xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" ЦелевойИд="{TargetDocumentId}" Дата="{IopDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:ЦелевойДокументИд>{TargetDocumentId}</xs:ЦелевойДокументИд>
            <xs:ДатаИзвещения>{IopDate:yyyy-MM-dd}</xs:ДатаИзвещения>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:ИОП>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="ИОП" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Извещение о получении для документа № {TargetDocumentId}</Описание>
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
        <Квитанция Тип="ИОП" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для ИОП № {DocumentId}</Информация>
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

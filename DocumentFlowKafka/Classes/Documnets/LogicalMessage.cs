using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Логическое сообщение (ЛС) — транспортная единица.
    /// </summary>
    public class LogicalMessage : DocumentsClass
    {
        public LogicalMessage(string sender, string receiver, string messageId, DocumentsClass innerDocument)
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = messageId;
            InnerDocument = innerDocument;
            DocType = DocumentType.ЛС;
            Type = new Types { Name = "ЛС" };
            Name = "Логическое сообщение";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(InnerDocument.DocumentLogick);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:ЛС xmlns:xs="http://www.w3.org/2001/XMLSchema" Ид="{DocumentId}" Отправитель="{Sender}" Получатель="{Receiver}">
          <xs:Заголовок>
            <xs:ИдСообщения>{DocumentId}</xs:ИдСообщения>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:ЛС>
        """;
    }
}

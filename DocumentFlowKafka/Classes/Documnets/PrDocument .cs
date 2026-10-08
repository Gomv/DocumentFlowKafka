using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Приглашение (ПР) для установления/разрыва связи.
    /// </summary>
    public class PrDocument : DocumentsClass
    {
        public string PrType { get; init; } // "Запрос" | "Разрыв"

        public PrDocument(string sender, string receiver, string documentId, string prType, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            PrType = prType;
            DocType = DocumentType.ПР;
            Type = new Types { Name = $"ПР ({prType})" };
            Name = $"Приглашение ({prType})";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:ПР xmlns:xs="http://www.w3.org/2001/XMLSchema" Тип="{PrType}" Ид="{DocumentId}">
          <xs:Заголовок>
            <xs:ТипПриглашения>{PrType}</xs:ТипПриглашения>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:ПР>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="ПР" ПодТип="{PrType}" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Приглашение ({PrType})</Описание>
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
        <Квитанция Тип="ПР" ПодТип="{PrType}" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для приглашения ({PrType})</Информация>
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

using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Транспортная квитанция (ТрК) (Приложение 3, 5).
    /// Пример: receipts.xml
    /// </summary>
    public class TransportReceipt : DocumentsClass
    {
        public string ReceiptType { get; init; }
        public string TargetPackageId { get; init; }
        public DateTime ReceiptDate { get; init; }

        public TransportReceipt(string sender, string receiver, string documentId, string receiptType, string targetPackageId, DateTime receiptDate, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            ReceiptType = receiptType;
            TargetPackageId = targetPackageId;
            ReceiptDate = receiptDate;
            DocType = DocumentType.ТрК;
            Type = new Types { Name = "ТрК" };
            Name = "Транспортная квитанция";

            HeaderReceipt = GenerationHeaderReceipt();
            BodyReceipt = GenerationBodyReceipt(content);
        }

        public override string GenerationHeaderReceipt() => $"""
        <ТранспортнаяКвитанция xmlns:xs="http://www.w3.org/2001/XMLSchema" Тип="{ReceiptType}" Ид="{DocumentId}" ИдПакета="{TargetPackageId}" Дата="{ReceiptDate:dd.MM.yyyy HH:mm:ss}">
          <xs:Заголовок>
            <xs:ТипКвитанции>{ReceiptType}</xs:ТипКвитанции>
            <xs:ИдЦелевогоПакета>{TargetPackageId}</xs:ИдЦелевогоПакета>
            <xs:ДатаФормирования>{ReceiptDate:dd.MM.yyyy HH:mm:ss}</xs:ДатаФормирования>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        public override string GenerationBodyReceipt(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </ТранспортнаяКвитанция>
        """;
    }
}

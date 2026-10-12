using DocumentFlowKafka.Classes.Documnets;
using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public enum DocumentFlowType
    {
        НеформализованныйДокументооборот,
        СчетФактура,
        ПервичныйДокументПродавца,
        ПервичныйДокументПокупателя,
        КорректировочныйСчетФактура,
        КорректировкаПервичногоДокумента,
        ТоварнаяНакладная,
        ТоварноТранспортнаяНакладная,
        АктВыполненныхРабот,
        АктСверки,
        Договор,
        Заказ,
        Счет,
        ПлатежноеПоручение,
        КС11,
        КС2,
        КС3,
        Ведомость,
        ИзвещениеОПолучении,
        УведомлениеОбУточнении,
        ПредложениеОбАннулировании,
        ОтказОтПодписи,
        Приглашение
    }

    public abstract class DocumentFlow
    {

        public string sender { get; set; } = "Отправитель";
        public string receiver { get; set; } = "Получатель";

        protected List<DocumentsClass> SetStageNumbers(List<DocumentsClass> documents)
        {
            for (var index = 0; index < documents.Count; index++)
            {
                documents[index].StageNumber = index;
            }

            return documents;
        }

        protected GenericDocument CreateOperatorConfirmation(string sender, string receiver, string targetDocumentId)
        {
            return new GenericDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString(),
                docTypeCustom: "ТК",
                docNumber: targetDocumentId,
                docDate: DateTime.UtcNow);
        }

        protected LogicalMessage CreateLogicalMessage(DocumentsClass document)
        {
            return new LogicalMessage(
                sender: document.Sender,
                receiver: document.Receiver,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: document);
        }

        protected GenericDocument CreateOperatorConfirmation(LogicalMessage logicalMessage)
        {
            return new GenericDocument(
                sender: logicalMessage.Receiver,
                receiver: logicalMessage.Sender,
                documentId: Guid.NewGuid().ToString(),
                docTypeCustom: "ТК",
                docNumber: logicalMessage.DocumentId,
                docDate: DateTime.UtcNow);
        }

        public virtual List<DocumentsClass> Generate() { return new(); }
    }
}

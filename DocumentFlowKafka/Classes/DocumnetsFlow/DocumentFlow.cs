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

    public class DocumentFlow
    {
        public static Dictionary<DocumentFlowType, List<DocumentsClass>> DocumentFlowDct { get => field ??= initDict(); }

        private static Dictionary<DocumentFlowType, List<DocumentsClass>> initDict()
        {
            var dictionary = new Dictionary<DocumentFlowType, List<DocumentsClass>>();

            if (DocumentFlowDct is null)
            {
                foreach (var enm in Enum.GetValues<DocumentFlowType>())
                {
                    List<DocumentsClass> documents;

                    switch (enm)
                    {
                        case DocumentFlowType.СчетФактура:
                            documents = GenerateСчетФактура();
                            break;

                        case DocumentFlowType.КорректировочныйСчетФактура:
                            documents =
                                GenerateКорректировочныйСчетФактура();
                            break;

                        case DocumentFlowType.НеформализованныйДокументооборот:
                            documents =
                                GenerateНеформализованныйДокументооборот();
                            break;

                        case DocumentFlowType.ПервичныйДокументПродавца:
                            documents =
                                GenerateПервичныйДокументПродавца();
                            break;

                        case DocumentFlowType.ПервичныйДокументПокупателя:
                            documents =
                                GenerateПервичныйДокументПокупателя();
                            break;

                        case DocumentFlowType.ПредложениеОбАннулировании:
                            documents =
                                GenerateПредложениеОбАннулировании();
                            break;

                        case DocumentFlowType.ОтказОтПодписи:
                            documents =
                                GenerateОтказОтПодписи();
                            break;

                        case DocumentFlowType.Приглашение:
                            documents =
                                GenerateПриглашение();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(
                                nameof(enm),
                                enm,
                                "Документооборот ещё не реализован.");
                    }

                    dictionary.Add(enm, documents);
                }
            }

            return dictionary;
        }

        private static List<DocumentsClass> SetStageNumbers(List<DocumentsClass> documents)
        {
            for (var index = 0; index < documents.Count; index++)
            {
                documents[index].StageNumber = index;
            }

            return documents;
        }

        private static List<DocumentsClass> GenerateСчетФактура()
        {
            const string sender = "Отправитель";
            const string receiver = "Получатель";

            var scf = new ScfDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString(),
                scfNumber: "СЧФ-001",
                scfDate: DateTime.UtcNow);

            var logicalMessage = new LogicalMessage(
                sender: sender,
                receiver: receiver,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: scf);

            var operatorConfirmation =
                CreateOperatorConfirmation(
                    sender: receiver,
                    receiver: sender,
                    targetDocumentId: logicalMessage.DocumentId);

            var iop = new IopDocument(
                sender: receiver,
                receiver: sender,
                documentId: Guid.NewGuid().ToString(),
                targetDocumentId: scf.DocumentId,
                iopDate: DateTime.UtcNow);

            var iopLogicalMessage = new LogicalMessage(
                sender: receiver,
                receiver: sender,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: iop);

            var iopConfirmation =
                CreateOperatorConfirmation(
                    sender: sender,
                    receiver: receiver,
                    targetDocumentId: iopLogicalMessage.DocumentId);

            return SetStageNumbers(
            [
                scf,
                logicalMessage,
                operatorConfirmation,
                iop,
                iopLogicalMessage,
                iopConfirmation
            ]);
        }

        private static GenericDocument CreateOperatorConfirmation(string sender, string receiver, string targetDocumentId)
        {
            return new GenericDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString(),
                docTypeCustom: "ТК",
                docNumber: targetDocumentId,
                docDate: DateTime.UtcNow);
        }

        public class TechnologyReceiptDocument : DocumentsClass
        {
        }

        private static List<DocumentsClass> GenerateНеформализованныйДокументооборот()
        {
            const string sender = "Отправитель";
            const string receiver = "Получатель";

            var document = new NotFormalDocumnet(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString(),
                docName: "Неформализованный документ",
                docDate: DateTime.UtcNow);

            var logicalMessage = new LogicalMessage(
                sender: sender,
                receiver: receiver,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: document);

            var operatorConfirmation =
                CreateOperatorConfirmation(
                    sender: receiver,
                    receiver: sender,
                    targetDocumentId: logicalMessage.DocumentId);

            var iop = new IopDocument(
                sender: receiver,
                receiver: sender,
                documentId: Guid.NewGuid().ToString(),
                targetDocumentId: document.DocumentId,
                iopDate: DateTime.UtcNow);

            var iopLogicalMessage = new LogicalMessage(
                sender: receiver,
                receiver: sender,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: iop);

            var iopConfirmation =
                CreateOperatorConfirmation(
                    sender: sender,
                    receiver: receiver,
                    targetDocumentId: iopLogicalMessage.DocumentId);

            return SetStageNumbers(
            [
                document,
                    logicalMessage,
                    operatorConfirmation,
                    iop,
                    iopLogicalMessage,
                    iopConfirmation
            ]);
        }

        private static List<DocumentsClass> GenerateПервичныйДокументПродавца()
        {
            const string sender = "Продавец";
            const string receiver = "Покупатель";

            var primaryDocument = new PrimaryDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString(),
                function: "ДОП",
                subType: "Продавец",
                docKind: "Накладная",
                docNumber: "ДОП-001",
                docDate: DateTime.UtcNow);

            var logicalMessage = new LogicalMessage(
                sender: sender,
                receiver: receiver,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: primaryDocument);

            var operatorConfirmation =
                CreateOperatorConfirmation(
                    sender: receiver,
                    receiver: sender,
                    targetDocumentId: logicalMessage.DocumentId);

            var iop = new IopDocument(
                sender: receiver,
                receiver: sender,
                documentId: Guid.NewGuid().ToString(),
                targetDocumentId: primaryDocument.DocumentId,
                iopDate: DateTime.UtcNow);

            var iopLogicalMessage = new LogicalMessage(
                sender: receiver,
                receiver: sender,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: iop);

            var iopConfirmation =
                CreateOperatorConfirmation(
                    sender: sender,
                    receiver: receiver,
                    targetDocumentId: iopLogicalMessage.DocumentId);

            return SetStageNumbers(
            [
                primaryDocument,
                    logicalMessage,
                    operatorConfirmation,
                    iop,
                    iopLogicalMessage,
                    iopConfirmation
            ]);
        }

        private static List<DocumentsClass> GenerateПервичныйДокументПокупателя()
        {
            const string sender = "Покупатель";
            const string receiver = "Продавец";

            var primaryDocument = new PrimaryDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString(),
                function: "ДОП",
                subType: "Покупатель",
                docKind: "Акт",
                docNumber: "ДОП-ПОКУПАТЕЛЬ-001",
                docDate: DateTime.UtcNow);

            var logicalMessage = new LogicalMessage(
                sender: sender,
                receiver: receiver,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: primaryDocument);

            var operatorConfirmation =
                CreateOperatorConfirmation(
                    sender: receiver,
                    receiver: sender,
                    targetDocumentId: logicalMessage.DocumentId);

            var iop = new IopDocument(
                sender: receiver,
                receiver: sender,
                documentId: Guid.NewGuid().ToString(),
                targetDocumentId: primaryDocument.DocumentId,
                iopDate: DateTime.UtcNow);

            var iopLogicalMessage = new LogicalMessage(
                sender: receiver,
                receiver: sender,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: iop);

            var iopConfirmation =
                CreateOperatorConfirmation(
                    sender: sender,
                    receiver: receiver,
                    targetDocumentId: iopLogicalMessage.DocumentId);

            return SetStageNumbers(
            [
                primaryDocument,
                logicalMessage,
                operatorConfirmation,
                iop,
                iopLogicalMessage,
                iopConfirmation
            ]);
        }

        private static LogicalMessage CreateLogicalMessage(DocumentsClass document)
        {
            return new LogicalMessage(
                sender: document.Sender,
                receiver: document.Receiver,
                messageId: Guid.NewGuid().ToString(),
                innerDocument: document);
        }

        private static GenericDocument CreateOperatorConfirmation(LogicalMessage logicalMessage)
        {
            return new GenericDocument(
                sender: logicalMessage.Receiver,
                receiver: logicalMessage.Sender,
                documentId: Guid.NewGuid().ToString(),
                docTypeCustom: "ТК",
                docNumber: logicalMessage.DocumentId,
                docDate: DateTime.UtcNow);
        }

        private static List<DocumentsClass> GenerateКорректировочныйСчетФактура()
        {
            const string sender = "Отправитель";
            const string receiver = "Получатель";

            var correctiveInvoice = new KschfDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString("N"),
                kschfNumber: "КСЧФ-001",
                kschfDate: DateTime.UtcNow,
                baseScfNumber: "СЧФ-001",
                baseScfDate: DateTime.UtcNow.AddDays(-1));

            var logicalMessage =
                CreateLogicalMessage(correctiveInvoice);

            var operatorConfirmation =
                CreateOperatorConfirmation(logicalMessage);

            var iop = new IopDocument(
                sender: receiver,
                receiver: sender,
                documentId: Guid.NewGuid().ToString("N"),
                targetDocumentId: correctiveInvoice.DocumentId,
                iopDate: DateTime.UtcNow);

            var iopLogicalMessage =
                CreateLogicalMessage(iop);

            var iopOperatorConfirmation =
                CreateOperatorConfirmation(iopLogicalMessage);

            return SetStageNumbers(
            [
                correctiveInvoice,
                logicalMessage,
                operatorConfirmation,
                iop,
                iopLogicalMessage,
                iopOperatorConfirmation
            ]);
        }

        private static List<DocumentsClass> GenerateПредложениеОбАннулировании()
        {
            const string sender = "Отправитель";
            const string receiver = "Получатель";

            var targetDocumentId =
                Guid.NewGuid().ToString("N");

            var cancellationProposal = new PoaDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString("N"),
                targetDocumentId: targetDocumentId,
                targetDocumentType: "СЧФ",
                reason: "Ошибка в исходном документе");

            var logicalMessage =
                CreateLogicalMessage(cancellationProposal);

            var operatorConfirmation =
                CreateOperatorConfirmation(logicalMessage);

            var iop = new IopDocument(
                sender: receiver,
                receiver: sender,
                documentId: Guid.NewGuid().ToString("N"),
                targetDocumentId: cancellationProposal.DocumentId,
                iopDate: DateTime.UtcNow);

            var iopLogicalMessage =
                CreateLogicalMessage(iop);

            var iopOperatorConfirmation =
                CreateOperatorConfirmation(iopLogicalMessage);

            return SetStageNumbers(
            [
                cancellationProposal,
                logicalMessage,
                operatorConfirmation,
                iop,
                iopLogicalMessage,
                iopOperatorConfirmation
            ]);
        }

        private static List<DocumentsClass> GenerateОтказОтПодписи()
        {
            const string sender = "Получатель";
            const string receiver = "Отправитель";

            var targetDocumentId =
                Guid.NewGuid().ToString("N");

            var reject = new RejectDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString("N"),
                targetDocumentId: targetDocumentId,
                reason: "Документ не принят получателем",
                rejectDate: DateTime.UtcNow);

            var logicalMessage =
                CreateLogicalMessage(reject);

            var operatorConfirmation =
                CreateOperatorConfirmation(logicalMessage);

            return SetStageNumbers(
            [
                reject,
                logicalMessage,
                operatorConfirmation
            ]);
        }

        private static List<DocumentsClass> GenerateПриглашение()
        {
            const string sender = "Отправитель";
            const string receiver = "Получатель";

            var invitation = new PrDocument(
                sender: sender,
                receiver: receiver,
                documentId: Guid.NewGuid().ToString("N"),
                prType: "Запрос");

            var logicalMessage =
                CreateLogicalMessage(invitation);

            var operatorConfirmation =
                CreateOperatorConfirmation(logicalMessage);

            return SetStageNumbers(
            [
                invitation,
                logicalMessage,
                operatorConfirmation
            ]);
        }

    }
}

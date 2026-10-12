using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class PrimarySellerDocumentFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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
    }
}

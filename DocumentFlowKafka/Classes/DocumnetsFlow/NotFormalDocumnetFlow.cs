using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class NotFormalDocumnetFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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
    }
}

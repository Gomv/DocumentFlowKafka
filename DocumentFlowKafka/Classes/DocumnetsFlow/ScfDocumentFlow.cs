using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class ScfDocumentFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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
    }
}

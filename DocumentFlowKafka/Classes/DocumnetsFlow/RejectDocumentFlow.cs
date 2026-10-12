using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class RejectDocumentFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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
    }
}

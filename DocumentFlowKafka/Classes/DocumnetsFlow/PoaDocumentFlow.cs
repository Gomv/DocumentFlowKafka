using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class PoaDocumentFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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
    }
}

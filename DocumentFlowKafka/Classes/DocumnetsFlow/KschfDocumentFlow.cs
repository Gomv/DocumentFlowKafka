using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class KschfDocumentFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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
    }
}

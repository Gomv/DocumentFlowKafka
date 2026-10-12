using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class PrDocumentFlow : DocumentFlow
    {
        public override List<DocumentsClass> Generate()
        {
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

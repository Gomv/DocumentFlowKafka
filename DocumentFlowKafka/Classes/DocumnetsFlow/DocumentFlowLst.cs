using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Classes.DocumnetsFlow
{
    public class DocumentFlowLst
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
                            documents = new ScfDocumentFlow().Generate();
                            break;

                        case DocumentFlowType.КорректировочныйСчетФактура:
                            documents = new KschfDocumentFlow().Generate();
                            break;

                        case DocumentFlowType.НеформализованныйДокументооборот:
                            documents = new NotFormalDocumnetFlow().Generate();
                            break;

                        case DocumentFlowType.ПервичныйДокументПродавца:
                            documents = new PrimarySellerDocumentFlow().Generate();
                            break;

                        case DocumentFlowType.ПервичныйДокументПокупателя:
                            documents = new PrimaryBuierDocumentFlow().Generate();
                            break;

                        case DocumentFlowType.ПредложениеОбАннулировании:
                            documents = new PoaDocumentFlow().Generate();
                            break;

                        case DocumentFlowType.ОтказОтПодписи:
                            documents = new RejectDocumentFlow().Generate();
                            break;

                        case DocumentFlowType.Приглашение:
                            documents = new PrDocumentFlow().Generate();
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
    }
}

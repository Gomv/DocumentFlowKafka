using DocumentFlowKafka.Model;
using System.Text.Json;

namespace DocumentFlowKafka.Classes
{
    /// <summary>
    /// Документооборот с которым работаем
    /// </summary>
    public class DocumnetFlowConnector
    {
        /// <summary>
        /// Что за документ получили
        /// </summary>
        public Documents Documnet { get; init; }
        /// <summary>
        /// Текущий дкоументооборот с которм работаем
        /// </summary>
        public FlowsTypes FlowType { get; init; }
        /// <summary>
        /// Какой документ ожидаем дальше
        /// </summary>
        public Types NextDocumnet { get; init; }

        private DocumentFlowContext _context;
        
        public DocumnetFlowConnector(DocumentFlowContext _context, string documnet)
        {
            this._context = _context;
            Documnet = JsonSerializer.Deserialize<Documents>(documnet);
            FlowType = detectedType();
            NextDocumnet = FlowType.DocumnetTypeId[FlowType.DocumnetTypeId.IndexOf(Documnet.Type)];
        }

        /// <summary>
        /// Получаем тип документооборота, так сможем понять что отправить дальше или ожидать что то в ответ
        /// </summary>
        /// <returns></returns>
        private FlowsTypes detectedType()
        {
            Flows flow = null;

            try // пытаемся получить 
            {
                flow = _context.Flows.Single(s => s.Id == Documnet.FlowId.Id);
            }
            catch // если документооборот новый, ищем первый документ в ценпочки документооборота через тип
            {
                try
                {
                    flow = _context.Flows.Single(s => s.FlowsType.DocumnetTypeId.IndexOf(Documnet.Type) == 0);
                }
                catch //Если не нашли
                {
                    return null;
                }
            }

            return flow.FlowsType;
        }
    }
}

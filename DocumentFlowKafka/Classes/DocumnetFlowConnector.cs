using DocumentFlowKafka.Classes.Documnets;
using DocumentFlowKafka.Classes.DocumnetsFlow;
using DocumentFlowKafka.Model;
using System.Text.Json;

namespace DocumentFlowKafka.Classes
{
    /// <summary>
    /// Класс, в которм мы получаем информацию необходимую 
    /// для последующей работы. 
    /// </summary>
    public sealed class DocumnetFlowConnector
    {
        /// <summary>
        /// Что за документ получили
        /// </summary>
        public Documents Document { get; }
        /// <summary>
        /// Документооборот в котором он будет
        /// </summary>
        public Flows Flow { get; }
        /// <summary>
        /// Тип документооборота (позже уберу)
        /// </summary>
        public DocumentFlowType FlowType { get; }

        /// <summary>
        /// Документ с который мы ожидаем реализовать
        /// </summary>
        public DocumentsClass Current { get; }

        private DocumnetFlowConnector(
            Documents document,
            Flows flow,
            DocumentFlowType flowType,
            DocumentsClass? previous,
            DocumentsClass current,
            DocumentsClass? next,
            bool isDuplicate,
            bool isNewFlow)
        {
            Document = document;
            Flow = flow;
            FlowType = flowType;
            Current = current;
        }
    }
}

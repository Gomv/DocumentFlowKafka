using DocumentFlowKafka.Classes.Documnets;
using DocumentFlowKafka.Classes.DocumnetsFlow;
using DocumentFlowKafka.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DocumentFlowKafka.Classes
{
    /// <summary>
    /// Класс, добавляет документ, в базу, проверяет на дубликацию, 
    /// и корректность в цепочки документооборота. 
    /// Возвращает:
    ///     - информацию о документообороте;
    ///     - документ готовый к отправке
    /// </summary>
    public sealed class DocumnetFlowConnector
    {
        /// <summary>
        /// Документ
        /// </summary>
        public Documents Document { get; }
        /// <summary>
        /// Документооборот
        /// </summary>
        public Flows Flow { get; }

        /// <summary>
        /// Документ готовый к отправке
        /// </summary>
        public DocumentsClass Current { get; }

        private DocumnetFlowConnector(
            Documents document,
            Flows flow,
            DocumentsClass current)
        {
            Document = document;
            Flow = flow;
            Current = current;
        }

        /// <summary>
        /// Добавить документ в базу, 
        /// получить, всю информации о документообороте
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="json"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<DocumnetFlowConnector> CreateAsync(DocumentFlowContext _context,
                                                                    string json,
                                                                    CancellationToken cancellationToken = default)
        {
            var document = JsonSerializer.Deserialize<Documents>(json)
                ?? throw new ArgumentException("Не удалось десериализовать документ.", nameof(json));

            var isNewFlow = string.IsNullOrWhiteSpace(document.FlowIdString);

            document.FillObject(_context);

            var flow = document.FlowId
                ?? throw new InvalidOperationException("Не найден документооборот.");

            // Необходимо явное сопоставление FlowsTypes с ключом DocumentFlowDct.
            var flowType = document.FlowId.FlowsType.Type ?? throw new InvalidOperationException("Не корректный документооборот");

            var steps = DocumentFlowLst.DocumentFlowDct[flowType];

            // Сначала распознаём повтор уже обработанного документа.
            var duplicate = await _context.Documents.AnyAsync(d => d.Id == document.Id);

            if (duplicate)
            {
                throw new InvalidOperationException("Повтор документа");
            }

            var count = await _context.Documents.CountAsync(d => d.FlowId.Id == flow.Id, cancellationToken);

            if (count >= steps.Count)
                throw new InvalidOperationException("Сценарий уже завершён.");

            var current = steps[count];

            if (current.Type.Id != document.Type.Id)
                throw new InvalidOperationException(
                    $"На шаге {count} ожидался тип '{current.Type.Name}', " +
                    $"получен '{document.Type.Name}'.");

            _context.Documents.Add(document);
            await _context.SaveChangesAsync(cancellationToken);

            return new DocumnetFlowConnector(
                document,
                flow,
                current);
        }
    }
}

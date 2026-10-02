using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    public abstract class DocumentsClass
    {
        public string Name { get; init; }

        public Types Type { get; init; }

        public string Header { get; init; }

        public string Body { get; init; }

        protected virtual string GenerationHeader() { return string.Empty; }
        protected virtual string GenerationBody() { return string.Empty; }
    }
}

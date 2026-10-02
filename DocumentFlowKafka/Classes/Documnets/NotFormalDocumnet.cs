using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    public class NotFormalDocumnet : DocumentsClass
    {
        public NotFormalDocumnet(string Sender, string Receiver)
        {
            base.Type = new Types()
            {
                Id = 1,
                Name = "НеФормальный"
            };
            base.Name = "НеФормальный";
            base.Body = GenerationBody();
            base.Header = GenerationHeader();
        }

        protected override string GenerationBody()
        {
            return base.GenerationBody();
        }
        protected override string GenerationHeader()
        {
            return base.GenerationHeader();
        }

    }
}

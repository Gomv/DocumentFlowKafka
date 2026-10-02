using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    public class WelcomeDocumnet : DocumentsClass
    {
        public WelcomeDocumnet(string Sender, string Receiver)
        {
            base.Type = new Types()
            {
                Id = 0,
                Name = "Приглашение"
            };
            base.Name = "Приглашение";
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

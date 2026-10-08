using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Счёт-фактура + первичный документ (СЧФДОП).
    /// </summary>
    public class ScfDopDocument : DocumentsClass
    {
        public string ScfNumber { get; init; }
        public DateTime ScfDate { get; init; }
        public string DopNumber { get; init; }
        public DateTime DopDate { get; init; }
        public string SubType { get; init; } // "Продавец" | "Покупатель"

        public ScfDopDocument(string sender, string receiver, string documentId, string scfNumber, DateTime scfDate, string dopNumber, DateTime dopDate, string subType, string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            ScfNumber = scfNumber;
            ScfDate = scfDate;
            DopNumber = dopNumber;
            DopDate = dopDate;
            SubType = subType;
            DocType = subType == "Продавец" ? DocumentType.СЧФДОППродавец : DocumentType.СЧФДОППокупатель;
            Type = new Types { Name = $"СЧФДОП{subType}" };
            Name = $"Счёт-фактура с ДОП ({subType})";

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:СЧФДОП xmlns:xs="http://www.w3.org/2001/XMLSchema" Тип="{SubType}" Ид="{DocumentId}" 
                   СЧФНомер="{ScfNumber}" СЧФДата="{ScfDate:yyyy-MM-dd}"
                   ДОПНомер="{DopNumber}" ДОПДата="{DopDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:СЧФ>
              <xs:Номер>{ScfNumber}</xs:Номер>
              <xs:Дата>{ScfDate:yyyy-MM-dd}</xs:Дата>
            </xs:СЧФ>
            <xs:ДОП>
              <xs:Номер>{DopNumber}</xs:Номер>
              <xs:Дата>{DopDate:yyyy-MM-dd}</xs:Дата>
            </xs:ДОП>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:СЧФДОП>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="СЧФДОП" ПодТип="{SubType}" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Счёт-фактура № {ScfNumber} с ДОП № {DopNumber} ({SubType})</Описание>
            <Отправитель>{Sender}</Отправитель>
            <Получатель>{Receiver}</Получатель>
        """;

        public override string GenerationBodyDescription(string content = "") => $"""
          </Заголовок>
          <Содержимое>
            {content}
          </Содержимое>
        </ОписаниеДокумента>
        """;

        public override string GenerationHeaderReceipt() => $"""
        <Квитанция Тип="СЧФДОП" ПодТип="{SubType}" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для СЧФДОП ({SubType}): СЧФ № {ScfNumber}, ДОП № {DopNumber}</Информация>
            <ДатаФормирования>{DateTime.Now:dd.MM.yyyy HH:mm:ss}</ДатаФормирования>
        """;

        public override string GenerationBodyReceipt(string content = "") => $"""
          </Заголовок>
          <Содержимое>
            {content}
          </Содержимое>
        </Квитанция>
        """;
    }
}

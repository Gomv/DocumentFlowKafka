using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Описание сети (Приложение 2, 4).
    /// Пример: description.xml
    /// </summary>
    public class NetworkDescription : DocumentsClass
    {
        public string NetworkVersion { get; init; }
        public DateTime LastUpdate { get; init; }

        public NetworkDescription(string networkVersion, DateTime lastUpdate, string content = "")
        {
            NetworkVersion = networkVersion;
            LastUpdate = lastUpdate;
            DocType = DocumentType.Неизвестный;
            Type = new Types { Name = "Описание сети" };
            Name = "Описание сети обмена";

            HeaderDescription = GenerationHeaderDescription();
            BodyDescription = GenerationBodyDescription(content);
        }

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеСети xmlns:xs="http://www.w3.org/2001/XMLSchema" Версия="{NetworkVersion}" ДатаОбновления="{LastUpdate:dd.MM.yyyy HH:mm:ss}">
          <xs:Заголовок>
            <xs:ВерсияСети>{NetworkVersion}</xs:ВерсияСети>
            <xs:ДатаПоследнегоОбновления>{LastUpdate:dd.MM.yyyy HH:mm:ss}</xs:ДатаПоследнегоОбновления>
        """;

        public override string GenerationBodyDescription(string content = "") => $"""
          </xs:Заголовок>
          <xs:Операторы>
            {content}
          </xs:Операторы>
        </ОписаниеСети>
        """;
    }
}

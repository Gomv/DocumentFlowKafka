using DocumentFlowKafka.Model;

namespace DocumentFlowKafka.Classes.Documnets
{
    /// <summary>
    /// Универсальный класс для всех остальных типов документов РОСЭУ.
    /// Подходит для: ТН, ТТН, Акт, АктСверки, КС-2, КС-3, Договор, Счет, ПлатПоруч, Ведомость, Заказ и т.д.
    /// </summary>
    public class GenericDocument : DocumentsClass
    {
        public string DocTypeCustom { get; init; } // "ТН" | "ТТН" | "Акт" | "КС-2" | "Договор" | и т.д.
        public string DocNumber { get; init; }
        public DateTime DocDate { get; init; }

        public GenericDocument(
            string sender, string receiver, string documentId,
            string docTypeCustom, string docNumber, DateTime docDate,
            string content = "")
        {
            Sender = sender;
            Receiver = receiver;
            DocumentId = documentId;
            DocTypeCustom = docTypeCustom;
            DocNumber = docNumber;
            DocDate = docDate;

            // Определяем DocType по названию типа
            DocType = docTypeCustom.ToUpper() switch
            {
                "ТН" => DocumentType.ТН,
                "ТТН" => DocumentType.ТТН,
                "ТОРГ12ТИТУЛПРОДАВЦА" => DocumentType.Торг12ТитулПродавца,
                "ТОРГ12ТИТУЛПОКУПАТЕЛЯ" => DocumentType.Торг12ТитулПокупателя,
                "ДОКУМЕНТОПЕРЕДАЧЕТОВАРОВПРИТОРГОВЫХОПЕРАЦИЯХПРОДАВЕЦ" => DocumentType.ДокументОПередачеТоваровПриТорговыхОперацияхПродавец,
                "ДОКУМЕНТОПЕРЕДАЧЕТОВАРОВПРИТОРГОВЫХОПЕРАЦИЯХПОКУПАТЕЛЬ" => DocumentType.ДокументОПередачеТоваровПриТорговыхОперацияхПокупатель,
                "АКТ" => DocumentType.Акт,
                "АКТСВЕРКИ" => DocumentType.АктСверки,
                "АКТТИТУЛЗАКАЗЧИКА" => DocumentType.АктТитулЗаказчика,
                "АКТТИТУЛИСПОЛНИТЕЛЯ" => DocumentType.АктТитулИсполнителя,
                "ДОКУМЕНТОПЕРЕДАЧЕРЕЗУЛЬТАТОВРАБОТИСПОЛНИТЕЛЬ" => DocumentType.ДокументОПередачеРезультатовРаботИсполнитель,
                "ДОКУМЕНТОПЕРЕДАЧЕРЕЗУЛЬТАТОВРАБОТЗАКАЗЧИК" => DocumentType.ДокументОПередачеРезультатовРаботЗаказчик,
                "ПЛАТПОРУЧ" => DocumentType.ПлатПоруч,
                "ДОГОВОР" => DocumentType.Договор,
                "ЗАКАЗ" => DocumentType.Заказ,
                "СЧЕТ" => DocumentType.Счет,
                "ВЕДОМОСТЬ" => DocumentType.Ведомость,
                "КС-11" or "КС11" => DocumentType.КС11,
                "КС-2" or "КС2" => DocumentType.КС2,
                "КС-3" or "КС3" => DocumentType.КС3,
                "СТРУКТУРИРОВАННЫЕДАННЫЕ" => DocumentType.СтруктурированныеДанные,
                "ТК" => DocumentType.ТК,
                _ => DocumentType.Неизвестный
            };

            Type = new Types { Name = docTypeCustom };
            Name = docTypeCustom;

            HeaderLogick = GenerationHeaderLogick();
            BodyLogick = GenerationBodyLogick(content);
        }

        protected override string GenerationHeaderLogick() => $"""
        <xs:Документ xmlns:xs="http://www.w3.org/2001/XMLSchema" 
                     Тип="{DocTypeCustom}" Ид="{DocumentId}" Номер="{DocNumber}" Дата="{DocDate:yyyy-MM-dd}">
          <xs:Заголовок>
            <xs:ТипДокумента>{DocTypeCustom}</xs:ТипДокумента>
            <xs:Номер>{DocNumber}</xs:Номер>
            <xs:Дата>{DocDate:yyyy-MM-dd}</xs:Дата>
            <xs:Отправитель>{Sender}</xs:Отправитель>
            <xs:Получатель>{Receiver}</xs:Получатель>
        """;

        protected override string GenerationBodyLogick(string content = "") => $"""
          </xs:Заголовок>
          <xs:Содержимое>
            {content}
          </xs:Содержимое>
        </xs:Документ>
        """;

        public override string GenerationHeaderDescription() => $"""
        <ОписаниеДокумента Тип="{DocTypeCustom}" Ид="{DocumentId}">
          <Заголовок>
            <Описание>Документ "{DocTypeCustom}" № {DocNumber} от {DocDate:dd.MM.yyyy}</Описание>
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
        <Квитанция Тип="{DocTypeCustom}" Ид="{DocumentId}">
          <Заголовок>
            <Информация>Квитанция для документа "{DocTypeCustom}" № {DocNumber}</Информация>
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

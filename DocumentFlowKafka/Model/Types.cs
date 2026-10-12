using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DocumentFlowKafka.Classes.Documnets;

namespace DocumentFlowKafka.Model
{
    [Table("Type")]
    public class Types
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }

        [Column("Name")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "ТипДокументаНеНайден";

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public DocumentType? Type 
        { 
            get => Enum.TryParse<DocumentType>(Name, out var value) && Enum.IsDefined(value) ? value : null;
            set => Name = value?.ToString() ?? "ТипДокументаНеНайден";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

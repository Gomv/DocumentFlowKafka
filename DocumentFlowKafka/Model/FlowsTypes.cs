using DocumentFlowKafka.Classes.DocumnetsFlow;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DocumentFlowKafka.Model
{
    [Table("FlowsTypes")]
    public class FlowsTypes
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }

        [Column("Name")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "ТипДокументОборотаНеНайден";

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public DocumentFlowType? Type
        {
            get => Enum.TryParse<DocumentFlowType>(Name, out var value) && Enum.IsDefined(value) ? value : null;
            set => Name = value?.ToString() ?? "ТипДокументОборотаНеНайден";
        }

        [Column("DocumnetTypeId")]
        [Required]
        [MaxLength(100)]
        public Types[] DocumnetTypeId { get; set; }
    }
}

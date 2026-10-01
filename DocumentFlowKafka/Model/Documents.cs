using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DocumentFlowKafka.Model
{
    [Table("Documents")]
    public class Documents
    {
        private DocumentFlowContext _context;
        public Documents(DocumentFlowContext _context) { this._context = _context; }
        [Key]
        [Column("Id")]
        [Required]
        [MaxLength(250)]
        public string Id { get; set; } = string.Empty;

        [Column("Name")]
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Column("DateCreate")]
        [Required]
        public DateTime DateCreate { get; set; }

        [Column("TypeId")]
        [Required]
        [ForeignKey(nameof(Types))]
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Types Type 
        { 
            get => field;
            set 
            {
                try
                {
                    field = _context.Type.Single(s => s.Name == TypeString);
                }
                catch
                {
                    field = null;
                }
            } 
        }

        [JsonPropertyName("type")]
        [NotMapped]
        public string TypeString { get; set; }

        [Column("Body")]
        [Required]
        public byte[] Body { get; set; }

        [Column("FlowId")]
        [Required]
        [ForeignKey(nameof(Flows))]
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Flows FlowId { get; set; }

        [JsonPropertyName("flowId")]
        [NotMapped]
        public string FlowIdString
        {
            get => FlowId.Id;
            set
            {
                try
                {
                    FlowId = _context.Flows.Single(s => s.Id == value);
                }
                catch
                {
                    FlowId = new();
                }
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace DocumentFlowKafka.Model
{
    [Table("Documents")]
    public class Documents
    {
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
        public Types Type { get; set; } = null;

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
        public Flows FlowId { get; set; } = null;

        [JsonPropertyName("flowId")]
        [NotMapped]
        public string FlowIdString { get; set; }

        /// <summary>
        /// Заполнить объект при получении из json
        /// </summary>
        /// <param name="_context"></param>
        public void FillObject(DocumentFlowContext _context)
        {
            try
            {
                this.FlowId = _context.Flows.Single(s => s.Id == this.FlowIdString);
            }
            finally
            {

            }
            
            try
            {
                this.Type = _context.Type.Single(s => s.Name == this.TypeString);
            }
            finally
            {

            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Type TypeId { get; set; }

        [Column("Body")]
        [Required]
        public byte[] Body { get; set; }

        [Column("FlowId")]
        [Required]
        public Flows FlowId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowKafka.Model
{
    [Table("Flows")]
    public class Flows
    {
        [Key]
        [Column("Id")]
        [Required]
        [MaxLength(50)]
        public string Id { get; set; } = string.Empty;

        [Column("Name")]
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Column("SenderId")]
        [Required]
        [MaxLength(200)]
        public Users SenderId { get; set; }

        [Column("ReciverId")]
        [Required]
        [MaxLength(200)]
        public Users ReceiverId { get; set; }

        [Column("DateCreate")]
        public DateTime? DateCreate { get; set; }
    }
}

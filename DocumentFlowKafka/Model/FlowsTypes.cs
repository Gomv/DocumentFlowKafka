using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; }

        [Column("DocumnetTypeId")]
        [Required]
        [MaxLength(100)]
        public Types[] DocumnetTypeId { get; set; }
    }
}

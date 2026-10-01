using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public override string ToString()
        {
            return Name;
        }
    }
}

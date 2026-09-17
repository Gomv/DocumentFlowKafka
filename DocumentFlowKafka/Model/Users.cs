using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowKafka.Model
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }

        [Column("NameOrg")]
        [Required]
        [MaxLength(200)]
        public string NameOrg { get; set; }

        [Column("Uid")]
        [MaxLength(50)]
        public string? Uid { get; set; }

        [Column("Cert")]
        [Required]
        public byte[] Cert { get; set; }

        [Column("INN")]
        [MaxLength(15)]
        public string? INN { get; set; }

        [Column("KPP")]
        [MaxLength(15)]
        public string? KPP { get; set; }

        [Column("OGRN")]
        [MaxLength(15)]
        public string? OGRN { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Redis.Database.Models
{
    [ExcludeFromCodeCoverage]
    [Table("SubFundUnitPrice", Schema = "Dispositions")]
    public class SubFundUnitPriceModel
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDateTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDateTime { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; } = Array.Empty<byte>();

        public bool IsActive { get; set; }

        [Column(TypeName = "decimal(14, 2)")]
        public decimal UnitPrice { get; set; }

        public DateTime? UnitPriceDate { get; set; }

        public int SubFundId { get; set; }
    }
}

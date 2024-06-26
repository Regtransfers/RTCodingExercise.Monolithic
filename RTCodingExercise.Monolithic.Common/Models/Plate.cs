using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTCodingExercise.Monolithic.Common.Models
{
    public class Plate
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Sale Price")]
        public decimal SalePrice { get; set; }

        public string? Letters { get; set; }

        public int Numbers { get; set; }
        
        [Display(Name = "Sale Price (inc Markup)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal MarkUp { get; set; }
        
        public bool Reserved { get; set; }
    }
}

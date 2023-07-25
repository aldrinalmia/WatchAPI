using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;

namespace WatchAPI.Model
{
    [Table("Watches")]
    public class Watch
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string? ItemName { get; set; }
        [Required]
        [StringLength(50)]
        public string? ItemNumber { get; set; }
        [Required]
        public string? ShortDescription { get; set; }
        [Required]
        public string? FullDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(50)]
        public string? Caliber { get; set; }
        [Required]
        [StringLength(50)]
        public string? Movement { get; set; }
        [Required]
        public string? Chronograph { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Height { get; set; }
        [Required]
        public decimal Diameter { get; set; }
        [Required]
        public decimal Thickness { get; set; }
        [Required]
        public int Jewel { get; set; }
        [Required]
        [StringLength(30)]
        public string? CaseMaterial { get; set; }
        [Required]
        [StringLength(30)]
        public string? StrapMaterial { get; set; }

        //[JsonIgnore]
        [NotMapped]
        [Display(Name = "File")]
        public IFormFile? FormFile { get; set; }

        public string? URL { get; set; }
    }
}

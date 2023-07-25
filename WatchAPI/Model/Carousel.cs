using System.ComponentModel.DataAnnotations;

namespace WatchAPI.Model
{
    public class Carousel
    {
        [Key]
        public int ID { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? URL { get; set; }

        public int WatchFkID { get; set; }
    }
}

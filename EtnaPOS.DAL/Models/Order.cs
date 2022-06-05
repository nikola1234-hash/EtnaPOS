
using System.ComponentModel.DataAnnotations;

namespace EtnaPOS.DAL.Models
{
    public class Order 
    {
        [Key]
        public Guid Id { get; set; }
        public int ArtikalId { get; set; }
        public Artikal Artikal { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool IsDeleted { get; set; }
    }
}

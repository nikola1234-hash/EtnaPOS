
using System.ComponentModel.DataAnnotations;

namespace EtnaPOS.DAL.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }    
        public ICollection<Order> Orders { get; set; }
        public DateTime Date { get; set; }
        public bool IsOpen { get; set; }

    }
}

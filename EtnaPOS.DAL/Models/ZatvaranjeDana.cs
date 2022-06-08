using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;

namespace EtnaPOS.DAL.Models
{
    public class ZatvaranjeDana
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ZatvaranjeDana()
        {
            
        }
        public ZatvaranjeDana(DateTime date)
        {
            Date = date;
            IsClosed = false;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EtnaPOS.DAL.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

namespace EtnaPOS.DAL.Models
{
    public class KategorijaArtikla : BaseEntity
    {
        public string Kategorija { get; set; }
        public int ParentId { get; set; }
    }
}

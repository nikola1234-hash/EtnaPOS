namespace EtnaPOS.DAL.Models
{
    public class Artikal : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int KategorijaArtiklaId { get; set; }
        public KategorijaArtikla KategorijaArtikla { get; set; }
        public bool IsActive { get; set; }
        public Artikal()
        {

        }
        public Artikal(string name, double price, int kategorijaId)
        {
            Name = name;
            Price = (Decimal)price;
            KategorijaArtiklaId = kategorijaId;
            IsActive = true;
        }
        public Artikal(int id, string name, double price, int kategorijaId)
        {
            Id = id;
            Name = name;
            Price = (Decimal)price;
            KategorijaArtiklaId = kategorijaId;
            IsActive = true;
        }
    }
}

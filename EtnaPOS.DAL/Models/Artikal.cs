using System.Globalization;

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

        public static Artikal FromCsv(string csv, int kategorijaId)
        {
            string[] values = csv.Split(new [] { ',' }, 2);

            double price = double.Parse(values[1]);
            
            Artikal artikal = new Artikal(values[0], price, kategorijaId);
            artikal.CreatedBy = "System";
            artikal.DateCreated = DateTime.Now;
            return artikal;
        }
    }
}

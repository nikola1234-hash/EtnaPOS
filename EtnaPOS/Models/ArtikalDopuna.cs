namespace EtnaPOS.Models
{
    public class ArtikalDopuna
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public ArtikalDopuna(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}

using EtnaPOS.DAL.DataAccess;

namespace EtnaPOS.ViewModels
{
    public class TableViewModel : BaseViewModel
    {
        public int TableId { get; set; }
        private EtnaDbContext db => App.GetService<EtnaDbContext>();
        public TableViewModel(int tableId)
        {
            TableId = tableId;
        }
    }
}

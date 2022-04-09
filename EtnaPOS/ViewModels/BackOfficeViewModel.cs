using EtnaPOS.Models;
using System.Collections.Generic;

namespace EtnaPOS.ViewModels
{
    public class BackOfficeViewModel : BaseViewModel
    {
        public List<Category> Categories { get; set; }
        public BackOfficeViewModel()
        {
            Categories = Node.GetNodes();
        }
    }
}

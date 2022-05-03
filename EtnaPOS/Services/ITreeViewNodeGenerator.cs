using EtnaPOS.Models;
using System.Collections.ObjectModel;

namespace EtnaPOS.Services
{
    public interface ITreeViewNodeGenerator
    {
        ObservableCollection<Node> GenerateNodes();
    }
}
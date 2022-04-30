using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EtnaPOS.Models
{
    [Serializable]
    public class BaseModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;



        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void Dispose()
        {

        }
    }
}

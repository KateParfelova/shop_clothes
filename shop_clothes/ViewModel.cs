using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetValue<T>(ref T fieldValue, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(fieldValue, newValue)) return false;
            fieldValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

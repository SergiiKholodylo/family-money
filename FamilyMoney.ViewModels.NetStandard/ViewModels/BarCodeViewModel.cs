using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.ViewModels.NetStandard.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public sealed class BarCodeViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<IBarCode> _barCodes = new ObservableCollection<IBarCode>();
        private readonly IBarCodeStorage _storage;

        public BarCodeViewModel(IBarCodeStorage storage)
        {
            _storage = storage;
            LoadBarCodes();
        }

        public ObservableCollection<IBarCode> BarCodes => _barCodes;

        public void LoadBarCodes()
        {
            _barCodes.Clear();
            var barCodes = _storage.GetAllBarCodes();
            foreach (var barCode in barCodes)
            {
                _barCodes.Add(barCode);
            }
        }

        public void DeleteBarCode(IBarCode barCode)
        {
            _storage.DeleteBarCode(barCode);
            _barCodes.Remove(barCode);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.ViewModels.NetStandard.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

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
            _barCodes.CollectionChanged += _barCodes_CollectionChanged;
            
        }

        private void _barCodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        public ObservableCollection<IBarCode> BarCodes => _barCodes;

        private void LoadBarCodes()
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

        public void InverseIsWeight(IBarCode barCode)
        {
            barCode.IsWeight = !barCode.IsWeight;
            _storage.UpdateBarCode(barCode);
            LoadBarCodes();


        }
    }


}

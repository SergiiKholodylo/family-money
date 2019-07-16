using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels
{
    public class CategoryViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<ICategory> _categories;
        private readonly ICategoryManager _manager;

        public ObservableCollection<ICategory> Categories
        {
            set { _categories = value; OnPropertyChanged();}
            get => _categories;
        }

        public CategoryViewModel()
        {
            _manager = MainPage.GlobalSettings.CategoryManager;
            Categories = new ObservableCollection<ICategory>(_manager.GetAllCategories());
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void AddCategory()
        {
            Categories.Add(_manager.CreateCategory("Category","Description"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FamilyMoney.UWP.Annotations;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class AddCategoryViewModel:INotifyPropertyChanged
    {
        private string _name;
        private string _description;

        public string Name
        {
            set { _name = value; OnPropertyChanged();}
            get => _name;
        }

        public string Description
        {
            set { _description = value; OnPropertyChanged();}
            get => _description;
        }

        public void CreateNewCategory()
        {
            var manager = MainPage.GlobalSettings.CategoryManager;
            manager.CreateCategory(Name, Description);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

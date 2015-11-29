using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace TodoList.UWP.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private string newItemText;

        public MainPageViewModel()
        {
            AddNewItemCommand = new DelegateCommand(AddNewItem, () => !string.IsNullOrWhiteSpace(newItemText));
            Items = new ObservableCollection<ItemViewModel>();
        }

        public string NewItemText
        {
            get { return newItemText; }
            set { SetProperty(ref newItemText, value); }
        }

        public ICommand AddNewItemCommand { get; }

        public ObservableCollection<ItemViewModel> Items { get; set; }

        private void AddNewItem()
        {
            var item = new ItemViewModel { Text = newItemText };
            Items.Add(item);
            NewItemText = string.Empty;
        }
    }
}

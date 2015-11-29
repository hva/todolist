using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using TodoList.UWP.Data.Interfaces;
using TodoList.UWP.Data.Models;
using TodoList.UWP.ViewModels.MainPage;

namespace TodoList.UWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string newItemText;
        private readonly ICommand changeItemStateCommand;
        private readonly IItemsRepository itemsRepository;

        public MainPageViewModel(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;

            changeItemStateCommand = new DelegateCommand<ItemViewModel>(ChangeItemState);
            AddNewItemCommand = DelegateCommand.FromAsyncHandler(AddNewItemAsync, () => !string.IsNullOrWhiteSpace(newItemText));
            Items = new ObservableCollection<ItemViewModel>();
        }

        public ICommand AddNewItemCommand { get; }
        public ObservableCollection<ItemViewModel> Items { get; }

        public string NewItemText
        {
            get { return newItemText; }
            set { SetProperty(ref newItemText, value); }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var todos = await itemsRepository.GetTodosAsync();
            var viewModels = todos.Select(x => new ItemViewModel(x, false, changeItemStateCommand));
            Items.AddRange(viewModels);
        }

        private async Task AddNewItemAsync()
        {
            var item = new Item { Text = newItemText };
            NewItemText = string.Empty;

            await itemsRepository.CreateAsync(item);

            var viewModel = new ItemViewModel(item, false, changeItemStateCommand);
            Items.Insert(0, viewModel);
        }

        private void ChangeItemState(ItemViewModel viewModel)
        {
            
        }
    }
}

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
        private readonly ItemsListSeparator separator;
        private readonly IItemsRepository itemsRepository;

        public MainPageViewModel(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;

            separator = new ItemsListSeparator();
            AddNewItemCommand = DelegateCommand.FromAsyncHandler(AddNewItemAsync, () => !string.IsNullOrWhiteSpace(newItemText));
            Items = new ObservableCollection<object>();
        }

        public ICommand AddNewItemCommand { get; }
        public ObservableCollection<object> Items { get; }

        public string NewItemText
        {
            get { return newItemText; }
            set { SetProperty(ref newItemText, value); }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var list = await itemsRepository.GetTodoAsync();
            var todos = list.Select(CreateTodo);
            Items.AddRange(todos);

            Items.Add(separator);

            list = await itemsRepository.GetDoneAsync();
            var done = list.Select(CreateDone);
            Items.AddRange(done);
        }

        private async Task AddNewItemAsync()
        {
            var item = new Item { Text = newItemText };
            NewItemText = string.Empty;

            await itemsRepository.CreateAsync(item);

            var viewModel = CreateTodo(item);
            Items.Insert(0, viewModel);
        }

        private async void ChangeItemStateAsync(ItemViewModel viewModel)
        {
            var isDone = viewModel.IsDone;
            await itemsRepository.SetIsDoneAsync(viewModel.Item.Guid, isDone);

            Items.Remove(viewModel);
            var separatorIndex = Items.IndexOf(separator);
            if (isDone)
            {
                Items.Insert(separatorIndex + 1, CreateDone(viewModel.Item));
            }
            else
            {
                Items.Insert(separatorIndex, CreateTodo(viewModel.Item));
            }
        }

        private ItemViewModel CreateTodo(Item item)
        {
            return new ItemViewModel(item, false, ChangeItemStateAsync);
        }
        private ItemViewModel CreateDone(Item item)
        {
            return new ItemViewModel(item, true, ChangeItemStateAsync);
        }
    }
}

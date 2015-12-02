using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using TodoList.UWP.Data;
using TodoList.UWP.Models;

namespace TodoList.UWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string newItemText;
        private Guid? lastOperationId;
        //private readonly ItemsListSeparator separator;
        private readonly IDataRepository dataRepository;

        public MainPageViewModel()
        {
            dataRepository = new DataRepository();

            //separator = new ItemsListSeparator();
            AddNewItemCommand = DelegateCommand<KeyRoutedEventArgs>.FromAsyncHandler(AddNewItemAsync, CanAddNewItem);
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
            var feed = await dataRepository.GetFeedAsync();
            lastOperationId = feed.LastOperationId;
            Items.AddRange(feed.Items);
            //Items.Add(separator);
        }

        private bool CanAddNewItem(KeyRoutedEventArgs e)
        {
            return !string.IsNullOrWhiteSpace(newItemText);
        }

        private async Task AddNewItemAsync(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var operation = new Operation
                {
                    Type = OperationType.Create,
                    Value = newItemText,
                };
                NewItemText = string.Empty;

                var operations = await dataRepository.PostOperationsAsync(lastOperationId, operation);
                lastOperationId = operations.Last().Id;

                Items.Merge(operations);
            }
        }
    }
}

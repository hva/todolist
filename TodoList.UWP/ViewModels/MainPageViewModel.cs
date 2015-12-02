using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
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
        private bool isBusy;
        private string newItemText;
        private Guid? lastOperationId;
        private DispatcherTimer timer;

        private readonly IDataRepository dataRepository;

        public MainPageViewModel()
        {
            dataRepository = new DataRepository();

            //separator = new ItemsListSeparator();
            AddNewItemCommand = DelegateCommand<KeyRoutedEventArgs>.FromAsyncHandler(AddNewItemAsync, CanAddNewItem);
            Items = new ObservableCollection<Item>();
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        }

        public ICommand AddNewItemCommand { get; }
        public ObservableCollection<Item> Items { get; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string NewItemText
        {
            get { return newItemText; }
            set { SetProperty(ref newItemText, value); }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            IsBusy = true;
            var feed = await dataRepository.GetFeedAsync();
            IsBusy = false;

            lastOperationId = feed.LastOperationId;

            Items.CollectionChanged -= OnCollectionChanged;
            Items.AddRange(feed.Items);
            Items.CollectionChanged += OnCollectionChanged;

            timer.Tick += Refresh;
            timer.Start();
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

                IsBusy = true;
                var operations = await dataRepository.PostOperationsAsync(lastOperationId, operation);
                IsBusy = false;

                lastOperationId = operations.Last().Id;
                Items.CollectionChanged -= OnCollectionChanged;
                Items.Merge(operations);
                Items.CollectionChanged += OnCollectionChanged;
            }
        }

        private async void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;
            if (e.NewItems == null || e.NewItems.Count == 0) return;

            var item = e.NewItems[0] as Item;
            if (item == null) return;

            var operation = new Operation
            {
                Type = OperationType.Reorder,
                ItemId = item.Id,
                Value = e.NewStartingIndex.ToString(),
            };

            IsBusy = true;
            var operations = await dataRepository.PostOperationsAsync(lastOperationId, operation);
            IsBusy = false;

            lastOperationId = operations.Last().Id;

            Items.CollectionChanged -= OnCollectionChanged;
            Items.Merge(operations);
            Items.CollectionChanged += OnCollectionChanged;
        }

        private async void Refresh(object sender, object e)
        {
            timer.Tick -= Refresh;

            IsBusy = true;
            var operations = await dataRepository.GetOperationsAsync(lastOperationId);
            IsBusy = false;

            if (operations.Count > 0)
            {
                lastOperationId = operations.Last().Id;

                Items.CollectionChanged -= OnCollectionChanged;
                Items.Merge(operations);
                Items.CollectionChanged += OnCollectionChanged;
            }

            timer.Tick += Refresh;
        }
    }
}

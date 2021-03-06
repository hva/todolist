﻿using System;
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
using TodoList.UWP.ViewModels.MainPage;

namespace TodoList.UWP.ViewModels
{
    // MainPageViewModel is data context of MainPage.
    public class MainPageViewModel : ViewModelBase
    {
        private bool isBusy;
        private string newItemText;
        private Guid? lastOperationId;

        private readonly DispatcherTimer timer;
        private readonly IDataRepository dataRepository;

        public MainPageViewModel()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            dataRepository = new DataRepository();

            AddNewItemCommand = DelegateCommand<KeyRoutedEventArgs>.FromAsyncHandler(AddNewItemAsync, CanAddNewItem);
            Items = new ObservableCollection<ItemViewModel>();
        }

        // Executed when user presses Enter key in TextBox.
        public ICommand AddNewItemCommand { get; }

        // Collection of ItemViewModel items.
        public ObservableCollection<ItemViewModel> Items { get; }

        // Is used to show/hide progress bar.
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

        // Loads items from data repository.
        // Subscribes to collection changes.
        // Starts refresh data timer.
        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            IsBusy = true;
            var feed = await dataRepository.GetFeedAsync();
            IsBusy = false;

            lastOperationId = feed.LastOperationId;

            Items.Init(feed.Items, OnStatusChanged);
            Items.CollectionChanged += OnCollectionChanged;

            timer.Tick += Refresh;
            timer.Start();
        }

        // Returns true if AddNewItem command could be executed.
        private bool CanAddNewItem(KeyRoutedEventArgs e)
        {
            return !string.IsNullOrWhiteSpace(newItemText);
        }

        // Sends new 'Create' operation to data repository and updates UI.
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

                Merge(operations);
            }
        }

        // Catches reorder operations from UI, corrects position according to 'IsComplete' status,
        // sends 'Reorder' operation to data repository and updates UI.
        private async void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;
            if (e.NewItems == null || e.NewItems.Count == 0) return;

            var item = e.NewItems[0] as ItemViewModel;
            if (item == null) return;

            var newIndex = CorrectIndex(e.NewStartingIndex, item);

            var operation = new Operation
            {
                Type = OperationType.Reorder,
                ItemId = item.Item.Id,
                Value = newIndex.ToString(),
            };

            IsBusy = true;
            var operations = await dataRepository.PostOperationsAsync(lastOperationId, operation);
            IsBusy = false;

            Merge(operations);
        }

        // Periodically requests data repository for a new operations
        // and updates UI.
        private async void Refresh(object sender, object e)
        {
            timer.Tick -= Refresh;

            IsBusy = true;
            var operations = await dataRepository.GetOperationsAsync(lastOperationId);
            IsBusy = false;

            Merge(operations);

            timer.Tick += Refresh;
        }

        // Updates items according to operations list.
        private void Merge(List<Operation> operations)
        {
            if (operations.Count > 0)
            {
                lastOperationId = operations.Last().Id;

                Items.CollectionChanged -= OnCollectionChanged;
                Items.Merge(operations, OnStatusChanged);
                Items.CollectionChanged += OnCollectionChanged;
            }
        }

        // Sends 'StatusChange' operation to data repository
        // and updates UI.
        private async void OnStatusChanged(Item item)
        {
            var operation = new Operation
            {
                Type = OperationType.ChangeStatus,
                ItemId = item.Id,
                Value = item.IsComplete.ToString(),
            };

            IsBusy = true;
            var operations = await dataRepository.PostOperationsAsync(lastOperationId, operation);
            IsBusy = false;

            Merge(operations);
        }

        // Corrects item position according to 'IsComplete' status.
        private int CorrectIndex(int index, ItemViewModel item)
        {
            var minIndex = Items.Count(x => !x.IsComplete);
            if (item.IsComplete && index < minIndex)
            {
                Items.CollectionChanged -= OnCollectionChanged;
                Items.RemoveAt(index);
                Items.Insert(minIndex, item);
                Items.CollectionChanged += OnCollectionChanged;
                return minIndex;
            }

            var maxIndex = minIndex - 1;
            if (!item.IsComplete && index > maxIndex)
            {
                Items.CollectionChanged -= OnCollectionChanged;
                Items.RemoveAt(index);
                Items.Insert(maxIndex, item);
                Items.CollectionChanged += OnCollectionChanged;
                return maxIndex;
            }

            return index;
        }
    }
}

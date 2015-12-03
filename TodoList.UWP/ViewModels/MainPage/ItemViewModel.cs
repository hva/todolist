using System;
using Prism.Mvvm;
using TodoList.UWP.Models;

namespace TodoList.UWP.ViewModels.MainPage
{
    // ItemViewModel is a wrapper around Item.
    // It is used to catch Item status changes from UI and code.
    public class ItemViewModel : BindableBase
    {
        public Item Item { get; set; }

        public Action<Item> OnStatusChanged { get; set; }

        // Catches 'IsComplete' status change from UI
        // and updates Item model.
        public bool IsComplete
        {
            get { return Item.IsComplete; }
            set
            {
                if (Item.IsComplete != value)
                {
                    Item.IsComplete = value;
                    if (OnStatusChanged != null)
                    {
                        OnStatusChanged(Item);
                    }
                }
            }
        }

        // Catches 'IsComplete' status change from code
        // and updates UI.
        public void SetIsComplete(bool value)
        {
            Item.IsComplete = value;
            OnPropertyChanged(() => IsComplete);
        }
    }
}

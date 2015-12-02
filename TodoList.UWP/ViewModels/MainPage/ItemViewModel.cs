using System;
using Prism.Mvvm;
using TodoList.UWP.Models;

namespace TodoList.UWP.ViewModels.MainPage
{
    public class ItemViewModel : BindableBase
    {
        public Item Item { get; set; }

        public Action<Item> OnStatusChanged { get; set; }

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

        public void SetIsComplete(bool value)
        {
            Item.IsComplete = value;
            OnPropertyChanged(() => IsComplete);
        }
    }
}

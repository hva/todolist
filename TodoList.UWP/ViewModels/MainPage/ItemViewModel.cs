using System;
using Prism.Mvvm;
using TodoList.UWP.Data.Models;

namespace TodoList.UWP.ViewModels.MainPage
{
    public class ItemViewModel : BindableBase
    {
        private bool isDone;
        private readonly Action<ItemViewModel> updateState;

        public ItemViewModel(Item item, bool isDone, Action<ItemViewModel> updateState)
        {
            Item = item;
            this.isDone = isDone;
            this.updateState = updateState;
        }

        public Item Item { get; }

        public bool IsDone
        {
            get { return isDone; }
            set
            {
                isDone = value;
                if (updateState != null)
                {
                    updateState(this);
                }
            }
        }
    }
}

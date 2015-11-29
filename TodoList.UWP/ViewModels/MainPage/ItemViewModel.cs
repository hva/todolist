using System.Windows.Input;
using TodoList.UWP.Data.Models;

namespace TodoList.UWP.ViewModels.MainPage
{
    public class ItemViewModel
    {
        public ItemViewModel(Item item, ICommand changeStateCommand)
        {
            Item = item;
            ChangeStateCommand = changeStateCommand;
        }

        public Item Item { get; }
        public ICommand ChangeStateCommand { get; }
    }
}

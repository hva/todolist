using System.Windows.Input;
using TodoList.UWP.Data.Models;

namespace TodoList.UWP.ViewModels.MainPage
{
    public class ItemViewModel
    {
        public ItemViewModel(Item item, bool isDone, ICommand changeStateCommand)
        {
            Item = item;
            IsDone = isDone;
            ChangeStateCommand = changeStateCommand;
        }

        public Item Item { get; }
        public bool IsDone { get; }
        public ICommand ChangeStateCommand { get; }
    }
}

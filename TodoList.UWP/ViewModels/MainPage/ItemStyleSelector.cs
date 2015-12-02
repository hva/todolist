using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoList.UWP.ViewModels.MainPage
{
    public class ItemStyleSelector : StyleSelector
    {
        public Style TodoStyle { get; set; }
        public Style CompleteStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var vm = item as ItemViewModel;
            if (vm != null)
            {
                return vm.IsComplete ? CompleteStyle : TodoStyle;
            }

            return base.SelectStyleCore(item, container);
        }
    }
}

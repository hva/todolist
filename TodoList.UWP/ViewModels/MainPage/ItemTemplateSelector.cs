using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TodoList.UWP.Models;

namespace TodoList.UWP.ViewModels.MainPage
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Item)
            {
                return ItemTemplate;
            }

            return SeparatorTemplate;
        }
    }
}

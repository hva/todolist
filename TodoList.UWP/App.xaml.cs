using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Prism.Unity.Windows;
using TodoList.UWP.Constants;

namespace TodoList.UWP
{
    sealed partial class App : PrismUnityApplication
    {
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageName.Main, null);
            return Task.FromResult<object>(null);
        }
    }
}

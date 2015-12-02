using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Windows;
using TodoList.UWP.Constants;

namespace TodoList.UWP
{
    sealed partial class App : PrismApplication
    {
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageName.Main, null);
            return Task.FromResult<object>(null);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            // title bar colors fix
            ApplicationView.GetForCurrentView().Init();

            return rootFrame;
        }
    }
}

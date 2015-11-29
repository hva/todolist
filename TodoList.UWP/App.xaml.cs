using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using TodoList.UWP.Constants;
using TodoList.UWP.Data;
using TodoList.UWP.Data.Interfaces;

namespace TodoList.UWP
{
    sealed partial class App : PrismUnityApplication
    {
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageName.Main, null);
            return Task.FromResult<object>(null);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            ApplicationView.GetForCurrentView().Init();
            return rootFrame;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<IItemsRepository, ItemsRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IStorageService, StorageService>(new ContainerControlledLifetimeManager());
        }
    }
}

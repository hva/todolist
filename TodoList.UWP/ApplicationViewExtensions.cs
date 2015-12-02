using Windows.UI;
using Windows.UI.ViewManagement;

namespace TodoList.UWP
{
    public static class ApplicationViewExtensions
    {
        public static void Init(this ApplicationView applicationView)
        {
            var foreground = Colors.White;
            var background = Color.FromArgb(255, 63, 128, 124);
            var hover = Color.FromArgb(255, 56, 115, 111);

            var titleBar = applicationView.TitleBar;

            titleBar.ForegroundColor = foreground;
            titleBar.BackgroundColor = background;

            titleBar.ButtonForegroundColor = foreground;
            titleBar.ButtonBackgroundColor = background;

            titleBar.ButtonHoverForegroundColor = foreground;
            titleBar.ButtonHoverBackgroundColor = hover;

            titleBar.ButtonPressedForegroundColor = foreground;
            titleBar.ButtonPressedBackgroundColor = hover;

            titleBar.ButtonInactiveForegroundColor = foreground;
            titleBar.ButtonInactiveBackgroundColor = background;

            titleBar.InactiveForegroundColor = foreground;
            titleBar.InactiveBackgroundColor = background;
        }
    }
}

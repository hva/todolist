using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;

namespace TodoList.UWP.Behaviors
{
    public class TextBoxBehavior : DependencyObject, IBehavior
    {
        #region EnterPressedCommand

        public ICommand EnterPressedCommand
        {
            get { return (ICommand)GetValue(EnterPressedCommandProperty); }
            set { SetValue(EnterPressedCommandProperty, value); }
        }

        public static readonly DependencyProperty EnterPressedCommandProperty =
            DependencyProperty.Register(nameof(EnterPressedCommand), typeof(ICommand), typeof(TextBoxBehavior), new PropertyMetadata(null));

        #endregion

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            var textBox = associatedObject as TextBox;
            if (textBox != null)
            {
                textBox.Loaded += OnLoaded;
                textBox.Unloaded += OnUnloaded;
            }
        }

        public void Detach()
        {
            var textBox = AssociatedObject as TextBox;
            if (textBox != null)
            {
                textBox.Loaded -= OnLoaded;
                textBox.Unloaded -= OnUnloaded;
            }
        }

        public DependencyObject AssociatedObject { get; private set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.KeyDown += OnKeyDown;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.KeyDown -= OnKeyDown;
            }
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TryExecuteCommand(EnterPressedCommand, null);
            }
        }

        private void TryExecuteCommand(ICommand command, object parameter)
        {
            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
    }
}

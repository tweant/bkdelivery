using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using BKDelivery.Courier.Helpers;
using BKDelivery.Courier.ViewModel;

namespace BKDelivery.Courier.Model
{
    public class DialogService : IDialogService
    {
        private Border _dialogbox;
        private TextBlock _message;
        private ProgressBar _progress;
        private System.Windows.Threading.DispatcherTimer _dispatcherTimer;
        private int _curValue;
        private int _interval; //miliseconds
        private int _time; //miliseconds
        private DialogType _type;

        //Main grid
        private StackPanel panel;

        //Times
        private Duration halfSecond = new Duration(new TimeSpan(0, 0, 0, 0, 500));

        //Styles
        private static readonly Color BlackColor = Color.FromRgb(36, 36, 36);
        private static readonly Color WhiteColor = Color.FromRgb(255, 255, 255);
        private readonly SolidColorBrush _blackBrush = new SolidColorBrush(BlackColor);
        private readonly SolidColorBrush _whiteBrush = new SolidColorBrush(WhiteColor);

        private int _totalCount;

        private int Create(DialogType type, string message)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, 1.0, halfSecond);
            //doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, UIElement.OpacityProperty);
            //doubleAnimation.AutoReverse = true;

            //Storyboard storyboard = new Storyboard();
            //storyboard.Children.Add(doubleAnimation);

            //BeginStoryboard beginStoryboard = new BeginStoryboard();
            //beginStoryboard.Storyboard = storyboard;

            //EventTrigger trigger = new EventTrigger(FrameworkElement.LoadedEvent);
            //trigger.Actions.Add(beginStoryboard);

            Border border = new Border();
            border.SetValue(Grid.RowProperty, 2);
            border.HorizontalAlignment = HorizontalAlignment.Right;
            border.VerticalAlignment = VerticalAlignment.Bottom;
            border.Background = _blackBrush;
            border.BorderBrush = _blackBrush;
            border.BorderThickness = new Thickness(4);
            border.Margin = new Thickness(2);
            border.SetValue(Panel.ZIndexProperty, 99);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = message;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.FontFamily = new FontFamily("Segoe UI");
            textBlock.FontSize = 13;
            textBlock.Margin = new Thickness(10);
            textBlock.Foreground = _whiteBrush;
            textBlock.MaxWidth = 400;

            ProgressBar progressBar = new ProgressBar();
            progressBar.Minimum = 0;
            progressBar.Maximum = 10;
            progressBar.Height = 4;
            progressBar.IsIndeterminate = true;

            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(progressBar);
            border.Child = stackPanel;

            var locator = Application.Current.FindResource("Locator") as ViewModelLocator;
            locator.Main.NotificationsCollection.Add(border);

            border.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);

            return _totalCount++;
        }

        public void Show(DialogType type, string message)
        {
            if (_totalCount == 0)
                Create(DialogType.BusyWaiting, "Connecting to \"268770.database.windows.net\" database. Please wait.");
            else
                Create(DialogType.BusyWaiting, _totalCount.ToString());
            //_type = type;
            //if (message != null)
            //    if (EnsureDialog())
            //    {
            //        switch (type)
            //        {
            //            case DialogType.Error:
            //                _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 61, 61));
            //                break;
            //            case DialogType.Success:
            //                _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(77, 153, 0));
            //                break;
            //            case DialogType.Warning:
            //                _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(245, 184, 0));
            //                break;
            //            case DialogType.BusyWaiting:
            //                _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(245, 184, 0));
            //                break;
            //        }

            //        _message.Text = message;
            //        _dialogbox.Visibility = Visibility.Visible;

            //        if (_type == DialogType.BusyWaiting)
            //            _progress.IsIndeterminate = true;
            //        else
            //            _progress.IsIndeterminate = false;
            //        _progress.Visibility = Visibility.Visible;
            //        _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //        _dispatcherTimer.Tick += dispatcherTimer_Tick;
            //        _interval = 100;
            //        _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, _interval);
            //        _time = 5000;
            //        _progress.Value = 0;
            //        _progress.Maximum = _time;
            //        _dispatcherTimer.Start();
            //    }
        }

        public void Hide()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= dispatcherTimer_Tick;
            _dispatcherTimer.Tick += dispatcherTimer_TickClose;
            _progress.Value = _time;
            _dispatcherTimer.Start();
        }

        private void dispatcherTimer_TickClose(object sender, EventArgs e)
        {
            _dialogbox.Visibility = Visibility.Collapsed;
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Tick -= dispatcherTimer_TickClose;
            _curValue = 0;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_type == DialogType.BusyWaiting && _curValue >= 0.9*_time)
            {
                return;
            }
            _curValue += _interval;
            _progress.Value = _curValue;

            if (_curValue > _time)
            {
                _dialogbox.Visibility = Visibility.Collapsed;
                _dispatcherTimer.Stop();
                _curValue = 0;
            }
        }

        private bool EnsureDialog()
        {
            if (_dialogbox != null) return true;
            var appShell = Application.Current.MainWindow as MainWindow;
            if (appShell != null)
            {
                //_dialogbox = appShell.DialogBox;
                //_message = appShell.DialogBoxMessage;
                //_progress = appShell.DialogProgressBar;
            }
            return _dialogbox != null && _message != null && _progress != null;
        }
    }
}
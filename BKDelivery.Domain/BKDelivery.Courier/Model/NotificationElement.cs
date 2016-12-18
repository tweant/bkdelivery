using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using BKDelivery.Courier.Helpers;

namespace BKDelivery.Courier.Model
{
    public class NotificationElement
    {
        private readonly Duration halfSecond = new Duration(new TimeSpan(0, 0, 0, 0, 500));

        //UIElement Values
        public double ProgressBarValue { get; set; }

        //Internal
        private readonly Border _border;
        private readonly ProgressBar _progressBar;
        private readonly DoubleAnimation _enterAnimation;
        private readonly DoubleAnimation _exitAnimation;
        private readonly ObservableCollection<UIElement> _notificationsCollection;
        private System.Windows.Threading.DispatcherTimer _dispatcherTimer;
        private int _curValue;
        private int _interval; //miliseconds
        private readonly int? _time; //miliseconds
        private readonly DialogType _type;

        public NotificationElement(DialogType type, string message,ObservableCollection<UIElement> notificationsCollection, int? time =null)
        {
            _time = time ?? Constants.NotificationShowTime;
            _type = type;
            _notificationsCollection = notificationsCollection;
            _enterAnimation = new DoubleAnimation(0.0, 1.0, halfSecond);
            _exitAnimation = new DoubleAnimation(1.0, 0.0, halfSecond);

            _border = new Border();
            _border.SetValue(Grid.RowProperty, 2);
            _border.HorizontalAlignment = HorizontalAlignment.Right;
            _border.VerticalAlignment = VerticalAlignment.Bottom;
            _border.Background = Constants.BlackBrush;
            _border.BorderBrush = Constants.BlackBrush;
            _border.BorderThickness = new Thickness(4);
            _border.Margin = new Thickness(2);
            _border.SetValue(Panel.ZIndexProperty, 10);

            StackPanel stackPanel = new StackPanel {Orientation = Orientation.Vertical};

            TextBlock textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13,
                Margin = new Thickness(10),
                Foreground = Constants.WhiteBrush,
                MaxWidth = 400
            };

            _progressBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = 10,
                Height = 4,
                IsIndeterminate = type == DialogType.BusyWaiting
            };

            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(_progressBar);
            _border.Child = stackPanel;
        }

        public void BeginEnterAnimation()
        {
            _notificationsCollection.Add(_border);
            _border.BeginAnimation(UIElement.OpacityProperty, _enterAnimation);
            if (_type != DialogType.BusyWaiting)
            {
                _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                _dispatcherTimer.Tick += dispatcherTimer_Tick;
                _interval = 10;
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, _interval);
                _progressBar.Value = 0;
                _progressBar.Maximum = (int)_time;
                _dispatcherTimer.Start();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _curValue += _interval;
            _progressBar.Value = _curValue;

            if (_curValue > _time)
            {
                _dispatcherTimer.Stop();
                _curValue = 0;
                this.Hide();
            }
        }

        public void BeginExitAnimation()
        {
            _border.BeginAnimation(UIElement.OpacityProperty, _exitAnimation);
        }

        public void Hide()
        {
            this.BeginExitAnimation();
            if (_dispatcherTimer != null && _dispatcherTimer.IsEnabled)
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer = null;
            }
            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_TickDelete;
            _interval = 500;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, _interval);
            _dispatcherTimer.Start();
        }

        private void dispatcherTimer_TickDelete(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            _notificationsCollection.Remove(_border);
        }
    }
}

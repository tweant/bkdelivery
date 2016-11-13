using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BKDelivery.CallCenter.Helpers;

namespace BKDelivery.CallCenter.Model
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

        public void Show(DialogType type, string message)
        {
            _type = type;
            if(message!=null)
                if (EnsureDialog())
                {
                    switch (type)
                    {
                        case DialogType.Error:
                            _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 61, 61));
                            break;
                        case DialogType.Success:
                            _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(77,153,0));
                            break;
                        case DialogType.Warning:
                            _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(245,184,0));
                            break;
                        case DialogType.BusyWaiting:
                            _dialogbox.BorderBrush = new SolidColorBrush(Color.FromRgb(245, 184, 0));
                            break;
                    }
                    
                    _message.Text = message;
                    _dialogbox.Visibility = Visibility.Visible;

                    if (type == DialogType.BusyWaiting)
                    {
                        _progress.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        _progress.Visibility = Visibility.Visible;
                        _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                        _dispatcherTimer.Tick += dispatcherTimer_Tick;
                        _interval = 100;
                        _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, _interval);
                        _time = 5000;
                        _progress.Value = 0;
                        _progress.Maximum = _time;
                        _dispatcherTimer.Start();
                    }
                }
        }

        public void Hide()
        {
            _dialogbox.Visibility = Visibility.Collapsed;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
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
                _dialogbox = appShell.DialogBox;
                _message = appShell.DialogBoxMessage;
                _progress = appShell.DialogProgressBar;
            }
            return _dialogbox != null && _message!=null && _progress!=null;
        }
    }
}

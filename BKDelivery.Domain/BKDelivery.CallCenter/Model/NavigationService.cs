using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BKDelivery.CallCenter.Model
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Uri> _pagesDictionary;
        private Frame _mainFrame;

        public const string UnknownPageKey = "-- UNKNOWN --";

        public NavigationService()
        {
            _pagesDictionary = new Dictionary<string, Uri>();
        }

        public void Configure(string key, Uri pageType)
        {
            if (key == null || pageType == null) return;
            Uri value;
            if (_pagesDictionary.TryGetValue(key, out value))
            {
                if (value != pageType)
                {
                    throw new Exception(
                        $"Attempt to add a page of type '{pageType}' to already existing pair of '{key}:{value}'. Consider another string.");
                }
            }
            else
            {
                _pagesDictionary.Add(key, pageType);
            }
        }


        public void GoBack()
        {
            if (EnsureMainFrame() && _mainFrame.CanGoBack)
                _mainFrame.GoBack();
        }

        public void NavigateTo(string pageKey)
        {
            if (pageKey == null) return;
            Uri page;
            if (_pagesDictionary.TryGetValue(pageKey, out page))
            {
                if (EnsureMainFrame())
                {
                    _mainFrame.Navigate(page, null);
                }
            }
            else
            {
                throw new Exception($"There is no page associated with string '{pageKey}'.");
            }
        }

        public void NavigateTo(string pageKey, object context)
        {
            if (pageKey == null) return;
            Uri page;
            if (_pagesDictionary.TryGetValue(pageKey, out page))
            {
                if (EnsureMainFrame())
                {
                    _mainFrame.Navigate(page, context);
                }
            }
            else
            {
                throw new Exception($"There is no page associated with string '{pageKey}'.");
            }
        }

        private bool EnsureMainFrame()
        {
            if (_mainFrame != null) return true;
            var appShell = Application.Current.MainWindow as MainWindow;
            if (appShell != null) _mainFrame = appShell.MainFrame;
            return _mainFrame != null;
        }

        public string CurrentPageKey
        {
            get
            {
                string key;
                try
                {
                    key = _pagesDictionary.FirstOrDefault(x => x.Key == _mainFrame.CurrentSource.ToString()).Key;
                }
                catch (NullReferenceException)
                {
                    return UnknownPageKey;
                }
                return key;
            }
        }
    }
}
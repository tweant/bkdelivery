using System;

namespace BKDelivery.Courier.Model
{
    public interface INavigationService
    {
        void Configure(string key, Uri pageUri);
        void GoBack();
        void NavigateTo(string pageKey);
        void NavigateTo(string pageKey, object context);
        string CurrentPageKey { get; }
        object Parameter { get; }

    }
}

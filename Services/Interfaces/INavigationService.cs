using System;

namespace PCStoreApp.Services.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo(string viewName);
        void NavigateTo(string viewName, object? parameter);
        bool CanNavigateBack { get; }
        void NavigateBack();
        event EventHandler<string>? NavigationRequested;
    }
}
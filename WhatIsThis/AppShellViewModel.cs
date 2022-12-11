using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace WhatIsThis.ViewModels
{
    public sealed partial class AppShellViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isInAdministratorMode;

        [ObservableProperty]
        public ICommand _toggleAdministratorModeCommand;

        public AppShellViewModel() 
        {
            _toggleAdministratorModeCommand = new Command(() => 
            { 
                IsInAdministratorMode = !IsInAdministratorMode;
                Shell.Current.FlyoutIsPresented = false;
            });
        }  
    }
}

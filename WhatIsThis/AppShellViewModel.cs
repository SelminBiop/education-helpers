using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace WhatIsThis.ViewModels
{
    public sealed class AppShellViewModel : ObservableObject
    {
        private bool _isInAdministratorMode;
        public bool IsInAdministratorMode 
        {
            get =>_isInAdministratorMode;
            set => SetProperty(ref _isInAdministratorMode, value);
        }

        public ICommand AdministratorModeCommand { get; set; }

        public AppShellViewModel() 
        {
            AdministratorModeCommand = new Command(() => IsInAdministratorMode = true);
        }  
    }
}

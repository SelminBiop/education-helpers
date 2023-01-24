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

        [ObservableProperty]
        public ICommand _showPrivacyPolicyCommand;

        public AppShellViewModel() 
        {
            _toggleAdministratorModeCommand = new Command(() => 
            { 
                IsInAdministratorMode = !IsInAdministratorMode;
                Shell.Current.FlyoutIsPresented = false;
            });

            _showPrivacyPolicyCommand = new Command(() => 
            {
                try 
                {
                    Uri uri = new Uri("https://github.com/SelminBiop/education-helpers/blob/main/PrivacyPolicy.txt");
                    Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                } catch (Exception ex) {
                    // An unexpected error occured. No browser may be installed on the device.
                }
            });
        }  
    }
}

using WhatIsThis.ViewModels;

namespace WhatIsThis;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel appShellViewModel)
	{
		InitializeComponent();
		BindingContext = appShellViewModel;
	}
}

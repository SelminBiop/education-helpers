using CommunityToolkit.Mvvm.ComponentModel;

public sealed class MainPageViewModel : ObservableObject
{

    private string _imageDescription = "Cute dot net bot waving hi to you!";
    public string ImageDescription
    {
        get => _imageDescription;
        set => SetProperty(ref _imageDescription, value);
    }

    private string _hello = "Hello Simon, go to bed!";
    public string Hello
    {
        get => _hello;
        set => SetProperty(ref _hello, value);
    }


    public MainPageViewModel()
    {

    }
}
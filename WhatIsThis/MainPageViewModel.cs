using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.QuestionType;

namespace WhatIsThis.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<IQuestionType> _questionTypes;

    public MainPageViewModel(IEnumerable<IQuestionType> questionTypes)
    {
        _questionTypes = questionTypes;
    }
}
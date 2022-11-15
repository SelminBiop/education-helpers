using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;
using WhatIsThis.QuestionType;

namespace WhatIsThis.ViewModels;

public sealed class MainPageViewModel : ObservableObject
{
    private IEnumerable<IQuestionType> _questionTypes;
    public IEnumerable<IQuestionType> QuestionTypes
    {
        get => _questionTypes;
        set => SetProperty(ref _questionTypes, value);
    }

    public MainPageViewModel(IEnumerable<IQuestionType> questionTypes)
    {
        QuestionTypes = questionTypes;
    }
}
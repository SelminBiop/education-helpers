using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhatIsThis.ViewModels;

public sealed class JeuPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";
    private const int NumberOfPossibleAnswers = 6;

    private IList<Association> _associations = new List<Association>();

    private string _result = string.Empty;
    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private string _wordToFind = string.Empty;
    public string WordToFind
    {
        get => _wordToFind;
        set => SetProperty(ref _wordToFind, value);
    }

    private ObservableCollection<AnswerItem> _possibleAnswers = new ObservableCollection<AnswerItem>();
    public ObservableCollection<AnswerItem> PossibleAnswers
    {
        get => _possibleAnswers;
        set => SetProperty(ref _possibleAnswers, value);
    }

    public JeuPageViewModel()
    {
        var associationsJson = Preferences.Get(AssociationsKey, string.Empty);
        if(associationsJson != string.Empty){
            _associations = JsonSerializer.Deserialize<List<Association>>(associationsJson);
        }
        for(int i = 0; i < NumberOfPossibleAnswers; i++)
        {
            _possibleAnswers.Add(new AnswerItem());
        }

        SetNextRound();
    }

    private void SetNextRound()
    {
        Random r = new Random();
        int rInt = r.Next(0, _associations.Count);

        var associationtoFind = _associations[rInt];
        WordToFind = associationtoFind.word;

        var otherAnswers = _associations.Where(association => association.word != WordToFind).ToList();

        int insertAnswer = r.Next(0, NumberOfPossibleAnswers);
        int currentIteration = 0;
        while(currentIteration < NumberOfPossibleAnswers)
        {
            var answerItem = _possibleAnswers[currentIteration];
            if(currentIteration == insertAnswer)
            {
                answerItem.SetAction(() => 
                {
                    Result = "Bonne réponse!";
                    MainThread.BeginInvokeOnMainThread(SetNextRound);
                });
                answerItem.SetResource(associationtoFind.correspondingResource);
            }
            else
            {
                int rInt2 = r.Next(0, otherAnswers.Count);
                var otherAnswer = otherAnswers[rInt2];
                answerItem.SetResource(otherAnswer.correspondingResource);
                answerItem.SetAction(() => 
                {
                    Result = "Mauvaise réponse! Essayez a nouveau";
                });
                otherAnswers.RemoveAt(rInt2);
            }
            currentIteration++;
        }
    }

    public sealed class AnswerItem : ObservableObject
    {
        private ImageSource _resource;
        public ImageSource Resource
        {
            get => _resource;
            set => SetProperty(ref _resource, value);
        }

        private ICommand _onImageTappedCommand;
        public ICommand OnImageTappedCommand
        {
            get => _onImageTappedCommand;
            set => SetProperty(ref _onImageTappedCommand, value);
        }

        public AnswerItem()
        {
        }

        public void SetAction(Action onAnswerSelectedAction)
        {
            OnImageTappedCommand = new Command(_ => onAnswerSelectedAction.Invoke());
        }

        public void SetResource(string resource)
        {
            Resource = ImageSource.FromFile(resource);
        }
    }
}
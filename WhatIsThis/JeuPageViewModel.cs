using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;

public sealed class JeuPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";
    private const int NumberOfPossibleAnswers = 4;
    private const int NumberOfTimesBeforeRemovingAssociation = 1;

    private IEnumerable<Association> _associations = new List<Association>();
    private IList<Association> _removedAssociations = new List<Association>();
    private Dictionary<Association, int> _associationTimesAskedCounter = new();

    private string _result = string.Empty;
    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private bool _showPossibleAnswers;
    public bool ShowPossibleAnswers
    {
        get => _showPossibleAnswers;
        set => SetProperty(ref _showPossibleAnswers, value);
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

    public JeuPageViewModel(IAssociationStorageService storageService)
    {
        _associations = storageService.Get(AssociationsKey);

        for(int i = 0; i < NumberOfPossibleAnswers; i++)
        {
            _possibleAnswers.Add(new AnswerItem());
        }

        SetupForNewGame();
    }

    public void SetupForNewGame()
    {
        Result = string.Empty;

        ShowPossibleAnswers = true;

        _removedAssociations.Clear();
        _associationTimesAskedCounter.Clear();

        SetNextRound();
    }

    private void SetNextRound()
    {
        var associationsToPickFrom = _associations.Where(association => !_removedAssociations.Contains(association)).ToList();

        if(!associationsToPickFrom.Any())
        {
            ShowPossibleAnswers = false;
            WordToFind = string.Empty;
            Result = "Tous les mots ont été trouvés. Bravo!";

            return;
        }

        Random r = new Random();
        int rInt = r.Next(0, associationsToPickFrom.Count);

        var associationtoFind = associationsToPickFrom[rInt];
        WordToFind = associationtoFind.word;

        if(_associationTimesAskedCounter.TryGetValue(associationtoFind, out var associationAskedCount))
        {
            _associationTimesAskedCounter[associationtoFind] = ++associationAskedCount;
        }
        else
        {
            _associationTimesAskedCounter.Add(associationtoFind, 1);
        }

        if(_associationTimesAskedCounter[associationtoFind] >= NumberOfTimesBeforeRemovingAssociation)
        {
            _removedAssociations.Add(associationtoFind);
        }

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
            MainThread.BeginInvokeOnMainThread(() => Resource = ImageSource.FromFile(resource));
        }
    }
}
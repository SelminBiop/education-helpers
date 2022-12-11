using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;
[QueryProperty(nameof(Categories), "Categories")]
public sealed partial class JeuPageViewModel : ObservableObject
{
    public const int NumberOfPossibleAnswer = 4;

    private const string AssociationsKey = "AssociationsKey";    

    private readonly IAssociationStorageService _storageService;

    private IList<string> _removedAssociations = new List<string>();

    private ObservableCollection<string?> _categories = new();
    public ObservableCollection<string?> Categories
    {
        get => _categories;
        set 
        {
            _categories = value;

            var storedAssociations = _storageService.Get(AssociationsKey);

            Associations = storedAssociations
                .Where(association => _categories.Contains(association.Category))
                .Select(association => new AssociationsPageViewModel.AssociationItem(
                association.Word,
                association.CorrespondingResource,
                () => { })).ToList();

            SetupForNewGame();  
        }
    }

    [ObservableProperty]
    private string _result = string.Empty;

    [ObservableProperty]
    private bool _hasGameEnded;

    [ObservableProperty]
    private bool _isGameReady;

    [ObservableProperty]
    private string _wordToFind = string.Empty;

    [ObservableProperty]
    private IList<AssociationsPageViewModel.AssociationItem> _associations = new List<AssociationsPageViewModel.AssociationItem>();

    [ObservableProperty]
    private ObservableCollection<AnswerItem> _possibleAnswers = new();

    [ObservableProperty]
    private ICommand _restartGameCommand;

    public JeuPageViewModel(IAssociationStorageService storageService)
    {
        _storageService = storageService;

        for (int i = 0; i < NumberOfPossibleAnswer; i++) 
        {
            _possibleAnswers.Add(new AnswerItem());
        }

        _restartGameCommand = new Command(() =>
        {
            SetupForNewGame();  
        });
    }

    public void SetupForNewGame()
    {
        Result = string.Empty;

        HasGameEnded = false;

        _removedAssociations.Clear();

        SetNextRound();
    }

    private void SetNextRound()
    {
        var associationsToPickFrom = Associations.Where(association => !_removedAssociations.Contains(association.Word)).ToList();

        if(!associationsToPickFrom.Any())
        {
            WordToFind = string.Empty;
            HasGameEnded = true;
            Result = "Tous les mots ont été trouvés. Bravo!";

            return;
        }

        Random r = new();
        int rInt = r.Next(0, associationsToPickFrom.Count);

        var associationtoFind = associationsToPickFrom[rInt];
        WordToFind = associationtoFind.Word;

        _removedAssociations.Add(WordToFind);

        var otherAnswers = Associations.Where(association => association.Word != WordToFind).ToList();

        int insertAnswer = r.Next(0, NumberOfPossibleAnswer);
        int currentIteration = 0;
        while(currentIteration < NumberOfPossibleAnswer)
        {
            var answerItem = _possibleAnswers[currentIteration];
            if(currentIteration == insertAnswer)
            {
                answerItem.SetAction(() => 
                {
                    Result = "Bonne réponse!";
                    MainThread.BeginInvokeOnMainThread(SetNextRound);
                });
                answerItem.SetResource(associationtoFind.Resource);
            }
            else
            {
                int rInt2 = r.Next(0, otherAnswers.Count);
                var otherAnswer = otherAnswers[rInt2];
                answerItem.SetResource(otherAnswer.Resource);
                answerItem.SetAction(() => 
                {
                    Result = "Mauvaise réponse! Essayez a nouveau";
                });
                otherAnswers.RemoveAt(rInt2);
            }
            currentIteration++;
        }
    }

    public sealed partial class AnswerItem : ObservableObject
    {
        [ObservableProperty]
        private ImageSource? _resource;

        [ObservableProperty]
        private ICommand? _onImageTappedCommand;

        public AnswerItem()
        {
        }

        public void SetAction(Action onAnswerSelectedAction)
        {
            OnImageTappedCommand = new Command(_ => onAnswerSelectedAction.Invoke());
        }

        public void SetResource(ImageSource? source)
        {
            MainThread.BeginInvokeOnMainThread(() => Resource = source);
        }
    }
}
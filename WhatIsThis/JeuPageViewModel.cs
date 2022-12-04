using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;
[QueryProperty(nameof(Category), "Category")]
public sealed class JeuPageViewModel : ObservableObject
{
    public const int NumberOfPossibleAnswer = 4;

    private const string AssociationsKey = "AssociationsKey";    

    private readonly IAssociationStorageService _storageService;

    private IList<string> _removedAssociations = new List<string>();

    private string _category;
    public string Category 
    {
        get => _category;
        set 
        {
            _category = value;

            var storedAssociations = _storageService.Get(AssociationsKey);

            Associations = storedAssociations
                .Where(association => association.Category == _category)
                .Select(association => new AssociationsPageViewModel.AssociationItem(
                association.word,
                association.correspondingResource,
                () => { })).ToList();

            SetupForNewGame();  
        }
    }

    private string _result = string.Empty;
    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private bool _hasGameEnded;
    public bool HasGameEnded
    {
        get => _hasGameEnded;
        set => SetProperty(ref _hasGameEnded, value);
    }

    private bool _isGameReady;
    public bool IsGameReady
    {
        get => _isGameReady;
        set => SetProperty(ref _isGameReady, value);
    }

    private string _wordToFind = string.Empty;
    public string WordToFind
    {
        get => _wordToFind;
        set => SetProperty(ref _wordToFind, value);
    }

    private IList<AssociationsPageViewModel.AssociationItem> _associations = new List<AssociationsPageViewModel.AssociationItem>();
    public IList<AssociationsPageViewModel.AssociationItem> Associations
    {
        get => _associations;
        set => SetProperty(ref _associations, value);
    }

    private ObservableCollection<AnswerItem> _possibleAnswers = new ObservableCollection<AnswerItem>();
    public ObservableCollection<AnswerItem> PossibleAnswers
    {
        get => _possibleAnswers;
        set => SetProperty(ref _possibleAnswers, value);
    }

    private ICommand _onSetupGameCommand;
    public ICommand OnSetupGameCommand
    {
        get => _onSetupGameCommand;
        set => SetProperty(ref _onSetupGameCommand, value);
    }

    public JeuPageViewModel(IAssociationStorageService storageService)
    {
        _storageService = storageService;

        for (int i = 0; i < NumberOfPossibleAnswer; i++) 
        {
            _possibleAnswers.Add(new AnswerItem());
        }

        OnSetupGameCommand = new Command(() =>
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

        Random r = new Random();
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

        public void SetResource(ImageSource source)
        {
            MainThread.BeginInvokeOnMainThread(() => Resource = source);
        }
    }
}
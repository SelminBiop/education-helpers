using System;
using System.Windows.Input;

namespace WhatIsThis.QuestionType;

internal sealed class AssociationQuestion : IQuestionType
{
    public string FriendlyName => "Créer une Association";

    public ICommand StartCreationCommand { get; }

    public AssociationQuestion()
	{
        StartCreationCommand = new Command(() =>
        Shell.Current.GoToAsync("CreateAssociationPage"));
	}
}


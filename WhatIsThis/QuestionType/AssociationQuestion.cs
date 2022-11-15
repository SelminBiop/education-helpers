using System;
using System.Windows.Input;

namespace WhatIsThis.QuestionType;

internal sealed class AssociationQuestion : IQuestionType
{
	public AssociationQuestion()
	{
        StartCreationCommand = new Command(() =>
        Shell.Current.GoToAsync("//MainPage/CreateAssociationPage"));
	}

    public string FriendlyName => "Créer une Association";

    public ICommand StartCreationCommand { get; }
}


using System;
using System.Windows.Input;

namespace WhatIsThis.QuestionType;

public interface IQuestionType
{
	string FriendlyName { get; }
	ICommand StartCreationCommand { get; }
}


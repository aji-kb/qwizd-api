using System;

namespace qwizd_api.Service.ViewModel;

public class QuestionViewModel
{
    public int Id {get;set;}
    public string? Text {get;set;}
    public bool IsLastQuestion {get;set;}
    public int TotalQuestionCount {get;set;}
    public int CurrentQuestionIndex {get;set;}
    public List<AnswerViewModel>? Answers {get;set;}
}

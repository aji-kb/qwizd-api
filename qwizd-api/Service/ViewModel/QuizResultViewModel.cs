using System;

namespace qwizd_api.Service.ViewModel;

public class QuizResultViewModel
{
    public int TotalQuestions {get;set;}
    public int CorrectAnswers {get;set;}
    public List<QuestionResult>? QuestionResults {get;set;}
}

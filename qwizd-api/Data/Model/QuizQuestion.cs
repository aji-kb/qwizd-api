using System;

namespace qwizd_api.Data.Model;

public class QuizQuestion :BaseEntity
{
    public int Id {get;set;}
    public int QuizId {get;set;}
    public int QuestionId {get;set;}
    public int? UserAnswerId {get;set;}
    public bool? IsCorrectAnswer {get;set;}
}

using System;

namespace qwizd_api.Data.Model;

public class QuestionAnswerMapping : BaseEntity
{
    public int Id {get;set;}
    public int QuestionId {get;set;}
    public int AnswerId {get;set;}
    public bool IsCorrectAnswer {get;set;}
}

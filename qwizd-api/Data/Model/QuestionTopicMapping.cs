using System;

namespace qwizd_api.Data.Model;

public class QuestionTopicMapping :BaseEntity
{
    public int Id {get;set;}
    public int TopicId {get;set;}
    public int QuestionId {get;set;}
}

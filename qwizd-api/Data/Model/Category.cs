using System;

namespace qwizd_api.Data.Model;

public class Topic :BaseEntity
{
    public int Id {get;set;}
    public string? Name {get;set;}
    public int? ParentTopicId {get;set;}
}

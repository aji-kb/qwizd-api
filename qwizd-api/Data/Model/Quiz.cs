using System;

namespace qwizd_api.Data.Model;

public class Quiz :BaseEntity
{
    public int Id {get;set;}
    public int UserId {get;set;}
    public DateTime StartTimestamp {get;set;}
}

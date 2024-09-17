using System;

namespace qwizd_api.Data.Contract;

public interface ITrackedEntity
{
    public DateTime CreatedDateUTC {get;set;}
    public string? CreatedBy {get;set;}
    public DateTime? ModifiedDateUTC {get;set;}
    public string? ModifiedBy {get;set;}
}

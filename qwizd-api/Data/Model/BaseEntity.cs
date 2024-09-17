using System;
using qwizd_api.Data.Contract;

namespace qwizd_api.Data.Model;

public class BaseEntity : ITrackedEntity
{

    private DateTime _createdDateUTC;
    private string? _createdBy;
    private DateTime? _modifiedDateUTC;
    private string? _modifiedBy;

    public DateTime CreatedDateUTC { get => _createdDateUTC;  set => _createdDateUTC = value; }
    public string? CreatedBy { get => _createdBy; set => _createdBy = value; }
    public DateTime? ModifiedDateUTC { get => _modifiedDateUTC; set => _modifiedDateUTC = value; }
    public string? ModifiedBy { get => _modifiedBy; set => _modifiedBy = value; }

}

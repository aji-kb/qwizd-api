using System;
using Microsoft.AspNetCore.Identity;
using qwizd_api.Data.Contract;

namespace qwizd_api.Data.Model;

public class User : IdentityUser<int>, ITrackedEntity
{
    public DateTime CreatedDateUTC { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDateUTC { get; set; }
    public string? ModifiedBy { get ; set ; }
}

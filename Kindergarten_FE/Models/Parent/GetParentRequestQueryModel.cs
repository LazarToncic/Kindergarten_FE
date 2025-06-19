namespace Kindergarten_FE.Models.Parent;

public class GetParentRequestQueryModel
{
    public string? FirstName           { get; set; }
    public string? LastName            { get; set; }
    public Guid   KindergartenId       { get; set; }
    public bool?  IsOnlineApproved     { get; set; }
    public bool?  IsInPersonApproved   { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize   { get; set; } = 10;
}
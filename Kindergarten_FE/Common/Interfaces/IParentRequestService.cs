using Kindergarten_FE.Common.Enums;
using Kindergarten_FE.Models.Child;
using Kindergarten_FE.Models.Parent;

namespace Kindergarten_FE.Common.Interfaces;

public interface IParentRequestService
{
    Task CreateParentRequest(int numberOfChildren, ParentChildRelationship parentChildRelationship,
        string? additionalInfo, string preferredKindergarten, List<ChildModel> Children);

    Task<List<GetParentRequestSingleChildResponseModel>> GetParentRequestsAsync(GetParentRequestQueryModel query);
}
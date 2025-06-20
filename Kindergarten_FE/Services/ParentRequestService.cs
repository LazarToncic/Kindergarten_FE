using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Dtos.Parent;
using Kindergarten_FE.Common.Enums;
using Kindergarten_FE.Common.Interfaces;
using Kindergarten_FE.Models.Child;
using Kindergarten_FE.Models.Parent;
using Microsoft.AspNetCore.WebUtilities;

namespace Kindergarten_FE.Services;

public class ParentRequestService(HttpClient http) : IParentRequestService
{
    public async Task CreateParentRequest(int numberOfChildren,
        ParentChildRelationship parentChildRelationship,
        string? additionalInfo,
        string preferredKindergarten,
        List<ChildModel> children)
    {
        var childDtos = children.Select(c => new ParentRequestChildDto(
            c.FirstName,
            c.LastName,
            c.DateOfBirth,
            c.HasAllergies,
            c.Allergies,
            c.HasMedicalIssues,
            c.MedicalConditions
            ))
            .ToList();

        var command = new CreateParentRequestDto(
            numberOfChildren,
            parentChildRelationship,
            additionalInfo,
            preferredKindergarten,
            childDtos
            );
        
        var response = await http.PostAsJsonAsync(
            ApiRoutes.SendParentRequest,
            new { dto = command }
        );
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<GetParentRequestSingleChildResponseModel>> GetParentRequestsAsync(GetParentRequestQueryModel query)
    {
        var queryParams = new Dictionary<string, string?>();

        if (!string.IsNullOrWhiteSpace(query.FirstName))
            queryParams["Dto.FirstName"] = query.FirstName;
        if (!string.IsNullOrWhiteSpace(query.LastName))
            queryParams["Dto.LastName"] = query.LastName;
        if (query.KindergartenId != Guid.Empty)
            queryParams["Dto.KindergartenId"] = query.KindergartenId.ToString();
        if (query.IsOnlineApproved.HasValue)
            queryParams["Dto.IsOnlineApproved"] = query.IsOnlineApproved.Value.ToString();
        if (query.IsInPersonApproved.HasValue)
            queryParams["Dto.IsInPersonApproved"] = query.IsInPersonApproved.Value.ToString();

        queryParams["Dto.PageNumber"] = (query.PageNumber > 0 ? query.PageNumber : 1).ToString();
        queryParams["Dto.PageSize"]   = (query.PageSize   > 0 ? query.PageSize   : 10).ToString();

        var url = QueryHelpers.AddQueryString(ApiRoutes.GetParentRequests, queryParams);

        var wrapper = await http.GetFromJsonAsync<GetParentRequestQueryResponseModel>(url);
        return wrapper?.ParentRequests
               ?? new List<GetParentRequestSingleChildResponseModel>();
    }

    public async Task<GetParentRequestSingleChildResponseModel> GetParentRequestByIdAsync(Guid id)
    {
        var response = await http.GetFromJsonAsync<GetParentRequestSingleChildResponseModel>($"{ApiRoutes.GetParentRequest}?Id={id}");
        if (response == null)
            throw new InvalidOperationException("Parent request with id not found.");
        
        return response;
    }

    public async Task ApproveOnlineParentRequestAsync(Guid id)
    {
        var url = QueryHelpers.AddQueryString(ApiRoutes.ApproveParentRequestOnline,
            new Dictionary<string, string?>{ ["ParentRequestId"] = id.ToString() });

        var response = await http.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task ApproveInPersonParentRequestAsync(Guid id)
    {
        var url = QueryHelpers.AddQueryString(ApiRoutes.ApproveParentRequestInPerson, 
            new Dictionary<string, string?> { ["ParentRequestId"] = id.ToString() });
        var response = await http.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }
}
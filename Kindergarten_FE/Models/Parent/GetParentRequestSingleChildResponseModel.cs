using System.Text.Json.Serialization;
using Kindergarten_FE.Common.Dtos.Parent;
using Kindergarten_FE.Common.Enums;

namespace Kindergarten_FE.Models.Parent;

public record GetParentRequestSingleChildResponseModel(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string KindergartenName,
    bool IsOnlineApproved,
    bool IsInPersonApproved,
    DateTime CreatedAt,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    ParentChildRelationship ParentChildRelationship,
    List<ParentRequestChildDto>? ChildrenJson,
    string? OnlineApprovedBy,
    string? InPersonApprovedBy
    );
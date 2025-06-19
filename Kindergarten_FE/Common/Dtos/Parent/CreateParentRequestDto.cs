using Kindergarten_FE.Common.Enums;
using Kindergarten_FE.Models.Child;

namespace Kindergarten_FE.Common.Dtos.Parent;

public record CreateParentRequestDto(int NumberOfChildren,
    ParentChildRelationship ParentChildRelationship,
    string? AdditionalInfo,
    string PreferredKindergarten,
    List<ParentRequestChildDto> Children);
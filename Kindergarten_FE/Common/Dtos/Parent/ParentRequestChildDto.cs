namespace Kindergarten_FE.Common.Dtos.Parent;

public record ParentRequestChildDto(string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    bool HasAllergies,
    List<string>? Allergies,
    bool HasMedicalIssues,
    List<string>? MedicalConditions);
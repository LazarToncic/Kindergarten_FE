namespace Kindergarten_FE.Models.Child;

public class ChildModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public bool HasAllergies { get; set; }
    public string AllergiesString { get; set; } = string.Empty;
    public List<string>? Allergies { get; set; } = new();
    public string MedicalConditionsString { get; set; } = string.Empty;
    public bool HasMedicalIssues { get; set; }
    public List<string>? MedicalConditions { get; set; } = new();
}
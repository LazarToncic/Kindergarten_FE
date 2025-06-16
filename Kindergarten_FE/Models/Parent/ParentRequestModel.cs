using System.ComponentModel.DataAnnotations;
using Kindergarten_FE.Common.Enums;
using Kindergarten_FE.Models.Child;

namespace Kindergarten_FE.Models.Parent;

public class ParentRequestModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Please select your relationship to the child.")]
    public ParentChildRelationship ParentChildRelationship { get; set; }
    [Range(1, 10, ErrorMessage = "You must enter at least 1 child.")]
    public int NumberOfChildren { get; set; }
    [MaxLength(500, ErrorMessage = "Additional info is too long (max 500 characters).")]
    public string? AdditionalInfo { get; set; }
    [Required(ErrorMessage = "Please select a kindergarten.")]
    public string PreferredKindergarten { get; set; }
    [MinLength(1, ErrorMessage = "You must add at least one child.")]
    public List<ChildModel> Children { get; set; } = new List<ChildModel>();
}
namespace Kindergarten_FE.Common.Validation.Auth;

public record ValidationErrorResponseDto(Dictionary<string, List<string>> Errors);
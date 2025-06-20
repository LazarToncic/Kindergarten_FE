namespace Kindergarten_FE.Common.ApiRoutes;

public static class ApiRoutes
{
    public const string UserRegistration = "Auth/UserRegistration";
    public const string BaseAddress = "https://localhost:44309/";
    public const string UserLogin = "Auth/UserLogin";
    public const string GenerateRefreshToken = "Auth/GenerateRefreshToken";
    public const string UserLogout = "Auth/UserLogout";
    public const string GetKindergartenNamesAndIds = "Kindergarten/GetKindergartensInf";
    public const string SendParentRequest = "Parent/SendParentRequest";
    public const string GetParentRequests = "Parent/GetParentRequests";
    public const string GetParentRequest = "Parent/GetParentRequest";
    public const string EditParentRequest = "Parent/ParentRequests/Edit";
    public const string ApproveParentRequestOnline = "Parent/ApproveParentRequestOnline";
    public const string ApproveParentRequestInPerson = "Parent/ApproveParentRequestInPerson";
}
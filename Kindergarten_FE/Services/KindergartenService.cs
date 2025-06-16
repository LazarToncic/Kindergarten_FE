using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Interfaces;
using Kindergarten_FE.Models.Kindergarten;

namespace Kindergarten_FE.Services;

public class KindergartenService(HttpClient http) : IKindergartenService
{
    public async Task<List<KindergartenFroFormModel>> GetAllKindergartenNames()
    {
        var kindergartens = await http.GetFromJsonAsync<List<KindergartenFroFormModel>>(ApiRoutes.GetKindergartenNamesAndIds);
        return kindergartens ?? new List<KindergartenFroFormModel>();
    }
}
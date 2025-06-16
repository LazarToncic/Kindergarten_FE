using Kindergarten_FE.Models.Kindergarten;

namespace Kindergarten_FE.Common.Interfaces;

public interface IKindergartenService
{
    Task<List<KindergartenFroFormModel>> GetAllKindergartenNames();
}
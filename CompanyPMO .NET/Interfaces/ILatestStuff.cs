using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ILatestStuff
    {
        Task<LatestStuffDto> GetEntitiesCreatedLastWeek();
    }
}

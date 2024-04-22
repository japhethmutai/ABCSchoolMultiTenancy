using Domain.Entities;

namespace Application.Features.Schools
{
    public interface ISchoolService
    {
        Task<int> CreateSchoolAsync(School school);
        Task<int> UpdateSchoolAsync(School school);
        Task<int> DeleteSchoolAsync(School school);

        Task<School> GetSchoolByIdAsync(int schoolId);
        Task<List<School>> GetAllSchoolsAsync();
        Task<School> GetSchoolByNameAsync(string name);
    }
}

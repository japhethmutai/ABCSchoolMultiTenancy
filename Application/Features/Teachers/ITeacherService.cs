using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Teachers
{
    public interface ITeacherService
    {
        Task<int> CreateTeacherAsync(Teacher teacher);
        Task<int> UpdateTeacherAsync(Teacher teacher);
        Task<int> DeleteTeacherAsync(Teacher teacher);

        Task<Teacher> GetTeacherByIdAsync(int teacherId);
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher> GetTeacherByNameAsync(string name);
    }
}

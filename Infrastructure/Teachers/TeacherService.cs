using Application.Features.Teachers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Teachers
{
    public class TeacherService(ApplicationDbContext context) : ITeacherService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<int> CreateTeacherAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<int> DeleteTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Remove(teacher);

            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            var teachersInDb = await _context.Teachers.ToListAsync();
            return teachersInDb;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int teacherId)
        {
            var teacherInDb = await _context.Teachers
                .Where(s => s.Id == teacherId)
                .FirstOrDefaultAsync();
            return teacherInDb;
        }

        public async Task<Teacher> GetTeacherByNameAsync(string name)
        {
            var teacherInDb = await _context.Teachers
                .Where(s => s.Name == name)
                .FirstOrDefaultAsync();
            return teacherInDb;
        }

        public async Task<int> UpdateTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return teacher.Id;
        }
    }
}

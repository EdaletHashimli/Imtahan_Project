using Imtahan_Project.Models.ExamM;
using Imtahan_Project.Models.StudentM;
using Imtahan_Project.Models.SubjectM;
using Microsoft.EntityFrameworkCore;

namespace Imtahan_Project.Data
{
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> options) : base(options)
        {

        }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }

    }
}

using Imtahan_Project.Data;
using Imtahan_Project.Models;
using Imtahan_Project.Models.StudentM;
using Imtahan_Project.Models.SubjectM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imtahan_Project.Controllers.App
{
    public class StudentController : Controller
    {
        private readonly ExamDbContext _examDbContext;
        
        public StudentController(ExamDbContext examDbContext)
        {
            this._examDbContext = examDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _examDbContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            var _student = new Student()
            {
                StudentName = new string(student.StudentName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray()),
                StudentSurName = new string(student.StudentSurName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray()),
                ParentName = new string(student.ParentName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray()),
                Class = student.Class
            };
            await _examDbContext.Students.AddAsync(_student);
            await _examDbContext.SaveChangesAsync();
            return RedirectToAction("GetStudents");
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudent(decimal studentnumber)
        {
            var student = await _examDbContext.Students.FirstOrDefaultAsync(x => x.StudentNumber == studentnumber);
            if (student != null)
            {
                var viewModel = new Student()
                {
                    StudentNumber = student.StudentNumber,
                    StudentName=student.StudentName,
                    StudentSurName=student.StudentSurName,
                    ParentName = student.ParentName,
                    Class=student.Class,
                };
                return await Task.Run(() => View("ViewStudent", viewModel));
            }


            return RedirectToAction("GetStudents");
        }

        [HttpPost]
        public async Task<IActionResult> ViewStudent(Student student)
        {
            var _student = await _examDbContext.Students.FindAsync(student.StudentNumber);
            if (_student != null)
            {
                _student.StudentName = new string(student.StudentName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray());
                _student.StudentSurName = new string(student.StudentSurName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray());
                _student.ParentName = new string(student.ParentName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray());
                _student.Class=student.Class;
                await _examDbContext.SaveChangesAsync();

                return RedirectToAction("GetStudents");
            }
            return RedirectToAction("GetStudents");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student student)
        {
            var _student = await _examDbContext.Students.FindAsync(student.StudentNumber);
            if (_student != null)
            {
                _examDbContext.Students.Remove(_student);
                await _examDbContext.SaveChangesAsync();
                return RedirectToAction("GetStudents");
            }
            return RedirectToAction("GetStudents");
        }
    }
}

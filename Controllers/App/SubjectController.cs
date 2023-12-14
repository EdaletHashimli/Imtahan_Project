using Imtahan_Project.Data;
using Imtahan_Project.Models.SubjectM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imtahan_Project.Controllers.App
{
    public class SubjectController : Controller
    {
        private readonly ExamDbContext _examDbContext;

        public SubjectController(ExamDbContext examDbContext)
        {
            this._examDbContext = examDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _examDbContext.Subjects.ToListAsync();
            return View(subjects);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var subjectCodes = _examDbContext.Subjects.Select(s => s.SubjectCode).Distinct().ToList();

            ViewBag.SubjectCodes = subjectCodes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Subject addSubjectRequest)
        {
            var subject = new Subject()
            {
                SubjectCode=  new string(addSubjectRequest.Class.ToString() + addSubjectRequest.SubjectName[0]),
                SubjectName= addSubjectRequest.SubjectName,
                Class=addSubjectRequest.Class,
                SubjectTeacherName= new string(addSubjectRequest.SubjectTeacherName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray()),
                SubjectTeacherSurName = new string(addSubjectRequest.SubjectTeacherSurName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray()),

            };
            await _examDbContext.Subjects.AddAsync(subject);
            await _examDbContext.SaveChangesAsync();
            return RedirectToAction("GetSubjects");
        }

        [HttpGet]
        public async Task<IActionResult> View(string code)
        {
            var subject = await _examDbContext.Subjects.FirstOrDefaultAsync(x => x.SubjectCode == code);
            if (subject != null)
            {
                var viewModel = new Subject()
                {
                    SubjectCode = subject.SubjectCode,
                    SubjectName = subject.SubjectName,
                    Class = subject.Class,
                    SubjectTeacherName = subject.SubjectTeacherName,
                    SubjectTeacherSurName = subject.SubjectTeacherSurName,
                };
                return await Task.Run(() => View("View", viewModel));
            }


            return RedirectToAction("GetSubjects");
        }

        [HttpPost]
        public async Task<IActionResult> View(Subject subject)
        {
            var _subject = await _examDbContext.Subjects.FindAsync(subject.SubjectCode);
            if (_subject != null)
            {
                _subject.SubjectCode = subject.SubjectCode.ToUpper();
                _subject.SubjectName = subject.SubjectName;
                _subject.Class = subject.Class;
                _subject.SubjectTeacherName = new string(subject.SubjectTeacherName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray());
                _subject.SubjectTeacherSurName = new string(subject.SubjectTeacherSurName.Select((c, i) => i == 0 ? char.ToUpper(c) : c).ToArray());
                await _examDbContext.SaveChangesAsync();

                return RedirectToAction("GetSubjects");
            }
            return RedirectToAction("GetSubjects");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Subject subject)
        {
            var _subject = await _examDbContext.Subjects.FindAsync(subject.SubjectCode);
            if (_subject != null)
            {
                _examDbContext.Subjects.Remove(_subject);
                await _examDbContext.SaveChangesAsync();
                return RedirectToAction("GetSubjects");
            }
            return RedirectToAction("GetSubjects");
        }

        [HttpGet]
        public async Task<IActionResult> CheckSubjectCodeExists(string subjectCode)
        {
            var subject = await _examDbContext.Subjects.FirstOrDefaultAsync(x => x.SubjectCode == subjectCode);

            bool codeExists = subject != null;

            return Json(new { exists = codeExists });
        }


    }
}

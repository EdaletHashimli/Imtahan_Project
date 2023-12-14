using Imtahan_Project.Data;
using Imtahan_Project.Models.ExamM;
using Imtahan_Project.Models.StudentM;
using Imtahan_Project.Models.SubjectM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imtahan_Project.Controllers.App
{
    public class ExamController : Controller
    {
        private readonly ExamDbContext _examDbContext;

        public ExamController(ExamDbContext examDbContext)
        {
            this._examDbContext = examDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetExams()
        {
            var exams = await _examDbContext.Exams.ToListAsync();
            foreach (var exam in exams)
            {
                // İlgili öğrenci numarasını al
                decimal studentNumber = exam.StudentNumber;

                // Student tablosundan öğrenci numarasına göre öğrenciyi bul
                var student = await _examDbContext.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);

                // Eğer öğrenci varsa, student tablosundaki verileri exam tablosuna aktar
                if (student != null)
                {
                    exam.StudentName = student.StudentName + " " + student.StudentSurName + " " + student.ParentName;
                }
                else
                {
                    // Eğer öğrenci yoksa, exam tablosundan bu kaydı sil
                    _examDbContext.Exams.Remove(exam);
                }
            }

            // Değişiklikleri kaydet
            await _examDbContext.SaveChangesAsync();
            var restoredexams = await _examDbContext.Exams.ToListAsync();
            return View(restoredexams);
        }

        [HttpGet]
        public IActionResult AddExam()
        {
                var subjectNames = _examDbContext.Subjects.Select(s => s.SubjectName).Distinct().ToList();
                var studenClasses = _examDbContext.Students.Select(s => s.Class).Distinct().ToList();

                ViewBag.SubjectNames = subjectNames;

                // Ders adlarına ait sınıfları hazırla
                var classesData = new Dictionary<string, List<int>>();
                foreach (var subject in subjectNames)
                {
                    var classesForSubject = _examDbContext.Subjects.Where(e => e.SubjectName == subject).Select(e => e.Class).Distinct().ToList();

                // Sınıfları ders adlarına göre ekle
                    classesData[subject] = classesForSubject ?? new List<int>();
                }

                // JavaScript tarafına göndermek için JSON formatına çevir
                ViewBag.ClassesData = Newtonsoft.Json.JsonConvert.SerializeObject(classesData);



                var studentsData = new Dictionary<int, List<string>>();
                foreach (var studentclass in studenClasses)
                {
                var studentsForClasses = _examDbContext.Students.Where(e => e.Class == studentclass).Select(e => e.StudentName + " " + e.StudentSurName + " " + e.ParentName).ToList();

                studentsData[studentclass] = studentsForClasses ?? new List<string>();
                }

                ViewBag.StudentsData = Newtonsoft.Json.JsonConvert.SerializeObject(studentsData);


                var examsDictionary = _examDbContext.Exams.GroupBy(exam => exam.SubjectName).ToDictionary( group => group.Key, group => group.Select(exam => exam.StudentName).ToList());
                ViewBag.Exams = examsDictionary;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExam(Exam exam)
        {
            var student = _examDbContext.Students.FirstOrDefault(e => e.StudentName + " " + e.StudentSurName + " " + e.ParentName == exam.StudentName);


            var _exam = new Exam()
            {
                SubjectCode = new string(exam.Class.ToString() + exam.SubjectName[0]),
                SubjectName = exam.SubjectName,
                Class = exam.Class,
                StudentNumber = student.StudentNumber,
                StudentName = exam.StudentName,
                ExamDate = exam.ExamDate,
                Result = exam.Result
            };
            await _examDbContext.Exams.AddAsync(_exam);
            await _examDbContext.SaveChangesAsync();
            return RedirectToAction("GetExams");
        }

        [HttpGet]
        public async Task<IActionResult> ViewExam(int id)
        {
            var exam = await _examDbContext.Exams.FirstOrDefaultAsync(x => x.ExamId == id);
            if (exam != null)
            {
                var viewModel = new Exam()
                {
                    ExamId = exam.ExamId,
                    SubjectCode = exam.SubjectCode,
                    SubjectName = exam.SubjectName,
                    Class = exam.Class,
                    StudentNumber = exam.StudentNumber,
                    StudentName = exam.StudentName,
                    ExamDate= exam.ExamDate,
                    Result = exam.Result

                };
                return await Task.Run(() => View("ViewExam", viewModel));
            }


            return RedirectToAction("GetExams");
        }

        [HttpPost]
        public async Task<IActionResult> ViewExam(Exam exam)
        {
            var _exam = await _examDbContext.Exams.FindAsync(exam.ExamId);

            if (_exam != null)
            {
                _exam.ExamId = exam.ExamId;
                _exam.SubjectName = exam.SubjectName;
                _exam.Class = exam.Class;
                _exam.StudentNumber = exam.StudentNumber;
                _exam.StudentName = exam.StudentName;
                _exam.ExamDate = exam.ExamDate;
                _exam.Result = exam.Result;
                await _examDbContext.SaveChangesAsync();

                return RedirectToAction("GetExams");
            }
            return RedirectToAction("GetExams");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Exam exam)
        {
            var _exam = await _examDbContext.Exams.FindAsync(exam.ExamId);
            if (_exam != null)
            {
                _examDbContext.Exams.Remove(_exam);
                await _examDbContext.SaveChangesAsync();
                return RedirectToAction("GetExams");
            }
            return RedirectToAction("GetExams");
        }
    }
}

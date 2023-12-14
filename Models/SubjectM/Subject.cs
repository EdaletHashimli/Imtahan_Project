using Imtahan_Project.Models.ExamM;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imtahan_Project.Models.SubjectM
{
    public class Subject
    {
        [Key]
        [Column(TypeName = "char(3)")]
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Class { get; set; }
        public string SubjectTeacherName { get; set; }
        public string SubjectTeacherSurName { get; set; }
    }

}

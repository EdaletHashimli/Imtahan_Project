using Imtahan_Project.Models.StudentM;
using Imtahan_Project.Models.SubjectM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imtahan_Project.Models.ExamM
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }
        [Column(TypeName = "char(3)")]
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Class { get; set; }
        [Column(TypeName = "numeric(5,0)")]
        public decimal StudentNumber { get; set; }
        public string StudentName { get; set; } 
        public DateTime ExamDate { get; set; }
        public int Result { get; set; }
        
    }
}

using Imtahan_Project.Models.ExamM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imtahan_Project.Models.StudentM
{
    public class Student
    {
        [Key]
        [Column(TypeName = "numeric(5,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string StudentSurName { get; set; }
        public string ParentName { get; set; }
        public int Class { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Student
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "نام پدر")]
        public string FatherName { get; set; }

        [Display(Name = "محل تولد")]
        public string BtirhPlace { get; set; }

        [Display(Name = "محل صدور شناسنامه")]
        public string IdentityCardPlace { get; set; }

        [Display(Name = "چپ دست؟")]
        public bool RightOrLeft { get; set; }

        [Display(Name = "نام مدرسه سال قبل")]
        public string OldSchool { get; set; }

        [Display(Name = "معدل سال قبل")]
        public float LastYearAvreage { get; set; }

        [Display(Name = "شغل پدر")]
        public string FatherJob { get; set; }

        [Display(Name = "تصحیلات پدر")]
        public string IdFatherEdu { get; set; }

        [ForeignKey("IdFatherEdu")]
        public virtual EducationType EducationTypeFather { get; set; }

        [Display(Name = "شغل مادر")]
        public string MotherJob { get; set; }

        [Display(Name = "تصحیلات مادر")]
        public string IdMotherEdu { get; set; }

        [ForeignKey("IdMotherEdu")]
        public virtual EducationType EducationTypeMother { get; set; }

        [Display(Name = "پدر و مادر مطعلقه")]
        public bool IsDivorced { get; set; }

        [Display(Name = "آدرس محل کار پدر")]
        public string FatherJobAddress { get; set; }

        [Display(Name = "تلفن محل کار پدر")]
        public string FatherJobTell { get; set; }

        [Display(Name = "آدرس محل کار مادر")]
        public string MotherJobTell { get; set; }

        [Display(Name = "تلفن محل کار مادر")]
        public string MotherJobAddress { get; set; }

        [Display(Name = "تلفن منزل")]
        public string Phone { get; set; }

        [Display(Name = "شماره همراه پدر")]
        public string FatherMobile { get; set; }

        [Display(Name = "شماره همراه مادر")]
        public string MotherMobile { get; set; }

        public string IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "ثبت نام اولیه")]
        public bool IsPreSubmit { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime SubmitDate { get; set; }


    }
}

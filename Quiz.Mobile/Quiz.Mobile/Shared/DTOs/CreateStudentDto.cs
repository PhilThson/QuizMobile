using System;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Shared.ViewModels;

namespace Quiz.Mobile.Shared.DTOs
{
	public class CreateStudentDto : CreatePersonDto
	{
        //[Required]
        //[StringLength(15)]
        public string DisabilityCert { get; set; }
        //[Required]
        public byte? BranchId { get; set; }

        public static explicit operator CreateStudentDto(
            StudentViewModel studentVM)
        {
            var createStudent = new CreateStudentDto();
            createStudent.CopyPropertiesExtension(studentVM);
            createStudent.BranchId = studentVM?.Branch?.Id;
            return createStudent;
        }
    }
}


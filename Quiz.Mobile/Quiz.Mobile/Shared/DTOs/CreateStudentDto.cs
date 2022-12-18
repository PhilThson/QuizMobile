using System;
namespace Quiz.Mobile.Shared.DTOs
{
	public class CreateStudentDto : CreatePersonDto
	{
        //[Required]
        public byte? BranchId { get; set; }
    }
}


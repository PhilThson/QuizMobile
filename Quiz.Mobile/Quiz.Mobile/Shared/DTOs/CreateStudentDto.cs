using System;
namespace Quiz.Mobile.Shared.DTOs
{
	public class CreateStudentDto : CreatePersonDto
	{
        //[Required]
        //[StringLength(15)]
        public string DisabilityCert { get; set; }
        //[Required]
        public byte? BranchId { get; set; }
    }
}


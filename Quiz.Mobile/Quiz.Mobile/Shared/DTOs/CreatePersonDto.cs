﻿using System;
namespace Quiz.Mobile.Shared.DTOs
{
	public class CreatePersonDto
	{
        //[Required]
        //[StringLength(20)]
        public string? FirstName { get; set; }
        //[StringLength(20)]
        public string? SecondName { get; set; }
        //[Required]
        //[StringLength(50)]
        public string? LastName { get; set; }
        //[StringLength(256)]
        public string? BirthCity { get; set; }
        public DateTime? DateOfBirth { get; set; }
        //[Required]
        //[StringLength(11)]
        public string? PersonalNumber { get; set; }
    }
}


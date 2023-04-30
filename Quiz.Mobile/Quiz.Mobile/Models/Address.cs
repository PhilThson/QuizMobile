using System;
namespace Quiz.Mobile.Models
{
	public class Address
	{
        //[Required]
        //[StringLength(64)]
        public string Country { get; set; }

        //[Required]
        //[StringLength(128)]
        public string City { get; set; }

        //[StringLength(128)]
        public string Street { get; set; }
    }
}


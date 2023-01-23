using System;
using System.Text;
using Quiz.Mobile.Models;

namespace Quiz.Mobile.Shared.DTOs
{
	public class AddressDto : Address
	{
        public int Id { get; set; }

        //[Required]
        //[StringLength(10)]
        public string HouseNumber { get; set; }

        //[StringLength(10)]
        public string FlatNumber { get; set; }

        //[Required]
        //[StringLength(10)]
        public string PostalCode { get; set; }

        public override string ToString()
        {
            var addressDescription = new StringBuilder();
            if (!string.IsNullOrEmpty(Country))
                addressDescription.Append(Country);
            if (!string.IsNullOrEmpty(Street))
                addressDescription.Append($", ul. {Street}");
            if (!string.IsNullOrEmpty(HouseNumber))
                addressDescription.Append($" {HouseNumber}");
            if (!string.IsNullOrEmpty(FlatNumber))
                addressDescription.Append($"/{FlatNumber}");
            if (!string.IsNullOrEmpty(PostalCode))
                addressDescription.Append($", {PostalCode}");
            if (!string.IsNullOrEmpty(City))
                addressDescription.Append($" {City}");

            return addressDescription.ToString();
        }
    }
}


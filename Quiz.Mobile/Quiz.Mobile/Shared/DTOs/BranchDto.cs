using System;
namespace Quiz.Mobile.Shared.DTOs
{
	public class BranchDto
	{
        public byte Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Teacher { get; set; }
    }
}
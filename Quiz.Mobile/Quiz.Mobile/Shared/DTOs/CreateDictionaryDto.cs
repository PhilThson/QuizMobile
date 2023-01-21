using System;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Shared.ViewModels;

namespace Quiz.Mobile.Shared.DTOs
{
	public class CreateDictionaryDto
	{
        public byte Id { get; set; }
        //Required 512
        public string Name { get; set; }
		//1024
		public string Description { get; set; }
        //1024
        public string ExtendedName { get; set; }

        public static explicit operator CreateDictionaryDto(
            AreaViewModel areaVM)
        {
            var createDictionary = new CreateDictionaryDto();
            createDictionary.CopyPropertiesExtension(areaVM);
            return createDictionary;
        }

        public static explicit operator CreateDictionaryDto(
            DifficultyViewModel difficultyVM)
        {
            var createDictionary = new CreateDictionaryDto();
            createDictionary.CopyPropertiesExtension(difficultyVM);
            return createDictionary;
        }
    }
}


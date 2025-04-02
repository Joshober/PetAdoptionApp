using System.Collections.Generic;
using System.Text.RegularExpressions;
using MAUI_Tutorial1_TodoList.Helpers.Models;
using MAUI_Tutorial1_TodoList.Models;


namespace MAUI_Tutorial1_TodoList.Extensions
{
    public static class MappingExtensions
    {
        public static Animal ToAnimal(this UpdateAnimalUrl update)
        {
            string valueInside = "";
            string pattern = @"\((.*?)\)"; // Matches text between parentheses
            var match = Regex.Match(update.Name, pattern);
            if (match.Success)
            {
                valueInside = match.Groups[1].Value;
                Console.WriteLine("Value inside parentheses: " + valueInside);
            }
            else
            {
                Console.WriteLine("No match found.");
            }
            return new Animal
            {
        
            
        
        // For Petfinder mapping
                AnimalId = update.OldAnimalId ?? string.Empty,
                // For PetPlace mapping
                OldId = match.Groups[1].Value ?? string.Empty,
                OldName = update.OldName ?? string.Empty,
                OldBreed = update.OldBreed ?? string.Empty,
                ClientId = update.ClientId ?? string.Empty,
                SecondaryBreed = update.SecondaryBreed ?? string.Empty,

                // Common properties
                Name = update.Name ?? string.Empty,
                BreedsLabel = update.BreedsLabel ?? string.Empty,
                Description = update.Description ?? string.Empty,
                PrimaryPhotoUrl = update.PrimaryPhotoUrl ?? string.Empty,
                PrimaryPhotoCroppedUrl = update.PrimaryPhotoCroppedUrl ?? string.Empty,
                LocatedAt = update.LocatedAt ?? string.Empty,
                ShelterName = update.ShelterName ?? string.Empty,
                ShelterAddress = update.ShelterAddress ?? string.Empty,
                CoverImagePath = update.CoverImagePath,
                PetLocation = update.PetLocation ?? string.Empty,
                PetLocationAddress = update.PetLocationAddress ?? string.Empty,
                PetLocationPhone = update.PetLocationPhone ?? string.Empty,
                City = update.City ?? string.Empty,
                State = update.State ?? string.Empty,
                Zip = update.Zip ?? string.Empty,
                Email = update.Email ?? string.Empty,
                PhoneNumber = update.PhoneNumber ?? string.Empty,
                Website = update.Website ?? string.Empty,

                // Additional fields from PetPlace
                Url = update.Url ?? string.Empty,
                Age = update.Age ?? string.Empty,
                Sex = update.Sex ?? string.Empty,
                Size = update.Size ?? string.Empty,
                Type = update.Type ?? string.Empty,
                Species = update.Species ?? string.Empty,
                IsMixedBreed = update.IsMixedBreed,
                AdoptionStatus = update.AdoptionStatus ?? string.Empty,
                PublishedAt = update.PublishedAt,

                // Filter values
                FilterAge = update.FilterAge ?? string.Empty,
                FilterDOB = update.FilterDOB,
                FilterGender = update.FilterGender ?? string.Empty,
                FilterSize = update.FilterSize ?? string.Empty,
                FilterDaysOut = update.FilterDaysOut,
                FilterAnimalType = update.FilterAnimalType ?? string.Empty,
                FilterPrimaryBreed = update.FilterPrimaryBreed ?? string.Empty,

                // Used for filtering in the app – not from JSON
                Source = update.Source ?? string.Empty,

                ImageUrls = update.ImageUrls ?? new List<string>()
            };
        }
    }
}

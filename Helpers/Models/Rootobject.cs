using System;
using System.Text.Json.Serialization;

public class Rootobject
{
    public string[] imageURL { get; set; }
    public Pprequired[] ppRequired { get; set; }
    public object animalDetail { get; set; }
    public object clientDetail { get; set; }
}

public class Pprequired
{
    public string AnimalId { get; set; }

    [JsonPropertyName("Pet Name")]
    public string PetName { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Animal Type")]
    public string AnimalType { get; set; }

    [JsonPropertyName("Primary Breed")]
    public string PrimaryBreed { get; set; }

    [JsonPropertyName("Secondary Breed")]
    public object SecondaryBreed { get; set; }
    [JsonPropertyName("Located at")]
    public string LocatedAt { get; set; }
    public string Age { get; set; }
    public string Gender { get; set; }

    [JsonPropertyName("Size Category")]
    public object SizeCategory { get; set; }

    public string Description { get; set; }

    [JsonPropertyName("Pet Location")]
    public string PetLocation { get; set; }

    [JsonPropertyName("Pet Location Address")]
    public string PetLocationAddress { get; set; }

    [JsonPropertyName("Pet Location Phone")]
    public string PetLocationPhone { get; set; }

    public string ClientId { get; set; }

    [JsonPropertyName("Shelter Name")]
    public string ShelterName { get; set; }

    [JsonPropertyName("Shelter Address")]
    public string ShelterAddress { get; set; }

    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public object Email { get; set; }

    [JsonPropertyName("Phone Number")]
    public string PhoneNumber { get; set; }

    public string Website { get; set; }
    public string filterAge { get; set; }
    public DateTime filterDOB { get; set; }
    public string filterGender { get; set; }
    public object filterSize { get; set; }
    public int filterDaysOut { get; set; }
    public string filterAnimalType { get; set; }
    public string filterPrimaryBreed { get; set; }
}

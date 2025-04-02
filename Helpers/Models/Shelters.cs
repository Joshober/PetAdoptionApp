using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Models
{


    public class Rootobject
    {
        public Shelter[] ShelterList { get; set; }
    }

    public class Shelter
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public float Distance { get; set; }
        public int CountAnimals { get; set; }
        public int CountDogs { get; set; }
        public int CountCats { get; set; }
        public int CountOthers { get; set; }
    }

}

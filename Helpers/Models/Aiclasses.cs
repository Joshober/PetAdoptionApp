using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Helpers.Models
{
    public class AiPhotoResult
    {
        public string Age_Group { get; set; }
        public List<BreedResult> Breeds { get; set; }
        public string Url { get; set; }
    }

    public class BreedResult
    {
        public string Label { get; set; }
        public double Percent { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class PetEvent
    {
        public string? Title { get; set; }
        public string? When { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime StartDate { get; set; }

    }
}

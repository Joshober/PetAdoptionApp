using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Services
{
    public interface IPetfinderService
    {
        Task<IEnumerable<Animal>> GetAnimalsAsync();
    }

   
}

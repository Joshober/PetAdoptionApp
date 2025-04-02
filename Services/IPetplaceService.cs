using MAUI_Tutorial1_TodoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Services
{
    public interface IPetplaceService
    {
        Task<IEnumerable<Animal>> GetAnimalsAsync();
    }
}

using MAUI_Tutorial1_TodoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Services
{
    public interface IPetBackendService
    {
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<IEnumerable<Animal>> GetReccomendation();
        Task<IEnumerable<IEnumerable<Animal>>> GetAllCloseAnimalsAsync();
        Task<IEnumerable<Animal>> GetUnofficalPets();

        Task<Animal> GetDetailAsync(Animal basic);
        Task<IEnumerable<Animal>> GetFavorites();
    }

}

using MAUI_Tutorial1_TodoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MAUI_Tutorial1_TodoList.Helpers
{
    public static class CurrentUserSettings
    {
        // This holds the currently logged in user's info.
        public static UserDTO CurrentUser { get; set; } = new UserDTO();
    }
}

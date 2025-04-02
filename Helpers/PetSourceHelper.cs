using System;
using System.Collections.Generic;
using System.Linq;
using MAUI_Tutorial1_TodoList.ViewModel;

namespace MAUI_Tutorial1_TodoList.Helpers
{
    public static class PetSourceHelper
    {
        public static List<CombinedPetsViewModel.PetSource> Values =>
            Enum.GetValues(typeof(CombinedPetsViewModel.PetSource))
                .Cast<CombinedPetsViewModel.PetSource>()
                .ToList();
    }
}

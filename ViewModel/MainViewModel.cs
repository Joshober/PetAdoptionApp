using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Tutorial1_TodoList;
using MAUI_Tutorial1_TodoList.Views;
using System.Collections.ObjectModel;

namespace MauiApp2.ViewModel;

public partial class AccountViewModel : ObservableObject
{
	public AccountViewModel()
	{
		Items = new ObservableCollection<string>();
	}

	[ObservableProperty]
	ObservableCollection<string> items = new(); // Auto-initialized

	[ObservableProperty]
	private string text = string.Empty; // Default to empty string

	[RelayCommand]
	void Add()
	{
		if (!string.IsNullOrWhiteSpace(Text)) // Ensure text is not empty
		{
			Items.Add(Text);
			Text = string.Empty; // Clear input after adding
		}
	}
	[RelayCommand]
	void Delete(string s)
	{
		if (Items.Contains(s))
		{
			Items.Remove(s);
		}
	}
	[RelayCommand]
	async Task Tap(string s)
	{
		//await Shell.Current.GoToAsync("DetailPage");
		//    await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}",
		//        new Dictionary<string, object> { { "Text", s } });

		//}
		await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
	}
    [RelayCommand]
    public async Task GoToPetDetailsAsync(string animalId)
    {
        if (!string.IsNullOrEmpty(animalId))
        {
            await Shell.Current.GoToAsync($"PetDetailsPage?animalId={animalId}");
        }
    }
    [RelayCommand]
    public async Task GoToAllPetsAsync()
    {
        await Shell.Current.GoToAsync("///AllPetsPage");
    }


}

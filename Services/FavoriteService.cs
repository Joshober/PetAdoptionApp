public static class FavoriteService
{
    const string KEY = "favorites";
    public static HashSet<string> Favorites =>
        Preferences.Get(KEY, "").Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet();

    public static bool IsFavorite(string id) => Favorites.Contains(id);

    public static void AddFavorite(string id)
    {
        var favs = Favorites; favs.Add(id);
        Preferences.Set(KEY, string.Join(",", favs));
    }

    public static void RemoveFavorite(string id)
    {
        var favs = Favorites; favs.Remove(id);
        Preferences.Set(KEY, string.Join(",", favs));
    }
}

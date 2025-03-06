namespace FN.Application.Catalog.Blogs
{
    public class LocalGameDatabase
    {
        private static readonly HashSet<string> _games = new(StringComparer.OrdinalIgnoreCase)
        {
            "Diablo IV", "Cyberpunk 2077", "Elden Ring",
            "Apex Legends", "Valorant", "League of Legends",
            "Counter-Strike 2", "Baldur's Gate 3", "Starfield",
            "Final Fantasy XVI", "The Legend of Zelda: Tears of the Kingdom"
        };
        public static bool Contains(string title)
        {
            return _games.Contains(title) ||
                   _games.Any(g => g.Contains(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}

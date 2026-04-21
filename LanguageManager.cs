using System.Globalization;
using System.Threading;

public static class LanguageManager
{
    // Támogatott nyelvek: kód + megjelenítendő név
    public static readonly (string Code, string DisplayName)[] SupportedLanguages =
    {
        ("hu", "Hungarian"),
        ("en", "English"),
        ("de", "Deutsch"),
        ("fr", "Français"),
        ("es", "Español")
    };

    // Aktuális nyelv indexe
    private static int currentIndex = 0;

    public static string CurrentLanguage => SupportedLanguages[currentIndex].Code;

    public static string CurrentLanguageName => SupportedLanguages[currentIndex].DisplayName;

    // Következő nyelvre váltás
    public static void ToggleLanguage(int langI)
    {
        currentIndex = langI;
        //currentIndex = (currentIndex + 1) % SupportedLanguages.Length;
    }

    // Globális kultúra beállítása
    public static void ApplyCulture()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(CurrentLanguage);
    }
}

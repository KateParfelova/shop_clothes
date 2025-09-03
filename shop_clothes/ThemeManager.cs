using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace shop_clothes
{
    public static class ThemeManager
    {
        private const string ThemeSettingsFile = "theme.config";

        private static ResourceDictionary GetResourceDictionary(string themeName)
        {
            return new ResourceDictionary()
            {
                Source = new Uri($"/Resources/Styles/{themeName}Theme.xaml", UriKind.Relative)
            };
        }

        public static void ChangeTheme(string themeName)
        {
            var dict = GetResourceDictionary(themeName);

            // Удаляем старые темы
            var existingThemes = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"))
                .ToList();

            foreach (var theme in existingThemes)
            {
                Application.Current.Resources.MergedDictionaries.Remove(theme);
            }

            // Добавляем новую тему
            Application.Current.Resources.MergedDictionaries.Add(dict);

            // Сохраняем выбор в файл
            SaveThemeToFile(themeName);
        }

        public static string CurrentTheme => LoadThemeFromFile() ?? "Light";

        private static void SaveThemeToFile(string themeName)
        {
            try
            {
                File.WriteAllText(ThemeSettingsFile, themeName);
            }
            catch
            {
                // Игнорируем ошибки записи
            }
        }

        private static string LoadThemeFromFile()
        {
            try
            {
                if (File.Exists(ThemeSettingsFile))
                {
                    return File.ReadAllText(ThemeSettingsFile);
                }
            }
            catch
            {
                // Игнорируем ошибки чтения
            }
            return "Light";
        }
    }
}
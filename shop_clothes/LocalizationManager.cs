using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace shop_clothes
{
    public static class LocalizationManager
    {
        static ResourceDictionary? setDict;
        private static ResourceDictionary GetResourceDictionary(string culture)
        {
            return new ResourceDictionary()
            {
                Source = new Uri($"/Resources/Languages/Strings.{culture}.xaml", UriKind.Relative)
            };
        }

        public static void ChangeLanguage(string culture)
        {
            var dict = GetResourceDictionary(culture);
            if (setDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(setDict);
            }
            setDict = dict;
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public static string GetString(string key)
        {
            return Application.Current.TryFindResource(key) as string ?? string.Empty;
        }
    }
}

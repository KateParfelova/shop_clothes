using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Globalization;
using System.IO;
using shop_clothes.repositories;
using System.Collections.ObjectModel;

namespace shop_clothes
{
    class AddProductWindowVM : ViewModel, INotifyPropertyChanged
    {
        Products _currentProduct;
        public Products CurrentProduct
        {
            get => _currentProduct;
            set => SetValue(ref _currentProduct, value);
        }
        private ObservableCollection<FilterOption> _filterCategories = new();
        public ObservableCollection<FilterOption> Categories
        {
            get => _filterCategories;
            set
            {
                SetValue(ref _filterCategories, value);
            }
        }
        private FilterOption _selectedFilterCategories;
        public FilterOption SelectedCategories
        {
            get => _selectedFilterCategories;
            set
            {
                SetValue(ref _selectedFilterCategories, value);
                CurrentProduct.category = value.Id;
            }
        }
        public ICommand AddCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public AddProductWindowVM()
        {
            CurrentProduct = new Products() { };
            AddCommand = new RelayCommand(Add);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            ChangeThemeCommand = new RelayCommand(ChangeTheme);
            // Загружаем сохраненную тему
            ThemeManager.ChangeTheme(ThemeManager.CurrentTheme);
            AddCategories();
        }
        void AddCategories()
        {
            List<category> categories;
            using (var context = new Context())
            {
                repositories.Repository repository = new repositories.Repository(context);
                categories = repository.GetAll().ToList();
            }
            foreach (var item in categories)
            {
                Categories.Add(new FilterOption { Id = item.Id, Name = item.name });
            }
            SelectedCategories = Categories.First();
        }
        private void Add(object parameter)
        {
            using (var context = new Context())
            {
                var repository = new RepositoryProducts(context);
                repository.Add(CurrentProduct);
                context.SaveChanges();
            }
            var window = Application.Current.Windows
                .OfType<AddProductWindow>()
                .FirstOrDefault(w => w.DataContext == this);

            window.DialogResult = true;
        }
        public string CurrentCulture => CultureInfo.CurrentUICulture.Name;
        private void ChangeLanguage(object parameter)
        {
            if (parameter is string culture)
            {
                LocalizationManager.ChangeLanguage(culture);
                OnPropertyChanged(nameof(CurrentCulture));
            }
        }
        void ChangeTheme(object parameter)
        {
            if (parameter is string themeName)
            {
                ThemeManager.ChangeTheme(themeName);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using shop_clothes.repositories;

namespace shop_clothes
{
    public class FilterOption : INotifyPropertyChanged
    {
        private int _id;
        private string _name;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class EditProductViewModel : ViewModel, INotifyPropertyChanged
    {

        Products _currentProduct;
        public Products CurrentProduct {
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
        public ICommand SaveCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand RemoveImageCommand { get; }
        public EditProductViewModel(Products product)
        {
            CurrentProduct = product.Clone();
            SaveCommand = new RelayCommand(Save);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            ChangeThemeCommand = new RelayCommand(ChangeTheme);
            SelectImageCommand = new RelayCommand(SelectImage);
            RemoveImageCommand = new RelayCommand(RemoveImage);
            // Загружаем сохраненную тему
            ThemeManager.ChangeTheme(ThemeManager.CurrentTheme);
            AddCategories();

            
        }
        private void SelectImage(object parameter)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Выберите изображение товара"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SetImage(openFileDialog.FileName);
            }
        }

        private void SetImage(string filePath)
        {
            if (IsImageFile(filePath))
            {
                CurrentProduct.imagePath = filePath;
                OnPropertyChanged(nameof(CurrentProduct.imagePath));
            }
            else
            {
                MessageBox.Show("Выберите файл изображения (JPG, PNG, BMP, GIF)",
                              "Неверный формат", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveImage(object parameter)
        {
            CurrentProduct.imagePath = null;
            OnPropertyChanged(nameof(CurrentProduct.imagePath));
        }

        private bool IsImageFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            return allowedExtensions.Contains(extension);
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
            SelectedCategories = Categories.First(x=>x.Id == CurrentProduct.category);
        }
        private void Save(object parameter)
        {
            using (var context = new Context())
            {
                var repository = new RepositoryProducts(context);
                repository.Update(CurrentProduct);
                context.SaveChanges();
            }
            var window = Application.Current.Windows
                .OfType<EditProductWindow>()
                .FirstOrDefault(w => w.DataContext == this);

            window.DialogResult = true;
            window.Close();
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

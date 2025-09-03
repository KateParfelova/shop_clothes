using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Globalization;
using shop_clothes.repositories;
using System.Runtime.CompilerServices;

namespace shop_clothes
{
    class ClientMainWindowVM : ViewModel, INotifyPropertyChanged
    {
        WindowService _windowService;
        User user { get; set; }
        ObservableCollection<Products> _products;
        public ObservableCollection<Products> Products
        {
            get => _products;
            set => SetValue(ref _products, value);
        }
        private ObservableCollection<FilterOption> _filterOptions = new(){

            new FilterOption { Id = 1, Name = "Все" },
            new FilterOption { Id = 2, Name = "Есть в наличии" },
            new FilterOption { Id = 3, Name = "Нет в наличии" }
        };
        public ObservableCollection<FilterOption> FilterOptions
        {
            get => _filterOptions;
            set
            {
                SetValue(ref _filterOptions, value);
                SelectedFilter=FilterOptions.First();
            }
        }
        private FilterOption _selectedFilter;
        public FilterOption SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                SetValue(ref _selectedFilter, value);
                FilterItems();
            }
        }
       
        private ObservableCollection<FilterOption> _filterCategories = new()
        {
            new FilterOption { Id = 0, Name = "Все" }
        };
        public ObservableCollection<FilterOption> FilterCategories
        {
            get => _filterCategories;
            set
            {
                SetValue(ref _filterCategories, value);
                SelectedFilterCategories= FilterCategories.First();
            }
        }
        private FilterOption _selectedFilterCategories;
        public FilterOption SelectedFilterCategories
        {
            get => _selectedFilterCategories;
            set
            {
                SetValue(ref _selectedFilterCategories, value);
                FilterItems();
            }
        }
        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetValue(ref _searchText, value);
                Search();
            }
        }
        
        public ICommand AddToCartCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand BakageCommand { get; }
        public ICommand ProfCommand { get; }

        public ClientMainWindowVM(User user, WindowService windowService)
        {
            _windowService = windowService;
            using (var context = new Context())
            {
                RepositoryProducts repository = new RepositoryProducts(context);
                _products = new ObservableCollection<Products>(repository.GetAllWithCategories().ToList());
            }
            this.user = user;
            AddToCartCommand = new RelayCommand(AddToCart);
            ViewDetailsCommand = new RelayCommand(ViewDetails);
            ExitCommand = new RelayCommand(Exit);
            SelectedFilter = FilterOptions[0];
            SelectedFilterCategories = FilterCategories[0];
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            AddCategories();
            ChangeThemeCommand = new RelayCommand(ChangeTheme);
            // Загружаем сохраненную тему
            ThemeManager.ChangeTheme(ThemeManager.CurrentTheme);
            BakageCommand = new RelayCommand(OpenListProducts);
            ProfCommand = new RelayCommand(ToProf);
        }
        void OpenListProducts(object parameter)
        {
            ListProducts window = new ListProducts
            {
                _windowService = (WindowService)_windowService,
                DataContext = new ListProductsVm(user, _windowService)
            };
            _windowService.CloseWindow();
            _windowService.SetCurrentWindow(window);
            window.Show();

        }
        void ToProf(object parameter)
        {
            Profile window = new Profile
            {
                _windowService = (WindowService)_windowService,
                DataContext = new ProfileVM(user, _windowService)
            };
            _windowService.CloseWindow();
            _windowService.SetCurrentWindow(window);
            window.Show();
        }
        void AddToCart(object parameter)
        {
            if (parameter is Products products)
            {
                using (var context = new Context())
                {
                    RepositoryRequest repository = new RepositoryRequest(context);
                    repository.Add(new Request { Id = 0, IdProducts = products.Id, IdUser = user.Id });
                }
            }
        }
        void FilterItems()
        {
            List<Products> filteredProducts = new();
            switch (SelectedFilter.Id)
            {
                case 1:
                    {
                        using (var context = new Context())
                        {
                            RepositoryProducts repository = new RepositoryProducts(context);
                            filteredProducts = repository.GetAllWithCategories().ToList();
                        }
                        break;
                    }
                case 2:
                    {
                        using (var context = new Context())
                        {
                            RepositoryProducts repository = new RepositoryProducts(context);
                            filteredProducts = repository.GetAllWithCategories().Where(x => x.isOutOfStock == true).ToList();
                        }
                        break;
                    }
                case 3:
                    {
                        using (var context = new Context())
                        {
                            RepositoryProducts repository = new RepositoryProducts(context);
                            filteredProducts = repository.GetAllWithCategories().Where(x => x.isOutOfStock == false).ToList();
                        }
                        break;
                    }
                default: break;
            }
            if (SelectedFilterCategories != null && SelectedFilterCategories.Id != 0)
            {
                filteredProducts = filteredProducts.Where(x => x.category == SelectedFilterCategories.Id).ToList();
            }

            // Очищаем и заполняем ObservableCollection
            Products.Clear();
            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }
        void Exit(object parameter)
        {
            //var currentWindow = Application.Current.Windows
            //.OfType<Window>()
            //.FirstOrDefault(w => w.IsActive);

            //if (currentWindow != null)
            //{
            //    currentWindow.Close();
            //}
            //var authWindow = new MainWindow();
            //authWindow.Show();
            var authWindow = new MainWindow();
            _windowService.CloseWindow();
            // _windowService.SetCurrentWindow(authWindow);
            authWindow.Show();
        }
        void Search()
        {
            using (var context = new Context())
            {
                RepositoryProducts repository = new RepositoryProducts(context);
                var searchResults = repository.GetAllWithCategories()
                    .Where(x => x.shortName.Contains(SearchText) || x.fullName.Contains(SearchText))
                    .ToList();

                // Очищаем и заполняем ObservableCollection
                Products.Clear();
                foreach (var product in searchResults)
                {
                    Products.Add(product);
                }
            }
        }
        public string CurrentCulture => CultureInfo.CurrentUICulture.Name;
        void ChangeLanguage(object parameter)
        {
            if (parameter is string culture)
            {
                LocalizationManager.ChangeLanguage(culture);
                OnPropertyChanged(nameof(CurrentCulture));

                if (culture == "en-US")
                {
                    FilterOptions[0].Name = "All";
                    FilterOptions[1].Name = "Available in stock";
                    FilterOptions[2].Name = "Out of stock";
                    FilterCategories[0].Name = "All";
                   
                }
                else
                {
                    FilterOptions[0].Name = "Все";
                    FilterOptions[1].Name = "Есть в наличии";
                    FilterOptions[2].Name = "Нет в наличии";
                    FilterCategories[0].Name = "Все";
                }
                SelectedFilter = FilterOptions.First();
                SelectedFilterCategories = FilterCategories.First();
                AddCategories();
            }
        }
        void AddCategories()
        {
            List<category> categories;
            using (var context = new Context())
            {
                repositories.Repository repository = new repositories.Repository(context);
                categories = repository.GetAll().ToList();
            }
            if (FilterCategories.Count <= 1)
            {
                foreach (var item in categories)
                {
                    FilterCategories.Add(new FilterOption { Id = item.Id, Name = item.name });
                }
            }
        }
        void ChangeTheme(object parameter)
        {
            if (parameter is string themeName)
            {
                ThemeManager.ChangeTheme(themeName);
            }
                
        }
        void ViewDetails(object parameter)
        {
            if (parameter is Products product)
            {
                var detailsWindow = new ProductDetailsWindow(product, user);
                detailsWindow.Owner = Application.Current.MainWindow;
                detailsWindow.ShowDialog();

                if (detailsWindow.DialogResult == true)
                {
                    using (var context = new Context())
                    {
                        RepositoryProducts repository = new RepositoryProducts(context);
                        _products = new ObservableCollection<Products>(repository.GetAllWithCategories().ToList());
                    }
                    SelectedFilter = FilterOptions[0];
                    SelectedFilterCategories = FilterCategories[0];
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

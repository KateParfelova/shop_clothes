using shop_clothes.repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace shop_clothes
{
    class StoreMainWindowVM: ViewModel, INotifyPropertyChanged
    {
        IWindowService _windowService;
        User user { get; set; }
        ObservableCollection<Products> _products;

        private 
        public ObservableCollection<Products> Products
        {
            get => _products;
            set => SetValue(ref _products, value);
        }
        Products _selectedProduct;
        public Products SelectedProduct
        {
            get => _selectedProduct;
            set => SetValue(ref _selectedProduct, value);
        }
        ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetValue(ref _users, value);
        }
        User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set => SetValue(ref _selectedUser, value);
        }
        bool _isTable1Visible = true;
        bool _isTable2Visible = false;
        public bool IsTable1Visible
        {
            get => _isTable1Visible;
            set => SetValue(ref _isTable1Visible, value);
        }

        public bool IsTable2Visible
        {
            get => _isTable2Visible;
            set => SetValue(ref _isTable2Visible, value);
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
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand ShowTable1Command { get; }
        public ICommand ShowTable2Command { get; }

        public StoreMainWindowVM(User user,IWindowService _windowService)
        {
            this._windowService = _windowService;
            this.user = user;
            using (var context = new Context())
            {
                RepositoryProducts repository = new RepositoryProducts(context);
                _products= new ObservableCollection<Products>(repository.GetAllWithCategories().ToList());
            }
            using (var context = new Context())
            {
                RepUser repository = new RepUser(context);
                _users = new ObservableCollection<User>(repository.GetAll().ToList());
            }
            EditProductCommand = new RelayCommand(EditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            ExitCommand = new RelayCommand(Exit);
            AddCommand = new RelayCommand(Add);
            SelectedFilter = FilterOptions[0];
            SelectedFilterCategories = FilterCategories[0];
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            ShowTable1Command = new RelayCommand(ShowTable1);
            ShowTable2Command = new RelayCommand(ShowTable2);
            AddCategories();
            ChangeThemeCommand = new RelayCommand(ChangeTheme);
            // Загружаем сохраненную тему
            ThemeManager.ChangeTheme(ThemeManager.CurrentTheme);
        }
        private void ShowTable1(object parametr)
        {
            IsTable1Visible = true;
            IsTable2Visible = false;
        }

        private void ShowTable2(object parametr)
        {
            IsTable1Visible = false;
            IsTable2Visible = true;
        }
        void EditProduct(object parameter)
        {
            if (parameter is Products product)
            {
                SelectedProduct = product;
                OpenEditWindow();
            }
        }
        void OpenEditWindow()
        {
            var editWindow = new EditProductWindow
            {
                DataContext = new EditProductViewModel(SelectedProduct)
            };

            editWindow.ShowDialog();

            // Обновляем данные после закрытия окна редактирования
            if (editWindow.DialogResult == true)
            {
                using (var context = new Context())
                {
                    RepositoryProducts repository = new RepositoryProducts(context);
                    Products = new ObservableCollection<Products>(repository.GetAllWithCategories().ToList());
                }
                SelectedFilter = FilterOptions[0];
                SelectedFilterCategories = FilterCategories[0];
            }
        }
        void DeleteProduct(object parameter)
        {
            if (parameter is Products product)
            {
                SelectedProduct = product;
                using (var context = new Context())
                {
                    RepositoryProducts repository = new RepositoryProducts(context);
                    repository.Delete(SelectedProduct);
                    Products = new ObservableCollection<Products>(repository.GetAllWithCategories().ToList());
                }
                SelectedFilter = FilterOptions[0];
                SelectedFilterCategories = FilterCategories[0];
            }
        }
        void DeleteUser(object parameter)
        {
            if (parameter is User user)
            {
                SelectedUser = user;
                using (var context = new Context())
                {
                    RepositoryRequest request = new RepositoryRequest(context);
                    foreach(var item in request.GetAll().ToList())
                    {
                        if (item.IdUser == user.Id) request.Delete(item); 
                    }
                }
                using (var context = new Context())
                {
                    RepUser repository = new RepUser(context);
                    repository.Delete(SelectedUser);
                    Users = new ObservableCollection<User>(repository.GetAll().ToList());
                }
                SelectedFilter = FilterOptions[0];
                SelectedFilterCategories = FilterCategories[0];
            }
        }
        void DeleteRequest()
        {

        }
        void FilterItems()
        {
            List<Products> filteredProducts=new();
            if (SelectedFilter!=null)
            switch (SelectedFilter.Id) {
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
            if(SelectedFilterCategories != null && SelectedFilterCategories.Id != 0)
            {
                filteredProducts = filteredProducts.Where(x => x.category == SelectedFilterCategories.Id).ToList();
            }
            Products.Clear();
            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }
        void Exit(object parameter)
        {
            var authWindow = new MainWindow();
            _windowService.CloseWindow();
           // _windowService.SetCurrentWindow(authWindow);
            authWindow.Show();
        }
        void Add(object parameter)
        {
            var addWindow = new AddProductWindow
            {
                DataContext = new AddProductWindowVM()
            };

            addWindow.ShowDialog();

            // Обновляем данные после закрытия окна редактирования
            if (addWindow.DialogResult == true)
            {
                using (var context = new Context())
                {
                    RepositoryProducts repository = new RepositoryProducts(context);
                    Products = new ObservableCollection<Products>(repository.GetAllWithCategories().ToList());
                }
                SelectedFilter = FilterOptions[0];
                SelectedFilterCategories = FilterCategories[0];
            }
        }
        void Search()
        {
            using (var context = new Context())
            {
                RepositoryProducts repository = new RepositoryProducts(context);
                var searchResults = repository.GetAllWithCategories()
                    .Where(x => x.shortName.Contains(SearchText) || x.fullName.Contains(SearchText))
                    .ToList();

                Products.Clear();
                foreach (var product in searchResults)
                {
                    Products.Add(product);
                }
            }
        }
        public string CurrentCulture => CultureInfo.CurrentUICulture.Name;
        private void ChangeLanguage(object parameter)
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
    }
}

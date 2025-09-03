using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace shop_clothes
{
    class MainWindowVM: ViewModel
    {
        
        private readonly WindowService _windowService;

        string _userEmail;
        string _password;
        public string UserEmail 
        {
            get => _userEmail;
            set => SetValue(ref _userEmail, value);
        }
        public string Password
        {
            get => _password;
            set => SetValue(ref _password, value);
        }
        public ICommand LoginCommand { get; }
        public ICommand RegCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public MainWindowVM(WindowService windowService)
        {
            _windowService = windowService;
            LoginCommand = new RelayCommand(Login);
            RegCommand = new RelayCommand(Reg);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            ChangeThemeCommand = new RelayCommand(ChangeTheme);
            // Загружаем сохраненную тему
            ThemeManager.ChangeTheme(ThemeManager.CurrentTheme);
        }
        void Login(object parameter)
        {

            //UserEmail = "admin@admin";
            //Password = "Admin123!";
            //string role = "admin";

            UserEmail = "admin@admin";
            Password = "Admin123!";
            string role = "admin";
            int id = 2;


            // Проверка введенных данных
            if (string.IsNullOrEmpty(UserEmail) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка учетных данных (в реальном приложении это должно быть в базе данных)
            List<User> users = new List<User>();
            using (var context = new Context())
            {
               users = new RepUser(context).GetAll().ToList();
            }
                if (users.Where(x=>x.Password==Password && x.Email==UserEmail).Count() > 0)
                {
                    // Открытие главного окна приложения
                    OpenMainApplicationWindow(new User { Email = UserEmail, Password = Password, Role=role, Id=id }, parameter as Window);

                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }

        }
        void Reg(object parameter)
        {
            

                
           OpenMainApplicationWindow2(parameter as Window);

        }
        void OpenMainApplicationWindow(User username, Window authWindow)
        {
            string isAdmin = username.Role;

            if (isAdmin=="admin")
            {
                //username.Id = 2;
                // Создаем главное окно приложения
                StoreMainWindow mainWindow = new StoreMainWindow {
                    _windowService = (WindowService)_windowService,
                    DataContext = new StoreMainWindowVM(username,_windowService),
                };
                _windowService.CloseWindow();
                _windowService.SetCurrentWindow(mainWindow);
                
                //// Показываем новое окно
                mainWindow.Show();
                //authWindow?.Close();
                
            }
            else
            {
               // username.Id = 1;
                // Создаем главное окно приложения
                ClientMainWindow mainWindow = new ClientMainWindow {
                    _windowService = (WindowService)_windowService,
                    DataContext = new ClientMainWindowVM(username, _windowService)
                };

                _windowService.CloseWindow();
                _windowService.SetCurrentWindow(mainWindow);

                //// Показываем новое окно
                mainWindow.Show();
            }

        }
        void OpenMainApplicationWindow2(Window authWindow)
        {

            Registration mainWindow = new Registration
            {
                    _windowService = _windowService,
                    DataContext = new RegistrationVM( _windowService),
                };
                _windowService.CloseWindow();
                _windowService.SetCurrentWindow(mainWindow);

               
                mainWindow.Show();
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

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
    class RegistrationVM:ViewModel
    {
       
        private readonly WindowService _windowService;

        string _userEmail;
        string _password;
        string _reppassword;
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
        public string RepPassword
        {
            get => _reppassword;
            set => SetValue(ref _reppassword, value);
        }
        public ICommand LoginCommand { get; }
        public ICommand RegCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public RegistrationVM(WindowService windowService)
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
            OpenMainApplicationWindow2(parameter as Window);
        }
        void OpenMainApplicationWindow2(Window authWindow)
        {

            MainWindow mainWindow = new MainWindow
            {
                _windowService = _windowService,
                DataContext = new MainWindowVM(_windowService),
            };
            _windowService.CloseWindow();
            _windowService.SetCurrentWindow(mainWindow);


            mainWindow.Show();
        }
        void Reg(object parameter)
        {
            List<User> users = new();
            using (var context = new Context())
            {
                RepUser rep = new RepUser(context);
                users = rep.GetAll().ToList();
            }
            if (Password != RepPassword)
            {

            }
            else if(users.Where(x=>x.Email == UserEmail || x.Password == Password).Count() == 0)
            {
                using (var context = new Context()) {
                    RepUser rep = new RepUser(context);
                    rep.Add(new User { Email = UserEmail, Password = Password, Role = "client" });
                }
                OpenMainApplicationWindow2(parameter as Window);
            }

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

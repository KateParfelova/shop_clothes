using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace shop_clothes
{
     class ProfileVM:ViewModel, INotifyPropertyChanged
    {
        WindowService _windowService;

        User user { get; set; }
        string _email;
        public string Email
        {
            get => _email;
            set => SetValue(ref _email, value);
        }
        string _password;
        public string Password
        {
            get => _password;
            set => SetValue(ref _password, value);
        }
        public ICommand ExitCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand MainCommand { get; }
        public ICommand CanselCommand {  get; }
        public ICommand SaveCommand {  get; }
        public ProfileVM(User user, WindowService windowService)
        {
            _windowService = windowService;
            this.user = user;
            ExitCommand = new RelayCommand(Exit);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            ChangeThemeCommand = new RelayCommand(ChangeTheme);
            ThemeManager.ChangeTheme(ThemeManager.CurrentTheme);
            MainCommand = new RelayCommand(ToMain);
            SaveCommand = new RelayCommand(Save);
            CanselCommand = new RelayCommand(Cansel);
            Email = user.Email;
            Password = user.Password;

        }
        void Save(object parameter)
        {
            if (Email != null && Email != "" && Password != null && Password != "")
            {
                bool search = false;
                using (var context = new Context())
                {
                    RepUser rep = new RepUser(context);

                    foreach (var item in rep.GetAll().ToList())
                    {
                        if (item.Email == Email) { search = true; break; }
                    }
                    if (!search)
                    {
                        rep.Update(new User { Id = user.Id, Email = Email, Password = Password, Role = user.Role });
                        user.Email = Email;
                        user.Password = Password;
                    }

                }

            }
        }
        void Cansel(object parameter)
        {
            user.Email = Email;
            user.Password = Password;
        }
        void ToMain(object parameter)
        {
            _windowService.CloseWindow();
            var window = new ClientMainWindow
            {
                DataContext = new ClientMainWindowVM(user, _windowService)
            };
            _windowService.SetCurrentWindow(window);
            window.Show();
        }
        void Exit(object parameter)
        {
            var authWindow = new MainWindow();
            _windowService.CloseWindow();
            authWindow.Show();
        }
        public string CurrentCulture => CultureInfo.CurrentUICulture.Name;
        void ChangeLanguage(object parameter)
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

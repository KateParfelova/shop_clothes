using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace shop_clothes
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public WindowService _windowService;
        public Registration()
        {
            InitializeComponent();
            _windowService = new WindowService(this);
            this.DataContext = new RegistrationVM(_windowService);

            StreamResourceInfo sri = Application.GetResourceStream(
            new Uri("Resources/custom_cursor.cur", UriKind.Relative));
            Cursor customCursor = new Cursor(sri.Stream);
            this.Cursor = customCursor;
            this.Closed += MainWindow_Closed;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = DataContext as RegistrationVM;

            if (viewModel != null)
            {
                viewModel.Password = Password.Password;
            }

        }
        private void PasswordBox_PasswordChanged2(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = DataContext as RegistrationVM;

            if (viewModel != null)
            {
                viewModel.RepPassword = Password2.Password;
            }

        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // Освобождаем ресурсы сервиса при закрытии окна
            if (_windowService is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _windowService = null;
        }
    }
}

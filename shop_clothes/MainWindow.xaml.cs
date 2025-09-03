using System.Text;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Resources;


namespace shop_clothes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WindowService _windowService;
        public MainWindow()
        {
            InitializeComponent();
            _windowService = new WindowService(this);
            this.DataContext = new MainWindowVM(_windowService);

            StreamResourceInfo sri = Application.GetResourceStream(
            new Uri("Resources/custom_cursor.cur", UriKind.Relative));
            Cursor customCursor = new Cursor(sri.Stream);
            this.Cursor = customCursor;
            this.Closed += MainWindow_Closed;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = DataContext as MainWindowVM;

            if (viewModel != null)
            {
                viewModel.Password = Password.Password;
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
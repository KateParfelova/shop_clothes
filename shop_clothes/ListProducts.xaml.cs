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
using System.Windows.Shapes;

namespace shop_clothes
{
    /// <summary>
    /// Логика взаимодействия для ListProducts.xaml
    /// </summary>
    
    public partial class ListProducts : Window
    {
        public WindowService _windowService;
        public ListProducts()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
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

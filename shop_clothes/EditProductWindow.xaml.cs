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
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public WindowService _windowService;
        public EditProductWindow()
        {
            InitializeComponent();
            DropArea.DragOver += DropArea_DragOver;
            DropArea.PreviewDragOver += DropArea_PreviewDragOver;
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
        private void DropArea_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void DropArea_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }


        // Обработчик клика на область для выбора файла
        private void DropArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as EditProductViewModel;
            vm?.SelectImageCommand.Execute(null);
        }


    }
}

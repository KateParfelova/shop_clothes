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
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public WindowService _windowService;
        public AddProductWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
        }
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Делаем поле ID нередактируемым
            if (e.PropertyName == nameof(User.Id))
            {
                e.Column.IsReadOnly = true;
            }

            // Форматирование для числовых полей
            if (e.PropertyType == typeof(float) || e.PropertyType == typeof(double))
            {
                var column = e.Column as DataGridTextColumn;
                if (column != null)
                {
                    column.Binding.StringFormat = "N2";
                }
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
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Принудительно обновляем привязку
                var binding = ((TextBox)e.EditingElement).GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }
    }
}

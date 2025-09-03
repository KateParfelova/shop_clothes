using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace shop_clothes
{
    public interface IWindowService
    {
        void CloseWindow();
        void SetCurrentWindow(Window window);
        void MinimizeWindow();
        void MaximizeWindow();
    }
    public class WindowService : IWindowService, IDisposable
    {
        private Window _window;
        private CancellationTokenSource _cts; // для отмены фоновых операций

        public WindowService(Window window)
        {
            _window = window;
            _cts = new CancellationTokenSource();
        }
        public void SetCurrentWindow(Window window)
        {
            _window = window;
        }

        public void CloseWindow()
        {
            _window?.Close();
        }

        public void MinimizeWindow()
        {
            if (_window != null)
                _window.WindowState = WindowState.Minimized;
        }

        public void MaximizeWindow()
        {
            if (_window != null)
                _window.WindowState = WindowState.Maximized;
        }
        public void Dispose()
        {
            // Отменяем все фоновые операции
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}

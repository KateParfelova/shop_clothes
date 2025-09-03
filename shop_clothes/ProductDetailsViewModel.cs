using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using shop_clothes.repositories;

namespace shop_clothes
{
    class ProductDetailsViewModel : ViewModel, INotifyPropertyChanged
    {
        private Products _selectedProduct;
        User user;

        public Products CurrentProduct
        {
            get => _selectedProduct;
            set
            {
                SetValue(ref _selectedProduct, value);
            }
        }
        public double Rating
        {
            get => (double)CurrentProduct.rating;
        }

        public ICommand AddToCartCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action RequestClose;

        public ProductDetailsViewModel(Products product,User user)
        {
            this.user = user;
            CurrentProduct = product;
            AddToCartCommand = new RelayCommand(AddToCart);
            CloseCommand = new RelayCommand(Close);
        }

        private void AddToCart(object parameter)
        {
          
           if(parameter is Products product) {
                using (var context = new Context())
                {
                    RepositoryRequest repository = new RepositoryRequest(context);
                    repository.Add(new Request { Id = 0, IdProducts = product.Id, IdUser = user.Id });
                }
                var window = Application.Current.Windows
                .OfType<ProductDetailsWindow>()
                .FirstOrDefault(w => w.DataContext == this);

                window.DialogResult = true;
                window.Close();
            }
            //MessageBox.Show($"{CurrentProduct.shortName} добавлен в корзину!");
        }

        private void Close(object parameter)
        {
            RequestClose?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

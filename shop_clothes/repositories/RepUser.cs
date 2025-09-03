using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes
{
    internal class RepUser : IRepository<User>
    {
        private DbSet<User> _products;
        private Context _context;

        public RepUser(Context context)
        {
            _context = context;
            _products = _context.Users;
        }
        public List<User> Search(User item)
        {
            List<User> products = [];
            foreach (var prod in _products)
            {
                if(prod.Id == item.Id)
                {
                    products.Add(prod);
                    break;
                }
            }
            return products;
        }
        public void Add(User item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.Id = 0;
            try
            {
                _products.Add(item);
                Save();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error adding product: {ex.InnerException?.Message}");

            }
        }
        public void DeleteById(int id)
        {
            var item = _products.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                Delete(item);
                Save();
            }
        }

        public void Delete(User item)
        {
            _products.Remove(item);
            Save();
        }
        public void DeleteByName(string login)
        {

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<User> GetAll()
        {
            return _products;
        }

        public User? GetById(int id)
        {
            var item = _products.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                return item;
            }
            else
            {
                return new User { };//проверить
            }
        }

        public List<User> GetByName(string login)
        {

            return [];//проверить

        }

        public User GetItem(User item)
        {
            var it = _products.Where(x => x.Id == item.Id).ToList();
            if (it != null)
            {
                return it[0];
            }
            else
            {
                return new User { };//проверить
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User newItem)//протестировать
        {
            var item = GetById(newItem.Id);
            Type type = newItem.GetType();
            PropertyInfo[] fields = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                // Копируем значения полей
                object? value = field.GetValue(newItem);
                field.SetValue(item, value);
            }
            Save();
        }

        public void DeleteAll()
        {
            _context.Products.RemoveRange(_context.Products);
            Save();
        }
    }
}

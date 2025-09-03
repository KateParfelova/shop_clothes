using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes.repositories
{
    internal class Repository : IRepository<category>
    {
        private DbSet<category> _categories;
        private Context _context;
        public Repository(Context context)
        {
            _context = context;
            _categories = _context.Categories;
        }
        public void Add(category item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.Id = 0;
            try
            {
                _categories.Add(item);
                Save();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error adding category: {ex.InnerException?.Message}");

            }
        }

        public void DeleteById(int id)
        {
            var item = _categories.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                Delete(item);
                Save();
            }
        }

        public void DeleteByName(string name)
        {
            var item = _categories.FirstOrDefault(u => u.name == name);
            if (item != null)
            {
                Delete(item);
                Save();
            }
        }

        public void Delete(category item)
        {
            _categories.Remove(item);
            Save();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<category> GetAll()
        {
            return _categories;
        }

        public category? GetById(int id)
        {
            var item = _categories.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;//проверить
            }
        }

        public List<category> GetByName(string name)
        {
            var item = new List<category> { _categories.FirstOrDefault(u => u.name == name) };
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;//проверить
            }
        }

        public category GetItem(category item)
        {
            var it = _categories.Where(x => x.Id == item.Id).ToList();
            if (it != null)
            {
                return it[0];
            }
            else
            {
                return new category { };//проверить
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public List<category> Search(category item)
        {
            List<category> products = [];
            foreach (var prod in _categories)
            {
                if (prod.Id == item.Id)
                {
                    products.Add(prod);
                }
            }
            return products;
        }

        public void Update(category newItem)
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
            _context.Categories.RemoveRange(_context.Categories);
            Save();
        }
    }
}

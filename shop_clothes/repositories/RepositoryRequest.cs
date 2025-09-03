using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes.repositories
{
    class RepositoryRequest : IRepository<Request>
    {
        private DbSet<Request> _requests;
        private Context _context;
        public RepositoryRequest(Context context)
        {
            _context = context;
            _requests = _context.Requests;
        }
        public void Add(Request item)
        {
           _requests.Add(item);
           Save();
        }

        public void Delete(Request item)
        {
            _requests.Remove(item);
            Save();
        }

        public void DeleteAll()
        {
            _context.Requests.RemoveRange(_context.Requests);
            Save();
        }

        public void DeleteById(int id)
        {
            var item = _requests.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                Delete(item);
                Save();
            }
        }

        public void DeleteByName(string name)
        {
            
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Request> GetAll()
        {
            return _requests;
        }

        public Request? GetById(int id)
        {
            var item = _requests.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public List<Request> GetByName(string name)
        {
            return [];
        }

        public Request GetItem(Request item)
        {
            var it = GetById(item.Id);
            if(it != null) { return it; }
            else { return new Request(); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public List<Request> Search(Request item)
        {
            List<Request> products = [];
            foreach (var prod in _requests)
            {
                if (prod != null && prod.Id == item.Id)
                {
                    products.Add(prod);
                }
            }
            return products;
        }

        public void Update(Request newItem)
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
    }
}

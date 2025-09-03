using Microsoft.EntityFrameworkCore;
using shop_clothes.repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace shop_clothes
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll();//ICollection<T> GetAll();
        T? GetById(int id);//
        T GetItem(T item);//
        List<T> GetByName(string name);//
        List<T> Search(T item);//
        void Add(T item);//
        void Update(T newItem);
        void DeleteById(int id);//
        void Delete(T item);//
        void DeleteByName(string name);//
        void Save();
        void DeleteAll();
    }
    internal class RepositoryProducts : IRepository<Products>
    {
        private DbSet<Products> _products;
        private Context _context;

        public RepositoryProducts(Context context)
        {
            _context = context;
            _products = _context.Products;
        }
        public IEnumerable<Products> GetAllWithCategories()
        {
           
                return _context.Products
                    .Include(p => p.Category) // Важно: включаем категории
                    .ToList();
            
        }
        public List<Products> Search(Products item)
        {
            List<Products> products = [];
            foreach (var prod in _products)
            {
                if (prod.Id == item.Id)
                {
                    products.Add(prod);
                }
            }
            return products;
        }
        public void Add(Products item)
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

        public void Delete(Products item)
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

        public IEnumerable<Products> GetAll()
        {
            return _products;
        }

        public Products? GetById(int id)
        {
            var item = _products.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                return item;
            }
            else
            {
                return new Products { };//проверить
            }
        }

        public List<Products> GetByName(string login)
        {
           
                return [];//проверить
            
        }

        public Products GetItem(Products item)
        {
            var it = _products.Where(x => x.Id == item.Id).ToList();
            if (it != null)
            {
                return it[0];
            }
            else
            {
                return new Products { };//проверить
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Products newItem)//протестировать
        {
            var item = GetById(newItem.Id);
            item = newItem.Clone();
            Save();
        }

        public void DeleteAll()
        {
            _context.Products.RemoveRange(_context.Products);
            Save();
        }
    }
}
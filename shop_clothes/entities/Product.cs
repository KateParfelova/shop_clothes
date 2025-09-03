using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace shop_clothes
{
    public class Products : INotifyPropertyChanged, ICreateble<Products>, IEquatable<Products>
    {
        [Key]
        public int Id { get; set; }                  // Уникальный идентификатор
        public string shortName { get; set; }           // Краткое название
        public string fullName { get; set; }            // Полное название
        public string description { get; set; }         // Описание
        public string imagePath { get; set; }    // Путь к изображению
        public int category { get; set; }            // Категория (например, "Футболки")
        public float rating { get; set; }              // Рейтинг (0-5)
        public float price { get; set; }              // Цена
        public int quantity { get; set; }               // Количество на складе
        public float discount { get; set; }           // Скидка (%)                                             // Доп. поля (опционально для ЛР 4-5):
        public string color { get; set; }               // Цвет
        public string size { get; set; }                // Размер
        public string deliveryCountry { get; set; }
        public bool isOutOfStock { get; set; }          // Нет в наличии

        public int purchasedCount { get; set; }
        public string manufacturer { get; set; }
        public virtual category? Category { get; set; }

        public decimal FinalPrice
        {
            get
            {
                if (discount > 0)
                {
                    return (decimal)(price - (price * discount / 100));
                }
                return (decimal)price;
            }
        }
        public Products Clone()
        {
            return new Products
            {
                Id = this.Id,
                shortName = this.shortName,
                fullName = this.fullName,
                description = this.description,
                imagePath = this.imagePath,
                category = this.category,
                rating = this.rating,
                price = this.price,
                quantity = this.quantity,
                color = this.color,
                size = this.size,
                isOutOfStock = this.isOutOfStock,
                discount = this.discount,
                deliveryCountry = this.deliveryCountry,
                purchasedCount = this.purchasedCount,
                manufacturer = this.manufacturer,
                Category = this.Category?.Clone() as category,
            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Products Create(SqlDataReader reader)
        {
            return new Products()
            {
                Id = (int)reader.GetValue(0),
                shortName = (string)reader.GetValue(1),
                fullName = (string)reader.GetValue(2),
                description = (string)reader.GetValue(3),
                imagePath = (string)reader.GetValue(4),
                category = (int)reader.GetValue(5),
                rating = (float)reader.GetValue(6),
                price = (float)reader.GetValue(7),
                quantity = (int)reader.GetValue(8),
                discount = (float)reader.GetValue(9),
                color = (string)reader.GetValue(10),
                size = (string)reader.GetValue(11),
                deliveryCountry = (string)reader.GetValue(12),
                isOutOfStock = (bool)reader.GetValue(13),
                purchasedCount = (int)reader.GetValue(14),
                manufacturer = (string)reader.GetValue(15),
                Category = (category)reader.GetValue(16)
            };
        }

        public bool Equals(Products other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id &&
                   string.Equals(shortName, other.shortName, StringComparison.Ordinal) &&
                   string.Equals(fullName, other.fullName, StringComparison.Ordinal) &&
                   string.Equals(description, other.description, StringComparison.Ordinal) &&
                   string.Equals(imagePath, other.imagePath, StringComparison.Ordinal) &&
                   category == other.category &&
                   rating.Equals(other.rating) &&
                   price.Equals(other.price) &&
                   quantity == other.quantity &&
                   discount.Equals(other.discount) &&
                   string.Equals(color, other.color, StringComparison.Ordinal) &&
                   string.Equals(size, other.size, StringComparison.Ordinal) &&
                   string.Equals(deliveryCountry, other.deliveryCountry, StringComparison.Ordinal) &&
                   isOutOfStock == other.isOutOfStock &&
                   purchasedCount == other.purchasedCount &&
                   string.Equals(manufacturer, other.manufacturer, StringComparison.Ordinal);
        }

        // Переопределение Equals(object)
        public override bool Equals(object obj)
        {
            if (obj is Products other)
            {
                return Equals(other);
            }
            return false;
        }

        // Переопределение GetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + (shortName?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + (fullName?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + (description?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + (imagePath?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + category.GetHashCode();
                hash = hash * 23 + rating.GetHashCode();
                hash = hash * 23 + price.GetHashCode();
                hash = hash * 23 + quantity.GetHashCode();
                hash = hash * 23 + discount.GetHashCode();
                hash = hash * 23 + (color?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + (size?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + (deliveryCountry?.GetHashCode(StringComparison.Ordinal) ?? 0);
                hash = hash * 23 + isOutOfStock.GetHashCode();
                hash = hash * 23 + purchasedCount.GetHashCode();
                hash = hash * 23 + (manufacturer?.GetHashCode(StringComparison.Ordinal) ?? 0);
                return hash;
            }
        }
    }
}
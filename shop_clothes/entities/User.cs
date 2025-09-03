using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes
{
    public class User: ICreateble<User>, ICloneable, IEquatable<User>
    {
        [Key]
        public int Id { get; set; }
       
        public string? Email { get; set; }
        //[Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string? Password { get; set; }
        //[Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string? Role { get; set; }

        public static User Create(SqlDataReader reader)
        {
            return new User()
            {
                Id = (int)reader.GetValue(0),
                Email = (string?)reader.GetValue(1),
                Password = (string?)reader.GetValue(2),
                Role = (string?)reader.GetValue(3)
            };
        }

        public object Clone() => new User()
        {
            Id = Id,
            Email = Email,
            Password = Password,    
            Role = Role
        };

        public bool Equals(User? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.Id == other.Id &&
                this.Email == other.Email &&
                this.Password == other.Password &&
                this.Role == other.Role;
        }

        public override bool Equals(object obj)
        {
            if (obj is Products other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + (Email?.GetHashCode() ?? 0);
                hash = hash * 23 + (Password?.GetHashCode() ?? 0);
                hash = hash * 23 + (Role?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}

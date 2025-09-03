using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes
{
    public class category: ICreateble<category>, ICloneable
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public virtual ICollection<Products>? Products { get; set; }

        public static category Create(SqlDataReader reader)
        {
            return new category()
            {
                Id = (int)reader.GetValue(0),
                name = (string)reader.GetValue(1),
                Products= (ICollection<Products>?)reader.GetValue(2)
            };
        }

        public object Clone() => new category()
        {
            Id= Id,
            name = name,
            Products= Products?.Select(x => x.Clone()).ToList()
        };
       
    }
}
